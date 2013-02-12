//
// ProgressWindow.cs
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
	  
	  
	  /******************************************
			THIS IS AUTO GENERATED CODE BY GLADEBUILDER
			DO NOT EDIT
			USE THE IMPLEMENTATION CLASS INSTEAD
	  *******************************************/
#if !GTK2
namespace Sharpend.GtkSharp{public partial class ProgressWindow
					: DockableWidget{private Gtk.ProgressBar progressbar1;public Gtk.ProgressBar Progressbar1{get{return progressbar1;}}public ProgressWindow() : this(String.Empty) {}public ProgressWindow(String name) : base(name){this.Name = name;progressbar1 = new Gtk.ProgressBar();progressbar1.Name ="progressbar1";progressbar1.Visible =true;progressbar1.PulseStep = Convert.ToDouble("0.01");this.WidthRequest=100;this.HeightRequest=100;			this.Add(progressbar1);init();} //constructor} //class} //namespace
#endif