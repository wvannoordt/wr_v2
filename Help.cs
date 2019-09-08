using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace wr
{
	public static class Help
	{
		public static void Display()
		{
			int functioncount = Enum.GetNames(typeof(SupportedFunction)).Length;
			string[] help_strings = new string[functioncount];
			Info.WriteLine();
			help_strings[(int)(SupportedFunction.HELP)]   = "help                         (display help)";
			help_strings[(int)(SupportedFunction.OPEN)]   = "open                         (open the repository)";
			help_strings[(int)(SupportedFunction.DELETE)] = "delete=file1,file2,...       (delete the specified files)";
			help_strings[(int)(SupportedFunction.PUSH)]   = "push=file1,file2,...         (push the specified files)";
			help_strings[(int)(SupportedFunction.PULL)]   = "open=file1,file2,...         (pull the specified files)";
			help_strings[(int)(SupportedFunction.SHOW)]   = "show[=search1,search2,...]   (show files in the repo)";
			bool all_documented = true;
			foreach (string i in help_strings)
			{
				all_documented = all_documented && !string.IsNullOrEmpty(i);
				Info.WriteLine(i);
			}
			if (!all_documented) Info.Warning("Help", "at least one function is not documented.");
		}
	}
}