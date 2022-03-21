# StartWithOSSettings

## About
This package allows the user to configure start-up of an application with the OS. Apart from adding the entry to start-up, this package also supports Enabling or Disabling the Startup entry (as can be configured using Task Manager on Windows).

## Usage
Instantiate `StartupOptions` from `Clicksrv.StartWithOSSettings`. An example piece of code below demonstrates how this package can be used.

```csharp
    var startup = new StartupOptions("<AppName>", "<PathToExecutable>", "<AnyArgumentsToBeSupplied>", global: false);
    startup.CreateStartupEntry();
    startup.Enable();
    startup.Disable();
    startup.DeleteStartupEntry();
```

`StartupOptions` is an auto-platform class that creates the required instance underneath based on the execution platform. User can also use specific classes like `WindowsStartupOptions` to force a particular platform.

### Release Notes

> <b><i>Roadmap</b></i>
> - Test for Admin mode
> - Add Support for Unix Machines
> - Add Support for other Operating Systems supported by .NET.
> - Expand compatibility to older versions of .NET.
> - More to come...

#### 0.2.0
- Fixes for Windows Startup
- Restructure to src and test
- Dev tested for use with User Level Startup, testing for Machine Level pending
- Add Unit Tests
- Included symbols for debugging by projects choosing to debug this package

> <b><i>Known Issues</b></i>
> Admin Mode is currently untested

#### 0.1.x
- Initial Release, buggy, do not use

## Contribution
Create a PR with details regarding what is being added and with what purpose. I'll be happy to review and merge it with the codebase if it suits the requirement.
