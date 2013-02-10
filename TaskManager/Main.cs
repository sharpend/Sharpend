//
// Main.cs
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
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Sharpend;
using Sharpend.Utils;
using Sharpend.Utils.TaskManager;
using System.Collections.Generic;
using Mono.Unix;
using System.IO;

#if DBUS
using TaskManager.DBus;
using NDesk.DBus;
using Gtk;
#endif

namespace TaskManager
{	
		
	class MainClass
	{
		public static bool Running {
			get;
			private set;
		}
		
		// Catch SIGINT and SIGUSR1
		private static UnixSignal[] signals = new UnixSignal [] {
			new UnixSignal(Mono.Unix.Native.Signum.SIGINT),
			//new UnixSignal(Mono.Unix.Native.Signum.SIGKILL)
		    //new UnixSignal (Mono.Unix.Native.Signum.SIGINT),
		    new UnixSignal (Mono.Unix.Native.Signum.SIGUSR1),
			//new UnixSignal (Mono.Unix.Native.Signum.SIGKILL)
		};
		
		private static List<TaskData> tasks;
		private static Dictionary<String,TaskData> runningTasks;
		private static DateTime lastWriteTime;
		protected static log4net.ILog log;
		
		#if DBUS
		static private DBusRemoteControl rc;	
		#endif
		
		public static void Main (string[] args)
		{
			Sharpend.Utils.Utils.initLog4Net();
			log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
			
			log.Info("Welcome to the Taskmanager");

			log.Info("wait a bit");
			Thread.Sleep(10000);
			log.Info("ready");


			#if DBUS
			BusG.Init();
			rc = Sharpend.Utils.DBusBaseProxy<DBusRemoteControl>.Register<DBusRemoteControl>("eu.shaprend.taskmanager","/eu/shaprend/taskmanager");

			if (rc == null)
			{
				Sharpend.Utils.DBusBaseProxy<DBusRemoteControl>.ReleaseName("eu.shaprend.taskmanager");
				Sharpend.Utils.DBusBaseProxy<DBusRemoteControl>.UnRegister("/eu/shaprend/taskmanager");
				//Sharpend.Utils.DBusBaseProxy<DBusRemoteControl>.ReleaseName("eu.shaprend.taskmanager");
				rc = Sharpend.Utils.DBusBaseProxy<DBusRemoteControl>.Register<DBusRemoteControl>("eu.shaprend.taskmanager","/eu/shaprend/taskmanager");
			}

			if (rc == null)
			{
				throw new Exception("could not register dbus eu.shaprend.taskmanager");
			}

			rc.OnStartTask += HandleRcOnStartTask;
			#endif
			
			tasks = new List<TaskData>(100);
			runningTasks = new Dictionary<string, TaskData>(100);
						
			loadTasks();	
			
			Running = true;
			System.Threading.Thread s = new System.Threading.Thread(new ThreadStart(signalThread));
			s.Start();
			
#if !DBUS
			Thread t = new Thread(new ThreadStart(mainLoop));
			t.Start();
			t.Join();
#endif	
	
#if DBUS
			//GLib.MainLoop ml = new GLib.MainLoop();
			Application.Init();
			
			GLib.Timeout.Add (500, () => {
				mainLoop2();
				return true;
			});
			
			//ml.Run();
			Application.Run();
#endif		
			
			log.Info("End: TaskManager");
		}
		
#if DBUS
		static void HandleRcOnStartTask (string classname, String parameters)
		{
			log.Debug("HandleRcOnStartTask: " + classname);
			TaskData td = getTask(classname);
			if (td != null)
			{
				td.addParams(parameters);
				td.ExtraRun = true;
				runTask(td);
			} else
			{
				log.Error("could not find task: " + classname  + "  probably missing in tasks.config ??");
			}		 
		}
#endif
		
		public static Task startTask<T>(System.Func<T> func)
		{		
			log.Debug("start task" + func.GetType().ToString() );
			return Task.Factory.StartNew(func).ContinueWith(
				(task) => { callBack(task); }
				);	
		}
		
		private static void callBack<T>(Task<T> ar)
		{
			log.Debug("finished: " + ar.GetType());
			
			if (ar is Task)
			{
			  TaskCompleted tc = (ar.Result as TaskCompleted);
			  if (tc != null)
			  {
					try 
					{
						TaskData td = runningTasks[tc.TaskId];
						
						if (td.CurrentInstance != null)
						{
							(td.CurrentInstance as ITask).forceQuit();
							td.CurrentInstance = null;
						}
						
						td.IncNextRun();
						
						//if we are not running anymore we remove the tasks in the storeTasks function
						if (Running)
						{
							runningTasks.Remove(tc.TaskId);
							td.ExtraRun = false;
						}
						
#if DBUS
						rc.TaskFinished(tc.Message);
#endif
						
						switch (tc.State)
						{
							case TaskCompletedState.None:
							case TaskCompletedState.Error:
								log.Error(tc.Message);
								break;
							case TaskCompletedState.Success:
								log.Debug(tc.Message);
								break;
							default:
								throw new Exception("undefined state");
						}
						
					} catch (Exception ex)
					{
						log.Error("callBack:: " + ex.ToString()); //TODO logging
						throw;
					}
					
			  } else 
			  {
				throw new Exception("method should return an TaskCompleted object");
			  }				
			}
			
			log.Debug("end finished");
		}
		
