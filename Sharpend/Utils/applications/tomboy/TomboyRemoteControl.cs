//
// TomboyRemoteControl.cs
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

#if DBUS
using NDesk.DBus;
using org.freedesktop.DBus;

namespace Sharpend.Utils.Applications.Tomboy
{
	
	[Interface("org.gnome.Tomboy.RemoteControl")]
	public class TomboyRemoteControl : MarshalByRefObject, ITomboyRemoteControl
	{
		private const string Path = "/org/gnome/Tomboy/RemoteControl";
		private const string Namespace = "org.gnome.Tomboy.RemoteControl";
		
		public TomboyRemoteControl ()
		{
			
		}
		
		public static TomboyRemoteControl GetInstance () {

			//BusG.Init ();

			if (! Bus.Session.NameHasOwner (Namespace))
				Bus.Session.StartServiceByName (Namespace);

			return Bus.Session.GetObject<TomboyRemoteControl> (Namespace,
			                new ObjectPath (Path));
		}
		
		public static TomboyRemoteControl Register()
		{
			//BusG.Init ();

			TomboyRemoteControl remote_control = new TomboyRemoteControl ();
			Bus.Session.Register (Namespace,
			                      new ObjectPath (Path),
			                      remote_control);
			
			/* */
			if (Bus.Session.NameHasOwner (Namespace)) {
                    Console.WriteLine(Namespace + "ist schon registriert !!");
					return null;
             }
			
			//Bus.Session.Register(new ObjectPath(Path),remote_control);
			//Bus.Session.Register(new ObjectPath(Path),null);
			
			if (Bus.Session.RequestName (Namespace)
			                != RequestNameReply.PrimaryOwner)
			{
				Console.WriteLine(Namespace + "ist schon registriert !!");
				return null;
			}
			
			//remote_control.initScope();
			
			
			Console.WriteLine("alles ok");
			
			//Console.WriteLine(remote_control.Version());
			
			return remote_control;
		}
		
		
		#region ITomboyRemoteControl implementation
		public event RemoteAddedHandler NoteAdded;

		public event RemoteDeletedHandler NoteDeleted;

		public event RemoteSavedHandler NoteSaved;

		public bool AddNotebook (string notebook_name)
		{
			throw new NotImplementedException ();
		}

		public bool AddNoteToNotebook (string uri, string notebook_name)
		{
			throw new NotImplementedException ();
		}

		public bool AddTagToNote (string uri, string tag_name)
		{
			throw new NotImplementedException ();
		}

		public string CreateNamedNote (string linked_title)
		{
			throw new NotImplementedException ();
		}

		public string CreateNamedNoteWithUri (string linked_title, string uri)
		{
			throw new NotImplementedException ();
		}

		public string CreateNote ()
		{
			throw new NotImplementedException ();
		}

		public bool DeleteNote (string uri)
		{
			throw new NotImplementedException ();
		}

		public bool DisplayNote (string uri)
		{
			throw new NotImplementedException ();
		}

		public bool DisplayNoteWithSearch (string uri, string search)
		{
			throw new NotImplementedException ();
		}

		public void DisplaySearch ()
		{
			throw new NotImplementedException ();
		}

		public void DisplaySearchWithText (string search_text)
		{
			throw new NotImplementedException ();
		}

		public string FindNote (string linked_title)
		{
			throw new NotImplementedException ();
		}

		public string FindStartHereNote ()
		{
			throw new NotImplementedException ();
		}

		public string[] GetAllNotesInNotebook (string notebook_name)
		{
			throw new NotImplementedException ();
		}

		public string[] GetAllNotesWithTag (string tag_name)
		{
			throw new NotImplementedException ();
		}

		public string GetNotebookForNote (string uri)
		{
			throw new NotImplementedException ();
		}

		public long GetNoteChangeDate (string uri)
		{
			throw new NotImplementedException ();
		}

		public string GetNoteCompleteXml (string uri)
		{
			throw new NotImplementedException ();
		}

		public string GetNoteContents (string uri)
		{
			throw new NotImplementedException ();
		}

		public string GetNoteContentsXml (string uri)
		{
			throw new NotImplementedException ();
		}

		public long GetNoteCreateDate (string uri)
		{
			throw new NotImplementedException ();
		}

		public string GetNoteTitle (string uri)
		{
			throw new NotImplementedException ();
		}

		public string[] GetTagsForNote (string uri)
		{
			throw new NotImplementedException ();
		}

		public bool HideNote (string uri)
		{
			throw new NotImplementedException ();
		}

		public string[] ListAllNotes ()
		{
			throw new NotImplementedException ();
		}

		public bool NoteExists (string uri)
		{
			throw new NotImplementedException ();
		}

		public bool RemoveTagFromNote (string uri, string tag_name)
		{
			throw new NotImplementedException ();
		}

		public string[] SearchNotes (string query, bool case_sensitive)
		{
			throw new NotImplementedException ();
		}

		public bool SetNoteCompleteXml (string uri, string xml_contents)
		{
			throw new NotImplementedException ();
		}

		public bool SetNoteContents (string uri, string text_contents)
		{
			throw new NotImplementedException ();
		}

		public bool SetNoteContentsXml (string uri, string xml_contents)
		{
			throw new NotImplementedException ();
		}

		public string Version ()
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

#endif