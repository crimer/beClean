using beClean.Helpers;
using beClean.Models;
using beClean.Services;
using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace beClean.Views.Base
{
    public class BaseVM : Bindable, IDisposable
    {
        //public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        readonly ConcurrentDictionary<string, ICommand> _cachedCommands = new ConcurrentDictionary<string, ICommand>();
        //public bool IsConnected => !CrossConnectivity.IsSupported || CrossConnectivity.IsSupported && CrossConnectivity.Current.IsConnected;
        public bool IsBusy
        {
            get => Get<bool>() || false;
            set => Set(value);
        }

        public string Title
        {
            get => Get<string>();
            set => Set(value);
        }
        public BaseVM(string title)
        {
            Title = title;
        }
        public virtual Task OnPageAppearing()
        {
            return Task.FromResult(0);
        }

        public virtual Task OnPageDisappearing()
        {
            return Task.FromResult(0);
        }
        protected ICommand MakeCommand(Action commandAction, [CallerMemberName] string propertyName = null)
        {
            return GetCommand(propertyName) ?? SaveCommand(new Command(commandAction), propertyName);
        }

        protected ICommand MakeCommand(Action<object> commandAction, [CallerMemberName] string propertyName = null)
        {
            return GetCommand(propertyName) ?? SaveCommand(new Command(commandAction), propertyName);
        }
        ICommand SaveCommand(ICommand command, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            if (!_cachedCommands.ContainsKey(propertyName))
                _cachedCommands.TryAdd(propertyName, command);

            return command;
        }

        ICommand GetCommand(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            return _cachedCommands.TryGetValue(propertyName, out var cachedCommand)
                ? cachedCommand
                : null;
        }

        public void Dispose()
        {
            //Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BaseVM()
        {
            Dispose();
        }

        //protected virtual void Dispose(bool disposing)
        //{
        //    ClearDialogs();
        //    CancelNetworkRequests();
        //}
    }
}
