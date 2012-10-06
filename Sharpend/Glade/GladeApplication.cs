//
// GladeApplication.cs
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
using Gdk;
using Sharpend.Configuration;

namespace Sharpend.Gui
{
	/// <summary>
	/// Base class for a Glade application. This is deprecated
	/// </summary>
	public class GladeApplication
	{
		protected Gtk.Window MainWindow;
		
		/// <summary>
		/// Gets or sets the width of the current window.
		/// </summary>
		/// <value>
		/// The width of the current window.
		/// </value>
		public int CurrentWidth {
			get;
			protected set;
		}
		
		/// <summary>
		/// Gets or sets the height of the current window.
		/// </summary>
		/// <value>
		/// The height of the current window.
		/// </value>
		public int CurrentHeight {
			get;
			protected set;
		}
		
		/// <summary>
		/// Gets or sets the root_ x.
		/// </summary>
		/// <value>
		/// The root_ x.
		/// </value>
		public int Root_X {
			get;
			protected set;
		}
		
		/// <summary>
		/// Gets or sets the root_ y.
		/// </summary>
		/// <value>
		/// The root_ y.
		/// </value>
		public int Root_Y {
			get;
			protected set;
		}
		
		/// <summary>
		/// Gets the name of the current assembly.
		/// </summary>
		/// <value>
		/// The name of the current assembly.
		/// </value>
		public String CurrentAssemblyName {
			get
			{
				String name = this.GetType().Assembly.FullName;
				String[] l = name.Split(',');
				return l[0];
			}
		}
		
		/// <summary>
		/// only for serialization
		/// </summary>
		public GladeApplication ()
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Sharpend.Gui.GladeApplication"/> class.
		/// </summary>
		/// <param name='doInitialize'>
		/// Do initialize.
		/// </param>
		public GladeApplication (bool doInitialize)
		{
			if (doInitialize)
			{
				initialize();
			}
		}
				
		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public virtual void initialize()
		{
			Application.Init();
			
			String gladexml = ConfigurationManager.AppSettings["mainwindow_xml"];
			String mainwindowname = ConfigurationManager.AppSettings["mainwindow_name"];
			
			if (String.IsNullOrEmpty(gladexml))
			{
				throw new Exception("Missing configuration entry in app.config: mainwindow_xml");
			}
			
			if (String.IsNullOrEmpty(mainwindowname))
			{
				throw new Exception("Missing configuration entry in app.config: mainwindow_name");
			}
			
			//Glade.XML gxml = new Glade.XML(gladexml,mainwindowname,null);	
			//MainWindow = gxml.GetWidget(mainwindowname) as Gtk.Window;
			
			if (MainWindow == null)
			{
				throw new Exception("could not load the mainwindow : " + mainwindowname + " from " + gladexml);
			}
			
			MainWindow.Destroyed += new EventHandler(MainWindow_Destroyed);
			
			//MainWindow.SizeRequested += HandleMainWindowSizeRequested;			
			MainWindow.WidgetEventAfter += HandleMainWindowWidgetEventAfter;	
			//MainWindow.VisibilityNotifyEvent += HandleMainWindowVisibilityNotifyEvent;
			
			//gxml.Autoconnect(this);		
		}


		void HandleMainWindowWidgetEventAfter (object o, WidgetEventAfterArgs args)
		{
			int root_x;
			int root_y;
			
			MainWindow.GetPosition(out root_x,out root_y);
			Root_X = root_x;
			Root_Y = root_y;		
		}

		
//		void HandleMainWindowSizeRequested (object o, SizeRequestedArgs args)
//		{			
//			int width;
//			int height;
//			MainWindow.GetSize(out width,out height);
//									
//			CurrentWidth = width;
//			CurrentHeight = height;	
//		}
		
		
		/// <summary>
		/// called before Application.Quit();
		/// </summary>
		protected virtual void beforeApplicationQuit()
		{
		}
		
		protected void MainWindow_Destroyed(object sender, EventArgs e)
        {
			beforeApplicationQuit();
			Application.Quit();
		}
		
	}
}

