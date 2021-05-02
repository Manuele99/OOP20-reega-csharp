using Reega.Salomone.DI;
using Reega.Salomone.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Reega.Tests.Salomone
{
    public class NavigationTest : IClassFixture<NavigationFixture>
    {
        private NavigationFixture NavigationFixture { get; }

        public NavigationTest(NavigationFixture fixture)
        {
            NavigationFixture = fixture;
        }

        /// <summary>
        /// Push one viewmodel
        /// </summary>
        [Fact]
        public void PushOneViewModel()
        {
            Navigator navigator = new Navigator(NavigationFixture.ServiceProvider);
            FirstTestViewModel vm = navigator.BuildViewModel<FirstTestViewModel>();
            Assert.NotNull(vm);
            navigator.PushViewModelToStack(vm, false);
            Assert.IsType<FirstTestViewModel>(navigator.SelectedViewModel);
            Assert.False(navigator.NavigationStackNotEmpty);
        }

        /// <summary>
        /// Push two viewmodels without clearing the stack
        /// </summary>
        [Fact]
        public void PushTwoViewModels()
        {
            Navigator navigator = new Navigator(NavigationFixture.ServiceProvider);
            FirstTestViewModel vm = navigator.BuildViewModel<FirstTestViewModel>();
            Assert.NotNull(vm);
            navigator.PushViewModelToStack(vm, false);
            Assert.IsType<FirstTestViewModel>(navigator.SelectedViewModel);
            Assert.False(navigator.NavigationStackNotEmpty);
            SecondTestViewModel secondVm = navigator.BuildViewModel<SecondTestViewModel>();
            Assert.NotNull(secondVm);
            navigator.PushViewModelToStack(secondVm, false);
            Assert.IsType<SecondTestViewModel>(navigator.SelectedViewModel);
            Assert.True(navigator.NavigationStackNotEmpty);
        }

        /// <summary>
        /// Push two viewmodels than pop one
        /// </summary>
        [Fact]
        public void PushTwoPopOneViewModel()
        {
            Navigator navigator = new Navigator(NavigationFixture.ServiceProvider);
            FirstTestViewModel vm = navigator.BuildViewModel<FirstTestViewModel>();
            Assert.NotNull(vm);
            navigator.PushViewModelToStack(vm, false);
            Assert.IsType<FirstTestViewModel>(navigator.SelectedViewModel);
            Assert.False(navigator.NavigationStackNotEmpty);
            SecondTestViewModel secondVm = navigator.BuildViewModel<SecondTestViewModel>();
            navigator.PushViewModelToStack(secondVm, false);
            Assert.NotNull(secondVm);
            Assert.IsType<SecondTestViewModel>(navigator.SelectedViewModel);
            Assert.True(navigator.NavigationStackNotEmpty);
            navigator.PopController();
            Assert.IsType<FirstTestViewModel>(navigator.SelectedViewModel);
            Assert.False(navigator.NavigationStackNotEmpty);
        }

        /// <summary>
        /// Push two viewmodels clearing the stack when pushing the second one
        /// </summary>
        [Fact]
        public void PushTwoClearingStack()
        {
            Navigator navigator = new Navigator(NavigationFixture.ServiceProvider);
            FirstTestViewModel vm = navigator.BuildViewModel<FirstTestViewModel>();
            Assert.NotNull(vm);
            navigator.PushViewModelToStack(vm, false);
            Assert.IsType<FirstTestViewModel>(navigator.SelectedViewModel);
            Assert.False(navigator.NavigationStackNotEmpty);
            SecondTestViewModel secondVm = navigator.BuildViewModel<SecondTestViewModel>();
            navigator.PushViewModelToStack(secondVm, true);
            Assert.NotNull(secondVm);
            Assert.IsType<SecondTestViewModel>(navigator.SelectedViewModel);
            Assert.False(navigator.NavigationStackNotEmpty);
            navigator.PopController();
            Assert.IsType<SecondTestViewModel>(navigator.SelectedViewModel);
            Assert.False(navigator.NavigationStackNotEmpty);
        }
    }

    /// <summary>
    /// Fixture for the test class
    /// </summary>
    public class NavigationFixture
    {
        public ServiceProvider ServiceProvider { get; }

        public NavigationFixture()
        {
            ServiceCollection svcCollection = new ServiceCollection();
            svcCollection.AddTransient<FirstTestViewModel>();
            svcCollection.AddTransient<SecondTestViewModel>();
            ServiceProvider = svcCollection.BuildServiceProvider();
        }
    }

    /// <summary>
    /// First viewmodel
    /// </summary>
    internal class FirstTestViewModel : IViewModel
    {
    }

    /// <summary>
    /// Second viewmodel
    /// </summary>
    internal class SecondTestViewModel : IViewModel
    {
    }
}
