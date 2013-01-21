//
// MainWindow.cs
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
using Sharpend.GtkSharp.Docking;
using System.Collections.Generic;
using System.IO;
using Sharpend.Utils;
using System.Xml;
using System.Xml.Serialization;

#if !GTK2 //TODO this should be a .NET4 and not GTK2 switch
using System.Threading.Tasks;  
#endif 

#if !GTK2
namespace Sharpend.GtkSharp
{
	/// <summary>
	/// If you want to create a dockapplication you have to derive a class from MainWindow
	/// 
	/// Here is an Example for the Main function
	/// 
	/// public static void Main (string[] args)
	//		{
	//			Application.Init ();
	//
	//			win = MyDockApplication.RestoreInstance<MyDockApplication>();
	//
	//			if (win == null)
	//			{
	//				win = MyDockApplication.CreateInstance<MyDockApplication>();
	//			}
	//
	//			win.Title = "MyDockApplication";
	//			win.DestroyEvent += HandleDestroyEvent;
	//			win.DeleteEvent += HandleDeleteEvent;
	//			win.ShowAll();
	//			Application.Run ();
	//		}
	/// 
	/// </summary>
	public class MainWindow : Gtk.Window, IXmlSerializable
	{
		protected static MainWindow instance;
		protected static ProgressWindow progress;
		protected Gtk.Box box1;
		protected Gtk.Toolbar toolbar1;
		protected DockFrame frame;
		protected Dictionary<String, DockableWidget> windowlist;
		
		private int currentWidth;
		/// <summary>
		/// Gets the width of the current window.
		/// </summary>
		/// <value>
		/// The width of the current window.
		/// </value>
		public int CurrentWidth {
			get
			{
				getSize();
				return currentWidth;
			}
		}
		
		public int currentHeight;
		/// <summary>
		/// Gets the height of the current window.
		/// </summary>
		/// <value>
		/// The height of the current window.
		/// </value>
		public int CurrentHeight {
			get
			{
				getSize();
				return currentHeight;
			}
		}
		
		/// <summary>
		/// Width stored in xml ... Serialization
		/// </summary>
		/// <value>
		/// The width of the stored.
		/// </value>
		public int StoredWidth {
			get;
			set;
		}
		
		/// <summary>
		/// height in xml ... serialization
		/// </summary>
		/// <value>
		/// The height of the stored.
		/// </value>
		public int StoredHeight {
			get;
			set;
		}

		/// <summary>
		/// true if the window is maximized
		/// </summary>
		/// <value>
		/// <c>true</c> if maximized; otherwise, <c>false</c>.
		/// </value>
		public bool Maximized {
			get;
			protected set;
		}

		/// <summary>
		/// list of dockable widgets in the mainwindow
		/// </summary>
		/// <value>
		/// The window list.
		/// </value>
		public Dictionary<String,DockableWidget> WindowList {
			get { return windowlist; }
		}

		/// <summary>
		/// DockFrame of the mainwindow
		/// </summary>
		/// <value>
		/// The frame.
		/// </value>
		public DockFrame Frame {
			get
			{
				return frame;
			}
		}
		
		public String CurrentAssemblyName {
			get
			{
				String name = this.GetType().Assembly.FullName;
				String[] l = name.Split(',');
				return l[0];
			}
		}
		
		/// <summary>
		/// Serialization only
		/// </summary>
		public MainWindow() : base(WindowType.Toplevel)
		{
			createChildren();
		}
		
		protected MainWindow(IntPtr raw) : base(raw)
		{	
			createChildren();
		}
		
		protected MainWindow(String title) : base(title)
		{
			createChildren();
		}
		
		protected MainWindow(WindowType type) : base(type)
		{
			createChildren();
		}

		public static T CreateInstance<T>(WindowType type) where T:MainWindow
		{
			if (instance == null)
			{
				Type t = typeof(T);
				instance = (T)Activator.CreateInstance(t,new object[] {type});
				instance.init();
			}

			return (instance as T);
		}

		public static T CreateInstance<T>(IntPtr raw) where T:MainWindow
		{
			if (instance == null)
			{
				Type t = typeof(T);
				instance = (T)Activator.CreateInstance(t,new object[] {raw});
				instance.init();
			}

			return (instance as T);
		}


		public static T CreateInstance<T>(String title) where T:MainWindow
		{
			if (instance == null)
			{
				Type t = typeof(T);
				instance = (T)Activator.CreateInstance(t,new object[] {title});
				instance.init();
			}

			return (instance as T);
		}


