//
// PopupWindow.cs
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
using Gdk;

#if !GTK2
namespace Sharpend.GtkSharp
{
	public class PopupWindow : Gtk.Window
	{
		private Box box = new Box(Orientation.Vertical,0);

		public event EventHandler OnClose;

		public PopupWindow () : base (Gtk.WindowType.Popup)
		{
		}
		
		public CustomWidget CurrentWidget {
			get;
			private set;
		}
		
		public PopupWindow (CustomWidget widget) : base (Gtk.WindowType.Toplevel)
		{
			SetSizeRequest(320,200);
			box.Expand = true;
			this.Add(box);

			Decorated = true;
			//TransientFor = (Gtk.Window) frame.Toplevel;
			TypeHint = WindowTypeHint.Utility;
			WindowPosition = WindowPosition.Center;
			widget.Visible = true;
			box.PackEnd(widget,true,true,0);
			widget.Visible = true;
			//
			CurrentWidget = widget;
			//Console.WriteLine(widget.Parent);
		}
		
		public void Close()
		{
			//box.Remove(CurrentWidget);
			//CurrentWidget.Destroy();
			if (OnClose != null)
			{
				OnClose(this,new EventArgs());
			}

			Destroy();
		}
	
		
	}
}
#endif
