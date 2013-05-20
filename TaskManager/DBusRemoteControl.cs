//
// DBusRemoteControl.cs
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
using DBus;
using System.Xml;

namespace TaskManager.DBus
{
#if DBUS
	
	public delegate void StartTaskHandler(String classname, String xmlparameters);
	
	public class DBusRemoteControl : MarshalByRefObject, IDBusRemoteControl
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		
		private const string Path = "/eu/shaprend/taskmanager";
		private const string Namespace = "eu.shaprend.taskmanager";
		
		public event StartTaskHandler OnStartTask;
		
		public DBusRemoteControl ()
		{
			
		}
		
								
		#region IDBusRemoteControl implementation
//		public void startTask (string classname, string[] parameters)
//		{
//			Console.WriteLine("startTask");
//			if (OnStartTask != null)
//			{
//				OnStartTask(classname,parameters);
//			}
//		}
//		#endregion
//
//		#region IDBusRemoteControl implementation
		public void startTask (string classname, String xmlparameters)
		{
			log.Debug("startTask: " + classname);
			if (OnStartTask != null)
			{
				OnStartTask(classname,xmlparameters);
			}
		}
		#endregion

		#region IDBusRemoteControl implementation
		public event TaskFinished OnTaskFinished;
		#endregion
		
		public void TaskFinished(string message)
		{
			//Console.WriteLine("TaskFinished: " + message);
			if (OnTaskFinished != null)
			{
				log.Debug("TaskFinished: " + message);
				OnTaskFinished(message);
			}
		}

		public string getTaskList()
		{
			log.Debug("get Tasklist");
			String ret = String.Empty;
			XmlNodeList lst = Sharpend.Configuration.ConfigurationManager.getValues("tasks.config", "//task");

			foreach (XmlNode nd in lst)
			{
				String classname = nd.Attributes["class"].Value;
				String assembly = nd.Attributes["assembly"].Value;

				String alias = String.Empty;
				XmlAttribute at = nd.Attributes["alias"];
				if (at != null)
				{
					alias = at.Value;
				}

				if (!String.IsNullOrEmpty(alias))
				{
					ret += alias;
				} else {
					ret += classname + "," + assembly + ";"; 
				}
				log.Debug("add task: " + classname);
			}

			return ret.Trim(';');
		}
	}
	
	
	
#endif
}

