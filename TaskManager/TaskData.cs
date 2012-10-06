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
	public class TaskData
	{
		
		public String Id {
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
			private set;
		}
		
		public object CurrentInstance {
			get;
			set;
		}
		
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
		
		public TaskData (DateTime nextRun, int intervall, ParameterSet ps,String classname, String assembly)
		{
			NextRun = nextRun;
			Intervall = intervall;
			Params = ps;
			Classname = classname;
			Assembly = assembly;
			ExtraRun = false;
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
		
		public void addParams(String parameters)
		{
			Params = ParameterSet.CreateInstance(Classname,Assembly,parameters);
		}
		
		
		public static TaskData CreateInstance(XmlNode node)
		{
			String nextRun = XmlHelper.getAttributeValue(node,"nextrun");
			String intervall = XmlHelper.getAttributeValue(node,"intervall");
			String classname = XmlHelper.getAttributeValue(node,"class");
			String assembly = XmlHelper.getAttributeValue(node,"assembly");
			
			ParameterSet ps = ParameterSet.CreateInstance(node);
			
			return new TaskData(Convert.ToDateTime(nextRun),Convert.ToInt32(intervall),ps,classname,assembly);
		}
		
		
				
	}
}

