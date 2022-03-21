using Microsoft.Win32;
using System.Text;
using System.Text.RegularExpressions;

namespace Clicksrv.StartWithOSSettings.Windows
{
#pragma warning disable CA1416 // Validate platform compatibility

    public class WindowsStartupOptions : IStartupOptions
    {
        private const byte EnabledFirstByte = 2;
        private const byte DisabledFirstByte = 3;

        private static readonly byte[] EnabledDefaultValue = new byte[] { EnabledFirstByte, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private static readonly byte[] DisabledDefaultValue = new byte[] { DisabledFirstByte, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private static readonly Regex argRegex = new(@"--(\b[a-zA-Z0-9=]*)\b");

        private const string StartupPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string EnableStartupPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";
        private const string ArgPrefix = " --";

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

        public string? GetSavedAddress()
        {
            using var key = StartupKey(false);
            var value = (string?)key.GetValue(Name);

            if (string.IsNullOrWhiteSpace(value))
                return null;

            var lastColon = value!.IndexOf('"', 1);

            return lastColon < 0 
                ? value!.Substring(1, value!.IndexOf("exe", 0) + 3) 
                : value!.Substring(1, lastColon - 1);
        }

        public string[] GetSavedArguments()
        {
            using var key = StartupKey(false);
            var value = (string?)key.GetValue(Name);

            if (string.IsNullOrWhiteSpace(value))
                return Array.Empty<string>();

            var matches = argRegex.Matches(value);
            if (matches.Count == 0)
                return Array.Empty<string>();

            return matches.Select(x=> x.Groups[1].Value).ToArray();
        }

        public bool IsPlatformSupported
            => Environment.OSVersion.Platform == PlatformID.Win32NT;

        public void CheckPlatform()
        {
            if (!IsPlatformSupported)
                throw new PlatformNotSupportedException();
        }

        private RegistryKey StartupKey(bool writable)
            => (Global ? Registry.LocalMachine : Registry.CurrentUser).OpenSubKey(StartupPath, writable)!;
        private RegistryKey EnableStartupKey(bool writable)
            => (Global ? Registry.LocalMachine : Registry.CurrentUser).OpenSubKey(EnableStartupPath, writable)!;

        public bool Created
            => StartupKey(false).HasValueWithName(Name);

        public bool Enabled
        {
            get
            {
                var registryKey = EnableStartupKey(false);
                return Created && registryKey.HasValueWithName(Name) && registryKey.GetBinaryValue(Name).First() == EnabledFirstByte;
            }
        }

        public void CreateStartupEntry()
        {
            using var startupKey = StartupKey(true)!;
            using var enableStartupKey = EnableStartupKey(true)!;

            if (startupKey.DoesntHaveName(Name))
                startupKey.SetValue(Name, $"\"{Address}\"{(Arguments.Any() ? ArgPrefix : string.Empty)}{string.Join(ArgPrefix, Arguments)}");

            if (enableStartupKey.DoesntHaveName(Name))
                enableStartupKey.SetValue(Name, EnabledDefaultValue, RegistryValueKind.Binary);
        }

        public string? GetStartupEntryValue()
        {
            using var startupKey = StartupKey(true)!;
            return (string?)startupKey.GetValue(Name);
        }

        public void DeleteStartupEntry()
        {
            using var startupKey = StartupKey(true)!;
            if (startupKey.HasValueWithName(Name))
                startupKey.DeleteValue(Name);

            using var enableStartupKey = EnableStartupKey(true)!;
            if (enableStartupKey.HasValueWithName(Name))
                enableStartupKey.DeleteValue(Name);
        }

        public void Enable()
        {
            CreateStartupEntry();

            using var enableStartupKey = EnableStartupKey(true)!;

            if (enableStartupKey.HasValueWithName(Name))
            {
                var value = enableStartupKey!.GetBinaryValue(Name)!;
                value.SetValue(EnabledFirstByte, 0);
                enableStartupKey.SetValue(Name, value, RegistryValueKind.Binary);
            }
            else
            {
                enableStartupKey.SetValue(Name, EnabledDefaultValue, RegistryValueKind.Binary);
            }
        }

        public void Disable()
        {
            using var enableStartupKey = EnableStartupKey(true)!;

            if (enableStartupKey.HasValueWithName(Name))
            {
                var value = enableStartupKey!.GetBinaryValue(Name)!;
                value.SetValue(DisabledFirstByte, 0);
                enableStartupKey.SetValue(Name, value, RegistryValueKind.Binary);
            }
            else
            {
                enableStartupKey.SetValue(Name, DisabledDefaultValue, RegistryValueKind.Binary);
            }
        }
    }

#pragma warning restore CA1416 // Validate platform compatibility

}