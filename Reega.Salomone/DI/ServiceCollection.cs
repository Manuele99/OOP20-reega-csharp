using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reega.Salomone.DI
{
    public class ServiceCollection
    {
        private readonly Dictionary<Type, object> _singletonDictionary = new();
        private readonly Dictionary<Type, Func<ServiceProvider, object>> _transientDictionary = new();
        private ServiceProvider ServiceProvider { get; set; }
        private bool _svcProviderAlreadyCreated;

        public ServiceCollection()
        {
            this.BuildServiceProvider(false);
        }

        /// <summary>
        /// Adds a singleton with a constant value.
        /// </summary>
        /// <typeparam name="TClass">Type of the class</typeparam>
        /// <param name="value">Singleton value</param>
        public void AddSingleton<TClass>(TClass value)
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));
            this._singletonDictionary.Add(typeof(TClass), value);
        }

        /// <summary>
        /// Adds a singleton with an implementation function.
        /// </summary>
        /// <typeparam name="TClass">Type of the class</typeparam>
        /// <param name="implementationFunction">Function that uses <see cref="ServiceProvider"/>
        /// to build the instance of the singleton</param>
        public void AddSingleton<TClass>(Func<ServiceProvider, TClass> implementationFunction)
        {
            _ = implementationFunction ?? throw new ArgumentNullException(nameof(implementationFunction));
            this._singletonDictionary.Add(typeof(TClass), implementationFunction.Invoke(ServiceProvider));
        }

        /// <summary>
        /// Adds a singleton based on the <see cref="Inject"/> annotated constructor or a no parameter
        /// constructor if no <see cref="Inject"/> annotated constructor is found.
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        public void AddSingleton<TClass>()
        {
            if (IsInterfaceOrAbstractClass<TClass>())
                throw new ArgumentException("The type needs to be a class(not interface) not abstract");

            this._singletonDictionary.Add(typeof(TClass), this.CreateInstance<TClass>());
        }

        /// <summary>
        /// Adds a singleton based on the <see cref="Inject"/> annotated constructor of <typeparamref name="TImplementation"/>
        /// or a no parameter constructor if no <see cref="Inject"/> annotated constructor is found.
        /// </summary>
        /// <typeparam name="TInterface">Interface type</typeparam>
        /// <typeparam name="TImplementation">Implementation type</typeparam>
        public void AddSingleton<TInterface,TImplementation>() where TImplementation : TInterface
        {
            if (!typeof(TInterface).IsInterface)
                throw new ArgumentException($"{typeof(TInterface)} needs to be an interface");
            if (IsInterfaceOrAbstractClass<TImplementation>())
                throw new ArgumentException($"{typeof(TImplementation)} needs to be a class not abstract");

            this._singletonDictionary.Add(typeof(TInterface), this.CreateInstance<TImplementation>());
        }

        /// <summary>
        /// Adds a transient with an implementation function.
        /// </summary>
        /// <typeparam name="TClass">Type of the class</typeparam>
        /// <param name="implementationFunction">Function that uses <see cref="ServiceProvider"/> to build the instance of the transient</param>
        public void AddTransient<TClass>(Func<ServiceProvider, TClass> implementationFunction)
        {
            _ = implementationFunction ?? throw new ArgumentNullException(nameof(implementationFunction));
            this._transientDictionary.Add(typeof(TClass), serviceProvider => implementationFunction.Invoke(serviceProvider));
        }

        /// <summary>
        /// Adds a transient based on the <see cref="Inject"/> annotated constructor or a no parameter
        /// constructor if no <see cref="Inject"/> annotated constructor is found.
        /// </summary>
        /// <typeparam name="TClass">Type of the class</typeparam>
        public void AddTransient<TClass>()
        {
            if (IsInterfaceOrAbstractClass<TClass>())
                throw new ArgumentException("The type needs to be a class(not interface) not abstract");

            this.AddTransient(serviceProvider => this.CreateInstance<TClass>());
        }

        /// <summary>
        /// Adds a transient based on the <see cref="Inject"/> annotated constructor of <typeparamref name="TImplementation"/>
        /// or a no parameter constructor if no <see cref="Inject"/> annotated constructor is found.
        /// </summary>
        /// <typeparam name="TInterface">Interface type</typeparam>
        /// <typeparam name="TImplementation">Implementation type</typeparam>
        public void AddTransient<TInterface, TImplementation>() where TImplementation : TInterface
        {
            if (!typeof(TInterface).IsInterface)
                throw new ArgumentException($"{typeof(TInterface)} needs to be an interface");
            if (IsInterfaceOrAbstractClass<TImplementation>())
                throw new ArgumentException($"{typeof(TImplementation)} needs to be a class not abstract");

            this.AddTransient<TInterface>(serviceProvider => this.CreateInstance<TImplementation>());
        }

        /// <summary>
        /// Get a service.
        /// </summary>
        /// <typeparam name="TService">Type of the service</typeparam>
        /// <returns>Instance of the service, null if hasn't been found</returns>
