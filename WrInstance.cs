using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace wr
{
	public class WrInstance
	{
		private WrCommand command;
		public WrInstance(WrCommand input)
		{
			command = input;
		}
		public void Run()
		{
			switch (command.Function)
			{
				case SupportedFunction.HELP:
				{
					Help.Display();
					break;
				}
				case SupportedFunction.PULL:
				{
					run_pull(command);
					break;
				}
				case SupportedFunction.PUSH:
				{
					run_push(command);
					break;
				}
				case SupportedFunction.SHOW:
				{
					run_show(command);
					break;
				}
				case SupportedFunction.OPEN:
				{
					run_open(command);
					break;
				}
				case SupportedFunction.DELETE:
				{
					run_delete(command);
					break;
				}
			}
		}
		private void run_open(WrCommand _command)
		{
			Process.Start(Program.RepoName);
		}
		private void run_delete(WrCommand _command)
		{
			_command.RequireFilesExistInRepo();
			foreach (string argument in _command.Arguments)
			{
				File.Delete(Path.Combine(Program.RepoName, argument));
			}
		}
		private void run_pull(WrCommand _command)
		{
			_command.RequireFilesNotExistLocally();
			_command.RequireFilesExistInRepo();
			foreach (string argument in _command.Arguments)
			{
				File.Copy(Path.Combine(Program.RepoName,argument), argument);
			}
		}
		private void run_push(WrCommand _command)
		{
			_command.RequireFilesExistLocally();
			_command.RequireFilesNotExistInRepo();
			foreach (string argument in _command.Arguments)
			{
				File.Copy(argument, Path.Combine(Program.RepoName, argument));
			}
		}
		private void run_show(WrCommand _command)
		{
			if (_command.HasArgs)
			{
				List<string> to_display = new List<string>();
				foreach (string argument in _command.Arguments)
				{
					string current_search_pattern = argument;
					string[] files = Directory.GetFiles(Program.RepoName, current_search_pattern);
					foreach (string file in files)
					{
						if (!(to_display.Contains(Path.GetFileName(file)))) to_display.Add(Path.GetFileName(file));
					}
				}
				foreach (string disp in to_display)
				{
					Info.WriteLine(disp);
				}
			}
			else
			{
				string[] allfiles = Directory.GetFiles(Program.RepoName);
				Info.WriteLine();
				foreach (string i in allfiles) Info.WriteLine(Path.GetFileName(i));
			}
		}
	}
}