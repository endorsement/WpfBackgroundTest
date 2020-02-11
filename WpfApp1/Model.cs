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
			int count = 100;
			for (int i = 0; i < count; i++)
			{
				if (worker.CancellationPending)
				{
					break;
				}
				Thread.Sleep(5);
				System.Console.WriteLine("DoWork Call Before");
				vs.Add("doworkadd_" + i);
				var clone = new List<string>();
				clone.AddRange(vs);
				this?.StateChangedEvent(clone);
				worker.ReportProgress(i * 100 / count);
				System.Console.WriteLine("DoWork Call After");
				Thread.Sleep(1);
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
