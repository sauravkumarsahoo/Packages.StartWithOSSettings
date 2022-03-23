using Microsoft.Win32;
#pragma warning disable CA1416 // Validate platform compatibility
namespace Clicksrv.Packages.StartWithOSSettings
{
    internal static class RegistryKeyExtensions
    {
        internal static bool HasValueWithName(this RegistryKey startupKey, string name)
            => startupKey.GetValueNames().Any(x => x.Equals(name, StringComparison.Ordinal));

        internal static bool DoesntHaveName(this RegistryKey startupKey, string name)
            => !startupKey.HasValueWithName(name);

        internal static byte[] GetBinaryValue(this RegistryKey startupKey, string name)
            => (byte[]) startupKey!.GetValue(name)!;
    }

#pragma warning restore CA1416 // Validate platform compatibility

}