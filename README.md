# KsDumper-11
https://github.com/user-attachments/assets/7558d492-859a-429b-b51e-285cae623c91

## Critical: Disable Memory Integrity and Vulnerable Driver Blocklist
**KDU (Kernel Driver Utility) relies on loading vulnerable signed drivers to map the KsDumper kernel driver. Modern Windows security features block this behavior by default. You MUST disable the following for KsDumper-11 to work:**

1. **Memory Integrity (HVCI)**  
   - Open Windows Start Menu, type **Core Isolation**, and open it.  
   - Toggle **Memory Integrity** to **Off**.  
   - **Reboot your computer.**  
   - *After the reboot*, you will then be able to disable the Microsoft Vulnerable Driver Blocklist (see step 2).

2. **Microsoft Vulnerable Driver Blocklist**  
   - After rebooting from step 1, open **Core Isolation** again.  
   - Toggle **Microsoft Vulnerable Driver Blocklist** to **Off**.  
   - (Alternative for Windows Pro/Enterprise: Use `gpedit.msc` → Computer Configuration → Administrative Templates → System → Device Guard → "Turn on Virtualization-Based Security" to Disabled, or specifically disable the driver blocklist policy.)

**Without these changes, KDU will fail to load the vulnerable drivers (all providers will fail), and KsDumper-11 will not be able to start its kernel driver.**

## Whats new v1.4.0

<img width="2282" height="1278" alt="image" src="https://github.com/user-attachments/assets/a78cfab8-e1a6-46d6-8dc9-4e44ef70bf69" />
<img width="1304" height="1195" alt="image" src="https://github.com/user-attachments/assets/ad8314cb-5290-4628-bab5-dec275520317" />
<img width="1363" height="997" alt="image" src="https://github.com/user-attachments/assets/4bff7501-1c13-45c1-bb8d-6e9791425bb6" />
<img width="1304" height="1195" alt="image" src="https://github.com/user-attachments/assets/634cd9f6-e485-4933-ba2d-d83c871597dd" />

+ Migrated the entire user-mode stack to .NET 10
    + User-mode application and driver interface now target `net10.0-windows` / .NET 10 (`KsDumper11`, `DriverInterface`)
    + The legacy DarkControls library and the Panel-based custom title bar were dropped in favor of native Windows Forms dark mode (`Application.SetColorMode(SystemColorMode.Dark)`), `ApplicationConfiguration.Initialize()`, and PerMonitorV2 high-DPI (`Application.SetHighDpiMode(HighDpiMode.PerMonitorV2)`)
    + Application Settings were removed; all user settings are now stored in `Settings.json` via `JsonSettingsManager`
+ Added kernel driver enumeration and dumping
    + New "Kernel Drivers" button on the main window opens the new `KernelModulesForm`
    + Loaded kernel modules are listed with name, base address, size, and full path
    + "Dump Driver Code" writes two files from a *single* kernel snapshot:
        + `.bin` — the full runtime image, byte-for-byte (all sections, as-is in kernel memory)
        + `.sys` — a compacted, code-only PE produced by walking the in-memory image's PE headers and rewriting the section table so that only sections flagged `IMAGE_SCN_MEM_EXECUTE` remain
    + "Copy Base Address" context menu option for the selected driver
    + New user-mode bridge class `KernelDriverOperations` opens its own handle to `\\.\KsDumper` and exposes `GetKernelModules()` and `DumpKernelModule()`
+ Added kernel driver support for kernel-module dumping
    + New IOCTL `IO_GET_KERNEL_DRIVERS` (0x1728) — enumerates loaded kernel modules via `ZwQuerySystemInformation(SystemModuleInformation)`
    + New IOCTL `IO_DUMP_KERNEL_MODULE` (0x1729) — copies a kernel image from a kernel VA into a caller-supplied user buffer
    + New driver source files `KernelModuleLister.c` / `KernelModuleLister.h`
    + `DumpKernelDriver` performs `ProbeForWrite` on the user buffer up-front, stages the copy through a NonPagedPool buffer with `MmCopyMemory(MM_COPY_MEMORY_VIRTUAL)`, and then `RtlCopyMemory`s the staged bytes into the user buffer inside a `__try`/`__except`. Partial reads are normalized to `STATUS_SUCCESS` so truncated snapshots are still usable
    + A 64 MB upper bound on requested image size prevents pathological allocations
+ Updated KDU to v1.5.0 (from v1.4.4)
    + 10 new providers added by upstream KDU
    + Provider Selector now displays the new KDU v1.5.0 provider metadata: Advisory, Image Size, File Hash (SHA1), Authenticode Hash (SHA1), Page Hash (SHA1), and Page Hash (SHA256)
    + Provider parsing was rewritten to handle the v1.5.0 list format, including provider names that contain extra commas and CVE IDs
    + KDU self-extraction now compares on-disk file lengths against the embedded resources, forcing re-extraction when bundled KDU binaries change
