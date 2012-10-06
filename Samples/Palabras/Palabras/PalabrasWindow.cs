//
// PalabrasWindow.cs
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
	 
	  
	  /******************************************
			THIS IS AUTO GENERATED CODE BY GLADEBUILDER
			DO NOT EDIT
			USE THE IMPLEMENTATION CLASS INSTEAD
	  *******************************************/
	  namespace Palabras{public partial class PalabrasWindow: Gtk.Window{private Gtk.Box box1;private Gtk.Label lblWord;private Gtk.Expander expander1;private Gtk.Box box2;private Gtk.Label lblTranslated;private Gtk.Box box4;private Gtk.Entry entry1;private Gtk.Button btnSearch;private Gtk.Label label1;private Gtk.Box box3;private Gtk.Button btnWeb;private Gtk.Button btnNewWord;public PalabrasWindow() : this(String.Empty) {}public PalabrasWindow(String name) : base(name){this.Name = name;box1 = new Gtk.Box(Orientation.Vertical,0);box1.Name ="box1";box1.Visible =true;lblWord = new Gtk.Label();lblWord.Name ="lblWord";lblWord.Visible =true;lblWord.Text ="label";expander1 = new Gtk.Expander("Übersetzung");expander1.Name ="expander1";expander1.Visible =true;box2 = new Gtk.Box(Orientation.Vertical,0);box2.Name ="box2";box2.Visible =true;lblTranslated = new Gtk.Label();lblTranslated.Name ="lblTranslated";lblTranslated.Visible =true;lblTranslated.Text ="label";box4 = new Gtk.Box(Orientation.Horizontal,0);box4.Name ="box4";box4.Visible =true;entry1 = new Gtk.Entry();entry1.Name ="entry1";entry1.Visible =true;btnSearch = new Gtk.Button();btnSearch.Name ="btnSearch";btnSearch.Visible =true;btnSearch.Label ="Suche";box4.PackStart(entry1,true,true,0);box4.PackStart(btnSearch,false,true,0);box2.PackStart(lblTranslated,true,true,0);box2.PackStart(box4,false,true,0);label1 = new Gtk.Label();label1.Name ="label1";label1.Visible =true;label1.Text ="Übersetzung";expander1.Add(box2);expander1.Add(label1);box3 = new Gtk.Box(Orientation.Horizontal,0);box3.Name ="box3";box3.Visible =true;btnWeb = new Gtk.Button();btnWeb.Name ="btnWeb";btnWeb.Visible =true;btnWeb.Label ="Web";btnNewWord = new Gtk.Button();btnNewWord.Name ="btnNewWord";btnNewWord.Visible =true;btnNewWord.Label ="Weiter";box3.PackStart(btnWeb,false,true,0);box3.PackStart(btnNewWord,false,true,0);box1.PackStart(lblWord,false,true,0);box1.PackStart(expander1,true,true,0);box1.PackStart(box3,false,true,0);this.Add(box1);this.WindowPosition=WindowPosition.Mouse;this.Opacity=0.84999999999999998;this.WidthRequest=240;this.HeightRequest=150;init();} //constructor} //class} //namespace