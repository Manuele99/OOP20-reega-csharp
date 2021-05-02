using Reega.Salomone.DI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Reega.Salomone.Navigation
{
    public class Navigator : INavigator
    {
        private IViewModel _selectedViewModel;

        public IViewModel SelectedViewModel
        {
            get => _selectedViewModel;
            private set
            {
                _selectedViewModel = value;
                OnPropertyChanged();
            }
        }

        private bool _navigationStackNotEmpty;

        public bool NavigationStackNotEmpty
        {
            get => _navigationStackNotEmpty;
            set
            {
                _navigationStackNotEmpty = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly Stack<IViewModel> _navigationStack = new();
        private readonly ServiceProvider _serviceProvider;

        public Navigator(ServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public T BuildViewModel<T>() where T : IViewModel =>
            this._serviceProvider.GetService<T>() ?? throw new InvalidOperationException();

        public void PopController()
        {
            if (NavigationStackNotEmpty)
            {
                this._navigationStack.Pop();
                this.NavigationStackNotEmpty = this._navigationStack.Count > 1;
                this.SelectedViewModel = this._navigationStack.Peek();
            }
        }

        public void PushViewModelToStack(IViewModel viewModel, bool clearNavigationStack)
        {
            if (clearNavigationStack)
                this._navigationStack.Clear();

            this._navigationStack.Push(viewModel);
            this.NavigationStackNotEmpty = this._navigationStack.Count > 1;
            this.SelectedViewModel = viewModel;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