+ UI / robustness improvements
    + Process list, module list, kernel driver list, and provider list now auto-size columns to fit both the header text and the longest subitem (`Width = -2`), and re-apply the fit on every refresh
    + `ProcessListView` compensates for its bold custom-drawn column header font by enforcing a minimum width on the "Image Size" and "Image Type" columns
    + `ProviderSelector` tolerates non-contiguous provider IDs and malformed provider blocks by resolving the row from `SubItems[0]` instead of using the provider index as a positional ListView index, and by recording unparsable blocks with `ProviderIndex = -1` rather than throwing
    + `KduWrapper.populateProviders` now splits on `Provider #` tokens only when they appear at the start of a line, preventing provider descriptions that contain the phrase from being chopped up
+ Kernel driver additions
    + New IOCTL dispatch for `IO_GET_KERNEL_DRIVERS` and `IO_DUMP_KERNEL_MODULE` in `Driver.c`
    + `ProcessLister.c` continues to walk PEB / WoW64 PEB32 LDR lists with `SanitizeUserPointer` validation at every step, and adds a 10 MB sanity cap on the module buffer size
    + `Utility.c` exposes `DriverSleep` and `SanitizeUserPointer`
+ Project version bumped to 1.4.0

## Whats new v1.3.5
+ Updated KDU to v1.5.0 from v1.4.4 - 10 new Providers! 
    + Provider Selector now displays the new KDU v1.5.0 provider metadata: Advisory, Image Size, File Hash (SHA1), Authenticode Hash (SHA1), Page Hash (SHA1), and Page Hash (SHA256)
    + Improved KDU provider parsing to handle the v1.5.0 provider list format, including provider names that contain extra commas/CVE IDs
    + KDU self-extraction now compares embedded binary lengths against the on-disk files, forcing re-extraction when bundled KDU binaries are updated
+ Added module enumeration and module dumping (updated for .NET 10)
    + New "View Modules" context menu entry on the process list opens `ModuleForm`
    + Module enumeration walks PEB/LDR for both 64-bit and WoW64 processes through the existing `IO_GET_PROCESS_MODULES` IOCTL
    + A selected module can be dumped to a DLL/EXE using the standard `ProcessDumper` pipeline by synthesizing a `ProcessSummary` from the module info
    + `ProcessSummary`'s constructor is public to allow the synthetic module dump path
+ Added IAT reconstruction / import table rebuilding
    + `IATReconstructor` scans readable sections of the dumped image for pointers that resolve into a loaded module, resolves each pointer to an exported symbol name by parsing the corresponding DLL on disk, and synthesizes a brand-new `.idata` section containing a fresh Import Directory Table, Import Lookup Table, Hint/Name entries, and an IAT
    + The synthesized `.idata` section is appended via the new `PEFile.GetNextSectionRva()` / `AddSection()` / `SetDataDirectory()` API, and the IMPORT (1) and IAT (12) data directories are rewritten so the resulting executable can resolve imports more reliably
    + If module enumeration fails, the dump is still produced but may require manual import repair
+ Added kernel driver support for module enumeration
    + New IOCTL: IO_GET_PROCESS_MODULES
    + Kernel module info structure and user-mode bridge structures added
    + ProcessLister now walks PEB/LDR module lists for both 64-bit and WoW64 processes
+ ProcessSummary constructor is now public to support synthetic module dumping
+ Updated project version to 1.3.5

## Whats new v1.3.4
+ Added new feature Anti Anti Debugging Tools Detection
    + Randomized MainWindow Title, most Control Titles, and the exe file name during runtime
    + The process name is reverted to KsDumper11.exe upon program closing
    + Enable Anti Anti Debugging Tools Detection check box setting added
    + This feature was added in hopes to make KsDumper 11 more stealthy when trying to dump programs that have more rudimentary Anti Debugging techniques implemented
+ Lots of source code cleanup
+ Fixed Easter Egg window that would not close upon clicking of the close button
+ Changed all labels in every form to be manually drawn to get around label text being changed when Anti Anti Debugging Tools Detection feature is enabled
+ Migrated from Application Settings to custom Settings.json for saving and loading of settings

## Whats new v1.3.3
+ Updated KDU to v1.4.1
	New providers were added, see KDU patch notes on latest release.

## Whats new v1.3.2
+ Provider selction window now has a button to reset or wipe provider settings.
	This means that all the providers will be reset to needing to be tested, and the default provider will be reset.
