using System;
using Xwt;

namespace XwtTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			String engineType = "Xwt.GtkBackend.GtkEngine, Xwt.Gtk, Version=1.0.0.0";
			Application.Initialize (engineType);


			MainWindow w = new MainWindow ();
			w.Title = "Xwt Demo Application";
			w.Width = 500;
			w.Height = 400;
			w.Show ();
			
			Application.Run ();
			
			w.Dispose ();
		}
	}
}