		/// <summary>
		/// Stores the tasks.
		/// </summary>
		private static void storeTasks()
		{
			log.Debug("storeTasks");
			foreach (TaskData td in tasks)
			{
				//td.IncNextRun();
				
				if (!td.ExtraRun)
				{
					String[] cls = td.Id.Split('_');
					String xpath = "/tasks/task[(@class='" + cls[0] + "') and (@assembly = '"+cls[1]+"')]/@nextrun";
					Sharpend.Configuration.ConfigurationManager.setValue("tasks.config",xpath,td.NextRun.ToString());
				}
				
				try 
				{
					foreach (KeyValuePair<String,TaskData> kp in runningTasks)
					{
						if (kp.Value.CurrentInstance != null)
						{
							log.Debug("force");
							(kp.Value.CurrentInstance as ITask).forceQuit();
						}
					}
										
					runningTasks.Clear();	
				} catch (Exception ex)
				{
					log.Error("error");
					log.Error(ex);
				}
			}
		}
		
		
		/// <summary>
		/// Load the tasks from a config file
		/// </summary>
		private static void loadTasks()
		{
			if (Running)
			{
				FileInfo fi = Sharpend.Configuration.ConfigurationManager.getConfigFile("tasks.config");
				lastWriteTime = fi.LastWriteTime;
				
				XmlNodeList lst =  Sharpend.Configuration.ConfigurationManager.getValues("tasks.config","/tasks/task");				
				tasks.Clear();
				
				foreach (XmlNode nd in lst)
				{
					tasks.Add(TaskData.CreateInstance(nd));
				}
			}
		}
		
		private static void checkConfigFile()
		{
			if (Running)
			{
				FileInfo fi = Sharpend.Configuration.ConfigurationManager.getConfigFile("tasks.config");
				if (fi.LastWriteTime > lastWriteTime)
				{
					log.Debug("reload tasks");
					Sharpend.Configuration.ConfigurationManager.flushConfigfile("tasks.config");
					loadTasks();
				}
			}
		}
		
		/// <summary>
		/// returns a task by classname
		/// </summary>
		/// <returns>
		/// The task.
		/// </returns>
		/// <param name='classname'>
		/// Classname.
		/// </param>
		public static TaskData getTask(String classname)
		{
			String[] cls = null;
			if (classname.Contains(","))
			{
				cls = classname.Split(',');
			} else
			{
				cls = new string[1];
				cls[0] = classname;
			}
			
			if ((cls != null) && cls.Length > 0)
			{
				foreach (TaskData td in tasks)
				{
					if (td.Classname.Equals(cls[0],StringComparison.OrdinalIgnoreCase))
					{
						return td;
					}
				}
			}
			return null;
		}
				
		/// <summary>
		/// run a task if the task is not currently running
		/// </summary>
		/// <param name='td'>
		/// Td.
		/// </param>
		private static void runTask(TaskData td)
		{
			try
			{
				if (!runningTasks.ContainsKey(td.Id))
				{
					runningTasks.Add(td.Id,td);
					
					log.Debug("create new task: " + td.Params.BaseType.ToString() + " - " + td.Id);
					object o = Reflection.createInstance(td.Params);	
					if ((o != null) && (o is ITask))
					{
						 (o as ITask).setId(td.Id);
						 startTask<TaskCompleted>((o as ITask).doWork);
						 td.CurrentInstance = o;
					} else
					{
						log.Error("could not load task: " + td.Params.BaseType.ToString());
					}
				} else
				{
					//log.Debug("task is already running: " + td.Classname);
				}
			}
			catch (Exception ex)
			{
				if (runningTasks.ContainsKey(td.Id))
				{
					runningTasks.Remove(td.Id);
				}
				log.Error(ex.ToString()); //TODO log4Net ... throw ?? 
			}
		}
		
		/// <summary>
		/// check if we have a task which should run
		/// </summary>
		private static void checkTasks()
		{
			foreach (TaskData td in tasks)
			{
				if (td.ShouldRun)
				{
					runTask(td);
				}
			}
		}
		
		/// <summary>
		/// the main loop
		/// </summary>
		private static void mainLoop()
		{			
			while(Running)
			{
				try
				{				
					checkTasks();
					checkConfigFile();
					System.Threading.Thread.Sleep(500);
					
					//log.Debug("alive");
				} catch (Exception ex)
				{
					log.Error(ex.ToString()); //TODO log4net
				}
			}	
		}
		
		private static void mainLoop2()
		{			
			if (Running)
			{
				try
				{				
					checkTasks();
					checkConfigFile();
					//log.Debug("alive");
					
				} catch (Exception ex)
				{
					log.Error(ex.ToString()); //TODO log4net
				}
			}	
		}
		
		
		
		private static void stop()
		{
			Running = false;
			storeTasks();
			Application.Quit();
		}
		
		
		private static void signalThread()
		{
			while (Running) {
				// Wait for a signal to be delivered
				int index = UnixSignal.WaitAny (signals, -1);
				
				Mono.Unix.Native.Signum signal = signals [index].Signum;
				
				log.Info("signal: " + signal.ToString());
				// Notify the main thread that a signal was received,
				// you can use things like:
				//    Application.Invoke () for Gtk#
				//    Control.Invoke on Windows.Forms
				//    Write to a pipe created with UnixPipes for server apps.
				//    Use an AutoResetEvent
				
				// For example, this works with Gtk#	
				//Application.Invoke (delegate () { ReceivedSignal (signal);
				stop();
			}
		}
		
		
	} //class
} //namespace