+ Fixed a bug in the provider selection window that would prevent it from being closed when opened from the main Dumper window.
![image](https://github.com/user-attachments/assets/c9f3fd50-4438-4b96-beba-e5fbd82108e1)

## Whats new v1.3.1
+ Updated KDU to v1.4.0! Provider count is now 44

## Whats new v1.3
+ Updated KDU to KDU V1.3.4! Over 40 different providers are now available!
+ Removed the old auto detection of working providers and replaced it with a new provider selector. Users can now select which provider they want to use to load the driver. As well as test providers to see if they work on your system!
+ Testing some Providers may BSOD crash the system, KsDumper now has support for being ran again after a crash and will mark the last checked provider as non-working!
+ Anytime kdu loads and it detects a saved providers list, it will try to load the KsDumper driver using the default provider
+ Providers list and selected default provider are now saved as JSON files!
+ Updated to .NET Framework v4.8

![KsDumper v1.3 Provider Selector window](https://github.com/user-attachments/assets/391bcbf0-4255-4c7a-8cd9-3abb08da34f0)

## Whats new v1.2
+ KsDumper will now try and start the driver using the default kdu exploit provider #1 (RTCore64.sys)
+ If the default provider does not work, KsDumper will scan all kdu providers and save each one that works into a list.
+ Anytime kdu loads and it detects a saved providers list, it will try to load the KsDumper driver using each saved provider until one works.
+ This technique should increase the amount of systems that the driver will be able to be loaded on. 

## Support
You can join the official KsDumper 11 discord server where I will be managing ongoing issues. 
For those of you who find that ksDumper won't start on their system, please join the server and post your logs in the support channel. 
Please keep in mind that until others volunteer to help in development of this tool, I am only one person with a finite amount of knowledge. 
https://discord.gg/JqzWNdWBfG

## Features
- Selection of working kdu exploit providers.
- Auto dumping of selected exe.
- Unloading the KsDumper kernel driver is now supported! An option was added to unload on program exit, or system shutdown/restart.
- Splash screen for when driver is being loaded
- Auto Refresh (every 100ms)
- Suspend, resume, kill process
- Dump any process main module using a kernel driver (both x86 and x64)
- Rebuild PE32/PE64 header and sections
- ^ This can be defeated by stripping pe headers. Once pe headers are stripped, it cant dump.
- Works on protected system processes & processes with stripped handles (anti-cheats)
- Works on Windows 11, it doesnt crash anymore!
![Canary Channel Insider Build Win 11 Ksdumper](https://github.com/user-attachments/assets/8c386012-5cbe-43b6-8dc4-8de5e74f48d7)

**Note**: Import table reconstruction is now attempted during dumping when module enumeration succeeds. If module enumeration fails, the dumped file may still require manual import repair.

## Usage
The old way of loading the unsigned ksDumper.sys kernel driver was to use the capcom exploit to map it, this got patched in windows 11.
This one loads the driver with Kernel Driver Utility, or KDU for short. 

Loading of the driver is handled by the Provider Selector now. Simply select a provider from the list, click Test Driver, and if it works, then you can click Set Default provider and it will use the selected provider to load the KsDumper driver with. 

**Note2**: Even though it can dump both x86 & x64 processes, this has to run on x64 Windows.

## Disclaimer
The new kdu provider selector can and WILL crash windows if a bad provider is tested. As such, I have implimented functionality to allow KsDumper to be ran again after a crash, and it will mark the last tested provider as non-working. This way, users will be prevented from testing that provider again and less crashes should result from general usage of KsDumper 11.
Please do beware that it can sometimes crash the OS even still. I do not take any responsibility for any damage that may occur to your system from using this tool.

Due to the nature of how KDU works to map the kernel driver, it is unknown if the system you run this on 
will have a exploitable driver according to kdu providers.
If you try to boot KsDumper 11 and it fails to start the driver, trying again as administrator.
If it still fails post the log. There is a manualloader.bat you can try as well to see the output directly.
You MUST run KsDumper at least once for the kdu.exe file and its dlls to be self extracted for the ManualLoader.bat to work.

This project has been made available for informational and educational purposes only.
Considering the nature of this project, it is highly recommended to run it in a `Virtual Environment`. I am not responsible for any crash or damage that could happen to your system.

**Important**: This tool makes no attempt at hiding itself. If you target protected games, the anti-cheat might flag this as a cheat and ban you after a while. Use a `Virtual Environment` !

## Contributing

Contributions are welcome! If you have suggestions for improvements or encounter any issues, please feel free to open an issue or submit a pull request.

## Donation links

Anything is super helpful! Anything donated helps me keep developing this program and others!
- https://www.paypal.com/paypalme/lifeline42
- https://cash.app/$codoen314

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/codeon314/KsDumper-11/blob/main/LICENSE) file for details. 

## References
- https://github.com/EquiFox/KsDumper
- https://github.com/hfiref0x/KDU
- https://github.com/not-wlan/drvmap
- https://github.com/Zer0Mem0ry/KernelBhop
- https://github.com/NtQuery/Scylla/
- http://terminus.rewolf.pl/terminus/
- https://www.unknowncheats.me/

## Compile Yourself
- Requires **Visual Studio 2026** (the .NET 10 WinForms designer and SDK are required for the user-mode application)
- Requires the **.NET 10 SDK**
- Requires the **.NET 10 Desktop Runtime** to run the compiled application
- Open `KsDumper11.sln` at the repository root; the solution contains:
    - `KsDumper11` — the .NET 10 WinForms user-mode application
    - `DriverInterface` — the .NET 10 driver interface library
    - `KsDumperDriver` — the kernel driver (VS 2019 / 2019 WDK)