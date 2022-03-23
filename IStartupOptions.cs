namespace Clicksrv.StartWithOSSettings
{
    [Obsolete("Use package 'Clicksrv.Packages.StartWithOSSettings' instead, current namespace will be removed from v0.4.0.")]
    public interface IStartupOptions
    {
        bool IsPlatformSupported { get; }
        void CheckPlatform();

        string Name { get; init; }
        string Address { get; init; }
        string[] Arguments { get; init; }
        bool Global { get; init; }

        bool Created { get; }
        void CreateStartupEntry();
        string? GetStartupEntryValue();
        void DeleteStartupEntry();

        bool Enabled { get; }
        void Enable();
        void Disable();
        string[] GetSavedArguments();
        string? GetSavedAddress();
    }
}