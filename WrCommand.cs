using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace wr
{
	public class WrCommand
	{
		private SupportedFunction function;
		bool has_assignment;
		string[] arguments;
		public SupportedFunction Function {get {return function;}}
		public int ArgCount {get {return arguments.Length;}}
		public bool HasArgs {get {return arguments.Length > 0;}}
		public string this[int i] {get {return arguments[i];}}
		public string[] Arguments {get{return arguments;}}
		public WrCommand(string _input)
		{
			string[] eq_split = _input.Split('=');
			has_assignment = eq_split.Length > 1;
			if (!(Enum.TryParse(eq_split[0], true, out function)))
			{
				Info.Kill(this, "unknown function " + eq_split[0]);
			}
			if (has_assignment)
			{
				arguments = eq_split[1].Split(',');
			}
			else
			{
				arguments = new string[]{};
			}
		}
		public void Summarize()
		{
			Info.WriteLine("root: " + function.ToString().ToLower());
			Info.WriteLine("has_assignment: " + has_assignment);
			Info.WriteLine("arguments:");
			foreach (string i in arguments)
			{
				Info.WriteLine("  " + i);
			}
		}
		public void RequireFilesExistLocally()
		{
			for (int i = 0; i < arguments.Length; i++)
			{
				if (!File.Exists(arguments[i])) Info.Kill(this, "cannot fine file \"" + arguments[i] + "\" locally.");
			}
		}
		public void RequireFilesExistInRepo()
		{
			for (int i = 0; i < arguments.Length; i++)
			{
				if (!File.Exists(Path.Combine(Program.RepoName, arguments[i]))) Info.Kill(this, "cannot fine file \"" + arguments[i] + "\" in rpeo.");
			}
		}
		public void RequireFilesNotExistInRepo()
		{
			for (int i = 0; i < arguments.Length; i++)
			{
				if (File.Exists(Path.Combine(Program.RepoName, arguments[i]))) Info.Kill(this, "file \"" + arguments[i] + "\" already exists in repo.");
			}
		}
		public void RequireFilesNotExistLocally()
		{
			for (int i = 0; i < arguments.Length; i++)
			{
				if (File.Exists(arguments[i])) Info.Kill(this, "file \"" + arguments[i] + "\" already exists locally.");
			}
		}
	}
}