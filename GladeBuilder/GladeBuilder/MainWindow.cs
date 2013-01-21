//
// MainWindow.cs
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

namespace GladeBuilder
{
	/// <summary>
	/// This is the gladebuilder mainwindow
	/// </summary>
	public class MainWindow
	{
		public static void Main (string[] args)
		{
			Application.Init ();

			builder b = new builder();
			b.DeleteEvent += HandleDeleteEvent;
			b.DestroyEvent += HandleDestroyEvent;

			b.ShowAll();

			Application.Run ();
		}

		static void HandleDeleteEvent (object o, DeleteEventArgs args)
		{
			Application.Quit();
		}

		static void HandleDestroyEvent (object o, DestroyEventArgs args)
		{
			Application.Quit();
		}
	}
}

