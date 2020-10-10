using beClean.Helpers;
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
        readonly ConcurrentDictionary<string, ICommand> _cachedCommands = new ConcurrentDictionary<string, ICommand>();
        public bool IsBusy
        {
            get => Get<bool>() || false;
            set => Set(value);
        }
        public bool IsDispose
        {
            get => Get<bool>();
            set => Set(value);
        }

        public string Title
        {
            get => Get<string>();
            set => Set(value);
        }
        public BaseVM(string title, bool isDispose = true)
        {
            Title = title;
            IsDispose = isDispose;
        }


        public virtual Task OnPageAppearing()
        {
            return Task.FromResult(0);
        }

        public virtual Task OnPageDisappearing()
        {
            return Task.FromResult(0);
        }

        #region Commands

        protected ICommand MakeCommand<T>(Action<T> commandAction, [CallerMemberName] string propertyName = null)
        {
            return GetCommand(propertyName) ?? SaveCommand(new Command<T>(commandAction), propertyName);
        }

        protected ICommand MakeCommand(Action commandAction, [CallerMemberName] string propertyName = null)
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
        #endregion

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
