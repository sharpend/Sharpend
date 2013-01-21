
	   using System;
	   using Gtk;
	 
		 using Sharpend.GtkSharp;
	  
	  
	  /******************************************
			THIS IS AUTO GENERATED CODE BY GLADEBUILDER
			DO NOT EDIT
			USE THE IMPLEMENTATION CLASS INSTEAD
	  *******************************************/
	  namespace DockApplication{public partial class TreelistWindow
					: DockableWidget{private Gtk.TreeView treeview1;public Gtk.TreeView Treeview1{get{return treeview1;}}public TreelistWindow() : this(String.Empty) {}public TreelistWindow(String name) : base(name){this.Name = name;treeview1 = new Gtk.TreeView();treeview1.Name ="treeview1";treeview1.Visible =true;//omit GtkTreeSelectionthis.Add(treeview1);init();} //constructor} //class} //namespace