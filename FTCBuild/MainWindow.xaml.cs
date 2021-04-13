using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Medallion.Shell;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FTCBuild
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainWindow : Window
	{
		public ObservableCollection<string> OutputLines { get; set; }
		public bool IsBuildEnabled { get
			{
				return true;
			} }
		public string CurStdout { 
			get {
				if(OutputLines.Count == 0)
				{
					return "No output yet";
				}
				return string.Join('\n', OutputLines);
			}
		}
		public MainWindow()
		{
			OutputLines = new();
			this.InitializeComponent();
			Output.ItemsSource = OutputLines;
			OutputLines.CollectionChanged += OutputLines_CollectionChanged;
			Title = "FTCBuild";
		}

		private void OutputLines_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			Output.ItemsSource = OutputLines;
		}

		private async Task<bool> RunGradleTask(string name)
		{
			TaskDisplay.Text = "Running " + name + Variant.SelectedValue;
			var cmd = Command.Run(Location.Text + "\\gradlew.bat", new string[] { name + Variant.SelectedValue }, options => options.WorkingDirectory(Location.Text).DisposeOnExit());
			cmd.StandardOutput.PipeToAsync(OutputLines);
			var result = await cmd.Task;
			return result.Success;
		}
		private async void Build_Click(object sender, RoutedEventArgs e)
		{
			contentDialog.CloseButtonText = "";
			PleaseWait.Text = "Please wait for the task to finish...";
			contentDialog.ShowAsync();
			ProgressIndicator.IsActive = true;
			OutputLines.Add("------------------");
			OutputLines.Add("Started: Task Build, Variant " + Variant.SelectedValue);
			if (!await RunGradleTask("assemble"))
			{
				OutputLines.Add("Build Failed");
				TaskDisplay.Text = "Build Failed";
			} else
			{
				OutputLines.Add("Build Succeeded");
				TaskDisplay.Text = "Build Succeeded";
			}
			PleaseWait.Text = "Task finished.";
			contentDialog.CloseButtonText = "Close";
			ProgressIndicator.IsActive = false;
		}

		private async void Run_Click(object sender, RoutedEventArgs e)
		{
			contentDialog.CloseButtonText = "";
			PleaseWait.Text = "Please wait for the task to finish...";
			contentDialog.ShowAsync();
			ProgressIndicator.IsActive = true;
			OutputLines.Add("------------------");
			OutputLines.Add("Started: Task Install, Variant " + Variant.SelectedValue);
			if (!await RunGradleTask("install"))
			{
				OutputLines.Add("Failed");
				TaskDisplay.Text = "Build Failed";
			}
			else
			{
				OutputLines.Add("Succeeded");
				TaskDisplay.Text = "Build Succeeded";
			}
			TaskDisplay.Text = "Launching on Control Hub...";
			OutputLines.Add("Launching...");
			var cmd = Command.Run(ASDKLoc.Text + "\\adb.exe", new string[] { "shell", "monkey", "-p", "com.qualcomm.ftcrobotcontroller", "1" }, options => options.DisposeOnExit());
			cmd.StandardOutput.PipeToAsync(OutputLines);
			await cmd.Task;
			PleaseWait.Text = "Task finished.";
			contentDialog.CloseButtonText = "Close";
			ProgressIndicator.IsActive = false;
			OutputLines.Add("Note: This tool cannot yet start Logcat.");
			OutputLines.Add("See madelson/MedallionShell#86 for more info.");
			//await Command.Run(@"C:\Windows\System32\conhost.exe", new string[] { ASDKLoc.Text + "\\adb.exe", "logcat" }, options => options.DisposeOnExit()).Task;
			OutputLines.Add("Finished!");
		}

		private async void Install_Click(object sender, RoutedEventArgs e)
		{
			contentDialog.CloseButtonText = "";
			PleaseWait.Text = "Please wait for the task to finish...";
			contentDialog.ShowAsync();
			ProgressIndicator.IsActive = true;
			OutputLines.Add("------------------");
			OutputLines.Add("Started: Task Install, Variant " + Variant.SelectedValue);
			if (!await RunGradleTask("install"))
			{
				OutputLines.Add("Build Failed");
				TaskDisplay.Text = "Build Failed";
			}
			else
			{
				OutputLines.Add("Build Succeeded");
				TaskDisplay.Text = "Build Succeeded";
			}
			PleaseWait.Text = "Task finished.";
			contentDialog.CloseButtonText = "Close";
			ProgressIndicator.IsActive = false;
		}
	}
}
