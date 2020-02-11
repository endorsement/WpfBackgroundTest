﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly Model model = new Model();
		private readonly ConcurrentQueue<List<string>> cq = new ConcurrentQueue<List<string>>();
		public MainWindow()
		{
			InitializeComponent();
			model.StateChangedEvent += Model_StateChangedEventHandler;
			model.ProgressChangedEvent += Model_ProgressChangedEventHandler;
			model.CompletedEvent += Model_CompletedEventHandler;
		}

		private void Model_CompletedEventHandler(string obj)
		{
			this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
				(Action)(() =>
				{
					TextBox.Text += obj + "\r\n";
				}));
		}

		private void Model_ProgressChangedEventHandler(string obj)
		{
			this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
				(Action)(() =>
				{
					TextBox.Text += obj + "\r\n";
				}));
		}

		private void Model_StateChangedEventHandler(List<string> obj)
		{
			cq.Enqueue(obj);
			this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
				(Action)(() =>
				{
					var vs = new List<string>();
					cq.TryDequeue(out vs);
					TextBox.Text = "";
					foreach (var item in vs)
					{
						TextBox.Text += item + "\r\n";
					}
				}));
		}

		private void StartButton_Click(object sender, RoutedEventArgs e)
		{
			model.Start();
		}

		private void StopButton_Click(object sender, RoutedEventArgs e)
		{
			model.Stop();
		}
	}
}
