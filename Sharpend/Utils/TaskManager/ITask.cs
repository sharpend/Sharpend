//
// ITask.cs
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

namespace Sharpend.Utils.TaskManager
{
	/// <summary>
	/// Base interface for Taskmanager Tasks
	/// </summary>
	public interface ITask
	{	
        /// <summary>
        /// this ist the worker function
        /// </summary>
        /// <returns></returns>
		TaskCompleted doWork();
		void forceQuit();
		
        /// <summary>
        /// name of the task ... usually class and assemblyname ... will be set from the taskmanager
        /// </summary>
        String Name { get; set; }
        /// <summary>
        /// unique id for a running instance
        /// </summary>
        String Uid { get; set; }
        /// <summary>
        /// if true the taskmanager will start multiple instances from this task if required
        /// </summary>
        bool AllowMultipleInstances { get; }
	}
	
}