#nullable enable
        public TService? GetService<TService>()
        {
            Type typeOfT = typeof(TService);
            if (this._singletonDictionary.ContainsKey(typeOfT))
                return (TService?)this._singletonDictionary.GetValueOrDefault(typeOfT);
            else if (this._transientDictionary.ContainsKey(typeOfT))
                return (TService?)this._transientDictionary.GetValueOrDefault(typeOfT)?.Invoke(ServiceProvider);
            return default;
        }

        /// <summary>
        /// <see cref="GetService{T}"/> called with dynamic type with reflection.
        /// </summary>
        /// <param name="type">Type of the service</param>
        /// <returns>The type of the service</returns>
        private object? GetObjectService(Type type)
        {
            return typeof(ServiceCollection)
                .GetMethod(nameof(ServiceCollection.GetService))?
                .MakeGenericMethod(type)
                .Invoke(this, null);
        }

        /// <summary>
        /// Get the constructor and the parameters of the constructor of the class <paramref name="type"/>.
        /// </summary>
        /// <param name="type">Class type</param>
        /// <returns>A Tuple consisting of the constructor and its resolved parameters' implementations</returns>
        private (ConstructorInfo ctor, object?[] paramsImplementations) GetConstructorAndParametersByType(Type type)
        {
            _ = type ?? throw new ArgumentNullException(nameof(type));
            List<ConstructorInfo> injectConstructors = type.GetConstructors().Where(ctor => ctor.CustomAttributes.Any(elem => elem.AttributeType == typeof(Inject))).ToList();
            ConstructorInfo ctor;
            if (injectConstructors.Count == 0)
            {
                ConstructorInfo? ctorInfo = type.GetConstructors().Where(ctor => ctor.GetParameters().Length == 0).FirstOrDefault();
                ctor = ctorInfo ??
                        throw new ArgumentException("You don't have a constructor annotated with the [Inject] attribute, nor a constructor with no parameters"); ;
            }
            else if (injectConstructors.Count != 1)
                throw new ArgumentException("You need to have only one constructor with [Inject] attribute");
            else
                ctor = injectConstructors.First();

            object?[] paramsImplementations = ctor.GetParameters().Select(elem =>
            {
                return this.GetObjectService(elem.ParameterType) ??
                    throw new ArgumentException($"{elem.ParameterType} doesn't have a DI Service");
            }).ToArray();

            return (ctor, paramsImplementations);
        }

        /// <summary>
        /// Create an instance of <typeparamref name="TClass"/>.
        /// </summary>
        /// <typeparam name="TClass">Type of the class</typeparam>
        /// <returns>The instance of the class</returns>
        /// <seealso cref="GetConstructorAndParametersByType(Type)"/>
        private TClass CreateInstance<TClass>()
        {
            (ConstructorInfo ctor, object?[] paramsImplementations) = this.GetConstructorAndParametersByType(typeof(TClass));
            return (TClass)ctor.Invoke(paramsImplementations);
        }
#nullable disable

        /// <summary>
        /// Build the service provider from this service collection.
        /// </summary>
        /// <returns>A Service Provider that manages this services</returns>
        public ServiceProvider BuildServiceProvider()
        {
            return this.BuildServiceProvider(true);
        }

        /// <summary>
        /// Build a service provider.
        /// </summary>
        /// <param name="checkForSecondCall">true if you want to check if the service provider has been already built,
        /// false otherwise</param>
        /// <returns>A Service Provider that manages this services</returns>
        private ServiceProvider BuildServiceProvider(bool checkForSecondCall)
        {
            if (checkForSecondCall)
            {
                if (this._svcProviderAlreadyCreated)
                    throw new InvalidOperationException("Cannot build the service provider twice");

                this._svcProviderAlreadyCreated = true;
            }

            ServiceProvider = new ServiceProvider(this);
            return ServiceProvider;
        }

        /// <summary>
        /// Check if <typeparamref name="T"/> is an interface or an abstract class
        /// </summary>
        /// <typeparam name="T">Type of the class to check</typeparam>
        /// <returns>True if it is an interface or an abstract class, false otherwise</returns>
        private bool IsInterfaceOrAbstractClass<T>()
        {
            Type typeOfT = typeof(T);
            return typeOfT.IsAbstract || typeOfT.IsInterface;
        }
    }
}
