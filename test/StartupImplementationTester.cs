using Ardalis.GuardClauses;
using NUnit.Framework.Interfaces;

namespace Clicksrv.StartWithOSSettings.Tests
{
    public abstract class StartupImplementationTester
    {
        private bool stop = false;
        private IStartupOptions? StartupOptions { get; set; }

        protected void SetImplentation(IStartupOptions startupOptions)
        {
            Guard.Against.Null(startupOptions);
            StartupOptions = startupOptions;
        }

        [SetUp]
        public void SetUp()
        {
            if (stop)
                Assert.Inconclusive("A prior required test failed.");
        }

        [TearDown]
        public void TearDown()
            => stop = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed;

        [Test, Order(1)]
        public void ShouldNotBeCreated()
            => Assert.False(StartupOptions!.Created);

        [Test, Order(2)]
        public void ShouldNotBeEnabledBeforeCreate()
            => Assert.False(StartupOptions!.Enabled);

        [Test, Order(3)]
        public void Create() 
            => Assert.DoesNotThrow(() => StartupOptions!.CreateStartupEntry());

        [Test, Order(4)]
        public void ShouldBeCreated()
            => Assert.True(StartupOptions!.Created);

        [Test, Order(5)]
        public void ShouldBeEnabledOnCreate()
            => Assert.True(StartupOptions!.Enabled);

        [Test, Order(6)]
        public void Disable()
            => Assert.DoesNotThrow(() => StartupOptions!.Disable());

        [Test, Order(7)]
        public void ShouldBeDisabled()
            => Assert.False(StartupOptions!.Enabled);

        [Test, Order(8)]
        public void ShouldNotBeDeletedOnDisable()
            => Assert.True(StartupOptions!.Created);

        [Test, Order(9)]
        public void Enable()
            => Assert.DoesNotThrow(() => StartupOptions!.Enable());

        [Test, Order(10)]
        public void ShouldBeEnabled()
            => Assert.True(StartupOptions!.Enabled);

        [Test, Order(11)]
        public void ShouldNotBeDeletedOnEnable()
            => Assert.True(StartupOptions!.Created);

        [Test, Order(12)]
        public void Delete()
            => Assert.DoesNotThrow(() => StartupOptions!.DeleteStartupEntry());

        [Test, Order(13)]
        public void ShouldNotBeEnabled()
            => Assert.False(StartupOptions!.Enabled);

        [Test, Order(14)]
        public void ShouldNotBeCreatedOnDelete()
            => Assert.False(StartupOptions!.Created);
    }
}