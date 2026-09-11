#include "NTUndocumented.h"
#include "ProcessLister.h"
#include "Utility.h"

static PSYSTEM_PROCESS_INFORMATION GetRawProcessList()
{
	ULONG bufferSize = 0;
	PVOID bufferPtr = NULL;

	if (ZwQuerySystemInformation(SystemProcessInformation, 0, bufferSize, &bufferSize) == STATUS_INFO_LENGTH_MISMATCH)
	{
		bufferPtr = ExAllocatePool(NonPagedPool, bufferSize);

		if (bufferPtr != NULL)
		{
			ZwQuerySystemInformation(SystemProcessInformation, bufferPtr, bufferSize, &bufferSize);
		}
	}
	return (PSYSTEM_PROCESS_INFORMATION)bufferPtr;
}

static ULONG CalculateProcessListOutputSize(PSYSTEM_PROCESS_INFORMATION rawProcessList)
{
	int size = 0;

	while (rawProcessList->NextEntryOffset)
	{
		size += sizeof(PROCESS_SUMMARY);
		rawProcessList = (PSYSTEM_PROCESS_INFORMATION)(((CHAR*)rawProcessList) + rawProcessList->NextEntryOffset);
	}
	return size;
}

static PLDR_DATA_TABLE_ENTRY_PE GetMainModuleDataTableEntry(PPEB64_PE peb)
{
	if (SanitizeUserPointer(peb, sizeof(PEB64_PE)))
	{
		if (peb->Ldr)
		{
			if (SanitizeUserPointer(peb->Ldr, sizeof(PEB_LDR_DATA_PE)))
			{
				if (!peb->Ldr->Initialized)
				{
					int initLoadCount = 0;

					while (!peb->Ldr->Initialized && initLoadCount++ < 4)
					{
						DriverSleep(250);
					}
				}

				if (peb->Ldr->Initialized)
				{
					return CONTAINING_RECORD(peb->Ldr->InLoadOrderModuleList.Flink, LDR_DATA_TABLE_ENTRY_PE, InLoadOrderLinks);
				}
			}
		}
	}
	return NULL;
}

NTSTATUS GetProcessList(PVOID listedProcessBuffer, INT32 bufferSize, PINT32 requiredBufferSize, PINT32 processCount)
{
	PPROCESS_SUMMARY processSummary = (PPROCESS_SUMMARY)listedProcessBuffer;
	PSYSTEM_PROCESS_INFORMATION rawProcessList = GetRawProcessList();
	PVOID listHeadPointer = rawProcessList;
	*processCount = 0;

	if (rawProcessList)
	{
		int expectedBufferSize = CalculateProcessListOutputSize(rawProcessList);

		if (!listedProcessBuffer || bufferSize < expectedBufferSize)
		{
			*requiredBufferSize = expectedBufferSize;
			return STATUS_INFO_LENGTH_MISMATCH;
		}

		while (rawProcessList->NextEntryOffset)
		{
			PEPROCESS targetProcess;
			PKAPC_STATE state = NULL;

			if (NT_SUCCESS(PsLookupProcessByProcessId(rawProcessList->UniqueProcessId, &targetProcess)))
			{
				PVOID mainModuleBase = NULL;
				PVOID mainModuleEntryPoint = NULL;
				UINT32 mainModuleImageSize = 0;
				PWCHAR mainModuleFileName = NULL;
				BOOLEAN isWow64 = 0;

				__try
				{
					KeStackAttachProcess(targetProcess, &state);

					__try
					{
						mainModuleBase = PsGetProcessSectionBaseAddress(targetProcess);

						if (mainModuleBase)
						{
							PPEB64_PE peb = (PPEB64_PE)PsGetProcessPeb(targetProcess);

							if (peb)
							{
								PLDR_DATA_TABLE_ENTRY_PE mainModuleEntry = GetMainModuleDataTableEntry(peb);
								mainModuleEntry = SanitizeUserPointer(mainModuleEntry, sizeof(LDR_DATA_TABLE_ENTRY_PE));

								if (mainModuleEntry)
								{
									mainModuleEntryPoint = mainModuleEntry->EntryPoint;
									mainModuleImageSize = mainModuleEntry->SizeOfImage;
									isWow64 = IS_WOW64_PE(mainModuleBase);

									mainModuleFileName = ExAllocatePool(NonPagedPool, 256 * sizeof(WCHAR));
									RtlZeroMemory(mainModuleFileName, 256 * sizeof(WCHAR));
									RtlCopyMemory(mainModuleFileName, mainModuleEntry->FullDllName.Buffer, 256 * sizeof(WCHAR));
								}
							}
						}
					}
					__except (GetExceptionCode())
					{
						DbgPrintEx(0, 0, "Peb Interaction Failed.\n");
					}
				}
				__finally
				{
					KeUnstackDetachProcess(&state);
				}

				if (mainModuleFileName)
				{
					RtlCopyMemory(processSummary->MainModuleFileName, mainModuleFileName, 256 * sizeof(WCHAR));
					ExFreePool(mainModuleFileName);

					processSummary->ProcessId = (INT32)(ULONG_PTR)rawProcessList->UniqueProcessId;
					processSummary->MainModuleBase = mainModuleBase;
					processSummary->MainModuleEntryPoint = mainModuleEntryPoint;
					processSummary->MainModuleImageSize = mainModuleImageSize;
					processSummary->WOW64 = isWow64;

					processSummary++;
					(*processCount)++;
				}

				ObDereferenceObject(targetProcess);
			}

			rawProcessList = (PSYSTEM_PROCESS_INFORMATION)(((CHAR*)rawProcessList) + rawProcessList->NextEntryOffset);
		}

		ExFreePool(listHeadPointer);
		return STATUS_SUCCESS;
	}
	return STATUS_UNSUCCESSFUL;
}

