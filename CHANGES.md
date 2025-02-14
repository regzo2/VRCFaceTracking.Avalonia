# Changes

While VRCFaceTracking.Avalonia strives to be as similar to the WinUI3 version of VRCFaceTracking both internally and externally, a number of key changes were made to the source code of VRCFaceTracking. This document aims to cover those changes.

## Path Separation

This was by far the most common issue porting VRCFaceTracking.Core to macOS/Linux.

### AvatarConfigFileParser.cs (L24):

Before:
```csharp
foreach (var avatarFile in Directory.GetFiles(userFolder + "\\Avatars"))
```

After:
```csharp
foreach (var avatarFile in Directory.GetFiles(Path.Combine(userFolder, "Avatars")))
```

### Redirectors.cs (L34), Utils.cs (L34)

Before:
```csharp
public static readonly string CustomLibsDirectory = PersistentDataDirectory + "\\CustomLibs";
```

After:
```csharp
public static readonly string CustomLibsDirectory = Path.Combine(PersistentDataDirectory, "CustomLibs");
```

## Windows Exclusive Functions

In `MainStandalone.cs L40` `Utils.TimeEndPeriod()` is Windows exclusive. Subsequent calls should check the OS before running.

In `ModuleInstaller.cs L68` `RemoveZoneIdentifier()` applies only to files hosted on the Windows OS. This should also check the OS before running.

## fti_osc

VRCFaceTracking uses a rust library `fti_osc` to process OSC messages. This library was compiled for macOS and Linux without issue, but proper marshalling needed to be included in the interop file.

Firstly, we need UTF-8 string formatting for marshalled strings on macOS and Linux. We can use `LPUTF8Str`, and an OS preprocessor directive is used to determine which `UnmanagedType` should be used.

```csharp
#if WINDOWS_DEBUG || WINDOWS_RELEASE
    [MarshalAs(UnmanagedType.LPStr)]
#else
    [MarshalAs(UnmanagedType.LPUTF8Str)]
#endif
    public string some_string_name;
```

Secondly, the `fti_osc` class received a number of OS preprocessor directives to determine which library it should load.

```csharp
    public static class fti_osc
    {
#if WINDOWS_DEBUG || WINDOWS_RELEASE
    private const string DllName = "fti_osc.dll";
#elif macOS_DEBUG || macOS_RELEASE
    private const string DllName = "fti_osc.dylib";
#elif LINUX_DEBUG || LINUX_RELEASE
    private const string DllName = "fti_osc.so";
#endif

        /// <summary>
        /// Parses a byte buffer of specified length into a single pointer to an osc message
        /// </summary>
        /// <param name="buffer">The target byte buffer to parse osc from</param>
        /// <param name="bufferLength">The length of <paramref name="buffer"/></param>
        /// <param name="byteIndex">The index of the first byte of the message. This is modified after a message is parsed
        /// This way we can sequentially read messages by passing in the value this int was last modified to be</param>
        /// <returns>Pointer to an OscMessageMeta struct</returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr parse_osc(byte[] buffer, int bufferLength, ref int byteIndex);

        /// <summary>
        /// Serializes a pointer to an OscMessageMeta struct into a 4096 length byte buffer
        /// </summary>
        /// <param name="buf">Target write buffer</param>
        /// <param name="osc_template">Target OscMessageMeta to serialize</param>
        /// <returns>Amount of bytes written to <paramref name="buf"/></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int create_osc_message([MarshalAs(UnmanagedType.LPArray, SizeConst = 4096)] byte[] buf, ref OscMessageMeta osc_template);

        /// <summary>
        /// Serializes a pointer to an array of OscMessageMeta structs to a byte array of size 4096
        /// </summary>
        /// <param name="buf">Target byte array</param>
        /// <param name="messages">Array of messages to be contained within the bundle</param>
        /// <param name="len">Length of <paramref name="messages"/></param>
        /// <param name="messageIndex">Index of the last message written to <paramref name="buf"/> before it was filled</param>
        /// <returns></returns>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int create_osc_bundle(
            [MarshalAs(UnmanagedType.LPArray, SizeConst = 4096)] byte[] buf,
            [MarshalAs(UnmanagedType.LPArray)] OscMessageMeta[] messages,
            int len,
            ref int messageIndex);

        /// <summary>
        /// Free memory allocated to OscMessageMeta by fti_osc lib
        /// </summary>
        /// <param name="oscMessage">Target message pointer</param>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void free_osc_message(IntPtr oscMessage);
}
```

## Misc

### VRChat.VRCData

Logic was added to get the user's VRChat OSC folder on macOS/Linux machines:

Before:

```csharp
public static readonly string VRCData = Path.Combine($"{Environment.GetEnvironmentVariable("localappdata")}Low", "VRChat\\VRChat");
```

After:

```csharp
private static string VRCData
    {
        get
        {
#if WINDOWS_DEBUG || WINDOWS_RELEASE
            // On Windows, VRChat's OSC folder is under %appdata%/LocalLow/VRChat/VRChat
            return Path.Combine(
                $"{Environment.GetEnvironmentVariable("localappdata")}Low",
                "VRChat", "VRChat"
            );
#else
            /* On Linux, things are a little different. The above points to a non-existent folder
             * Thankfully, we can make some assumptions based on the fact VRChat on Linux runs through Proton
             * For reference, here is what a target path looks like:
             * /home/USER_NAME/.steam/steam/steamapps/compatdata/438100/pfx/drive_c/users/steamuser/AppData/LocalLow/VRChat/VRChat/OSC/
             * Where 438100 is VRChat's Steam GameID, and the path after "steam" is pretty much fixed */

            // 1) First, get the user profile folder
            // (/home/USER_NAME/)
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            // 2) Then, search for common Steam install paths
            // (/home/USER_NAME/.steam/steam/)
            string[] possiblePaths =
            {
                Path.Combine(home, ".steam", "steam"),
                Path.Combine(home, ".local", "share", "Steam"),
                Path.Combine(home, ".var", "app", "com.valvesoftware.Steam", ".local", "share", "Steam")
            };
            string steamPath = Array.Find(possiblePaths, Directory.Exists) ?? string.Empty;

            // 3) Finally, append the fixed path to find the OSC folder.
            return string.IsNullOrEmpty(steamPath) ?
                throw new DirectoryNotFoundException("Could not detect Steam install!") :
                Path.Combine(steamPath, "steamapps", "compatdata", "438100", "pfx", "drive_c", "users", "steamuser", "AppData", "LocalLow", "VRChat", "VRChat");
#endif
        }
    }
```

### Validator:

Uses of `Validator.TryValidateObject` in `OscRecvService.cs` and `OscSendService.cs` crashes on macOS/Linux.

### Modules

Not all modules inherently work on Linux. Some are dependent on Windows-exclusive runtimes/programs (IE, the SRanipal module, the Varjo module, etc.), and some modules need to be re-compiled to not target a specific platform (my own VRCFT-Babble module, for instance).

So far, the Babble module and ALVR module have been used and tested on Linux (Debian/Ubuntu/Arch), the latter with the Quest Pro.

Finally, going forward a supported platform field ought to be included with module manifests, so users cannot be given the option to install modules that are incompatible with their operating system.
