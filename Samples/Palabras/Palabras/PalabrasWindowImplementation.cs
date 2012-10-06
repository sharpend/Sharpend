//
// PalabrasWindowImplementation.cs
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
using System.Xml;
using System.IO;
using NDesk.DBus;

namespace Palabras{	
	public partial class PalabrasWindow: Gtk.Window	{		private XmlDocument doc;
		
		Gtk.EventBox header;
		Gtk.Alignment headerAlign;
		static Gdk.Cursor handCursor = new Gdk.Cursor (Gdk.CursorType.Hand2);
		private String spanishword = String.Empty;
		
		public static TomboyProxy Proxy
		{
			get;
			private set;
		}
		
		public void init() 
		{
			loadData();		
			showRandomWord();
			initBus();
			
			headerAlign = new Alignment (0.0f, 0.0f, 1.0f, 1.0f);
			headerAlign.TopPadding = headerAlign.BottomPadding = headerAlign.RightPadding = headerAlign.LeftPadding = 0;
			headerAlign.Add (box1);
			
			header = new EventBox ();
			header.Events |= Gdk.EventMask.KeyPressMask | Gdk.EventMask.KeyReleaseMask;
			//header.ButtonPressEvent += HeaderButtonPress;
			//header.ButtonReleaseEvent += HeaderButtonRelease;
//			header.MotionNotifyEvent += HeaderMotion;
//			header.KeyPressEvent += HeaderKeyPress;
//			header.KeyReleaseEvent += HeaderKeyRelease;
			header.Add (headerAlign);
			//header.Drawn += HandleHeaderDrawn;
			header.Realized += delegate {
				header.Window.Cursor = handCursor;
			};
			
			btnNewWord.Clicked += HandleBtnNewWordClicked;
			btnWeb.Clicked += HandleBtnWebClicked;
			btnSearch.Clicked += HandleBtnSearchClicked;
			
			GLib.Timeout.Add (10000, () => {
					showRandomWord();
					return true;
				});
			
		}

		void HandleBtnSearchClicked (object sender, EventArgs e)
		{
			String word = entry1.Text.Trim();
			XmlNode nd = doc.SelectSingleNode("//word[(translation/@language='es') and (translation = '"+word+"')]");
			showWord(nd);
		}

		void HandleBtnWebClicked (object sender, EventArgs e)
		{
			String url="http://www.verbix.com/webverbix/Spanisch/";
			
			String word = spanishword;
			
			url += word + ".html";
			
			System.Diagnostics.Process.Start(url);
		}

		void HandleBtnNewWordClicked (object sender, EventArgs e)
		{
			showRandomWord();	
		}
		
		public void loadData()
		{
			if (doc == null)
			{
				FileInfo fi = Sharpend.Configuration.ConfigurationManager.getConfigFile("spanish.xml");
				
				if (fi.Exists)
				{
					doc = new XmlDocument();
					doc.Load (fi.FullName);
				}
			}
		}
		
		private void initBus()
		{
			//Console.WriteLine("initBus");
			//BusG.Init();
			
			//Connection.Open(
						
			//Console.WriteLine("xxx");
			Proxy = new TomboyProxy(doc);
			//Console.WriteLine("endinitBus");
		}
		
		public void showWord(XmlNode node)
		{
			showWord(node,null,null);
		}
		
		public void showWord(XmlNode node,String srclng, String destlng)
		{
			if (node == null)
			{
				return;
			}
			
			if (String.IsNullOrEmpty(srclng) || String.IsNullOrEmpty(destlng))
			{
				srclng = "de";
				destlng = "es";
			}
			
			XmlNode src = node.SelectSingleNode("./translation[@language = '" + srclng + "']");
			XmlNode dest = node.SelectSingleNode("./translation[@language = '" + destlng + "']");
			
			XmlNode sp = node.SelectSingleNode("./translation[@language = 'es']");
			spanishword = sp.InnerText;			
			
			lblWord.Text  = src.InnerText;
			lblTranslated.Text = dest.InnerText;	
		}
				
		public void showRandomWord()
		{
			String srclng = "de";
			String destlng = "es";
			
			Random rnd = new Random();
			
			XmlNodeList lst = doc.SelectNodes("//word");
			
			int idx = rnd.Next(1,lst.Count);
			
			int l = rnd.Next(1,3);
			if (l == 1)
			{
			 srclng = "es";
			 destlng = "de";
			}
			
			
			XmlNode nd = doc.SelectSingleNode("//word[" + idx.ToString() + "]" );
			
			if (nd != null)
			{
				showWord(nd,srclng,destlng);	
			}
			
		}
		
			} //class} //namespace