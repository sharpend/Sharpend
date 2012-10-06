//
// SampleWindow.cs
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
	  namespace DockApplication{public partial class SampleWindow
					: DockableWidget{private Gtk.Box box1;public Gtk.Box Box1{get{return box1;}}private Gtk.Frame frame1;public Gtk.Frame Frame1{get{return frame1;}}private Gtk.Alignment alignment1;public Gtk.Alignment Alignment1{get{return alignment1;}}private Gtk.Label label2;public Gtk.Label Label2{get{return label2;}}private Gtk.Label label1;public Gtk.Label Label1{get{return label1;}}private Gtk.Box box2;public Gtk.Box Box2{get{return box2;}}private Gtk.Label label3;public Gtk.Label Label3{get{return label3;}}private Gtk.Entry entry1;public Gtk.Entry Entry1{get{return entry1;}}private Gtk.Box box3;public Gtk.Box Box3{get{return box3;}}private Gtk.Label label4;public Gtk.Label Label4{get{return label4;}}private Gtk.Entry entry2;public Gtk.Entry Entry2{get{return entry2;}}private Gtk.TextView textview1;public Gtk.TextView Textview1{get{return textview1;}}public SampleWindow() : this(String.Empty) {}public SampleWindow(String name) : base(name){this.Name = name;box1 = new Gtk.Box(Orientation.Vertical,0);box1.Name ="box1";box1.Visible =true;frame1 = new Gtk.Frame();frame1.Name ="frame1";frame1.Visible =true;alignment1 = new Gtk.Alignment(0.50999999046325684f,0.50999999046325684f,0.99000000953674316f,0.99000000953674316f);alignment1.Name ="alignment1";alignment1.Visible =true;alignment1.TopPadding =2;alignment1.BottomPadding =2;alignment1.LeftPadding =12;alignment1.RightPadding =2;label2 = new Gtk.Label();label2.Name ="label2";label2.Visible =true;label2.Text =@"You can simply create your own window with glade.
When you want to use it in a DockApplication you can use 
GladeBuilder to create c# code from the .glade file.

";alignment1.Add(label2);label1 = new Gtk.Label();label1.Name ="label1";label1.Visible =true;label1.Text =@"<b>frame1</b>";frame1.Add(alignment1);frame1.Add(label1);box2 = new Gtk.Box(Orientation.Horizontal,0);box2.Name ="box2";box2.Visible =true;label3 = new Gtk.Label();label3.Name ="label3";label3.Visible =true;label3.Text =@"Input1";entry1 = new Gtk.Entry();entry1.Name ="entry1";entry1.Visible =true;box2.PackStart(label3,false,true,0);box2.PackStart(entry1,false,true,0);box3 = new Gtk.Box(Orientation.Horizontal,0);box3.Name ="box3";box3.Visible =true;label4 = new Gtk.Label();label4.Name ="label4";label4.Visible =true;label4.Text =@"Input2";entry2 = new Gtk.Entry();entry2.Name ="entry2";entry2.Visible =true;box3.PackStart(label4,false,true,0);box3.PackStart(entry2,false,true,0);textview1 = new Gtk.TextView();textview1.Name ="textview1";textview1.Visible =true;box1.PackStart(frame1,false,true,0);box1.PackStart(box2,false,true,0);box1.PackStart(box3,false,true,0);box1.PackStart(textview1,true,true,0);this.Add(box1);init();} //constructor} //class} //namespace