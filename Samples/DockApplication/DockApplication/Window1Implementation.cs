//
// Window1Implementation.cs
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
using Gtk;
using Sharpend.GtkSharp;
using System.Threading;


namespace DockApplication{	public partial class Window1
						: DockableWidget	{

		public event EventHandler OnShowText;
		public void init() 
		{
				button1.Clicked += HandleButton1Clicked;
				button2.Clicked += HandleButton2Clicked;
		}

		void HandleButton2Clicked (object sender, EventArgs e)
		{
			if (OnShowText != null)
			{
				OnShowText("This is some text",new EventArgs());
			}
		}

		void HandleButton1Clicked (object sender, EventArgs e)
		{	
				MainWindow.Instance.OnProgressBarShown += HandleOnProgressBarShown;
				MainWindow.Instance.initProgress("","asd",100);

				Console.WriteLine("ende");
		}

		private bool workIt()
		{
				for (int i=0;i<100;i++)
				{
					MainWindow.Instance.PulseProgress("");
					Console.WriteLine("xxx");
					Thread.Sleep(100);
				}
				return true;
		}

		void HandleOnProgressBarShown (object sender, EventArgs e)
		{
			MainWindow.Instance.doWork(workIt);
		}
	} //class} //namespace