//
//  TaskManager.cs
//
//  Author:
//       Dirk Lehmeier <sharpend_develop@yahoo.de>
//
//  Copyright (c) 2013 Dirk Lehmeier
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
using Mono.Unix;
using System.Collections.Generic;
using System.Collections.Concurrent;
using TaskManager.DBus;
using Sharpend;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Gtk;
using Sharpend.Utils.TaskManager;
using System.Xml;
using Sharpend.Utils;
using System.ServiceModel;

namespace TaskManager
{
	public class TaskManager : ITaskManager
	{

		#region IDependency implementation
		public string doStuff ()
		{
			log.Debug ("doStuff");
			return "done";
		}
		#endregion

		private static TaskManager _instance;
		/// <summary>
		/// Get a Singleton Instance of the Taskmanager
		/// </summary>
		/// <value>The instance.</value>
		public static TaskManager Instance
		{
			get 
			{
				if (_instance == null)
				{
					_instance = new TaskManager();
				}
				return _instance;
			}
		}

		/// <summary>
		/// Create a new Taskmanager instance
		/// </summary>
		/// <returns>The instance.</returns>
		public static TaskManager CreateInstance()
		{
			return Instance;
		}

		public static bool Running {
			get;
			private set;
		}
		
		// Catch SIGINT and SIGUSR1
		private static UnixSignal[] signals = new UnixSignal [] {
			new UnixSignal(Mono.Unix.Native.Signum.SIGINT),
			new UnixSignal(Mono.Unix.Native.Signum.SIGTERM),
			new UnixSignal(Mono.Unix.Native.Signum.SIGHUP),
			//new UnixSignal(Mono.Unix.Native.Signum.SIGKILL),
			//new UnixSignal(Mono.Unix.Native.Signum.SIGSTOP),
			//new UnixSignal (Mono.Unix.Native.Signum.SIGINT),
			new UnixSignal (Mono.Unix.Native.Signum.SIGUSR1),
			//new UnixSignal (Mono.Unix.Native.Signum.SIGKILL)
		};
		
		//private static ConcurrentBag<TaskData> tasks;
		private  List<TaskData> tasks;
		//private static Dictionary<String,TaskData> runningTasks;
		private  ConcurrentDictionary <String,TaskData> runningTasks;
		private  Dictionary<String,String> finishedTasks;

		private  DateTime lastWriteTime;
		protected  log4net.ILog log;
		
		#if DBUS
		static  DBusRemoteControl rc;
		static  DBusServer<DBusRemoteControl> dbusServer;
		#endif
		
		
		#if WSControl
		Thread wsThread;
		bool wsListening=false;
		#endif
		
		static bool startup = true;
		static bool shutdown = false;


		private TaskManager ()
		{
			init ();
		}

		private void init()
		{
			Sharpend.Utils.Utils.initLog4Net();
			log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
			
			FileInfo fi = Sharpend.Configuration.ConfigurationManager.getConfigFile("tasks.config");
			lastWriteTime = fi.LastWriteTime;
			
			
			//log.Info("wait a bit");
			//Thread.Sleep(10000); //TODO config
			//log.Info("ready");
			
			log.Info("Welcome to the Taskmanager");
			
			#if DBUS
			//			BusG.Init();		
			//			rc = Sharpend.Utils.DBusBaseProxy<DBusRemoteControl>.Register<DBusRemoteControl>("eu.shaprend.taskmanager","/eu/shaprend/taskmanager");
			//
			//			if (rc == null)
			//			{
			//				Sharpend.Utils.DBusBaseProxy<DBusRemoteControl>.ReleaseName("eu.shaprend.taskmanager");
			//				Sharpend.Utils.DBusBaseProxy<DBusRemoteControl>.UnRegister("/eu/shaprend/taskmanager");
			//				//Sharpend.Utils.DBusBaseProxy<DBusRemoteControl>.ReleaseName("eu.shaprend.taskmanager");
			//				rc = Sharpend.Utils.DBusBaseProxy<DBusRemoteControl>.Register<DBusRemoteControl>("eu.shaprend.taskmanager","/eu/shaprend/taskmanager");
			//			}
			//
			//			if (rc == null)
			//			{
			//				throw new Exception("could not register dbus eu.shaprend.taskmanager");
			//			}
			//
			//			rc.OnStartTask += HandleRcOnStartTask;
			dbusServer = new DBusServer<DBusRemoteControl>("eu.shaprend.taskmanager","/eu/shaprend/taskmanager");
			if (!dbusServer.Register())
			{
				throw new Exception("could not register dbus eu.shaprend.taskmanager");
			}
			rc = dbusServer.GetClient();
			//rc.OnStartTask = HandleRcOnStartTask; //TODO ?
			#endif
			
			tasks = new List<TaskData>();
			runningTasks = new ConcurrentDictionary<string, TaskData>();
			finishedTasks = new Dictionary<String,String> (50);

			Running = true;
			loadTasks();	
			System.Threading.Thread s = new System.Threading.Thread(new ThreadStart(signalThread));
			s.Start();

			#if WSControl
			wsListening = true;
			wsThread = new Thread(new ThreadStart(createHost));
			wsThread.Start();
			#endif

			#if !DBUS
			Thread t = new Thread(new ThreadStart(mainLoop));
			t.Start();
			t.Join();
			#endif	
			

			
			#if DBUS
			//GLib.MainLoop ml = new GLib.MainLoop();
			Application.Init();
			
			
			GLib.Timeout.Add (500, () => {
				return mainLoop2();
			});
			
			
			//ml.Run();
			Application.Run();
			dbusServer.UnRegister();
			#endif		

#if WSControl
			log.Info("shutdown wsListener");
			wsListening = false;
			wsThread.Join();
			log.Info("done shutdown wsListener");
#endif
				
			log.Info("End: TaskManager");
		}


#if DBUS
		void HandleRcOnStartTask (string classname, String parameters)
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

