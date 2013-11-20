//
//  WebServiceControl.cs
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
using System.ServiceModel;

namespace TaskManager
{
    //(Namespace = "http://localhost:9050")
	[ServiceBehavior]
	public class WebServiceControl : IWebserviceControl
	{

		private ITaskManager _dep;

		public WebServiceControl (ITaskManager dep)
		{
			_dep = dep;
		}

		#region IWebserviceControl implementation

		public string StartTask (string classname, string parameters)
		{
			return _dep.StartTask (classname, parameters);
		}

		public string GetTaskStatus (string classname)
		{
			return _dep.GetTaskStatus(classname);
		}

		public string WaitForTask (string classname)
		{
			return _dep.WaitForTask(classname);
		}

		#endregion


	}
}

