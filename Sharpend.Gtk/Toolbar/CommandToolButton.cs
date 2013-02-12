//
// CommandToolButton.cs
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

namespace Sharpend.GtkSharp
{
	public class CommandToolButton : ToolButton
	{
		public CommandToolButton (String stock_id) : base(stock_id)
		{
		}
		
		public String EventArgs {
			get;
			private set;
		}
		
		public CommandToolButton (String iconfilename, String title, String stock_id, String eventargs) : base (stock_id)
		{			
			if (!String.IsNullOrEmpty(iconfilename))
			{
				Gtk.Image img = new Gtk.Image(new Gdk.Pixbuf(iconfilename));
				img.Visible = true;
				this.IconWidget = img;
				
				Gtk.IconFactory.LookupDefault(""); //TODO do we need this ??
			}
			
			TooltipText = title;
			Label = title;
			EventArgs = eventargs;
		}
		
				
	}
}