		public static T CreateInstance<T>() where T:MainWindow
		{
			if (instance == null)
			{
				Type t = typeof(T);
				instance = (T)Activator.CreateInstance(t);
				instance.init();
			}

			return (instance as T);
		}


		public static T RestoreInstance<T>() where T:class
		{
			try
			{
				String name = typeof(T).Assembly.FullName;
				String[] l = name.Split(',');
				String fn = l[0] + ".xml";
			
				if (File.Exists(fn))
				{
					XmlSerializer xs = new XmlSerializer(typeof(T));
					using (XmlReader reader = XmlReader.Create(fn))
					{							
						var wnd = xs.Deserialize(reader);				
						instance = (MainWindow)wnd;
						instance.init();
						return (T)wnd;
					}
				}
			} catch (Exception ex)
			{
				//TODO LOGGING
				Console.WriteLine(ex.ToString());
			}
			return null;
		}
		
		public void Save()
		{
			String fn = CurrentAssemblyName + ".xml";
			using (XmlWriter xw = XmlWriter.Create(fn))
			{
				XmlSerializer xs =  new XmlSerializer(this.GetType());
				xs.Serialize(xw,this);
			}
			//Console.WriteLine("Save() done");
		}
		
		/// <summary>
		/// The MainWindow instance
		/// </summary>
		/// <value>
		/// The instance.
		/// </value>
		public static MainWindow Instance {
			get
			{
				return instance;
			}
		}


		public event EventHandler OnProgressBarShown;
		public event EventHandler OnWorkDone;

		protected void getSize()
		{
			int width;
			int height;
			this.GetSize(out width,out height);
									
			currentWidth = width;
			currentHeight = height;
		}
		
		protected void createChildren()
		{
			box1 = new Gtk.Box(Orientation.Vertical,0);
			box1.Name ="box1";
			box1.Expand = true;
						
			toolbar1 = new Gtk.Toolbar();
			toolbar1.Name ="toolbar1";
			toolbar1.Visible = true;
			//TODO add toolbar or not changeable by config
			box1.PackStart(toolbar1,false,false,0);
			
			this.Add(box1);
		}
			
		/// <summary>
		/// initialize the main window
		/// </summary>
		public virtual void init()
		{
			windowlist = new Dictionary<string, DockableWidget>(20);
			
			this.Title="MainWindow"; 
			this.WindowPosition=WindowPosition.Center;
						
			if (frame == null)
			{
				frame = new DockFrame();
				box1.PackStart(frame,true,true,0);
				frame.Expand = true;
			}
			
			addWindows(null);
			loadToolbar();
			
			foreach(KeyValuePair<String,DockableWidget> kp in windowlist)
			{
				kp.Value.afterInit();
			}

			if ((StoredWidth > 0) && (StoredHeight > 0))
			{
				this.SetDefaultSize(StoredWidth,StoredHeight);
			} else
			{
				this.SetDefaultSize(800,600);
			}
			
			if (Maximized)
			{
				this.Maximize();
			}
		}

		/// <summary>
		/// check if we are maximized or not ... used if we want to restore the window
		/// </summary>
		/// <param name='evnt'>
		/// If set to <c>true</c> evnt.
		/// </param>
		protected override bool OnWindowStateEvent (Gdk.EventWindowState evnt)
		{
			if (evnt.NewWindowState == Gdk.WindowState.Maximized)
			{
				Maximized = true;
			}
			
			if ((int)evnt.NewWindowState == 128)
			{
				Maximized = false;
			}
			return base.OnWindowStateEvent (evnt);
		}
		
		/// <summary>
		/// loads toolbuttons from config and hook delegates
		/// </summary>
		public void loadToolbar()
		{
			ToolbarHelper.addToolbars(toolbar1,"toolbar.config",true,this.GetType(),this);
		}

		/// <summary>
		/// add a new dockable widegt to the mainwindow
		/// </summary>
		/// <param name='widget'>
		/// Widget.
		/// </param>
		protected void addWidget(DockableWidget widget)
		{
			if (!windowlist.ContainsKey(widget.ID))
			{
				windowlist.Add(widget.ID,widget);
				widget.Visible = true;
				frame.addItem(new DockItemContainer(frame,widget),ItemAlignment.Top,true);
			} else
			{
				Console.WriteLine("Window is already in list: " + widget.ID); //TODO logging
			}
		}
		
