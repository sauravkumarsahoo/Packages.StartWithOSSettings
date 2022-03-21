using Clicksrv.StartWithOSSettings.Windows;

namespace Clicksrv.StartWithOSSettings
{
    public sealed class StartupOptions : IStartupOptions
    {
        private IStartupOptions _options;

        public StartupOptions(string name, string address, string[] arguments, bool global = false)
        {
            _options = Environment.OSVersion.Platform switch
            {
                PlatformID.Win32S => new WindowsStartupOptions(name, address, arguments, global),
                PlatformID.Win32Windows => new WindowsStartupOptions(name, address, arguments, global),
                PlatformID.Win32NT => new WindowsStartupOptions(name, address, arguments, global),
                PlatformID.WinCE => new WindowsStartupOptions(name, address, arguments, global),
                _ => throw new NotImplementedException(),
            };
        }

        public string Name { get => _options.Name; init => throw new InvalidOperationException(); }
        public string Address { get => _options.Address; init => throw new InvalidOperationException(); }
        public string[] Arguments { get => _options.Arguments; init => throw new InvalidOperationException(); }
        public bool Global { get => _options.Global; init => throw new InvalidOperationException(); }

        public bool IsPlatformSupported => _options.IsPlatformSupported;
        public void CheckPlatform() => _options.CheckPlatform();

        public bool Created { get => _options.Created; }
        public void CreateStartupEntry() => _options.CreateStartupEntry();
        public string? GetStartupEntryValue() => _options.GetStartupEntryValue();
        public void DeleteStartupEntry() => _options.DeleteStartupEntry();

        public bool Enabled { get => _options.Enabled; }
        public void Disable() => _options.Disable();
        public void Enable() => _options.Enable();

    }
}