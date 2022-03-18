using Microsoft.Win32;

namespace Clicksrv.StartWithOSSettings.Windows
{
    public sealed class WindowsStartupOptions : IStartupOptions
    {
        private const byte EnabledFirstByte = 3;
        private const byte DisabledFirstByte = 2;

        private const string StartupPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string EnableStartupPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";

        public string Name { get; init; }
        public string Address { get; init; }
        public string[] Arguments { get; init; }
        public bool Global { get; init; }

        public WindowsStartupOptions(string name, string address, string[] arguments, bool global = false)
        {
            Name = name;
            Address = address;
            Arguments = arguments;
            Global = global;
        }

        public bool IsPlatformSupported => Environment.OSVersion.Platform == PlatformID.Win32NT;

        public void CheckPlatform()
        {
            if (!IsPlatformSupported)
                throw new PlatformNotSupportedException();
        }

#pragma warning disable CA1416 // Validate platform compatibility

        public RegistryKey StartupKey(bool writable) => (Global ? Registry.LocalMachine : Registry.CurrentUser).OpenSubKey(StartupPath, writable)!;

        public bool Created
        {
            get
            {
                using var startupKey = StartupKey(true)!;
                return AnyKeyMatches(startupKey);
            }
        }

        public void CreateStartupEntry()
        {
            using var startupKey = StartupKey(true)!;
            if (!AnyKeyMatches(startupKey))
                startupKey.SetValue(Name, $"\"{Address}\"{string.Join(" --", Arguments)}");
        }

        public void DeleteStartupEntry()
        {
            using var startupKey = StartupKey(true)!;
            if (AnyKeyMatches(startupKey))
                startupKey.DeleteValue(Name);

            using var enableStartupKey = EnableStartupKey(true)!;
            if (AnyKeyMatches(enableStartupKey))
                enableStartupKey.DeleteValue(Name);
        }


        public RegistryKey EnableStartupKey(bool writable) => (Global ? Registry.LocalMachine : Registry.CurrentUser).OpenSubKey(EnableStartupPath, writable)!;

        public bool Enabled
        {
            get
            {
                using var enableStartupKey = EnableStartupKey(true)!;
                return AnyKeyMatches(enableStartupKey);
            }
        }

        public void Enable()
        {
            CreateStartupEntry();

            using var enableStartupKey = EnableStartupKey(true)!;
            if (!AnyKeyMatches(enableStartupKey))
                enableStartupKey.SetValue(Name, EnabledFirstByte, RegistryValueKind.Binary);
        }

        public void Disable()
        {
            using var enableStartupKey = EnableStartupKey(true)!;
            if (AnyKeyMatches(enableStartupKey))
                enableStartupKey.SetValue(Name, DisabledFirstByte, RegistryValueKind.Binary);
        }


        private bool AnyKeyMatches(RegistryKey startupKey) => startupKey.GetValueNames().Any(x => x.Equals(Name, StringComparison.Ordinal));

#pragma warning restore CA1416 // Validate platform compatibility

    }
}