		public void removeWidget(DockableWidget widget)
		{
			widget.unHookDelegates();
			windowlist.Remove(widget.ID);
		}
		
		protected static List<object> getWindows(String classname, object callingObject)
		{
			List<object> ret = new List<object>(100);
			if (MainWindow.Instance != null)
			{
				if (classname.Equals("mainwindow",StringComparison.OrdinalIgnoreCase))
				{
					ret.Add(MainWindow.Instance);
					return ret;
				}
				
				if ((callingObject != null)  && (classname.Equals("this",StringComparison.OrdinalIgnoreCase)))
				{
					ret.Add(callingObject);
					return ret;
				}
				
				if ((callingObject != null)  && (callingObject.GetType().ToString().Equals(classname,StringComparison.OrdinalIgnoreCase)))
				{
					ret.Add(callingObject);
					return ret;
				}
								
				foreach(KeyValuePair<String,DockableWidget> kp in Sharpend.GtkSharp.MainWindow.Instance.WindowList)
				{
					if (kp.Value.GetType().ToString().Equals(classname,StringComparison.OrdinalIgnoreCase))
					{
						ret.Add(kp.Value);
					}	
				}
			}
			
			return ret;
		}
		
		public static object getFirstWindow(String classname)
		{
			if (MainWindow.Instance != null)
			{
				foreach(KeyValuePair<String,DockableWidget> kp in MainWindow.Instance.WindowList)
				{
					if (kp.Value.GetType().ToString().Equals(classname,StringComparison.OrdinalIgnoreCase))
					{
						return kp.Value;
					}	
				}
			}
			
			return null;
		}
		
		
		public static void hookDelegate(object senderwidget, String targetwidget, String eventname, String functionname, object callingObject)
		{
			List<object> lst = getWindows(targetwidget, callingObject);
			foreach(object obj in lst)
			{
				Sharpend.Utils.Reflection.hookDelegate(senderwidget,obj,eventname,functionname,"Single","Single");
			}	
			
			if (lst.Count == 0)
			{
				//TODO logging ??
				Console.WriteLine("found 0 windows while hooking delegates");
			}
			
		}

		public static void unHookDelegate(object senderwidget, String targetwidget, String eventname, String functionname, object callingObject)
		{
			List<object> lst = getWindows(targetwidget, callingObject);
			foreach(object obj in lst)
			{
				Sharpend.Utils.Reflection.unHookDelegate(senderwidget,obj,eventname,functionname,"Single","Single");
			}	
			
			if (lst.Count == 0)
			{
				//TODO logging ??
				Console.WriteLine("found 0 windows while unhooking delegates");
			}
			
		}
		
		public static void hookDelegate(object callingObject, DelegateData dd)
		{			
			List<object> lst = new List<object>(10);
			if (String.IsNullOrEmpty(dd.SourceName))
			{
				lst.Add(callingObject);
			} else
			{
				lst = getWindows(dd.SourceName,callingObject);
			}
			
			//List<object> 
			foreach (object obj in lst)
			{
				hookDelegate(obj,dd.Target,dd.EventName,dd.FunctionName,callingObject);
			}
		}

		public static void unHookDelegate(object callingObject, DelegateData dd)
		{			
			List<object> lst = new List<object>(10);
			if (String.IsNullOrEmpty(dd.SourceName))
			{
				lst.Add(callingObject);
			} else
			{
				lst = getWindows(dd.SourceName,callingObject);
			}
			
			//List<object> 
			foreach (object obj in lst)
			{
				unHookDelegate(obj,dd.Target,dd.EventName,dd.FunctionName,callingObject);
			}
		}


		
		public static void hookDelegates()
		{
			addWindows();
			foreach(KeyValuePair<String,DockableWidget> kp in MainWindow.Instance.WindowList)
			{
				kp.Value.hookDelegates();	
			}
		}
		
