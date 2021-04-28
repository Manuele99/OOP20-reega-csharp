using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reega.Navigation
{

    public interface IDataTemplate
    {
        /// <summary>
        /// Get the class of the data object
        /// </summary>
        /// <returns>Return the class of the data object</returns>
        Type GetDataObjectClass();
        /// <summary>
        /// Get the factory for creating a control based on the <paramref name="controller"/>
        /// </summary>
        /// <param name="controller">Controller to use</param>
        /// <returns>Return a <see cref="IReegaView"/> that is the view representation of the controller</returns>
        Func<IReegaView> GetControlFactory(object controller);
    }


    public interface IDataTemplate<TObject> : IDataTemplate
    {
        /// <summary>
        /// Get the factory for creating a control based on the <paramref name="controller"/>
        /// </summary>
        /// <param name="controller">Controller to use</param>
        /// <returns>Return a <see cref="IReegaView"/> that is the view representation of the controller</returns>
        Func<IReegaView> GetControlFactory(T controller); 
    }
}
