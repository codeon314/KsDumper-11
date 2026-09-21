// Intentionally empty.
//
// KernelDriverOperations (see Operations\KernelDriverOperations.cs) is now a
// standalone class in the KsDumper11.Driver namespace.  It opens its own
// handle to \\.\KsDumper via CreateFile and exposes GetKernelModules() /
// DumpKernelModule().  No partial-class extension of KsDumperDriverInterface
// is required (and none is possible, since KsDumperDriverInterface is not
// declared `partial` in its original source file).