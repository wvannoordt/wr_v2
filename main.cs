using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace wr
{
	public static class Program
	{
		private static readonly string REPO_VARNAME = "WR_REPO";
		public static string RepoName {get {return Environment.GetEnvironmentVariable(REPO_VARNAME);}}
		private static bool verbose = false;
		public static int Main(string[] args)
		{
			verbose = args.Length > 0 ? args[0] == "-debug" : false;
			string[] wr_input_args = args;
			try
			{
				string repo = GetInstallDir();
				if (args.Length == 0) Info.Kill("Main", "not enough input arguments given.");
				if (args[0] == "-debug")
				{
					wr_input_args = shift_up<string>(args);
					if (wr_input_args.Length == 0) Info.Kill("Main", "not enough input arguments given.");
				}
				if (verbose) Info.WriteLine("Reading from " + repo);
				WrCommand com = new WrCommand(wr_input_args[0]);
				WrInstance current_instance = new WrInstance(com);
				current_instance.Run();
				if (verbose) com.Summarize();
				if (verbose) Console.WriteLine(System.Environment.OSVersion);
				return 0;
			}
			catch (Exception e)
			{
				string ermsg = e.Message;
				if (ermsg.Contains("registry access")) ermsg += "You may need administrative permissions.";
				error(ermsg);
				return 1;
			}
		}
		//don't use for large arrays.
		private static T[] shift_up<T>(T[] arr)
		{
			T[] arrout = new T[arr.Length - 1];
			for (int i = 1; i < arr.Length; i++)
			{
				arrout[i-1] = arr[i];
			}
			if (verbose) Info.WriteLine("Shifting...");
			return arrout;
		}
		public static string GetInstallDir()
		{
			string repo_dir =  Environment.GetEnvironmentVariable(REPO_VARNAME);
			if (verbose) Info.WriteLine("Attempting retrieval from " + repo_dir);
			bool incompatible = string.IsNullOrEmpty(repo_dir) || !Directory.Exists(repo_dir);
			if (!Directory.Exists(repo_dir) && !string.IsNullOrEmpty(repo_dir))
			{
				Info.Kill(typeof(Program),"repo\"" + repo_dir + "\" could not be found. Consider creating this directory.");
			}				
			if (verbose) Info.WriteLine("incompatible = " + incompatible);
			return incompatible ? RunInstall() : repo_dir;
		}
		public static string RunInstall()
		{
			if (System.Environment.OSVersion.ToString().ToLower().Contains("windows"))
			{
				Info.WriteLine("This appears to be the first time you have run wr. Would you like to use this directory as the repository for wr files? (y/n)");
				bool done = false;
				string install_repo = "";
				for (string input = Info.ReadLine(); !done; input = Info.ReadLine())
				{
					if (input.ToLower() == "y")
					{
						install_repo = Environment.CurrentDirectory;
						done = true;
						break;
					}
					else if (input.ToLower() == "n")
					{
						install_repo = get_repo_input();
						done = true;
						break;
					}
					else
					{
						Info.WriteLine("Please enter \"y\" or \"n\".");
					}
				}
				install_repo = Path.GetFullPath(install_repo);
				Info.WriteLine("Setting install repo to " + install_repo);
				if (verbose) Info.WriteLine("Setting environement variable " + REPO_VARNAME);
				Environment.SetEnvironmentVariable(REPO_VARNAME, install_repo, EnvironmentVariableTarget.User);
				Info.Kill("General", "A system environement variable has been set. Please restart the terminal to use this application.");
				return null;
			}
			else
			{
				Info.WriteLine("It looks like this is running on Linux. Please add the following to your ~/.bashrc:");
				Info.WriteLine();
				Info.WriteLine("export WR_REPO=\"/path/to/reponame\"", "green");
				Info.WriteLine();
				Info.WriteLine("Of course, use the path that you want to.");
				Info.Kill("General", "Add the definition to your .bashrc.");
				return null;
			}
		}
		private static string get_repo_input()
		{
			string install_repo = "";
			bool done = false;
			Info.WriteLine("Enter a valid directory for repository use:");
			for (string input = Info.ReadLine(); !done; input = Info.ReadLine())
			{
				if (Directory.Exists(input))
				{
					install_repo = input;
					done = true;
					break;
				}
				else
				{
					Info.WriteLine("Please enter a valid directory.");
				}
			}
			return install_repo;
		}
		public static void error(string message)
		{
			Info.WriteLine(message + "\nTry \"wr help\".", "red");
		}
	}
}