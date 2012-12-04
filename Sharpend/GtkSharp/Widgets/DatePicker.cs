//
//  DatePicker.cs
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

namespace Sharpend.GtkSharp
{
	/// <summary>
	/// A Datepicker Widget
	/// 
	/// Derived from DockableWidget so you can use it in a dockable application
	/// or if you use the ShowAsPopup function the datepicker opens as popup using a
	/// GtkSharp.PopupWindow
	/// 
	/// If you use ShowAsPopup use like:
	/// 
	/// PopupWindow pw = DatePicker.ShowAsPopup();
	/// (pw.CurrentWidget as DatePicker).OnChanged += ...
	/// pw.ShowAll();
	/// 
	/// </summary>
	public class DatePicker : DockableWidget
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Sharpend.GtkSharp.DatePicker"/> class.
		/// </summary>
		public DatePicker ()
		{
			init();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Sharpend.GtkSharp.DatePicker"/> class.
		/// </summary>
		/// <param name='datetime'>
		/// this DateTime will set to the picker
		/// </param>
		public DatePicker(DateTime datetime)
			: this ()
		{
			Calc.Date = datetime;
		}

		public DatePicker(DateTime datetime, bool nodatebutton)
			: this (datetime)
		{
			NoDateButton = nodatebutton;

			if (NoDateButton)
			{
				Gtk.Button btn = new Gtk.Button("None");
				btn.Clicked += HandleNoneClicked;
				btn.Visible = true;
				VBox.PackEnd(btn,true,true,0);
			}
		}

		void HandleNoneClicked (object sender, EventArgs e)
		{
			Date = DateTime.MinValue;
			if (OnChanged != null)
			{
				OnChanged(this,new EventArgs());
			}

			this.Close();
		}

		public DatePicker(String name) : 
			base(name)
		{
			init();
		}

		/// <summary>
		/// The Gtk.Calendar Widget
		/// </summary>
		/// <value>
		/// The calculate.
		/// </value>
		public Gtk.Calendar Calc
		{
			get;
			private set;
		}

		/// <summary>
		/// The selected date
		/// </summary>
		/// <value>
		/// The date.
		/// </value>
		public DateTime Date {
			get
			{
				return Calc.Date;
			}
			set 
			{
				Calc.Date = value;
			}
		}

		public Gtk.Box VBox {
			get;
			private set;
		}

		public bool NoDateButton {
			get;
			private set;
		}

		/// <summary>
		/// Occurs when a selection per doubleclick is made
		/// </summary>
		public event EventHandler OnChanged;

		private void init()
		{
			NoDateButton = false;
			VBox = new Gtk.Box(Gtk.Orientation.Vertical,0);

			Calc = new Gtk.Calendar();
			Calc.ShowAll();
			//Calc.StateChanged += HandleStateChanged;
			VBox.PackEnd(Calc,true,true,0);
			Calc.DaySelectedDoubleClick += HandleDaySelectedDoubleClick;

			VBox.ShowAll();
			this.Add(VBox);
		}

		void HandleDaySelectedDoubleClick (object sender, EventArgs e)
		{
			//Console.WriteLine("StateChanged" + Calc.Date.ToString());

			if (OnChanged != null)
			{
				OnChanged(this,new EventArgs());
			}

			this.Close();
		}

//		void HandleStateChanged (object o, Gtk.StateChangedArgs args)
//		{
//			//Console.WriteLine("StateChanged" + Calc.Date.ToString());
//
//		}

		/// <summary>
		/// Shows Datepicker as popup.
		/// </summary>
		/// <returns>
		/// The as popup.
		/// </returns>
		/// <param name='date'>
		/// Date.
		/// </param>
		public static PopupWindow ShowAsPopup(DateTime date)
		{
			return new PopupWindow(new DatePicker(date));
		}

		/// <summary>
		/// Shows datepicker as popup.
		/// </summary>
		/// <returns>
		/// The as popup.
		/// </returns>
		public static PopupWindow ShowAsPopup()
		{
			return new PopupWindow(new DatePicker());
		}

	}
}

