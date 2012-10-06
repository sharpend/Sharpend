//
// PalabrasRunner.cs
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
using Sharpend.Utils.TaskManager;
using Gtk;
using GLib;
using System.IO;
using System.Xml;
using NDesk.DBus;

namespace Palabras
{
	public class PalabrasRunner: ITask
	{
		private static log4net.ILog log;
		
		private static GLib.MainLoop mainloop;
		
		public String Id {
			get;
			private set;
		}
		
		public static PalabrasWindow PopupWindow {
			get;
			private set;
		}
		
		
		
		public PalabrasRunner (String name)
		{
			Sharpend.Utils.Utils.initLog4Net();
			log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
			log.Debug("hello i'm PalabrasRunner");
		}
		
		private static void showWindow()
		{
			log.Debug("showWindow");
			try {
				if (PopupWindow == null)
				{
					PopupWindow = new PalabrasWindow("asd");
					PopupWindow.Destroyed += HandlePopupWindowDestroyed;
				}
				log.Debug("before show");
				PopupWindow.Show();
				log.Debug("after show");
			} catch (Exception ex)
			{
				log.Error(ex);
			}
		}

		static void HandlePopupWindowDestroyed (object sender, EventArgs e)
		{
			log.Debug("HandlePopupWindowDestroyed");
			
			if (PopupWindow != null)
			{
				PopupWindow = null;
			}
			mainloop.Quit();
			//Application.Quit();
		}
		
		
		
		
		#region ITask implementation
		/*
		public string doWork ()
		{
			try
			{
				GType.Init();
				Application.Init ();
				
				GLib.Timeout.Add (10000, () => {
					showWindow();
					return false;
				});
				
				Application.Run();
				log.Debug("end doWork");
				
			} catch (Exception ex)
			{
				log.Error(ex);
			}
			return Name;
		}
		*/
		#endregion


		#region ITask implementation
		public void forceQuit ()
		{
			if (PopupWindow != null)
			{
				PopupWindow.Destroy();
			}
			mainloop.Quit();
			//Application.Quit();
		}
		#endregion

		#region ITask implementation
		public Sharpend.TaskCompleted doWork ()
		{
			try
			{
				GType.Init();
				//Application.Init ();
				
				mainloop = new GLib.MainLoop();
				
				GLib.Timeout.Add (10000, () => {
					showWindow();
					return false;
				});
				
				
				mainloop.Run();
				//Application.Run();
				log.Debug("end doWork");
				return new Sharpend.TaskCompleted(getId(),Sharpend.TaskCompletedState.Success,"Palabras ist fertig");
				
			} catch (Exception ex)
			{
				return new Sharpend.TaskCompleted(getId(),Sharpend.TaskCompletedState.Error,ex.ToString());
			}
		}

		public string getId ()
		{
			return Id;
		}
		
		public void setId (String id)
		{
			Id = id;
		}
		
		#endregion

		


	}
}

