using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reega.Salomone.Navigation
{
    public interface INavigator : INotifyPropertyChanged
    {
        /// <summary>
        /// Build a <see cref="IViewModel"/>
        /// </summary>
        /// <typeparam name="T">Type of the ViewModel</typeparam>
        /// <returns> an instance of the ViewModel</returns>
        T BuildViewModel<T>() where T : IViewModel;

        /// <summary>
        /// Push <paramref name="viewModel"/> to the navigation stack
        /// </summary>
        /// <param name="viewModel">ViewModel that needs to be pushed</param>
        /// <param name="clearNavigationStack">Clear the navigation stack before pushing the ViewModel</param>
        void PushViewModelToStack(IViewModel viewModel, bool clearNavigationStack);

        /// <summary>
        /// Pop the current ViewModel that is in the peek of the stack <see cref="SelectedViewModelProperty"/>
        /// </summary>
        void PopController();

        /// <summary>
        /// Current ViewModel in the scene.
        /// </summary>
        IViewModel SelectedViewModel { get; }

        /// <summary>
        /// State of the navigation stack
        /// </summary>
        bool NavigationStackNotEmpty { get; }
    }
}
