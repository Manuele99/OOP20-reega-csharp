using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Reega.Salomone.DI;

namespace Reega.Salomone.Navigation
{
    public class Navigator : INavigator
    {
        private readonly Stack<IViewModel> _navigationStack = new();
        private readonly ServiceProvider _serviceProvider;

        private bool _navigationStackNotEmpty;
        private IViewModel _selectedViewModel;

        public Navigator(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IViewModel SelectedViewModel
        {
            get => _selectedViewModel;
            private set
            {
                _selectedViewModel = value;
                OnPropertyChanged();
            }
        }

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

        public T BuildViewModel<T>() where T : IViewModel
        {
            return _serviceProvider.GetService<T>() ?? throw new InvalidOperationException();
        }

        public void PopController()
        {
            if (NavigationStackNotEmpty)
            {
                _navigationStack.Pop();
                NavigationStackNotEmpty = _navigationStack.Count > 1;
                SelectedViewModel = _navigationStack.Peek();
            }
        }

        public void PushViewModelToStack(IViewModel viewModel, bool clearNavigationStack)
        {
            if (clearNavigationStack)
                _navigationStack.Clear();

            _navigationStack.Push(viewModel);
            NavigationStackNotEmpty = _navigationStack.Count > 1;
            SelectedViewModel = viewModel;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
