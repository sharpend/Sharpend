//
// TaskCompleted.cs
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

namespace Sharpend
{
	public enum TaskCompletedState
	{
		None = 0,
		Success = 1,
		Error = 2
	}
	
	
	public class TaskCompleted
	{
		public String Message {
			get;
			private set;
		}
		
		public TaskCompletedState State {
			get;
			private set;
		}
		
		public String TaskId {
			get;
			private set;
		}
		
		public TaskCompleted (String taskid, TaskCompletedState state, String message)
		{
			TaskId = taskid;
			State = state;
			Message = message;
		}
	}
}