NTSTATUS GetProcessModules(INT32 targetProcessId, PVOID bufferAddress, INT32 bufferSize, PINT32 moduleCount)
{
	NTSTATUS status = STATUS_SUCCESS;
	PEPROCESS targetProcess;
	KAPC_STATE state;
	PKERNEL_MODULE_INFO tempBuffer = NULL;
	ULONG maxModules = bufferSize / sizeof(KERNEL_MODULE_INFO);
	ULONG foundModules = 0;

	// Sanity check on buffer size to prevent massive allocations
	if (maxModules == 0 || bufferSize > 1024 * 1024 * 10) // 10MB limit
	{
		return STATUS_INVALID_PARAMETER;
	}

	// Allocate temp buffer in kernel space
	tempBuffer = (PKERNEL_MODULE_INFO)ExAllocatePoolWithTag(PagedPool, bufferSize, 'ModL');
	if (!tempBuffer)
	{
		return STATUS_INSUFFICIENT_RESOURCES;
	}
	RtlZeroMemory(tempBuffer, bufferSize);

	status = PsLookupProcessByProcessId((HANDLE)(ULONG_PTR)targetProcessId, &targetProcess);
	if (!NT_SUCCESS(status))
	{
		ExFreePoolWithTag(tempBuffer, 'ModL');
		return status;
	}

	// Attach to process to read memory
	KeStackAttachProcess(targetProcess, &state);

	__try
	{
		PVOID wow64Process = PsGetProcessWow64Process(targetProcess);

		if (wow64Process != NULL)
		{
			// 32-bit Process (WoW64)
			PPEB32 peb32 = (PPEB32)wow64Process;
			if (SanitizeUserPointer(peb32, sizeof(PEB32)))
			{
				PPEB_LDR_DATA32 ldr32 = (PPEB_LDR_DATA32)(ULONG_PTR)peb32->Ldr;
				if (SanitizeUserPointer(ldr32, sizeof(PEB_LDR_DATA32)))
				{
					PLIST_ENTRY32 listHead = &ldr32->InLoadOrderModuleList;
					PLIST_ENTRY32 current = (PLIST_ENTRY32)(ULONG_PTR)listHead->Flink;

					while (current != listHead && foundModules < maxModules)
					{
						PLDR_DATA_TABLE_ENTRY32 entry = CONTAINING_RECORD(current, LDR_DATA_TABLE_ENTRY32, InLoadOrderLinks);

						if (SanitizeUserPointer(entry, sizeof(LDR_DATA_TABLE_ENTRY32)))
						{
							if (entry->DllBase != 0)
							{
								tempBuffer[foundModules].BaseAddress = (PVOID)(ULONG_PTR)entry->DllBase;
								tempBuffer[foundModules].SizeOfImage = entry->SizeOfImage;

								// Handle Strings safely
								if (entry->FullDllName.Buffer != 0 && entry->FullDllName.Length > 0)
								{
									PWCHAR srcName = (PWCHAR)(ULONG_PTR)entry->FullDllName.Buffer;
									if (SanitizeUserPointer(srcName, entry->FullDllName.Length))
									{
										USHORT copyLen = entry->FullDllName.Length;
										if (copyLen > sizeof(tempBuffer[0].FullPathName) - sizeof(WCHAR))
											copyLen = sizeof(tempBuffer[0].FullPathName) - sizeof(WCHAR);

										RtlCopyMemory(tempBuffer[foundModules].FullPathName, srcName, copyLen);
									}
								}
								foundModules++;
							}
						}
						current = (PLIST_ENTRY32)(ULONG_PTR)current->Flink;
						// Basic loop protection
						if (!SanitizeUserPointer(current, sizeof(LIST_ENTRY32))) break;
					}
				}
			}
		}
		else
		{
			// 64-bit Process
			PPEB peb = PsGetProcessPeb(targetProcess);
			if (SanitizeUserPointer(peb, sizeof(PEB)))
			{
				PPEB_LDR_DATA ldr = peb->Ldr;
				if (SanitizeUserPointer(ldr, sizeof(PEB_LDR_DATA)))
				{
					PLIST_ENTRY listHead = &ldr->InLoadOrderModuleList;
					PLIST_ENTRY current = listHead->Flink;

					while (current != listHead && foundModules < maxModules)
					{
						PLDR_DATA_TABLE_ENTRY entry = CONTAINING_RECORD(current, LDR_DATA_TABLE_ENTRY, InLoadOrderLinks);

						if (SanitizeUserPointer(entry, sizeof(LDR_DATA_TABLE_ENTRY)))
						{
							if (entry->DllBase != NULL)
							{
								tempBuffer[foundModules].BaseAddress = entry->DllBase;
								tempBuffer[foundModules].SizeOfImage = entry->SizeOfImage;

								// Handle Strings safely
								if (entry->FullDllName.Buffer != NULL && entry->FullDllName.Length > 0)
								{
									if (SanitizeUserPointer(entry->FullDllName.Buffer, entry->FullDllName.Length))
									{
										USHORT copyLen = entry->FullDllName.Length;
										if (copyLen > sizeof(tempBuffer[0].FullPathName) - sizeof(WCHAR))
											copyLen = sizeof(tempBuffer[0].FullPathName) - sizeof(WCHAR);

										RtlCopyMemory(tempBuffer[foundModules].FullPathName, entry->FullDllName.Buffer, copyLen);
									}
								}
								foundModules++;
							}
						}
						current = current->Flink;
						// Basic loop protection
						if (!SanitizeUserPointer(current, sizeof(LIST_ENTRY))) break;
					}
				}
			}
		}
	}
	__except (EXCEPTION_EXECUTE_HANDLER)
	{
		// Catch access violations during PEB walking
		DbgPrintEx(0, 0, "KsDumper: Exception during PEB walk\n");
		status = STATUS_UNSUCCESSFUL;
	}

	KeUnstackDetachProcess(&state);
	ObDereferenceObject(targetProcess);

	// If successful so far, copy to user buffer
	if (NT_SUCCESS(status))
	{
		__try
		{
			// We assume bufferAddress is valid in the context of the calling process (Client)
			// Since we detached, we are back in Client context.
			// ProbeForWrite is safer but requires logic in Driver.c to be cleaner. 
			// For now, we rely on the try/except block.
			RtlCopyMemory(bufferAddress, tempBuffer, foundModules * sizeof(KERNEL_MODULE_INFO));
			*moduleCount = foundModules;
		}
		__except (EXCEPTION_EXECUTE_HANDLER)
		{
			status = STATUS_INVALID_USER_BUFFER;
		}
	}

	ExFreePoolWithTag(tempBuffer, 'ModL');
	return status;
}