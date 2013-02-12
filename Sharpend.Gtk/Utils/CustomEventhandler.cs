//
// CustomEventhandler.cs
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

namespace Sharpend.Utils
{
	public class CustomEventhandler<T> : CustomEventArgs<T>
	{
		public CustomEventhandler (T data, object sender) : base(data, sender)
		{
			invoke();
		}
				
		public object Value
		{
			get
			{
				return Data;
			}	
		}
		
		protected void invoke()
		{
			Gtk.Application.Invoke(Sender,this,new EventHandler(setDataHandler));
		}
				
		
		protected void setDataHandler(object sender, EventArgs args)
        {
			if (sender is Gtk.Button)
            {   
				(sender as Gtk.Button).Label = Value as String;
            }
            if (sender is Gtk.ToolButton)
            {
                (sender as Gtk.ToolButton).Label = Value as String;
            }
            if (sender is Gtk.Label)
            {
                (sender as Gtk.Label).Text = Value as String;
            }
			if (sender is Gtk.ProgressBar)
            {
				if (Data is String)
				{
					(sender as Gtk.ProgressBar).Text = Value as String;
				}
				
				if (Data is Double)
				{
					(sender as Gtk.ProgressBar).Fraction = (double)Value;
				}
			}
		}
	}
}

