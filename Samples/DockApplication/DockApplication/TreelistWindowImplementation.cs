using System;
using Gtk;	 
using Sharpend.GtkSharp;
using System.Collections;
using Sharpend.Utils;

namespace DockApplication{
	// columns
		public enum Column
		{
			HolidayName,
			Alex,
			Havoc,
			Tim,
			Owen,
			Dave,

			Visible,
			World,
		}
	public partial class TreelistWindow
					: DockableWidget	{

		ArrayList columns = new ArrayList ();

		public GtkListTreeView TreeList {
			get;
			private set;
		}

		public void GetDataHandler(String columnname,object input, out object data)
		{
//			if (columnname.Equals("pic",StringComparison.OrdinalIgnoreCase))
//			{
//				data = "asd";
//			}
			data = input;
		}


		public void GetComparisionDataHandler(String columnname,object input, out string data)
		{
			if (input == null)
			{
				data = "asd";
			} else
			{
				data = "asd";
				return;
//				if (input is Gdk.Pixbuf)
//				{
//					data = "asd";
//				} else
				{
					data = (String)input;
				}
			}
		}
		public void init() 
		{
			//AddColumns(treeview1);
			//here we get an "GCHandle value belongs to a different domain linux" errors
			//treeview1.Model = new TreeModelAdapter(new Sharpend.GtkSharp.XmlTreeModel());
			TreeList = new GtkListTreeView(treeview1);
			TreeList.loadStructureFromRessource("DockApplication.tree.xml");
			TreeList.SkipFirstChildrow = true;
			TreeList.OnGetComparisionData += GetComparisionDataHandler;
			TreeList.OnGetData += GetDataHandler;
			//VirtualGridGetComparisionDataHandler(String columnname,object input, out string data);Row row = TreeList.newRow();
			//row.setData("Title","asd");
			//TreeList.reload(); //initialize the treemodel
			VirtualGridRow row = TreeList.newRow();
//			TreeList.setData(row,"Title","xxxx");
//
//			row = TreeList.newRow();
//			TreeList.setData(row,"Title","assdasd");
//
//			row = TreeList.newRow();
//			TreeList.setData(row,"Title","xxxx");
//
//			row = TreeList.newRow();

 			
			//row.setData("Title","und nun ??");
			//row.setData("Pic","picture");
			Gdk.Pixbuf pb = new Gdk.Pixbuf("/home/dirk/Projects/MusicBrowserV4/MusicBrowserV4/player.png");
			row.setData("Title","geht das auch ??");
			row.setData("Pic",pb);
			row.setData("canedit",true);
			


			//row.setData("Pic","pic0");

			row = TreeList.newRow();

			//row.setData("Pic","pic1");
			row.setData("Title","Die zweite");
			//row.setData("Pic",pb);
			//TreeList.Root = TreeList.HeaderColumns[1];


			//row = TreeList.newRow();
			//row.setData("Pic",pb);
			//row.setData("Pic","pic2");
			//row.setData("Title","Nummer drei");
			TreeList.Root = TreeList.HeaderColumns[0];

			TreeList.reload();



		//	Gtk.CellRendererPixbuf
			//TreeList.reload();
		}

		/// <summary>
		/// this is from the gtk-sharp samples
		/// </summary>
		/// <param name='treeView'>
		/// Tree view.
		/// </param>
		private void AddColumns (TreeView treeView)
		{
			CellRendererText text;
			CellRendererToggle toggle;

			// column for holiday names
			text = new CellRendererText ();
			text.Xalign = 0.0f;
			columns.Add (text);
			TreeViewColumn column = new TreeViewColumn ("Holiday", text,
								    "text", Column.HolidayName);
			treeView.InsertColumn (column, (int) Column.HolidayName);

			// alex column
			toggle = new CellRendererToggle ();
			toggle.Xalign = 0.0f;
			columns.Add (toggle);
			//toggle.Toggled += new ToggledHandler (ItemToggled);
			column = new TreeViewColumn ("Alex", toggle,
						     "active", (int) Column.Alex,
						     "visible", (int) Column.Visible,
						     "activatable", (int) Column.World);
			column.Sizing = TreeViewColumnSizing.Fixed;
			column.FixedWidth = 50;
			column.Clickable = true;
			treeView.InsertColumn (column, (int) Column.Alex);

			// havoc column
			toggle = new CellRendererToggle ();
			toggle.Xalign = 0.0f;
			columns.Add (toggle);
			//toggle.Toggled += new ToggledHandler (ItemToggled);
			column = new TreeViewColumn ("Havoc", toggle,
						     "active", (int) Column.Havoc,
						     "visible", (int) Column.Visible);
			treeView.InsertColumn (column, (int) Column.Havoc);
			column.Sizing = TreeViewColumnSizing.Fixed;
			column.FixedWidth = 50;
			column.Clickable = true;

			// tim column
			toggle = new CellRendererToggle ();
			toggle.Xalign = 0.0f;
			columns.Add (toggle);
			//toggle.Toggled += new ToggledHandler (ItemToggled);
			column = new TreeViewColumn ("Tim", toggle,
						     "active", (int) Column.Tim,
						     "visible", (int) Column.Visible,
						     "activatable", (int) Column.World);
			treeView.InsertColumn (column, (int) Column.Tim);
			column.Sizing = TreeViewColumnSizing.Fixed;
			column.FixedWidth = 50;
			column.Clickable = true;

			// owen column
			toggle = new CellRendererToggle ();
			toggle.Xalign = 0.0f;
			columns.Add (toggle);
			//toggle.Toggled += new ToggledHandler (ItemToggled);
			column = new TreeViewColumn ("Owen", toggle,
						     "active", (int) Column.Owen,
						     "visible", (int) Column.Visible);
			treeView.InsertColumn (column, (int) Column.Owen);
			column.Sizing = TreeViewColumnSizing.Fixed;
			column.FixedWidth = 50;
			column.Clickable = true;

			// dave column
			toggle = new CellRendererToggle ();
			toggle.Xalign = 0.0f;
			columns.Add (toggle);
			//toggle.Toggled += new ToggledHandler (ItemToggled);
			column = new TreeViewColumn ("Dave", toggle,
						     "active", (int) Column.Dave,
						     "visible", (int) Column.Visible);
			treeView.InsertColumn (column, (int) Column.Dave);
			column.Sizing = TreeViewColumnSizing.Fixed;
			column.FixedWidth = 50;
			column.Clickable = true;
		}

	} //class} //namespace