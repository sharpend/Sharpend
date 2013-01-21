using System;
using Gtk;
using Sharpend.GtkSharp;

namespace ExifTagger
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
				
			window.window win = new window.window();
			win.DestroyEvent += HandleDestroyEvent;
			win.ShowAll();

			Application.Run();
		}

		static void HandleDestroyEvent (object o, DestroyEventArgs args)
		{
			Application.Quit();
		}
	}
}