		public string startTask(String classname, String parameters)
		{
			log.Debug("startTask: " + classname + " - " + parameters);
			TaskData td = getTask(classname);
			if (td != null)
			{
				td.addParams(parameters);
				td.ExtraRun = true;
				runTask(td);
				return "done";
			} else
			{
				log.Error("could not find task: " + classname  + "  probably missing in tasks.config ??");
			}
			return "error";
		}

		public string GetTaskStatus (string classname)
		{
			TaskData td = getTask(classname);

			TaskData running;
			if (runningTasks.TryGetValue (td.Id, out running)) {
				return "running";
			}

			lock (finishedTasks) {
				if (finishedTasks.ContainsKey(td.Id))
				{
					return finishedTasks[td.Id];
				}
			}

			return "undefined";
		}
		
		public string WaitForTask (string classname)
		{
			log.Debug ("wait for task: " + classname);
			TaskData td = getTask(classname);
			while (true) {		
				if (finishedTasks.ContainsKey(td.Id))
				{
					log.Debug ("done: " + classname);
					return finishedTasks[td.Id];
				}
				Thread.Sleep(1000);
			}
		}


		public Task startTask<T>(System.Func<T> func)
		{		
			log.Debug("start task" + func.GetType().ToString() );
			return Task.Factory.StartNew(func).ContinueWith(
				(task) => { callBack(task); }
			);	
		}
		
