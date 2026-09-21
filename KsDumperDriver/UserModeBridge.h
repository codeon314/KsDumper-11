#pragma once
#include <ntddk.h>
#define IO_GET_PROCESS_LIST CTL_CODE(FILE_DEVICE_UNKNOWN, 0x1724, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define IO_COPY_MEMORY CTL_CODE(FILE_DEVICE_UNKNOWN, 0x1725, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define IO_UNLOAD_DRIVER CTL_CODE(FILE_DEVICE_UNKNOWN, 0x1726, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define IO_GET_PROCESS_MODULES CTL_CODE(FILE_DEVICE_UNKNOWN, 0x1727, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
typedef struct _KERNEL_PROCESS_LIST_OPERATION
{
	PVOID bufferAddress;
	INT32 bufferSize;
	INT32 processCount;
} KERNEL_PROCESS_LIST_OPERATION, * PKERNEL_PROCESS_LIST_OPERATION;
typedef struct _KERNEL_COPY_MEMORY_OPERATION
{
	INT32 targetProcessId;
	PVOID targetAddress;
	PVOID bufferAddress;
	INT32 bufferSize;
} KERNEL_COPY_MEMORY_OPERATION, * PKERNEL_COPY_MEMORY_OPERATION;
// --------------------------------------------------------------------------------
// Structures for Module Enumeration
// --------------------------------------------------------------------------------
typedef struct _KERNEL_MODULE_INFO
{
	PVOID BaseAddress;
	ULONG SizeOfImage;
	WCHAR FullPathName[256];
} KERNEL_MODULE_INFO, * PKERNEL_MODULE_INFO;
typedef struct _KERNEL_GET_MODULES_OPERATION
{
	INT32 targetProcessId;
	PVOID bufferAddress; // Pointer to array of KERNEL_MODULE_INFO
	INT32 bufferSize; // Size of the buffer in bytes
	INT32 moduleCount; // Output: Number of modules found
} KERNEL_GET_MODULES_OPERATION, * PKERNEL_GET_MODULES_OPERATION;
// --------------------------------------------------------------------------------
// Structures & IOCTLs for Kernel Driver Enumeration / Dumping
// --------------------------------------------------------------------------------
#define IO_GET_KERNEL_DRIVERS CTL_CODE(FILE_DEVICE_UNKNOWN, 0x1728, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define IO_DUMP_KERNEL_MODULE CTL_CODE(FILE_DEVICE_UNKNOWN, 0x1729, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
typedef struct _KERNEL_DRIVER_INFO
{
	PVOID BaseAddress;
	ULONG SizeOfImage;
	WCHAR FullPathName[256];
} KERNEL_DRIVER_INFO, * PKERNEL_DRIVER_INFO;
typedef struct _KERNEL_GET_DRIVERS_OPERATION
{
	PVOID bufferAddress; // User-mode pointer to an array of KERNEL_DRIVER_INFO
	INT32 bufferSize;    // Size of that buffer in bytes
	INT32 moduleCount;   // Output: number of KERNEL_DRIVER_INFO entries written
} KERNEL_GET_DRIVERS_OPERATION, * PKERNEL_GET_DRIVERS_OPERATION;
typedef struct _KERNEL_DUMP_DRIVER_OPERATION
{
	PVOID baseAddress;   // Input: kernel VA of the driver image to dump
	PVOID bufferAddress; // Input: user-mode destination buffer
	INT32 bufferSize;    // Input: size of destination buffer in bytes
	INT32 bytesRead;     // Output: bytes actually copied
} KERNEL_DUMP_DRIVER_OPERATION, * PKERNEL_DUMP_DRIVER_OPERATION;