//
// MyDockApplication.cs
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
using Sharpend.GtkSharp;

namespace DockApplication
{
	/// <summary>
	/// If you want to create a dockapplication you have to derive a class from MainWindow
	/// 
	/// here you can implement your own functions
	/// 
	/// </summary>
	public class MyDockApplication : MainWindow
	{

		/// <summary>
		/// Serialization only
		/// </summary>
		public MyDockApplication() : base()
		{
		}
	}
}

