using GongSolutions.Wpf.DragDrop;
using ProductApp.Helper;
using ProductApp.Properties;
using ProductApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq.Expressions;
using System.IO;
using Microsoft.Win32;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Security;
using Excel = Microsoft.Office.Interop.Excel;

/* Рекомендации 

Рекомендуемое наименование таблиц: "@Name_..."
Рекомендуемое наименование хранимых процедур: "sp_@Name_..."
Рекомендуемое наименование представлений: "v_@Name_..."
Рекомендуемое наименование функций: "fn_@Name_..."

*/

/* Для вставки в файл MainWindowViewModel.cs

В регион Commands:
#region @Name_ (@Title_) 

public ICommand @Name_Command => new RelayCommand(() => @Name_ViewModel.Open(), true);

#endregion

В регион Constructors:
new MenuItemContent 
{ 
	Guid = Guid.Parse("@Guid_"), 
	Title = "@Title_", 
	Command = @Name_Command, 
},

*/

namespace ProductApp.ViewModel
{
	public sealed partial class @Name_ViewModel : BaseViewModel, IDisposable
	{
		#region Members
		
		/// <summary>
		/// Guid модуля @Title_
		/// </summary>		
		public static Guid GuidModule = Guid.Parse("@Guid_");
		
		/// <summary>
		/// Подключение к GZ.dbo 
		/// </summary>
		private GZEntities _db = new GZEntities(DbConnectionString.GZEntity.ConnectionString);

		private readonly Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;
		private readonly TaskScheduler _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

		#endregion

		#region Properties

		#region System

		/// <summary>
		/// проверка доступа для чтения к модулю @Title_
		/// </summary>
		public static bool IsReadable
		{
			get => AuthorityObject.IsModuleReadable(GuidModule);
		}
		/// <summary>
		/// проверка доступа для записи к модулю @Title_
		/// </summary>
		public static bool IsWritable
		{
			get => AuthorityObject.IsModuleWritable(GuidModule);
		}

		/// <summary>
		/// Статус загрузки/обработки контента
		/// </summary>
		private bool _isLoading = true;
		public bool IsLoading
		{
			get => _isLoading;
			set
			{
				if (_isLoading != value)
				{
					_isLoading = value;
					RaisePropertyChanged(nameof(IsLoading));
				}
			}
		}

		/// <summary>
		/// Текст статус загрузки/обработки контента
		/// </summary>
		private string _textLoading = string.Empty;
		public string TextLoading
		{
			get => _textLoading;
			set
			{
				if (_textLoading != value)
				{
					_textLoading = value;
					RaisePropertyChanged(nameof(TextLoading));
					StatusBarReload();
				}
			}
		}

		/// <summary>
		/// Обновление статус бара
		/// </summary>
		public void StatusBarReload()
		{
			RaisePropertyChanged(nameof(StatusBarText));
		}
		/// <summary>
		/// Сообщение в статус баре
		/// </summary>
		public string StatusBarText
		{
			get => $" " +
				$"{(_textLoading != string.Empty ? $"{_textLoading} " : $" ")}" +
				$" ";
		}

		#endregion

		#endregion

		#region Constructors

		public @Name_ViewModel() => _ = InitializeAsync();

		#region Open
		/// <summary>
		/// Метод загрузки модуля
		/// </summary>
		static public void Open()
		{
			try
			{
				if (!App.Current.Windows.Cast<Window>().Any(window => window is @Name_Window))
				{
					Mouse.SetCursor(Cursors.Wait);
					new @Name_Window
					{
						Title = "@Title_",
						DataContext = new @Name_ViewModel {}
					}.Show();
					Mouse.SetCursor(Cursors.Arrow);
				}
				else
				{
					App.Current.Windows.Cast<Window>()
						.Where(window => window is @Name_Window)
						.FirstOrDefault().Close();


					Mouse.SetCursor(Cursors.Wait);
					new @Name_Window
					{
						Title = "@Title_",
						DataContext = new @Name_ViewModel { }
					}.Show();
					Mouse.SetCursor(Cursors.Arrow);
				}
			}
			catch (Exception) { }
		}

		#endregion

		#endregion

		#region Methods
		
