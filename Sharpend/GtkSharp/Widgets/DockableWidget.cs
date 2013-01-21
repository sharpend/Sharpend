//
// DockableWidget.cs
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
using Sharpend.Utils;
using System.Reflection;
using System.Xml.Serialization;
using Gtk;

#if !GTK2
namespace Sharpend.GtkSharp
{
	/// <summary>
	/// Dockable widget.
	/// </summary>
	public class DockableWidget : CustomWidget
	{
		
		private MainWindow parentwindow;
		
		public MainWindow ParentWindow 
		{
			get
			{
				if (parentwindow == null)
				{
					setParentWindow();
				}
				return parentwindow;
			}
		}
		
		public String ID {
			get;
			protected set; //TODO private ???
		}
		
		public bool AllowMultipleWindows {
			get;
			set;
		}
		
				
		public DelegateSet Delegates {
			get;
			internal set;
		}
		
		public String Title {
			get;
			set;
		}
		
		public ParameterSet Params
		{
			get;
			internal set;
		}
		
		public DockableWidget () : base()
		{
			init();
			this.Title = "Empty";
		}
		
		public DockableWidget(String name) : base(name)
		{
			init();
			this.Title = name;
		}
		
		public DockableWidget(String name, bool allowmultiplewindows) : base(name)
		{
			AllowMultipleWindows = allowmultiplewindows;
			this.Title = name;
			init();
		}
		
		public static DockableWidget CreateInstance(String classname)
		{
			throw new NotImplementedException("CreateInstance(String classname)");
		}
		
		public static DockableWidget CreateInstance(XmlNode node)
		{
			return createWindow(node);
		}
		
		private void init()
		{
			ID = this.GetType().ToString();
			if (AllowMultipleWindows)
			{
				ID += "_" + Guid.NewGuid().ToString();
			}
		}

		private PopupWindow GetPopup(Gtk.Widget widget)
		{
			if (widget.Parent != null)
			{
				if (widget.Parent is PopupWindow)
				{
					return (widget.Parent as PopupWindow);
				} else
				{
					return GetPopup(widget.Parent);
				}
			}
			return null;
		}

		public void Close()
		{
			PopupWindow pw = GetPopup(this);

			if (pw != null)
			{
				pw.Close();
			} else
			{
				Destroy();
			}
		}

		public static DockableWidget createWindow(XmlNode node)
		{	
			//Console.WriteLine("createWindow");
			ParameterSet ps = ParameterSet.CreateInstance(node);
			DelegateSet ds = DelegateSet.CreateInstance(node);
			return createWindow(ps, ds);			
		}
		
		public static DockableWidget createWindow(ParameterSet param, DelegateSet delegates)
		{
			ConstructorInfo ci = param.BaseType.GetConstructor(param.Types);
			object o = ci.Invoke(param.Data);
			if (o != null)
			{
				DockableWidget wnd = (DockableWidget)o;
				wnd.Params = param;
				wnd.Delegates = delegates;
				wnd.hookDelegates();
				return wnd;
			}
			return null;
		}
		
		public void hookDelegates()
		{
			if (Delegates != null)
			{
				foreach (DelegateData dd in Delegates)
				{
					Sharpend.GtkSharp.MainWindow.hookDelegate(this,dd);
				}	
			}
		}

		public void unHookDelegates()
		{
			if (Delegates != null)
			{
				foreach (DelegateData dd in Delegates)
				{
					Sharpend.GtkSharp.MainWindow.unHookDelegate(this,dd);
				}	
			}
		}

		
		private void setParentWindow(Widget widget=null)
		{
			if (widget == null)
			{
				if (this.Parent != null)
				{
					setParentWindow(this.Parent);
				}
			}
			
			if (widget != null)
			{
				if (widget is MainWindow)
				{
					parentwindow = (MainWindow)widget;
					if (!parentwindow.WindowList.ContainsKey(this.ID))
					{
						parentwindow.WindowList.Add(this.ID,this);
					}
				} else
				{
					if (widget.Parent != null)
					{
						setParentWindow(widget.Parent);
					}
				}
			}
		}
		
		/// <summary>
		/// Called after Mainwindow init
		/// </summary>
		public virtual void afterInit()
		{
		}
				
		protected override void doReadXml (XmlReader reader)
		{
			this.ID = reader["ID"];
			this.AllowMultipleWindows = Convert.ToBoolean(reader["AllowMultipleWindows"]);
			
			base.doReadXml (reader);
			
			reader.Read();
			
			Delegates = new DelegateSet();
			Delegates.ReadXml(reader);
			
			if (MainWindow.Instance != null)
			{
				MainWindow.Instance.WindowList.Add(this.ID,this);
			}
		}
		
		protected override void doWriteXml (XmlWriter writer)
		{
			writer.WriteAttributeString("ID",this.ID);
			writer.WriteAttributeString("AllowMultipleWindows",this.AllowMultipleWindows.ToString());
			
			base.doWriteXml (writer);
			
			if (Delegates != null)
			{
				Delegates.WriteXml(writer);
			}			
		}
		
		
	}
}

#endif