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
using Sharpend.Utils.Applications.Tomboy;
using Sharpend.Utils;
using System.Xml;
using Sharpend;
using Sharpend.Extensions;
using System.IO;
using NDesk.DBus;

namespace Palabras
{
	public class TomboyProxy : DBusBaseProxy<ITomboyRemoteControl>
	{
		private XmlDocument xmldoc; 
		
		public TomboyProxy () : base("org.gnome.Tomboy","/org/gnome/Tomboy/RemoteControl")
		{
			this.Register();
		}
		
		private XmlNode root;
		
		public XmlNode Root {
			get
			{
				if (root == null)
				{
					root = xmldoc.SelectSingleNode("/words");
				}
				
				return root;
			}	
		}
		
		public override void Register ()
		{
			//Bus.Session.ReleaseName(DBusInterface);
			
			base.Register ();
			RemoteInterface.NoteAdded += HandleRemoteInterfaceNoteAdded;
			RemoteInterface.NoteSaved += HandleRemoteInterfaceNoteSaved;
		}
		
		public TomboyProxy (XmlDocument doc) : this()
		{
			xmldoc = doc;		
		}
		
		void HandleRemoteInterfaceNoteSaved (string uri)
		{
			Console.WriteLine("saved" + uri);
			String title = RemoteInterface.GetNoteTitle(uri);
			
			if (title.Equals("vokabeln",StringComparison.OrdinalIgnoreCase))
			{
				String txt = RemoteInterface.GetNoteContents(uri);
				
				//Console.WriteLine(txt);
				
				char[] delimiters = new char[] { '\r', '\n' };
				String[] lines = txt.Split(delimiters);
				
				bool sentence = false;
				foreach (String s in lines)
				{
					String[] word;
					
					if (s.Contains("@"))
					{
						word = s.Split('@');
						sentence = true;
					} 
					else
					{
				 		word = s.Split('-');
					}
					
					if (word.Length == 2)
					{
						if (sentence)
						{
							createSentence(word[0].Trim(),word[1].Trim());	
						} else
						{
							createWord(word[0].Trim(),word[1].Trim());	
						}
					}
				}
				
				FileInfo fi = Sharpend.Configuration.ConfigurationManager.getConfigFile("spanish.xml");
				xmldoc.Save(fi.FullName);
			}
		}
		
		
		private void createWord(String trans1, String trans2)
		{
			if (wordExists(trans1))
			{
				return;
			}
			
			//Console.WriteLine("createWord");
			trans1 = trans1.Trim();
			trans2 = trans2.Trim();
			
			XmlElement word = xmldoc.CreateElement("word");	
		   
		   	XmlElement t1 = xmldoc.CreateElement("translation");
		   	XmlElement t2 = xmldoc.CreateElement("translation");
		
			word.AppendChild(t1);
			word.AppendChild(t2);
			
		   	t1.AddAttributeValue("language","es");
		   	t2.AddAttributeValue("language","de");
			
		   	t1.InnerText = trans1;
		   	t2.InnerText = trans2;
			
			Root.AppendChild(word);
		}
		
		private void createSentence(String trans1, String trans2)
		{
			if (wordExists(trans1))
			{
				return;
			}
			
			trans1 = trans1.Trim();
			trans2 = trans2.Trim();
			
			//Console.WriteLine("createSentence");
			
			XmlElement word = xmldoc.CreateElement("sentence");	
		   
		   	XmlElement t1 = xmldoc.CreateElement("translation");
		   	XmlElement t2 = xmldoc.CreateElement("translation");
		
			word.AppendChild(t1);
			word.AppendChild(t2);
			
		   	t1.AddAttributeValue("language","es");
		   	t2.AddAttributeValue("language","de");
			
		   	t1.InnerText = trans1;
		   	t2.InnerText = trans2;
			
			Root.AppendChild(word);
		}
		
		
		private bool wordExists(String word)
		{
			XmlNodeList lst = xmldoc.SelectNodes("//word[translation = '"+word+"']");
			return lst.Count > 0;
		}
		
		void HandleRemoteInterfaceNoteAdded (string uri)
		{
			Console.WriteLine("HandleRemoteInterfaceNoteAdded");
			//Console.WriteLine(uri);
		}
		
		private int count=0;
		
		public void test()
		{
			Console.WriteLine("testing...");
			
			String[] s = RemoteInterface.GetAllNotesInNotebook("Alle Notizen");
//			if (RemoteInterface.AddNotebook("xxx"))
//			{
//				Console.WriteLine("sss");
//			}
			
			String ss = RemoteInterface.FindNote("Vokabeln");
			//Console.WriteLine(ss);
			if (String.IsNullOrEmpty(ss))
			{
				RemoteInterface.CreateNamedNote("Vokabeln");
			}
			
			if (count > 10)
			{
				RemoteInterface.DisplayNote(ss);
				
				//RemoteInterface.SetNoteContents(ss,"ser");
				//String cnts = RemoteInterface.SetNoteContentsXml
				//Console.WriteLine(cnts);
				
				
				count = 0;
			}
			
			//String ss = RemoteInterface.CreateNamedNote("Test");
			
			if ((s != null) && (s.Length > 0))
			{
				Console.WriteLine(s[0]);
			}
			
			//Console.WriteLine(ss);
			
			count++;
			
		}
		
		
		
		
		
	}
}

