//
// TomboyProxy.cs
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

namespace Sharpend.Utils.Applications.Tomboy
{
	/// <summary>
	/// Example for a Tomboy proxy.
	/// </summary>
	public class TomboyProxy : DBusBaseProxy<ITomboyRemoteControl>
	{
		public TomboyProxy () : base("org.gnome.Tomboy","/org/gnome/Tomboy/RemoteControl")
		{
			this.Register();
		}
		
		public override void Register ()
		{
			base.Register ();
			RemoteInterface.NoteAdded += HandleRemoteInterfaceNoteAdded;
		}

		void HandleRemoteInterfaceNoteAdded (string uri)
		{
			Console.WriteLine("HandleRemoteInterfaceNoteAdded");
		}
		
	}
}

