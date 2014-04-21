//
// TaskData.cs
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
using Sharpend;
using Sharpend.Utils;

namespace TaskManager
{
	/// <summary>
	/// Task data.
	/// Contains Information for one Task specified in the tasks.config file
	/// </summary>
	public class TaskData
	{
		
		public String Name {
			get
			{
				return Classname  + "_" + Assembly;
			}
		}
		
		public DateTime NextRun {
			get;
			private set;
		}
		
		public int Intervall {
			get;
			private set;
		}
		
		public ParameterSet Params {
			get;
			internal set;
		}
		
        //public object CurrentInstance {
        //    get;
        //    set;
        //}
		
		public String Classname {
			get;
			private set;
		}
		
		public String Assembly {
			get;
			private set;
		}
		
		public bool ShouldRun {
			get
			{
				if (Startup)
				{
					return false;
				}

				if (NextRun < DateTime.Now)
				{
					return true;
				}
				return false;
			}	
		}
		
		public bool ExtraRun {
			get;
			set;
		}


		/// <summary>
		/// If true the task will only one time when the taskmanager is started
		/// </summary>
		/// <value>
		/// <c>true</c> if startup; otherwise, <c>false</c>.
		/// </value>
		public bool Startup 
		{
			get;
			private set;
		}

		public TaskData (DateTime nextRun, int intervall, ParameterSet ps,String classname, String assembly,bool startup)
		{
			NextRun = nextRun;
			Intervall = intervall;
			Params = ps;
			Classname = classname;
			Assembly = assembly;
			ExtraRun = false;
			Startup = startup;
		}

				
		public void IncNextRun()
		{
			if (ExtraRun)
				return;
			
			if (Intervall == -1)
			{
				NextRun = DateTime.MaxValue;
				return;
			}
			
			while(NextRun < DateTime.Now)
			{
				NextRun = NextRun.AddMinutes(Intervall);
			}
		}

		/// <summary>
		/// add new parameters for the task constructor 
		/// then the task will not use the parameters specified in the tasks.config
		/// </summary>
		/// <param name="parameters">Parameters.</param>
		public void addParams(String parameters)
		{
			//Params = ParameterSet.CreateInstance(Classname,Assembly,parameters);

			XmlDocument doc = new XmlDocument ();
			doc.LoadXml (parameters);
			XmlNode nd = doc.SelectSingleNode ("//params");
			if (nd != null) {
				Params = ParameterSet.CreateInstance(nd);
			}
		}
		
		/// <summary>
		/// create a TaskData instance from an xml node
		/// </summary>
		/// <returns>The instance.</returns>
		/// <param name="node">Node.</param>
		public static TaskData CreateInstance(XmlNode node)
		{
			String nextRun = XmlHelper.getAttributeValue(node,"nextrun");
			String intervall = XmlHelper.getAttributeValue(node,"intervall");
			String classname = XmlHelper.getAttributeValue(node,"class");
			String assembly = XmlHelper.getAttributeValue(node,"assembly");
			String su = XmlHelper.getAttributeValue(node,"startup");

			//Console.WriteLine("su" + su);
			bool startup = false;
			if (!String.IsNullOrEmpty(su))
			{
				startup = Convert.ToBoolean(su);
			}

			ParameterSet ps = ParameterSet.CreateInstance(node);
			
			return new TaskData(Convert.ToDateTime(nextRun),Convert.ToInt32(intervall),ps,classname,assembly,startup);
		}
		
		
				
	}
}