		/// <summary>
		/// Переход в режим загрузки с выводом сообщения в статус бар
		/// </summary>
		private void LoadingOn(string msg) 
		{
			TextLoading = msg; 
			IsLoading = true;
		}
		/// <summary>
		/// Выход из режима загрузки
		/// </summary>
		private void LoadingOff() 
		{ 
			IsLoading = false; 
			TextLoading = string.Empty; 
		}

		private void InitializeMain()
		{

		}
		private void InitializeSecond()
		{

			// Обработка интерфейса в асинхронном вызове
			// _dispatcher.BeginInvoke((Action)(() => { /* Code here */ }));
		}
		private async Task InitializeAsync() => await Task.Factory
		#region Initialize Main

			.StartNew(() => LoadingOn("Загрузка модуля…"))
			.ContinueWith(ant => InitializeMain())
			.ContinueWith(ant => LoadingOff())

		#endregion
		#region Initialize Second

			.ContinueWith(ant => LoadingOn("Загрузка компонентов модуля…"))
			.ContinueWith(ant => InitializeSecond())
			.ContinueWith(ant => LoadingOff())

		#endregion
			.ConfigureAwait(false);

		public static void Close()
		{
			App.Current.Windows.Cast<Window>()
			.Where(window => window is @Name_Window)
			.ToList().ForEach(i => i.Close());
		}
		/*
			public static void Close(Параметр проверки)
			{
				App.Current.Windows.Cast<Window>()
				.Where(window => window is @Name_Window && (window.DataContext as @Name_ViewModel).Параметр == Параметр)
				.FirstOrDefault().Close();
			}
		*/

		public static bool IsOpened()
		{
			return App.Current.Windows.Cast<Window>()
			.Any(window => window is @Name_Window);
		}
		/*
			public static bool IsOpened(Параметр проверки)
			{
				return App.Current.Windows.Cast<Window>()
				.Any(window => window is @Name_Window && (window.DataContext as @Name_ViewModel).Параметр == Параметр);
			}
		*/

		#endregion

		
		#region Commands


		#region WindowClose

		private void WindowCloseExecut() 
		{

		}
		private bool CanWindowCloseExecut() => true;
		public ICommand WindowCloseCommand => new RelayCommand(WindowCloseExecut, CanWindowCloseExecut);

		#endregion

		/* Пример асинхронной команды
			
			#region (Async) Exemple

			private void ExempleMain_Execut()
			{

			}
			private void ExempleSecond_Execut()
			{

				// Обработка интерфейса в асинхронном вызове
				// _dispatcher.BeginInvoke((Action)(() => 
				{ 
					// Code here 
				}));
			}
			private async void Exemple_ExecutAsync() => await Task.Factory
			#region Main

				.StartNew(() => LoadingOn("Подготовка к выполнению команды…"))
				.ContinueWith(ant => ExempleMain_Execut())
				.ContinueWith(ant => LoadingOff())

			#endregion
			#region Second

				.ContinueWith(ant => LoadingOn("Выполнение команды…"))
				.ContinueWith(ant => ExempleSecond_Execut())
				.ContinueWith(ant => LoadingOff())

			#endregion
				.ConfigureAwait(false);
			private bool Can_Exemple_Execut() => true ;
			public ICommand Exemple_Command => new RelayCommand(Exemple_ExecutAsync, Can_Exemple_Execut);

			#endregion
		
		*/
		
		/* Пример команды
		
			#region Exemple

			private void Exemple_Execut() 
			{

			}
			private bool Can_Exemple_Execut() => true ;
			public ICommand Exemple_Command => new RelayCommand(Exemple_Execut, Can_Exemple_Execut);

			#endregion
		
		*/

		#endregion

		
		#region IDisposable Support

		// Для определения избыточных вызовов
		private bool disposedValue = false;

		private void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: освободить управляемое состояние (управляемые объекты).
					_db?.Dispose();
					_db = null;
				}

				// TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
				// TODO: задать большим полям значение NULL.

				disposedValue = true;
				GC.Collect();
			}
		}

		// TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
		//~ProductWindowViewModel()
		//{
		//    // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
		//    Dispose(false);
		//}

		// Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
		public void Dispose()
		{
			// Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
			Dispose(true);
			// TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
			GC.SuppressFinalize(this);
		}

		#endregion
	}

}