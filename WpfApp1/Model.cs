using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace WpfApp1
{
	public class Model
	{
		public event Action<List<string>> StateChangedEvent;
		public event Action<string> ProgressChangedEvent;
		public event Action<string> CompletedEvent;

		private BackgroundWorker worker;
		private List<string> vs = new List<string>();

		public Model()
		{
			worker = new System.ComponentModel.BackgroundWorker();
			worker.DoWork += DoWork;
			worker.RunWorkerCompleted += Completed;
			worker.ProgressChanged += ProgressChanged;
			worker.WorkerSupportsCancellation = true;
			worker.WorkerReportsProgress = true;
		}


		public void Start()
		{
			vs.Clear();
			if (worker.IsBusy != true)
				worker.RunWorkerAsync();
		}

		public void Stop()
		{
			if (worker.WorkerSupportsCancellation == true)
				worker.CancelAsync();
		}

		private void DoWork(object sender, DoWorkEventArgs e)
		{
			for (int i = 0; i < 10; i++)
			{
				if (worker.CancellationPending)
				{
					break;
				}
				Thread.Sleep(500);
				System.Console.WriteLine("DoWork Call Before");
				vs.Add("doworkadd_" + i);
				this?.StateChangedEvent(vs);
				worker.ReportProgress(i * 10);
				System.Console.WriteLine("DoWork Call After");
				Thread.Sleep(100);
			}
		}

		private void ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			string o = "";
			o = "ProgressChanged." + e.ProgressPercentage;
			System.Console.WriteLine(o);
			this?.ProgressChangedEvent(o);
		}

		private void Completed(object sender, RunWorkerCompletedEventArgs e)
		{
			string o = "";
			if (e.Cancelled)
				o = "Canceled.";
			else if (e.Error != null)
				o = "Canceled.";
			else
				o = "Done.";
			System.Console.WriteLine(o);
			this?.CompletedEvent(o);
		}
	}
}
