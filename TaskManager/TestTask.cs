//
// TestTask.cs
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
using System.Threading;

namespace TaskManager
{
	public class TestTask : ITask
	{
		public String Data {
			get;
			private set;
		}
		
		public String Id {
			get;
			private set;
		}
		
		public TestTask(String data)
		{
			
			Data = data;
		}
		
		/*
		public String doWork()
		{
			for (int i=0;i<100;i++)
			{
				//Console.WriteLine(Data.ToString() + "_" + i.ToString());
				Thread.Sleep(200);
			}
			Console.WriteLine("ich bin fertig" + Data.ToString());
			return Data;
		}
		*/

		
		public void forceQuit ()
		{
			throw new NotImplementedException ();
		}
		

		#region ITask implementation
//		Sharpend.TaskCompleted doWork ()
//		{
//			for (int i=0;i<100;i++)
//			{
//				//Console.WriteLine(Data.ToString() + "_" + i.ToString());
//				Thread.Sleep(200);
//			}
//			Console.WriteLine("ich bin fertig" + Data.ToString());
//			return new Sharpend.TaskCompleted(this.getId(), Sharpend.TaskCompletedState.Success,"");
//		}

		public string getId ()
		{
			throw new NotImplementedException ();
		}
		#endregion

		#region ITask implementation
		Sharpend.TaskCompleted ITask.doWork ()
		{
			for (int i=0;i<100;i++)
			{
				//Console.WriteLine(Data.ToString() + "_" + i.ToString());
				Thread.Sleep(200);
			}
			Console.WriteLine("ich bin fertig" + Data.ToString());
			return new Sharpend.TaskCompleted(this.getId(), Sharpend.TaskCompletedState.Success,"");
		}
		#endregion

		
		public void setId (string id)
		{
			this.Id = id;
		}
		
	}
}

