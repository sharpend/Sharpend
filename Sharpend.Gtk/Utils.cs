//
// Utils.cs
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
	public class DataEventArgs : EventArgs
	{
		public object Data {
			get;
			private set;
		}
		
		public DataEventArgs(object data) : base()
		{
			Data = data;
		}
	}

	/// <summary>
	/// Utils for Messagedialogs
	/// </summary>
	public static class Message
	{

		/// <summary>
		/// show a message dialog
		/// </summary>
		/// <returns>
		/// The message.
		/// </returns>
		/// <param name='message'>
		/// Message.
		/// </param>
		/// <param name='parent'>
		/// Parent.
		/// </param>
		/// <param name='flags'>
		/// Flags.
		/// </param>
		/// <param name='messagetype'>
		/// Messagetype.
		/// </param>
		/// <param name='buttonstype'>
		/// Buttonstype.
		/// </param>
		public static ResponseType ShowMessage(String message, Gtk.Window parent, Gtk.DialogFlags flags,Gtk.MessageType messagetype,ButtonsType buttonstype)
		{
			var dialog = new MessageDialog(parent,flags,messagetype,buttonstype,message,new object[0]);

			try
			{
				int res = dialog.Run ();
				return (ResponseType)res;
			}
			finally
			{	
				dialog.Destroy();
			}
		}

		/// <summary>
		/// shows an modal info message dialog with given buttons
		/// </summary>
		/// <returns>
		/// The info message.
		/// </returns>
		/// <param name='message'>
		/// Message.
		/// </param>
		/// <param name='parent'>
		/// Parent.
		/// </param>
		/// <param name='buttonstype'>
		/// Buttonstype.
		/// </param>
		public static ResponseType ShowInfoMessage(String message, Gtk.Window parent, ButtonsType buttonstype)
		{
			return ShowMessage(message, parent,Gtk.DialogFlags.Modal,MessageType.Info,buttonstype);
		}

		/// <summary>
		/// Shows an modal info message dialog with ok and cancel button
		/// </summary>
		/// <returns>
		/// The info message.
		/// </returns>
		/// <param name='message'>
		/// Message.
		/// </param>
		/// <param name='parent'>
		/// Parent.
		/// </param>
		public static ResponseType ShowInfoMessage(String message, Gtk.Window parent)
		{
			return ShowMessage(message, parent,Gtk.DialogFlags.Modal,MessageType.Info,ButtonsType.OkCancel);
		}

	}

}

