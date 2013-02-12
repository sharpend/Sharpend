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

		public DatePicker(DateTime datetime, bool nodatebutton, bool selectrange)
			: this (datetime, nodatebutton)
		{
			SelectRange = selectrange;

			if (SelectRange)
			{
				Calc2 = new Gtk.Calendar();
				Calc2.DaySelectedDoubleClick += HandleDaySelectedDoubleClick;
				Gtk.Separator sep = new Gtk.Separator(Gtk.Orientation.Vertical);
				sep.WidthRequest = 20;
				HBox.PackStart(sep,true,true,0);
				HBox.PackEnd(Calc2,true,true,0);
			}
		}

		public bool SelectRange {
			get;
			private set;
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
		/// 2nd Calculator if you have a range selection
		/// </summary>
		/// <value>
		/// The calc2.
		/// </value>
		public Gtk.Calendar Calc2 {
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

		/// <summary>
		/// 2nd date if you select a date range
		/// </summary>
		/// <value>
		/// The date2.
		/// </value>
		public DateTime Date2 {
			get
			{
				return Calc2.Date;
			}
			set 
			{
				Calc2.Date = value;
			}
		}

		/// <summary>
		/// VBox containing calendar(s) and nodate button
		/// </summary>
		/// <value>
		/// The V box.
		/// </value>
		public Gtk.Box VBox {
			get;
			private set;
		}

		/// <summary>
		/// HBox containing the calendars
		/// </summary>
		/// <value>
		/// The H box.
		/// </value>
		public Gtk.Box HBox {
			get;
			private set;
		}

		/// <summary>
		/// if true, a button for a datetime.minvalue selection will added
		/// </summary>
		/// <value>
		/// <c>true</c> if no date button; otherwise, <c>false</c>.
		/// </value>
		public bool NoDateButton {
			get;
			private set;
		}

		/// <summary>
		/// Occurs when a selection per doubleclick is made
		/// </summary>
		public event EventHandler OnChanged;

		/// <summary>
		/// initialize the widegt
		/// </summary>
		private void init()
		{
			this.Expand = true;
			SetSizeRequest(400,200);
			NoDateButton = false;
			VBox = new Gtk.Box(Gtk.Orientation.Vertical,0);
			HBox = new Gtk.Box(Gtk.Orientation.Horizontal,0);

			Calc = new Gtk.Calendar();
			//Calc.DisplayOptions = Gtk.CalendarDisplayOptions.ShowDetails;
			//Calc.SelectionNotifyEvent += HandleSelectionNotifyEvent;
			Calc.ShowAll();
			//Calc.StateChanged += HandleStateChanged;
			HBox.PackStart(Calc,true,true,0);
			VBox.PackEnd(HBox,true,true,0);

			Calc.DaySelectedDoubleClick += HandleDaySelectedDoubleClick;

			VBox.ShowAll();
			this.Add(VBox);
		}

		void HandleDaySelectedDoubleClick (object sender, EventArgs e)
		{
			if (OnChanged != null)
			{
				OnChanged(this,new EventArgs());
			}

			this.Close();
		}

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
		/// Shows as popup.
		/// </summary>
		/// <returns>
		/// The as popup.
		/// </returns>
		/// <param name='date'>
		/// Date.
		/// </param>
		/// <param name='nodatebutton'>
		/// Nodatebutton.
		/// </param>
		/// <param name='range'>
		/// Range.
		/// </param>
		public static PopupWindow ShowAsPopup(DateTime date,bool nodatebutton,bool range)
		{
			return new PopupWindow(new DatePicker(date,nodatebutton,range));
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

