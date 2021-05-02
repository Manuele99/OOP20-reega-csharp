using Reega.Salomone.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Reega.Tests.Salomone
{
    public class ServiceTest
    {
        /// <summary>
        /// Test for checking if registering an interface or an abstract class throws an exception
        /// </summary>
        [Fact]
        public void AbstractOrInterfaceRegistration()
        {
            ServiceCollection svcCollection = new();
            Assert.ThrowsAny<ArgumentException>(() => svcCollection.AddSingleton<IService>());
            Assert.ThrowsAny<ArgumentException>(() => svcCollection.AddSingleton<AbstractService>());
        }


        /// <summary>
        /// Test for checking a registration of a singleton service via an interface and its implementation
        /// </summary>
        [Fact]
        public void SingletonInterfaceImplementation()
        {
            ServiceCollection svcCollection = new();
            svcCollection.AddSingleton<IService, Service>();
            ServiceProvider svcProvider = svcCollection.BuildServiceProvider();
            IService svcInterface = svcProvider.GetService<IService>();
            Assert.NotNull(svcInterface);
            Assert.Equal(typeof(Service), svcInterface.GetType());
            Assert.Equal(svcInterface, svcProvider.GetService<IService>());
        }

        /// <summary>
        /// Test for checking a registration of a singleton service only via its implementation
        /// </summary>
        [Fact]
        public void SingletonImplementation()
        {
            ServiceCollection svcCollection = new();
            svcCollection.AddSingleton<Service>();
            ServiceProvider svcProvider = svcCollection.BuildServiceProvider();
            Service svc = svcProvider.GetService<Service>();
            Assert.NotNull(svc);
            Assert.Equal(svc, svcProvider.GetService<Service>());
        }

        /// <summary>
        /// Test for checking a registration of a transient service via an interface and its implementation
        /// </summary>
        [Fact]
        public void TransientInterfaceImplementation()
        {
            ServiceCollection svcCollection = new();
            svcCollection.AddTransient<IService, Service>();
            ServiceProvider svcProvider = svcCollection.BuildServiceProvider();
            IService svcInterface = svcProvider.GetService<IService>();
            Assert.NotNull(svcInterface);
            Assert.Equal(typeof(Service), svcInterface.GetType());
            Assert.NotEqual(svcInterface, svcProvider.GetService<IService>());
        }

        /// <summary>
        /// Test for checking a registration of a singleton service only via its implementation
        /// </summary>
        [Fact]
        public void TransientImplementation()
        {
            ServiceCollection svcCollection = new();
            svcCollection.AddTransient<Service>();
            ServiceProvider svcProvider = svcCollection.BuildServiceProvider();
            Service svc = svcProvider.GetService<Service>();
            Assert.NotNull(svc);
            Assert.NotEqual(svc, svcProvider.GetService<Service>());
        }

        /// <summary>
        /// Test for checking a registration of a singleton service via an interface and its implementation with an <see cref="Inject"/> constructor
        /// </summary>
        [Fact]
        public void SingletonInjectInterfaceImplementation()
        {
            ServiceCollection svcCollection = new();
            svcCollection.AddSingleton<IService, Service>();
            svcCollection.AddSingleton<ICompositeService, CompositeService>();
            ServiceProvider svcProvider = svcCollection.BuildServiceProvider();
            ICompositeService compositeService = svcProvider.GetService<ICompositeService>();
            Assert.NotNull(compositeService);
            Assert.Equal(typeof(CompositeService), compositeService.GetType());
            Assert.Equal(compositeService, svcProvider.GetService<ICompositeService>());
            IService service = svcProvider.GetService<IService>();
            Assert.Equal(service, compositeService.Service);
        }

        /// <summary>
        /// Test for checking a registration of a singleton service only via its implementation with an <see cref="Inject"/> constructor
        /// </summary>
        [Fact]
        public void SingletonInjectImplementation()
        {
            ServiceCollection svcCollection = new();
            svcCollection.AddSingleton<IService, Service>();
            svcCollection.AddSingleton<CompositeService>();
            ServiceProvider svcProvider = svcCollection.BuildServiceProvider();
            CompositeService compositeService = svcProvider.GetService<CompositeService>();
            Assert.NotNull(compositeService);
            Assert.Equal(typeof(CompositeService), compositeService.GetType());
            Assert.Equal(compositeService, svcProvider.GetService<CompositeService>());
            IService service = svcProvider.GetService<IService>();
            Assert.Equal(service, compositeService.Service);
        }

        /// <summary>
        /// Test for checking a registration of a transient service via an interface and its implementation with an <see cref="Inject"/> constructor
        /// </summary>
        [Fact]
        public void TransientInjectInterfaceImplementation()
        {
            ServiceCollection svcCollection = new();
            svcCollection.AddSingleton<IService, Service>();
            svcCollection.AddTransient<ICompositeService, CompositeService>();
            ServiceProvider svcProvider = svcCollection.BuildServiceProvider();
            ICompositeService compositeService = svcProvider.GetService<ICompositeService>();
            Assert.NotNull(compositeService);
            Assert.Equal(typeof(CompositeService), compositeService.GetType());
            Assert.NotEqual(compositeService, svcProvider.GetService<ICompositeService>());
            IService service = svcProvider.GetService<IService>();
            Assert.Equal(service, compositeService.Service);
        }

        /// <summary>
        /// Test for checking a registration of a singleton service only via its implementation with an <see cref="Inject"/> constructor
        /// </summary>
        [Fact]
        public void TransientInjectImplementation()
        {
            ServiceCollection svcCollection = new();
            svcCollection.AddSingleton<IService, Service>();
            svcCollection.AddTransient<CompositeService>();
            ServiceProvider svcProvider = svcCollection.BuildServiceProvider();
            CompositeService compositeService = svcProvider.GetService<CompositeService>();
            Assert.NotNull(compositeService);
            Assert.Equal(typeof(CompositeService), compositeService.GetType());
            Assert.NotEqual(compositeService, svcProvider.GetService<ICompositeService>());
            IService service = svcProvider.GetService<IService>();
            Assert.Equal(service, compositeService.Service);
        }
    }

    /// <summary>
    /// Simple service interface
    /// </summary>
    internal interface IService { }
    /// <summary>
    /// Simple service class that implements an interface
    /// </summary>
    internal class Service : IService { }
    /// <summary>
    /// Composite service that has a property inside
    /// </summary>
    internal interface ICompositeService
    {
        IService Service { get; }
    }
    /// <summary>
    /// Composite service with <see cref="Inject"/> constructor
    /// </summary>
    internal class CompositeService : ICompositeService
    {
        public IService Service { get; }
        [Inject]
        public CompositeService(IService service)
        {
            Service = service;
        }
        
    }

    /// <summary>
    /// Abstract service
    /// </summary>
    internal abstract class AbstractService { }
}
