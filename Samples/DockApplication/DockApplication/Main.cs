//
// Main.cs
//
//  Author:
//       Dirk Lehmeier <sharpend_develop@yahoo.de>
//
//  Copyright (c) 2012 Dirk Lehmeier
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Gtk;
using Sharpend.GtkSharp;

namespace DockApplication
{
	class MainClass
	{
		private static MyDockApplication win;

		public static void Main (string[] args)
		{
			Application.Init ();

			//try to restore the application from "applicationname.xml" in this case DockApplication.xml
			win = MyDockApplication.RestoreInstance<MyDockApplication>();

			if (win == null) //if we cannot restore then create a new one
			{
				win = MyDockApplication.CreateInstance<MyDockApplication>();
			}

			win.Title = "MyDockApplication";
			win.DestroyEvent += HandleDestroyEvent;
			win.DeleteEvent += HandleDeleteEvent;
			win.ShowAll();
			Application.Run ();
		}

		static void HandleDeleteEvent (object o, DeleteEventArgs args)
		{
			win.Save(); //save to "applicationname.xml" in this case DockApplication.xml
			Application.Quit();
		}

		static void HandleDestroyEvent (object o, DestroyEventArgs args)
		{
			win.Save(); //save to "applicationname.xml" in this case DockApplication.xml
			Application.Quit();
		}
	}
}
