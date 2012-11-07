
	   using System;
	   using Gtk;
	 
		 using Sharpend.GtkSharp;
	  
	  
	  /******************************************
			THIS IS AUTO GENERATED CODE BY GLADEBUILDER
			DO NOT EDIT
			USE THE IMPLEMENTATION CLASS INSTEAD
	  *******************************************/
	  namespace DockApplication{public partial class Window1
					: DockableWidget{private Gtk.Box box1;public Gtk.Box Box1{get{return box1;}}private Gtk.Toolbar toolbar1;public Gtk.Toolbar Toolbar1{get{return toolbar1;}}private Gtk.Button button1;public Gtk.Button Button1{get{return button1;}}private Gtk.Button button2;public Gtk.Button Button2{get{return button2;}}private Gtk.TextView textview1;public Gtk.TextView Textview1{get{return textview1;}}public Window1() : this(String.Empty) {}public Window1(String name) : base(name){this.Name = name;box1 = new Gtk.Box(Orientation.Vertical,0);box1.Name ="box1";box1.Visible =true;toolbar1 = new Gtk.Toolbar();toolbar1.Name ="toolbar1";toolbar1.Visible =true;button1 = new Gtk.Button();button1.Name ="button1";button1.Visible =true;button1.Label =@"Show a Progressbar (test)";button2 = new Gtk.Button();button2.Name ="button2";button2.Visible =true;button2.Label =@"Show ext in another dockable widget";textview1 = new Gtk.TextView();textview1.Name ="textview1";textview1.Visible =true;box1.PackStart(toolbar1,false,true,0);box1.PackStart(button1,false,true,0);box1.PackStart(button2,false,true,0);box1.PackStart(textview1,true,true,0);this.Add(box1);init();} //constructor} //class} //namespace