//
// ToolbarHelper.cs
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
using System.Xml;
using Gtk;
using System.Reflection;
using System.Text;
using System.IO;

namespace Sharpend.GtkSharp
{
	public static class ToolbarHelper
	{
		
		/// <summary>
		/// reads the given config file and creates toolbars and buttons
		/// </summary>
		/// <param name='frame'>
		/// Frame.
		/// </param>
		public static void addToolbars(Toolbar toolbar, String configfile, bool hookdelegates, Type type = null, object callingObject=null)
		{
			String xpath = "//toolbar";
			if (type != null)
			{
				xpath += "[@class='" + type.ToString() + "']";
			} else 
			{
				xpath += "[not(@class)]";
			}
			
			XmlNodeList lst = Sharpend.Configuration.ConfigurationManager.getValues(configfile, xpath);
			
			foreach (XmlNode nd in lst)	
			{						
				XmlNodeList btns = nd.SelectNodes(".//button");
				foreach (XmlNode bn in btns)
				{
					String classname = XmlHelper.getAttributeValue(bn,"class");
					String assembly = XmlHelper.getAttributeValue(bn,"assembly");
					String icon = XmlHelper.getAttributeValue(bn,"icon");
					String btn_title = XmlHelper.getAttributeValue(bn,"title");
					String stock_id = XmlHelper.getAttributeValue(bn,"stock_id");
					String eventargs = XmlHelper.getAttributeValue(bn,"eventargs");
					String buttonname = XmlHelper.getAttributeValue(bn,"name");
					
					Widget btn = createButton(classname,assembly,icon,btn_title,stock_id, eventargs);
					if (btn != null)
					{
						btn.Visible = true;
						toolbar.Add(btn);
						if (hookdelegates)
						{
							hookDelegates(bn,btn, callingObject);	
						}
						
						if (!String.IsNullOrEmpty(buttonname))
						{
							btn.Name = buttonname;
						}
					}
				}
			}
		}
		
		
		private static void hookDelegates(XmlNode parent, Gtk.Widget source, object callingObject=null)
		{	
			XmlNodeList lst = parent.SelectNodes(".//delegate");
			
			foreach(XmlNode nd in lst)
			{
				String target = XmlHelper.getAttributeValue(nd,"target");
				String functionname = XmlHelper.getAttributeValue(nd,"function");
				String eventname = XmlHelper.getAttributeValue(nd,"event");
				
#if !GTK2
				Sharpend.GtkSharp.MainWindow.hookDelegate(source,target,eventname,functionname,callingObject);
#endif

#if GTK2
	throw new Exception("Sharpend.GtkSharp.MainWindow does not work with gtk2");			
#endif
				
				
			}
		}
		
		
		static void HandleBtnClicked (object sender, EventArgs e)
		{
			
		}
		
		/// <summary>
		/// Creates the button.
		/// </summary>
		/// <returns>
		/// The button.
		/// </returns>
		/// <param name='classname'>
		/// Classname.
		/// </param>
		/// <param name='assembly'>
		/// Assembly.
		/// </param>
		/// <param name='iconfilename'>
		/// Iconfilename.
		/// </param>
		/// <param name='title'>
		/// Title.
		/// </param>
		/// <param name='stock_id'>
		/// Stock_id.
		/// </param>
		public static Widget createButton(String classname, String assembly, String iconfilename, String title, String stock_id, String eventargs)
		{
			Type[] types = new Type[4];
			object[] data = new object[4];
						
			types[0] = typeof(String);
			types[1] = typeof(String);
			types[2] = typeof(String);
			types[3] = typeof(String);			
			
			data[0] = iconfilename;
			data[1] = title;
			data[2] = stock_id;
			data[3] = eventargs;
			
			Type t = Type.GetType(classname + "," + assembly);
			if (t != null)
			{			
				ConstructorInfo ci = t.GetConstructor(types);
                if (ci != null)
				{
					object o = ci.Invoke(data);
					if (o != null)
					{
						return (Widget)o;
					}
				} else
				{
					ci = t.GetConstructor(new Type[0]);
					object o = ci.Invoke(new object[0]);
					if (o != null)
					{
						return (Widget)o;
					}
				}
			}
			return null;
		}
		
//		public static bool saveFrame(DockToolbarFrame frame)
//		{
//			if (frame == null)
//			{
//				throw new ArgumentNullException("frame");
//			}
//			
//			if (String.IsNullOrEmpty(frame.CurrentLayout))
//			{
//				throw new Exception("frame has no currentlayout");
//			}
//			
//			using (XmlTextWriter tw = new XmlTextWriter(frame.Name,Encoding.UTF8))
//			{
//				frame.SaveStatus(tw);	
//			}
//			return true;
//		}
		
//		public static bool loadFrame(DockToolbarFrame frame)
//		{
//			if (frame == null)
//			{
//				throw new ArgumentNullException("frame");
//			}
//			
//			if (String.IsNullOrEmpty(frame.CurrentLayout))
//			{
//				throw new Exception("frame has no currentlayout");
//			}
//			
//			if (File.Exists(frame.Name))
//			{
//				using (XmlTextReader reader = new XmlTextReader(frame.Name))
//				{	
//					frame.LoadStatus(reader);
//				}
//				return true;
//			}
//			
//			return false;
//		}
		
		
	}
}

