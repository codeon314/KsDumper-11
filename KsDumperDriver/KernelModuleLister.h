#pragma once
#include <ntddk.h>
#include "UserModeBridge.h"

// Enumerates the loaded kernel modules.  Internally this calls
// ZwQuerySystemInformation(SystemModuleInformation), which walks the kernel's
// PsLoadedModuleList and returns one RTL_PROCESS_MODULE_INFORMATION entry per
// driver/loaded image.
//
// On success, `moduleCount` receives the number of KERNEL_DRIVER_INFO entries
// written into `bufferAddress`.  If the caller's buffer is too small, only the
// entries that fit are written and `moduleCount` reflects the truncated count.
NTSTATUS GetKernelModules(
	PVOID bufferAddress,
	INT32 bufferSize,
	PINT32 moduleCount);

// Copies `bufferSize` bytes from the kernel VA `baseAddress` into the caller's
// user-mode buffer using MmCopyMemory(MM_COPY_MEMORY_VIRTUAL).  The number of
// bytes actually transferred is returned in `bytesRead`.
NTSTATUS DumpKernelDriver(
	PVOID baseAddress,
	PVOID userBuffer,
	INT32 bufferSize,
	PINT32 bytesRead);