		public static void addWindows(Container parent=null)
		{
			if (parent == null)
			{
				addWindows(MainWindow.Instance);	
			} else
			{
				DockableWidget wnd = parent as DockableWidget;
				if (wnd != null)
				{
					if (!MainWindow.Instance.WindowList.ContainsKey(wnd.ID))
					{
						MainWindow.Instance.WindowList.Add(wnd.ID,wnd);
						wnd.hookDelegates();
					}
				}
				
				if (parent.Children != null)
				{
					foreach (Widget w in parent.Children)
					{
						Container c = w as Container;
						if (c != null)
						{
							addWindows(c);
						}
					}
				}
			}
		}
		
		
		public void createWindow(String classname)
		{
		   	//String configfile = Configuration.ConfigurationManager.AppSettings["windows_config"];	
		   	XmlNode nd = Configuration.ConfigurationManager.getValue("windows.config","//window[@class = '"+classname+"']");
			if (nd != null)
			{
			  DockableWidget wnd =  DockableWidget.CreateInstance(nd);
			  addWidget(wnd);
			} else
			{
				//TODO exception or logging ??
				Console.WriteLine("Window " + classname + " has no entry in windows.config");
			}
		}

		//TODO nameing ... functionnames upper or lowercase ?
		public void createPopupWindow(String classname)
		{
		   	//String configfile = Configuration.ConfigurationManager.AppSettings["windows_config"];	
		   	XmlNode nd = Configuration.ConfigurationManager.getValue("windows.config","//window[@class = '"+classname+"']");
			if (nd != null)
			{
			  	DockableWidget wnd =  DockableWidget.CreateInstance(nd);
			  	//addWidget(wnd);
				showAsPopupWindow(wnd);
			} else
			{
				//TODO exception or logging ??
				Console.WriteLine("Window " + classname + " has no entry in windows.config");
			}
		}

		/// <summary>
		/// show a dockable widget as popup window
		/// </summary>
		/// <param name='widget'>
		/// Widget.
		/// </param>
		public void showAsPopupWindow(DockableWidget widget)
		{
			if (!windowlist.ContainsKey(widget.ID))
			{
				PopupWindow pw = new PopupWindow(widget);
				  
				windowlist.Add(widget.ID,widget);
				pw.SetSizeRequest(640,480);
				pw.WindowPosition = WindowPosition.CenterOnParent;
				pw.Destroyed += HandlePwDestroyed;
				pw.ShowAll();	
			}
		}

		/// <summary>
		/// called when a popup window is destoryed ... then we have to remove it from the windowlist
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandlePwDestroyed (object sender, EventArgs e)
		{
			PopupWindow pw = (sender as PopupWindow);
			if (pw != null)
			{
				DockableWidget dw = (pw.CurrentWidget as DockableWidget);
				if (dw != null)
				{
					windowlist.Remove(dw.ID);
				}
			}
		}
		
		public void createPopupWindow(ParameterSet param)
		{
			DockableWidget wnd =  DockableWidget.createWindow(param,null);
			//addWidget(wnd);
			PopupWindow pw = new PopupWindow(wnd);
			
			windowlist.Add(wnd.ID,wnd);
			pw.SetSizeRequest(640,480);
			pw.WindowPosition = WindowPosition.CenterAlways;
			pw.Destroyed += HandlePwDestroyed;
			pw.Shown += HandlePwShown;
			pw.ShowAll();
		}

		void HandlePwShown (object sender, EventArgs e)
		{
			PopupWindow pw = sender as PopupWindow;
			if (pw != null)
			{
				if ((pw.CurrentWidget as DockableWidget).ID == "progresswindow")
				{
					if (OnProgressBarShown != null)
					{
						OnProgressBarShown(sender,e);
					}
				}
			}
		}
		
		/// <summary>
		/// creates a new window
		/// </summary>
		/// <param name='sender'>
		/// sender must be a commandtoolbutton where eventargs contain the classname of the window
		/// </param>
		/// <param name='args'>
		/// Arguments.
		/// </param>
		public void CreateWindow(object sender, EventArgs args)
        {
			CommandToolButton btn = sender as CommandToolButton;
			if (btn != null)
			{
				createWindow(btn.EventArgs);
			}
		}

		/// <summary>
		/// create a new popup window
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='args'>
		/// Arguments.
		/// </param>
		public void CreatePopupWindow(object sender, EventArgs args)
        {
			CommandToolButton btn = sender as CommandToolButton;
			if (btn != null)
			{
				createPopupWindow(btn.EventArgs);
			}
		}
		
		protected override void OnDestroyed ()
		{
//TODO do we need this ??

//			List<String> keys = new List<String>();
//			foreach(KeyValuePair<String,DockableWidget> kp in this.WindowList)
//			{
//				keys.Add(kp.Key);
//			}
						
//			foreach (String s in keys)
//			{
//				Widget w = WindowList[s];
//				WindowList.Remove(s);
//				//w.Parent = null;
//				//w.Parent
//				Console.WriteLine("before");
//				w.Destroy();
//				Console.WriteLine("after");
//			}
			
			base.OnDestroyed ();
		}
		
