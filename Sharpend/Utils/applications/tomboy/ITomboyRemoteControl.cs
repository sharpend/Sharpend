//
// ITomboyRemoteControl.cs
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
using NDesk.DBus;

namespace Sharpend.Utils.Applications.Tomboy
{
	
	public delegate void RemoteDeletedHandler (string uri, string title);
	public delegate void RemoteAddedHandler (string uri);
	public delegate void RemoteSavedHandler (string uri);
	
	[Interface("org.gnome.Tomboy.RemoteControl")]
	public interface ITomboyRemoteControl
	{
		bool AddNotebook (string notebook_name);
		bool AddNoteToNotebook (string uri, string notebook_name);
		bool AddTagToNote (string uri, string tag_name);
		string CreateNamedNote (string linked_title);
		string CreateNamedNoteWithUri (string linked_title, string uri);
		string CreateNote ();
		bool DeleteNote (string uri);
		bool DisplayNote (string uri);
		bool DisplayNoteWithSearch (string uri, string search);
		void DisplaySearch ();
		void DisplaySearchWithText (string search_text);
		string FindNote (string linked_title);
		string FindStartHereNote ();
		string [] GetAllNotesInNotebook (string notebook_name);
 		string [] GetAllNotesWithTag (string tag_name);
		string GetNotebookForNote (string uri);
		long GetNoteChangeDate (string uri);
		string GetNoteCompleteXml (string uri);
		string GetNoteContents (string uri);
		string GetNoteContentsXml (string uri);
		long GetNoteCreateDate (string uri);
		string GetNoteTitle (string uri);
		string [] GetTagsForNote (string uri);
		bool HideNote (string uri);
		string [] ListAllNotes ();
		event RemoteAddedHandler NoteAdded;
		event RemoteDeletedHandler NoteDeleted;
		bool NoteExists (string uri);
		event RemoteSavedHandler NoteSaved;
		bool RemoveTagFromNote (string uri, string tag_name);
		string [] SearchNotes (string query, bool case_sensitive);
		bool SetNoteCompleteXml (string uri, string xml_contents);
		bool SetNoteContents (string uri, string text_contents);
		bool SetNoteContentsXml (string uri, string xml_contents);
		string Version ();
	}
}

