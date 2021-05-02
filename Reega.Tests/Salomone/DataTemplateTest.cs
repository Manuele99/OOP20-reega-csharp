using Reega.Salomone.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Reega.Tests.Salomone
{
    public class DataTemplateTest
    {
        /// <summary>
        /// Create one data template
        /// </summary>
        [Fact]
        public void OneDataTemplate()
        {
            DataTemplateManager.Instance.AddTemplate(new SimpleDataTemplate<object>((controller) => new ReegaView()));
            IDataTemplate dt = DataTemplateManager.Instance.GetTemplate<object>();
            Assert.NotNull(dt);
            Assert.Equal(typeof(object), dt.DataObjectClass);
            Assert.IsType<ReegaView>(dt.ControlFactory(new()));
        }

        /// <summary>
        /// Create two data templates
        /// </summary>
        [Fact]
        public void TwoDataTemplates()
        {
            DataTemplateManager.Instance.AddTemplate(new SimpleDataTemplate<object>((controller) => new ReegaView()));
            DataTemplateManager.Instance.AddTemplate(new SimpleDataTemplate<string>((controller) => new ReegaView()));
            IDataTemplate dt = DataTemplateManager.Instance.GetTemplate<object>();
            Assert.NotNull(dt);
            Assert.Equal(typeof(object), dt.DataObjectClass);
            Assert.IsType<ReegaView>(dt.ControlFactory(new()));
            IDataTemplate secondDt = DataTemplateManager.Instance.GetTemplate<string>();
            Assert.NotNull(secondDt);
            Assert.Equal(typeof(string), secondDt.DataObjectClass);
            Assert.IsType<ReegaView>(secondDt.ControlFactory(string.Empty));
        }

        /// <summary>
        /// Create one data templatr and then remove it
        /// </summary>
        [Fact]
        public void RemoveDataTemplate()
        {
            DataTemplateManager.Instance.AddTemplate(new SimpleDataTemplate<object>((controller) => new ReegaView()));
            IDataTemplate dt = DataTemplateManager.Instance.GetTemplate<object>();
            Assert.NotNull(dt);
            Assert.Equal(typeof(object), dt.DataObjectClass);
            Assert.IsType<ReegaView>(dt.ControlFactory(new()));
            DataTemplateManager.Instance.RemoveTemplate<object>();
            Assert.Null(DataTemplateManager.Instance.GetTemplate<object>());
        }
    }

    /// <summary>
    /// Simple <see cref="IDataTemplate{TObject}"/> implementation
    /// </summary>
    /// <typeparam name="TObject">Type of the <see cref="IDataTemplate{TObject}"/></typeparam>
    internal class SimpleDataTemplate<TObject> : IDataTemplate<TObject>
    {
        public Func<TObject, IReegaView> ControlFactory { get; }
        public Type DataObjectClass => typeof(TObject);

        Func<object, IReegaView> IDataTemplate.ControlFactory
        {
            get
            {
                return (controller) => ControlFactory((TObject)controller);
            }
        }

        public SimpleDataTemplate(Func<TObject, IReegaView> controlFactory)
        {
            ControlFactory = controlFactory;
        }
    }

    /// <summary>
    /// Implementation of a <see cref="IReegaView"/>
    /// </summary>
    internal class ReegaView : IReegaView { }
}
