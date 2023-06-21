// Copyright © 2012 Triamec Motion AG

using System;
using static System.Windows.Forms.Application;

namespace Triamec.Tam.Samples {

	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			EnableVisualStyles();
			SetCompatibleTextRenderingDefault(false);
			Run(new AcquisitionForm());
		}
	}
}
