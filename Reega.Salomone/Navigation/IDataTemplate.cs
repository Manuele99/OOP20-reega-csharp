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
        /// <summary>
        /// Control factory that maps a data object to its visual representation
        /// </summary>
        Func<object, IReegaView> ControlFactory { get; }
    }


    public interface IDataTemplate<TObject> : IDataTemplate
    {
        /// <summary>
        /// Control factory that maps a <typeparamref name="TObject"/> to its visual representation
        /// </summary>
        new Func<TObject, IReegaView> ControlFactory { get; }
    }
}