		protected override bool OnDestroyEvent (Gdk.Event evnt)
		{
			return base.OnDestroyEvent (evnt);
		}
		
		/// <summary>
		/// xml serialization
		/// </summary>
		/// <param name='writer'>
		/// Writer.
		/// </param>
		protected virtual void doWriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("assembly",CurrentAssemblyName);
			
			writer.WriteAttributeString("CurrentWidth",this.CurrentWidth.ToString());
			writer.WriteAttributeString("CurrentHeight",this.CurrentHeight.ToString());
			writer.WriteAttributeString("Maximized",this.Maximized.ToString());
			
			writer.WriteStartElement("frame");
			this.frame.WriteXml(writer);
			writer.WriteEndElement();
			
		}

		/// <summary>
		/// xml deserialization
		/// </summary>
		/// <param name='reader'>
		/// Reader.
		/// </param>
		protected virtual void doReadXml(XmlReader reader)
		{
			StoredWidth = Convert.ToInt32(reader["CurrentWidth"]);
			StoredHeight = Convert.ToInt32(reader["CurrentHeight"]);
			Maximized = Convert.ToBoolean(reader["Maximized"]);
			
			while(reader.LocalName != "frame" && reader.Read());
			
			if (reader.LocalName == "frame")
	        {
				if (!reader.IsEmptyElement)
				{
					reader.Read();
					String tp = reader.LocalName + "," + reader["assembly"];
					Widget w = (Widget)Sharpend.Utils.Reflection.createInstance(System.Type.GetType(tp));
					frame = (DockFrame)w;
					frame.ReadXml(reader);
					box1.PackEnd(frame,true,true,0);
				} else
				{
					reader.Read(); //TODO if and else reader.read
				}
			}
		}

		private void showProgressWindow(String id, String text, int count)
		{
			if (progress == null)
			{
				ParameterSet ps = new ParameterSet("Sharpend.GtkSharp.ProgressWindow","Sharpend");
				ps.addParameter(typeof(String),text);
				ps.addParameter(typeof(int),count);
				createPopupWindow(ps);
				progress = (ProgressWindow)windowlist["progresswindow"];

			} else
			{
				progress.Show();
			}
		}



		public void initProgress(String id,String text,int count)
		{
			showProgressWindow(id,text,count);
		}


		public void PulseProgress(String id)
		{

			GLib.Timeout.Add (100, delegate {
					progress.pulse();
				    return false;
                 });
		}

		public Task doWork(System.Func<bool> func)
		{		
			//log.Debug("start task" + func.GetType().ToString() );
			Console.WriteLine("start task" + func.GetType().ToString());
			return Task.Factory.StartNew(func).ContinueWith(
				(task) => { workCompleted(task); }
				);	
		}
		
		private bool  workCompleted(Task<bool> ar)
		{
			Console.WriteLine("finished: " + ar.GetType());

			if (ar is Task)
			{
				if (OnWorkDone != null)
				{
					OnWorkDone(this,new EventArgs());
				}
			}
			Console.WriteLine("end finished");
			return true;
		}


		
		#region IXmlSerializable implementation
		public System.Xml.Schema.XmlSchema GetSchema ()
		{
			return null;
		}

		/// <Docs>
		/// To be added: an object of type 'System.Xml.XmlReader'
		/// </Docs>
		/// <remarks>
		/// To be added
		/// </remarks>
		/// <summary>
		/// xml serialization
		/// </summary>
		/// <param name='reader'>
		/// Reader.
		/// </param>
		public void ReadXml (XmlReader reader)
		{
			if (reader.LocalName == this.GetType().Name)
			{
				reader.Read();
			}

			doReadXml(reader);
		}

		/// <Docs>
		/// To be added: an object of type 'System.Xml.XmlWriter'
		/// </Docs>
		/// <remarks>
		/// To be added
		/// </remarks>
		/// <summary>
		/// xml deserialization
		/// </summary>
		/// <param name='writer'>
		/// Writer.
		/// </param>
		public void WriteXml (XmlWriter writer)
		{
			writer.WriteStartElement(this.GetType().ToString());
			doWriteXml(writer);
			writer.WriteEndElement();
		}
		#endregion
	} //class
} //namespace

#endif