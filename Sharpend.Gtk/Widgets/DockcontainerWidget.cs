//
// DockcontainerWidget.cs
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
using Sharpend.GtkSharp.Docking;

#if !GTK2
namespace Sharpend.GtkSharp
{
	/// <summary>
	/// DockcontainerWidget widget.
	/// 
	/// The DockcontainerWidget contains itself a DockContainer
	/// so that you can drag and drop other Dockable Widgets into this one
	/// 
	/// TODO there are some problems when you drag the dockcontainerwidget into itself
	/// 
	/// </summary>
	public class DockcontainerWidget : DockableWidget
	{
		private DockContainer dock;

		private Gtk.Box vbox1;
		/// <summary>
		/// Box containing the dockcontainer
		/// </summary>
		/// <value>
		/// The Box
		/// </value>
		public Gtk.Box VBox1
		{
			get
			{
				return vbox1;
			}
			private set
			{
				vbox1 = value;
			}
		}

		public DockContainer Dock {
			get
			{
				return dock;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Sharpend.GtkSharp.DockcontainerWidget"/> class.
		/// </summary>
		public DockcontainerWidget ()
		{
			init();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Sharpend.GtkSharp.DockcontainerWidget"/> class.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		public DockcontainerWidget (String name) : base(name)
		{
			init();
		}

		private void init()
		{
			VBox1 = new Gtk.Box(Orientation.Vertical,0);
			VBox1.Visible =true;
			this.Add(VBox1);

			dock = new DockContainer(null);
			//xxxdock.Name = "xxxdock";
			dock.Visible = true;
			dock.Expand = true;
			VBox1.PackEnd(dock,true,true,0);
		}

		/// <summary>
		/// read xml
		/// </summary>
		/// <param name='reader'>
		/// Reader.
		/// </param>
		protected override void doReadXml (System.Xml.XmlReader reader)
		{
			base.doReadXml (reader);

			while((reader.LocalName != "dock") && reader.Read());
			if ((reader.LocalName == "dock") && !reader.IsEmptyElement)
			{
				reader.Read(); //skip dock1
				dock.ReadXml(reader);
				dock.Visible = true;
			}

			reader.Read();
		}

		/// <summary>
		/// write xml.
		/// </summary>
		/// <param name='writer'>
		/// Writer.
		/// </param>
		protected override void doWriteXml (System.Xml.XmlWriter writer)
		{
			base.doWriteXml (writer);

			writer.WriteStartElement("dock");
			dock.WriteXml(writer);
			writer.WriteEndElement();
		}

	}
}

#endif