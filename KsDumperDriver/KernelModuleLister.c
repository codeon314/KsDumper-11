#include "NTUndocumented.h"
#include "KernelModuleLister.h"

// -----------------------------------------------------------------------------
// Enumerate the loaded kernel modules.
//
// The `SystemModuleInformation` class is populated by the kernel by walking the
// internal PsLoadedModuleList, which is a doubly-linked list of
// _KLDR_DATA_TABLE_ENTRY structures (one per loaded driver/image).  Because
// PsLoadedModuleList is not an exported symbol, we do not attempt to resolve it
// directly; instead we use this documented query interface, which is the
// reliable, crash-free way to reach the same data.
// -----------------------------------------------------------------------------
NTSTATUS GetKernelModules(
	PVOID bufferAddress,
	INT32 bufferSize,
	PINT32 moduleCount)
{
	NTSTATUS status;
	ULONG neededSize = 0;
	PVOID sysInfoBuffer = NULL;
	PRTL_PROCESS_MODULES modules;
	PKERNEL_DRIVER_INFO out;
	ULONG maxEntries;
	ULONG written = 0;
	ULONG i;

	*moduleCount = 0;

	if (bufferAddress == NULL || bufferSize < (INT32)sizeof(KERNEL_DRIVER_INFO))
	{
		return STATUS_INVALID_PARAMETER;
	}

	// First call: get the required buffer size for the raw module list.
	status = ZwQuerySystemInformation(
		SystemModuleInformation, NULL, 0, &neededSize);
	if (neededSize == 0)
	{
		return STATUS_UNSUCCESSFUL;
	}

	sysInfoBuffer = ExAllocatePoolWithTag(NonPagedPool, neededSize, 'DvrM');
	if (sysInfoBuffer == NULL)
	{
		return STATUS_INSUFFICIENT_RESOURCES;
	}

	status = ZwQuerySystemInformation(
		SystemModuleInformation, sysInfoBuffer, neededSize, &neededSize);
	if (!NT_SUCCESS(status))
	{
		ExFreePoolWithTag(sysInfoBuffer, 'DvrM');
		return status;
	}

	modules = (PRTL_PROCESS_MODULES)sysInfoBuffer;
	maxEntries = (ULONG)bufferSize / (ULONG)sizeof(KERNEL_DRIVER_INFO);
	out = (PKERNEL_DRIVER_INFO)bufferAddress;

	__try
	{
		for (i = 0; i < modules->NumberOfModules && written < maxEntries; i++)
		{
			PRTL_PROCESS_MODULE_INFORMATION m = &modules->Modules[i];

			// Raw module name is ANSI.  Convert to WCHAR for the user-mode side.
			// `FullPathName` here is a path fragment such as
			// "\SystemRoot\system32\ntoskrnl.exe".
			ULONG j;
			for (j = 0; j < 255 && m->FullPathName[j] != 0; j++)
			{
				out[written].FullPathName[j] = (WCHAR)m->FullPathName[j];
			}
			out[written].FullPathName[j] = L'\0';

			out[written].BaseAddress = m->ImageBase;
			out[written].SizeOfImage = m->ImageSize;
			written++;
		}

		*moduleCount = (INT32)written;
		status = STATUS_SUCCESS;
	}
	__except (EXCEPTION_EXECUTE_HANDLER)
	{
		status = GetExceptionCode();
	}

	ExFreePoolWithTag(sysInfoBuffer, 'DvrM');
	return status;
}

// -----------------------------------------------------------------------------
// Dump a single kernel driver's in-memory image into the caller's user-mode
// buffer.
//
// IMPORTANT:
//   `userBuffer` originates from user mode (see KERNEL_DUMP_DRIVER_OPERATION in
//   UserModeBridge.h).  MmCopyMemory only accepts a KERNEL VA as its target;
//   feeding it a user VA causes SMAP to fire on x64 and bugchecks the box.
//
//   We therefore:
//     1. ProbeForWrite() the user buffer up-front (fail fast, before touching
//        kernel memory or allocating anything),
//     2. allocate a NonPagedPool staging buffer of `bufferSize` bytes,
//     3. MmCopyMemory(kernel stage <- kernel source) using MM_COPY_MEMORY_VIRTUAL,
//     4. RtlCopyMemory(user buffer <- kernel stage) inside a structured
//        exception handler.
//
//   MmCopyMemory can return a warning status for partially-resident images
//   (STATUS_PARTIAL_COPY etc.).  As long as at least one byte was actually
//   transferred and delivered to the caller, we normalize the status to
//   STATUS_SUCCESS so user-mode does not treat the partial snapshot as a hard
//   failure.  *bytesRead tells the caller how many bytes are valid.
// -----------------------------------------------------------------------------
NTSTATUS DumpKernelDriver(
	PVOID baseAddress,
	PVOID userBuffer,
	INT32 bufferSize,
	PINT32 bytesRead)
{
	NTSTATUS status;
	SIZE_T bytesTransferred = 0;
	MM_COPY_ADDRESS sourceAddress;
	PVOID kernelBuffer = NULL;

	if (bytesRead != NULL)
	{
		*bytesRead = 0;
	}

	if (baseAddress == NULL || userBuffer == NULL || bytesRead == NULL || bufferSize <= 0)
	{
		return STATUS_INVALID_PARAMETER;
	}

	// Cheap upper bound to keep a bogus SizeOfImage from requesting a
	// multi-gigabyte allocation.  64 MB covers every realistic .sys image.
	if (bufferSize > 64 * 1024 * 1024)
	{
		return STATUS_INVALID_PARAMETER;
	}

	// Fail fast if the caller handed us a bogus user pointer.  ProbeForWrite
	// raises on invalid ranges; we translate that into a clean NTSTATUS so we
	// do not waste an allocation on a doomed request.
	__try
	{
		ProbeForWrite(userBuffer, (SIZE_T)bufferSize, 1);
	}
	__except (EXCEPTION_EXECUTE_HANDLER)
	{
		return GetExceptionCode();
	}

	kernelBuffer = ExAllocatePoolWithTag(NonPagedPool, (SIZE_T)bufferSize, 'DmpD');
	if (kernelBuffer == NULL)
	{
		return STATUS_INSUFFICIENT_RESOURCES;
	}

	sourceAddress.VirtualAddress = baseAddress;

	status = MmCopyMemory(
		kernelBuffer,
		sourceAddress,
		(SIZE_T)bufferSize,
		MM_COPY_MEMORY_VIRTUAL,
		&bytesTransferred);

	*bytesRead = (INT32)bytesTransferred;

	// Copy whatever we actually read to the caller's user buffer.  This is
	// deliberately done even for warning statuses (e.g. STATUS_PARTIAL_COPY)
	// so the caller can still use a truncated snapshot.
	if (bytesTransferred > 0)
	{
		__try
		{
			ProbeForWrite(userBuffer, bytesTransferred, 1);
			RtlCopyMemory(userBuffer, kernelBuffer, bytesTransferred);

			// Normalize: we successfully delivered data, so report success
			// even if MmCopyMemory only managed a partial read.
			status = STATUS_SUCCESS;
		}
		__except (EXCEPTION_EXECUTE_HANDLER)
		{
			*bytesRead = 0;
			status = GetExceptionCode();
		}
	}

	ExFreePoolWithTag(kernelBuffer, 'DmpD');
	return status;
}