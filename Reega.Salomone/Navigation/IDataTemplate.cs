using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reega.Salomone.Navigation
{

    public interface IDataTemplate
    {
        /// <summary>
        /// Get the class of the data object
        /// </summary>
        Type DataObjectClass { get; }
        Func<object, IReegaView> ControlFactory { get; }
    }


    public interface IDataTemplate<TObject> : IDataTemplate
    {
        /// <summary>
        /// Factory for creating a control(<see cref="IReegaView"/>) from a controller(<typeparamref name="TObject"/>)
        /// </summary>
        new Func<TObject, IReegaView> ControlFactory { get; }
    }
}
