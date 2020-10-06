using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace beClean.Views.Base
{
	public class BasePage : ContentPage, IDisposable
	{
		protected BaseVM BaseViewModel => BindingContext as BaseVM;
		//public BasePage()
		//{
		//	this.Title = BaseViewModel.Title;
		//}
		public void Dispose()
		{
			BaseViewModel?.Dispose();
		}
		
		


		protected override void OnAppearing()
		{
			base.OnAppearing();
			Task.Run(async () =>
			{
				await Task.Delay(50); // Allow UI to handle events loop
				if (BaseViewModel != null)
				{
					await BaseViewModel.OnPageAppearing();
				}
			});
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			Task.Run(async () =>
			{
				await Task.Delay(50); // Allow UI to handle events loop
				if (BaseViewModel != null)
					await BaseViewModel.OnPageDisappearing();
			});
		}

	}

	public class BasePage<T> : BasePage where T : BaseVM
	{
		public T ViewModel => BaseViewModel as T;
	}
}
