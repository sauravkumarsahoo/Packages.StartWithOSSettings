namespace Clicksrv.StartWithOSSettings
{
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