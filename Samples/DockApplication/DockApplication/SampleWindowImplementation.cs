//
// SampleWindowImplementation.cs
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

namespace DockApplication{	public partial class SampleWindow
						: DockableWidget	{		public void init() 
		{
			Gtk.Button btn = new Gtk.Button("Date");
			btn.Clicked += HandleButtonClicked;
			this.box3.PackEnd(btn,true,true,0);
		}

		void HandleButtonClicked (object sender, EventArgs e)
		{
			PopupWindow pw = DatePicker.ShowAsPopup(DateTime.Now,false,true);
			//pw.SetSizeRequest(800,600);
			//PopupWindow pw = DatePicker.ShowAsPopup();
			//PopupWindow pw = DatePicker.ShowAsPopup(DateTime.Now);
			(pw.CurrentWidget as DatePicker).OnChanged += HandleOnDateChanged;
			pw.ShowAll();
		}

		void HandleOnDateChanged (object sender, EventArgs e)
		{
			Console.WriteLine(sender);
			DatePicker dt = sender as DatePicker;
			Console.WriteLine(dt.Date + "-" + dt.Date2);
		}

		public void ShowText(object sender, EventArgs e)
		{
			//String s = sender as String;
			String s = "call: ";

			String xx = sender as String;
			if (xx != null)
			{
				s+=xx;
			}

			if (!String.IsNullOrEmpty(s))
			{
				//textview1.Buffer.Clear();
				s = s + "\n";
				textview1.Buffer.Text += s;
			}
		}

		public void ShowText2(object sender, EventArgs e)
		{
			String s = sender as String;
			if (!String.IsNullOrEmpty(s))
			{
				//textview1.Buffer.Clear();
				textview1.Buffer.Text += "\n I'm Showtext2";
			}
		}
	} //class} //namespace