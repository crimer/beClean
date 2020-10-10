using beClean.Helpers;
using beClean.Views.Base;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace beClean.Services
{
	public class NavigationService
	{
		static readonly Lazy<NavigationService> LazyInstance = new Lazy<NavigationService>(() => new NavigationService(), true);
		public static NavigationService Instance => LazyInstance.Value;

		NavigationService()
		{
			MessagingCenter.Subscribe<MessageBus, BasePage>(this, Consts.NavigationPushMessage, NavigationPushCallback);
			MessagingCenter.Subscribe<MessageBus, BasePage>(this, Consts.NavigationPopMessage, NavigationPopCallback);
		}

		public static Task Init()
		{
			return Instance.Initialize();
		}

		Task Initialize()
		{
			var tks = new TaskCompletionSource<bool>();
			return tks.Task;
		}
		void NavigationPushCallback(MessageBus bus, BasePage basePage)
		{
			var tks = new TaskCompletionSource<bool>();
			if (basePage == null) throw new ArgumentNullException(nameof(basePage));
			Push(basePage, tks);
			System.Diagnostics.Debug.WriteLine("NavigationPushCallback");
		}

		void NavigationPopCallback(MessageBus bus, BasePage basePage)
		{
			var tks = new TaskCompletionSource<bool>();
			if (basePage == null) throw new ArgumentNullException(nameof(basePage));
			Pop(tks);
		}
		void Push(BasePage basePage, TaskCompletionSource<bool> completed)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				try
				{
					await Application.Current.MainPage.Navigation.PushAsync(basePage);
					completed.SetResult(true);
				}
				catch
				{
					completed.SetResult(false);
				}
			});
		}
		void Pop(TaskCompletionSource<bool> completed)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				try
				{
					//await GetTopNavigation().PopAsync();
					completed.SetResult(true);
				}
				catch
				{
					completed.SetResult(false);
				}
			});
		}
	}
}
