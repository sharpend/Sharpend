//
// ProgressWindowImplementation.cs
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

namespace Sharpend.GtkSharp{	public partial class ProgressWindow: DockableWidget	{

		public String Text {
			get;
			private set;
		}

		public int Count {
			get;
			private set;
		}


		public String Test {
			get;
			set;
		}

		public void init() 
		{
		}
		public void doInit() 
		{
			this.ID = "progresswindow";

			if (Count < 1)
			{
				return;
			}

			if (progressbar1 == null)
			{
				throw new ArgumentNullException("progressbar1");
			}

			progressbar1.Text = Text;
			progressbar1.Fraction = 0;

			double ct = Convert.ToDouble(Count);
			double step = 1 / ct;
			progressbar1.PulseStep = step;

		}



		public ProgressWindow(String text, int count) : this()
		{
			Text = text;
			Count = count;
			doInit();
		}

		public void pulse()
		{
			//progressbar1.Pulse();
			progressbar1.Fraction += progressbar1.PulseStep;
		}
	} //class} //namespace