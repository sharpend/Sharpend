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
		protected static log4net.ILog log;

		public String Language1 {
			get;
			set;
		}

		public String Language2 {
			get;
			set;
		}

		public String TranslationsFile {
			get;
			set;
		}

		public static TomboyProxy Proxy
		{
			get;
			private set;
		}

		public PalabrasWindow(String name,String translationsfile,String lng1, String lng2)
			: this (name)
		{
			TranslationsFile = translationsfile;
			Language1 = lng1;
			Language2 = lng2;
			myinit();
		}

		public void init()
		{
		}

		public void myinit() 
		{
			log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

//			log.Debug("Lng1" + Language1);
//			log.Debug("Lng2" + Language2);


			GLib.Timeout.Add (10000, () => {
					showRandomWord();
					return true;
				});
			
		}

		void HandleBtnSearchClicked (object sender, EventArgs e)
		{
			String word = entry1.Text.Trim();
			XmlNode nd = doc.SelectSingleNode("//word[(translation/@language='"+Language2+"') and (translation = '"+word+"')]"); //TODO language ??
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
				FileInfo fi = Sharpend.Configuration.ConfigurationManager.getConfigFile(TranslationsFile);

				if (fi == null)
				{
					throw new Exception("Translations file does not exist: " + TranslationsFile);
				}

				if (fi.Exists)
				{
					doc = new XmlDocument();
					doc.Load (fi.FullName);
				} else
				{
					throw new Exception("Translations file does not exist: " + fi.FullName);
				}
			}
		}
		
		private void initBus()
		{
			//Console.WriteLine("initBus");
			//BusG.Init();
			
			//Connection.Open(
						
			//Console.WriteLine("xxx");
			Proxy = new TomboyProxy(doc,Language1,Language2, TranslationsFile);
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
				log.Debug("set default" + Language1 + "-" + Language2);
				srclng = Language1;
				destlng = Language2;
			}
			
			XmlNode src = node.SelectSingleNode("./translation[@language = '" + srclng + "']");
			XmlNode dest = node.SelectSingleNode("./translation[@language = '" + destlng + "']");

			log.Debug("src dest: " + srclng + "-" + destlng);

			XmlNode sp = node.SelectSingleNode("./translation[@language = '"+Language2+"']"); //TODO language ??
			spanishword = sp.InnerText;			
			
			lblWord.Text  = src.InnerText;
			lblTranslated.Text = dest.InnerText;	
		}
				
		public void showRandomWord()
		{
			String srclng = Language1;
			String destlng = Language2;
			
			Random rnd = new Random();
			
			XmlNodeList lst = doc.SelectNodes("//word");
			
			int idx = rnd.Next(1,lst.Count);
			
			int l = rnd.Next(1,3);
			if (l == 1)
			{
				srclng = Language2;
				destlng = Language1;
			}
			log.Debug("rnd: " + Language1 + "-" + Language2);
			
			XmlNode nd = doc.SelectSingleNode("//word[" + idx.ToString() + "]" );
			
			if (nd != null)
			{
				showWord(nd,srclng,destlng);	
			}
			
		}
		
			} //class} //namespace