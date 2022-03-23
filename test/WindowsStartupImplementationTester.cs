using NUnit.Framework;

namespace Clicksrv.Packages.StartWithOSSettings.Tests
{
    public class WindowsStartupImplementationTester : StartupImplementationTester
    {
        public WindowsStartupImplementationTester()
        {
            WindowsStartupOptions startupOptions = new(Guid.NewGuid().ToString(), string.Empty, new string[] { "arg1" });
            SetImplentation(startupOptions);
        }
    }
}