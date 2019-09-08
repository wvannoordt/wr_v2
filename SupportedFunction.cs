using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace wr
{
	public enum SupportedFunction
	{
		HELP,
		PUSH,
		PULL,
		OPEN,
		DELETE,
		SHOW
	}
}