		private void callBack<T>(Task<T> ar)
		{
			log.Debug("finished: " + ar.GetType());
			
			if (ar is Task)
			{
				TaskCompleted tc = (ar.Result as TaskCompleted);
				if (tc != null)
				{
					try 
					{
						log.Debug("try get task");
						TaskData td = runningTasks[tc.TaskId];
						log.Debug("finished: " + td.Classname);
						if (td.CurrentInstance != null)
						{
							td.CurrentInstance = null;
						}
						
						td.IncNextRun();
						
						if (td.ExtraRun)
						{
							log.Info("Extrarun: " + tc.Message);
						}
						//if we are not running anymore we remove the tasks in the storeTasks function
						//if (Running)
						//{
						//runningTasks.Remove(tc.TaskId);
						TaskData removed;
						runningTasks.TryRemove(tc.TaskId,out removed);
						td.ExtraRun = false;
						//}
						
						#if DBUS
						// rc.TaskFinished(tc.Message);  //TODO Taskfinished Evenet for DBus
						#endif


						String finished = tc.State.ToString();
						finished += "," + tc.Message;
						log.Debug("finished task:"  +removed.Id);
						if (finishedTasks.ContainsKey(removed.Id))
						{
							log.Debug("remove from list: " + removed.Id);
							lock (finishedTasks)
							{
								finishedTasks.Remove(removed.Id);
							}
						}

						lock (finishedTasks)
						{
							log.Debug("add finished task:" + removed.Id);
							finishedTasks.Add(removed.Id,finished);
						}

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
			} else
			{
				throw new Exception("this should not happen");
			}
			
			log.Debug("end finished");
		}
		
		/// <summary>
		/// Stores the tasks.
		/// </summary>
		private void storeTasks()
		{
			log.Debug("storeTasks, we have " +  runningTasks.Count + " running tasks");
			lock (tasks) {
				foreach (TaskData td in tasks) {
					//td.IncNextRun();
					
					if (!td.ExtraRun) {
						String[] cls = td.Id.Split ('_');
						String xpath = "/tasks/task[(@class='" + cls [0] + "') and (@assembly = '" + cls [1] + "')]/@nextrun";
						Sharpend.Configuration.ConfigurationManager.setValue ("tasks.config", xpath, td.NextRun.ToString ());
					}
				}	
			}
		}
		
		private void forceQuitRunningTasks()
		{
			log.Debug("forceQuitRunningTasks, we have " + runningTasks.Count + " running tasks.");
			try 
			{
				ConcurrentDictionary<String,TaskData> toStore = new ConcurrentDictionary<string, TaskData>();
				
				foreach (var kp in runningTasks)
				{
					if (kp.Value.CurrentInstance != null)
					{
						toStore.TryAdd(kp.Key,kp.Value);
					}
				}
				
				foreach (var kp in toStore)
				{
					if (kp.Value.CurrentInstance != null)
					{
						log.Debug("forcequit: " + kp.Value.Classname);
						
						(kp.Value.CurrentInstance as ITask).forceQuit();
					}
				}
				
				toStore.Clear();
			} catch (Exception ex)
			{
				//log.Error("error");
				log.Error(ex);
			}
		}
		
		/// <summary>
		/// Load the tasks from a config file
		/// </summary>
		private void loadTasks()
		{
			if (Running)
			{
				FileInfo fi = Sharpend.Configuration.ConfigurationManager.getConfigFile("tasks.config");
				lastWriteTime = fi.LastWriteTime;
				
				XmlNodeList lst =  Sharpend.Configuration.ConfigurationManager.getValues("tasks.config","/tasks/task");				
				
				lock(tasks) {
					tasks.Clear();
					
					foreach (XmlNode nd in lst)
					{
						tasks.Add(TaskData.CreateInstance(nd));
					}
				}
			}
		}
		
		private void checkConfigFile()
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
		public TaskData getTask(String classname)
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
				lock (tasks)
				{
					foreach (TaskData td in tasks)
					{
						if (td.Classname.Equals(cls[0],StringComparison.OrdinalIgnoreCase))
						{
							return td;
						}
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
		private void runTask(TaskData td)
		{
			try
			{
				if (!runningTasks.ContainsKey(td.Id))
				{
					runningTasks.TryAdd(td.Id,td);
					log.Debug("classname: " + td.Classname + "->asm:" + td.Assembly);
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
					TaskData removed;
					runningTasks.TryRemove(td.Id,out removed);
				}
				log.Error(ex.ToString()); 
			}
		}
		
		/// <summary>
		/// check if we have a task which should run
		/// </summary>
		private void checkTasks(bool isstartup)
		{
			lock (tasks)
			{
				foreach (TaskData td in tasks)
				{
					if (isstartup)
					{
						if (td.Startup)
						{
							log.Debug("start startup task" + td.Classname);
							runTask(td);
						}
					} 
					else
					{
						if (td.ShouldRun)
						{
							runTask(td);
						}
					}
				}
			}
		}
		
		/// <summary>
		/// the main loop
		/// </summary>
		private void mainLoop()
		{			
			while(Running || shutdown)
			{
				try
				{				
					if (startup)
					{
						log.Debug("check startup tasks");
						checkTasks(true);
						startup = false;
					}
					
					if (Running)
					{
						checkTasks(false);
						//checkConfigFile(); //TODO
					}
					
					if (shutdown)
					{
						if (runningTasks.Count == 0)
						{
							log.Debug("shutting down... we have no running tasks anymore");
							shutdown = false;
							
						}
					}
					
					System.Threading.Thread.Sleep(500);	
				} catch (Exception ex)
				{
					log.Error(ex.ToString());
				}
			}
			storeTasks();
			//Application.Quit();
		}
		
		private bool mainLoop2()
		{			
			if (Running  || shutdown)
			{
				try
				{	
					if (startup)
					{
						log.Debug("check startup tasks");
						checkTasks(true);
						startup = false;
					}
					
					if (Running)
					{
						checkTasks(false);
						//checkConfigFile(); //TODO
						//log.Debug("alive");
					}
					
					if (shutdown)
					{
						if (runningTasks.Count == 0)
						{
							log.Debug("shutting down... we have no running tasks anymore");
							shutdown = false;
						}
					}
					
				} catch (Exception ex)
				{
					log.Error(ex.ToString()); 
					return true;
				}
				return true;
			} else
			{
				log.Debug("mainloop2");
				storeTasks();
				Application.Quit();
				return false;
			}
		}
		
		
		
		private void stop()
		{
			Running = false;
			forceQuitRunningTasks();
			shutdown = true;
			log.Info("shutting down the taskmanager");
			//Application.Quit();
		}
		
		
		private void signalThread()
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
		
		#if WSControl
		
		/// <summary>
		/// creates a webservice host to control the taskmanager
		/// </summary>
		private void createHost() 
		{
			if (Environment.GetEnvironmentVariable("MONO_STRICT_MS_COMPLIANT") != "yes")
			{
				Environment.SetEnvironmentVariable("MONO_STRICT_MS_COMPLIANT", "yes");
			}
			
			using (WebServiceHost host = new WebServiceHost(this,typeof(WebServiceControl)))
			{
				// Enable metadata publishing.
				//				ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
				//				smb.HttpGetEnabled = true;
				//				//smb.MetadataExporter.
				//				//smb.MetadataExporter
				//				host.Description.Behaviors.Add(smb);
				//
				//				//host.AddServiceEndpoint(typeof(IHelloWorldService), new WSHttpBinding(), "");
				//
				//
				//				//host.Description.Behaviors.Add(new ServiceMetadataBehavior());
				//				//host.AddServiceEndpoint(typeof(IHelloWorldService),new NetTcpBinding(),"hello");
				//				host.AddServiceEndpoint(typeof(IMetadataExchange),MetadataExchangeBindings.CreateMexTcpBinding(),"mex");
				//
				
				host.Open();
				
				while (wsListening) 
				{
					Thread.Sleep(500);
				}
				
				host.Close();
			}
		}
		#endif

	}
}

