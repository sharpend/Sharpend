//
// GtkListTreeView.cs
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
using System.Collections.Generic;
using Sharpend.Utils;
using System.Collections;

namespace Sharpend.GtkSharp
{
    public class RowChangedEventArgs : EventArgs
    {
        public VirtualGridRow ChangedRow { get; private set; }
        public String ColumnName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        public RowChangedEventArgs(VirtualGridRow row, String columnName)
            : base()
        {
            ChangedRow = row;
            ColumnName = columnName;
        }
    }
	
	struct EditableColumn
	{
		public Gtk.CellRenderer Cellrenderer;
		public Gtk.TreeViewColumn Column;
		public String ColumnName;
	}

	/// <summary>
	/// 
	/// This a TreeList Model Implementation for the Gtk.Treeview control
	/// Id like to change it to a "real" TreeModel implementation but currently
	/// i'm getting errors with the XmlTreeModel implementation
	/// 
	/// So i'm using the TreeStore Model 
	/// 
	/// </summary>
	public class GtkListTreeView : StandardGrid
	{
        private Dictionary<VirtualGridRow, Gtk.TreeIter> TreeIterators = null;
        public EventHandler AfterEdit = null;
		public EventHandler Toggled = null;
        public Gtk.TreeCellDataFunc OnTreeCellData = null;
        private ArrayList columns;
		
        /// <summary>
        /// wenn true werden pixbufs auch in der zeile angezeigt (sonst nur als tree knoten)
        /// </summary>
        public bool PixBufInRow { get; set; }
		public bool SkipFirstChildrow {
			get;
			set;
		}
		
        private class TreeListCellRendererText : Gtk.CellRendererText
        {
            public int ColumnIndex { get; set; }
            public String ColumnName { get; set; }
            public TreeListCellRendererText(String columnname, int columnindex)
                : base()
            {
                ColumnName = columnname;
                ColumnIndex = columnindex;
            }
        }

        public Gtk.TreeView Tree {
			get;
			protected set;
		}
		
		public GtkListTreeView (Gtk.TreeView tree) : base()
		{
			Tree = tree;
			SkipFirstChildrow = false;
		}
		
		protected override void addHeaderColumn (VirtualGridHeaderColumn headercolumn)
		{
            base.addHeaderColumn(headercolumn);
                            
            Gtk.TreeViewColumn column = new Gtk.TreeViewColumn();
            column.Visible = headercolumn.Visible;
            column.Title = headercolumn.ColumnName;
            column.Resizable = true;
            Gtk.CellRendererText artistNameCell = new Gtk.CellRendererText();
            //Gtk.CellRendererCombo artistNameCell = new Gtk.CellRendererCombo();
			
			column.PackStart(artistNameCell, true);
            Tree.AppendColumn(column);

            String attribute = "text";
            if (headercolumn.CustomOption.Equals("pixbuf", StringComparison.OrdinalIgnoreCase))
            {
                attribute = "pixbuf";
            }

            if (OnTreeCellData != null)
            {
                column.SetCellDataFunc(artistNameCell, OnTreeCellData);
            }

            column.AddAttribute(artistNameCell, attribute, HeaderColumns.Count - 1);            
		}
		
		public void reloadColumns()
		{
			int i=0;
				
			columns = new ArrayList(HeaderColumns.Count);
			List<EditableColumn> editablecolums = new List<EditableColumn>(10);
			foreach (VirtualGridHeaderColumn c in getHeaderColumns())
			{                
                Gtk.TreeViewColumn column = new Gtk.TreeViewColumn();
                column.Visible = c.Visible;
                column.Title = c.ColumnName;
                column.Resizable = true;
                Gtk.CellRenderer cell;
                TreeListCellRendererText treecellrenderer = null;
                if (c.CustomOption.Equals("pixbuf", StringComparison.OrdinalIgnoreCase))
                {
                    cell = new Gtk.CellRendererPixbuf ();
                }
				else if (!String.IsNullOrEmpty(c.Renderer))
				{
					Type tp = Type.GetType(c.Renderer);
					cell = (Gtk.CellRenderer)Sharpend.Utils.Reflection.createInstance(tp);
				}
                else
                {
                    treecellrenderer = new TreeListCellRendererText(c.ColumnName, i);
                    cell = treecellrenderer;
                }
                       
				columns.Add(cell);
                column.PackStart(cell, true);
                Tree.AppendColumn(column);

                String attribute = "text";
                if (c.CustomOption.Equals("pixbuf", StringComparison.OrdinalIgnoreCase))
                {
                    attribute = "pixbuf";
                }

                if (OnTreeCellData != null)
                {
                    column.SetCellDataFunc(treecellrenderer, OnTreeCellData);
                }

                column.AddAttribute(cell, attribute, i);
					
				if (cell is Gtk.CellRendererToggle)
				{
					(cell as Gtk.CellRendererToggle).Toggled += HandleToggled;
					(cell as Gtk.CellRendererToggle).Activatable = true;
					(cell as Gtk.CellRendererToggle).Mode = Gtk.CellRendererMode.Activatable;
					column.AddAttribute(cell,"active",i);
				}
				
                if ((treecellrenderer != null) && (c.Editable))
                {  
					if (String.IsNullOrEmpty(c.EditableColumn))
					{
						treecellrenderer.Editable = true;
					} else
					{
						editablecolums.Add(new EditableColumn{ColumnName=c.EditableColumn, Cellrenderer = treecellrenderer,Column = column});
					}
					
					treecellrenderer.Edited += new Gtk.EditedHandler(cell_Edited);
					treecellrenderer.EditingStarted += HandleTreecellrendererEditingStarted;
                }

                i++;                
			}
			
			foreach(EditableColumn c in editablecolums)
			{
				int idx = getColumnIndex(c.ColumnName);
				c.Column.AddAttribute(c.Cellrenderer,"editable",idx);
			}
		}

		void HandleToggled (object o, Gtk.ToggledArgs args)
		{	
			int column = columns.IndexOf (o);			
 			Gtk.TreeIter iter;
			
 			if (Tree.Model.GetIterFromString (out iter, args.Path)) {
 				bool val = (bool) Tree.Model.GetValue (iter, column);
				Tree.Model.SetValue (iter, column, !val);
				
				VirtualGridRow row = getGridRow(iter);
                if (row != null)
                {
					String colname = row.getColumnName(column);              
					row.setData(colname,!val, true);                    
					if (Toggled != null)
					{
						Toggled(this,new RowChangedEventArgs(row,colname));
					}
				}
 			}
		}

		void HandleTreecellrendererEditingStarted (object o, Gtk.EditingStartedArgs args)
		{
		  
		}

        /// <summary>
        /// called when a cell was edited ... sets the data for the virtualgridcell and the model in the gtk tree store
        /// </summary>
        /// <param name="o"></param>
        /// <param name="args"></param>
        void cell_Edited(object o, Gtk.EditedArgs args)
        {
            TreeListCellRendererText cell = o as TreeListCellRendererText;
            if (cell != null)
            {
                Gtk.TreeIter iter;
                Tree.Model.GetIter(out iter, new Gtk.TreePath(args.Path));

                VirtualGridRow row = getGridRow(iter);
                if (row != null)
                {
                    row.setData(cell.ColumnName, args.NewText, true);                    
                    Tree.Model.SetValue(iter, cell.ColumnIndex, args.NewText);

                    if (AfterEdit != null)
                    {
                        AfterEdit(this, new RowChangedEventArgs(row,cell.ColumnName));
                    }
                }
            }
        }
		
        /// <summary>
        /// returns a new Gtk.Treestore with the headercolums as columns in the store
        /// </summary>
        /// <returns></returns>
		protected Gtk.TreeStore getTreeStore()
		{
			Type[] types = new Type[HeaderColumns.Count + 1];
			
			int i=0;
			foreach (VirtualGridHeaderColumn col in getHeaderColumns())
			{
                if (!String.IsNullOrEmpty(col.ColumnType))
				{
					types[i] = Type.GetType(col.ColumnType);
					i++;
				} else
				{
					if (col.CustomOption.Equals("pixbuf", StringComparison.OrdinalIgnoreCase))
	                {
	                    types[i] = typeof(Gdk.Pixbuf); //TODO load types from headercolumn ... at the moment only string
	                    i++;
	                }
	                else
	                {
	                    types[i] = typeof(String); //TODO load types from headercolumn ... at the moment only string
	                    i++;
	                }
				}
			}

            types[i] = typeof(VirtualGridRow);

			return new Gtk.TreeStore(types);		
		}
		
		protected Gtk.TreeStore getTreeStoreWithData()
		{
			Gtk.TreeStore ts = getTreeStore();
			
			ts.DefaultSortFunc = null;
			
			if (this.Root != null)
			{
				addData(ts,Gtk.TreeIter.Zero,this.Root.RootRow,false);
			} else
			{					
				
				//startStopWatch("AppendValues");
				
				foreach (VirtualGridRow row in Rows)
				{				
					//String[] values = row.Values;						
					                    
					ts.AppendValues(row.Datas);
				}
				
				//ts.AppendValues(
				
				//stopStopWatch("AppendValues");
			}
			return ts;
		}
		
		protected bool renderImage(VirtualTreeRow row, VirtualTreeRow childrow)
		{
//			if ((row.Cells[0].CustomOption != null) && (row.Cells[0].CustomOption.Equals("nopic")))
//			{
//				return false;
//			}
			if (childrow.BaseRow != null)
			{
				VirtualGridCell cell = childrow.BaseRow.getCell(row.HeaderColumn);
				if (cell != null)
				{
					if (cell.CustomOption.Equals("nopic",StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
				}
			}
									
			if (row.HeaderColumn.CustomOption.Equals("pixbuf", StringComparison.OrdinalIgnoreCase))
            {
				return true;
			}	
			return false;
		}
		
		protected void addData(Gtk.TreeStore store, Gtk.TreeIter iter, VirtualTreeRow row, bool withiter)
		{						
			//Console.WriteLine("->"  + row.NodeValue);			
			foreach (KeyValuePair<String,VirtualTreeRow> kp in row.Children)
			{
				//GroupedChildRows grp = kp.Value;
				VirtualTreeRow childrow = kp.Value;
				
				Gtk.TreeIter it;
				bool wi = withiter;
				if (! withiter)
				{
                    if (renderImage(row, childrow))
                    {                      
                        it = store.AppendValues(childrow.DataWithImage);
					
                        wi = true;
                    }
                    else
                    {
                        it = store.AppendValues(childrow.Data);
                        wi = true;
                    }
				} else
				{
//                    if (renderImage(row, childrow))
//                    {
//                        it = store.AppendValues(iter, childrow.DataWithImage);
//                    }
//                    else
//                    {
//                        
//                    }
					it = store.AppendValues(iter, childrow.Data);
				}
								
				addData(store,it,childrow,wi);									
			} //foreach
			
			//add the data into a row
			if (row.HeaderColumn.ChildColumn == null)
			{								
				//startStopWatch("AppendValues");
				if ((row.Rows != null) && (row.Rows.Count > 1)) //TODO Count > 1 check .. or option ??
				{
					//foreach (VirtualGridRow vr in row.Rows)
					int idx = 0;
					if (SkipFirstChildrow)
					{
						idx = 1;
					}
					
					for (int i=idx;i<row.Rows.Count;i++)
					{
						VirtualGridRow vr = row.Rows[i];
	                    bool tmp = LoadPixBuf;
	                    if (!PixBufInRow)
	                    {
	                        LoadPixBuf = false;
	                    }
	                    store.AppendValues(iter,vr.Datas);
	                    LoadPixBuf = tmp;
					}
				}
				//stopStopWatch("AppendValues");
			}
			
			
		}
		
		public void removeColumns()
		{
			foreach (Gtk.TreeViewColumn c in Tree.Columns)
			{
				Tree.RemoveColumn(c);
			}
		}
		
		
		public void reload()
		{
			Tree.Model = null;
			if (Tree.Model != null)
			{
				(Tree.Model as Gtk.TreeStore).Clear();
				
			}
			removeColumns();
			reloadColumns();
            
			if ((Root != null) && (Root.RootRow != null))
			{			
				Root.RootRow.clearRows();
                Root.RootRow.clearChildren();
				Root.RootRow.clearChache(true);
			}
           									
			Gtk.TreeStore ts = getTreeStoreWithData();
			
			Tree.Model = ts;
		}
		
		public override void loadData (string filename)
		{
			//Console.WriteLine("start GtkListTreeView.loadData");
			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			
			base.loadData (filename);
			
			if (Tree.Model != null)
			{
				(Tree.Model as Gtk.TreeStore).Clear();
				
			}
			removeColumns();
			reloadColumns();
			
			Gtk.TreeStore ts = getTreeStoreWithData();
			
			Tree.Model = ts;
			
			sw.Stop();
			
			//Console.WriteLine("GtkListTreeView.loadData Time: " + sw.ElapsedMilliseconds.ToString());	
		}

        public virtual VirtualGridRow getGridRow(Gtk.TreeIter iter)
        {
            object o = Tree.Model.GetValue(iter, this.HeaderColumns.Count);
            if (o != null)
            {
                return o as VirtualGridRow;
            }
            return null;
        }

        public override VirtualGridRow getSelectedRow()
        {
			List<VirtualGridRow> lst = getSelectedRows();
			if (lst.Count > 0)
			{
				return lst[0];
			}
			return null;
        }

        public override List<VirtualGridRow> getSelectedRows()
        {
            List<VirtualGridRow> ret = new List<VirtualGridRow>(50);

            Gtk.TreeIter iter;
            Gtk.TreePath[] lst = Tree.Selection.GetSelectedRows();
                        
            foreach (Gtk.TreePath pt in lst)
            {
                Tree.Model.GetIter(out iter, pt);
                VirtualGridRow rw = getGridRow(iter);
                if (rw != null)
                {
                    ret.Add(rw);
                }
            }

            return ret;
        }

        public bool foreachItem(Gtk.TreeModel mode, Gtk.TreePath path, Gtk.TreeIter iter)
        {                        
            VirtualGridRow cr = getGridRow(iter);
            if (cr != null)
            {
                TreeIterators.Add(cr, iter); 
            }
            return false;
        }

        public bool getIter(VirtualGridRow row, out Gtk.TreeIter iter)
        {
            if (TreeIterators != null)
            {
                if (TreeIterators.ContainsKey(row))
                {
                    iter = TreeIterators[row];
                    return true;
                }                                
            }

            TreeIterators = new Dictionary<VirtualGridRow, Gtk.TreeIter>(1000);
            Tree.Model.Foreach(new Gtk.TreeModelForeachFunc(foreachItem));
            if (TreeIterators != null)
            {
                if (TreeIterators.ContainsKey(row))
                {
                    iter = TreeIterators[row];
                    return true;
                }
            }
            iter = Gtk.TreeIter.Zero;
            return false;
        }

        private int getColumnIndex(VirtualGridRow row, string columnName)
        {
            int i = 0;
			
			foreach (VirtualGridHeaderColumn hc in row.Grid.HeaderColumns)
			{
				if (hc.ColumnName.Equals(columnName,StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
				i++;
			}
			
//            foreach (VirtualGridCell c in row.Cells)
//            {
//                if (c.HeaderColumn.ColumnName.Equals(columnName,StringComparison.OrdinalIgnoreCase))
//                {
//                    return i;
//                }
//                i++;
//            }
            return -1;
        }
		
			
	
        public override void setData(VirtualGridRow row, string columnName, string data)
        {
            base.setData(row, columnName, data);

            Gtk.TreeIter iter;
            getIter(row, out iter);

            int cnt = getColumnIndex(row, columnName);
            if (cnt > -1)
            {
                Tree.Model.SetValue(iter, cnt, data);
            }
        }

        public void test()
        {
            Gtk.TreeIter iter;

            Tree.Model.GetIterFirst(out iter);

        }

        

        public override void selectRow(VirtualGridRow row)
        {
            Tree.Selection.Mode = Gtk.SelectionMode.Single;

            Tree.Selection.UnselectAll();

            Gtk.TreeIter iter;
            if (getIter(row,out iter))
            {
                Tree.Selection.SelectIter(iter);                
                Gtk.TreePath[] sel = Tree.Selection.GetSelectedRows();
                if (sel.Length > 0)
                {
                    Gdk.Rectangle rect = Tree.GetCellArea(sel[0], Tree.Columns[0]);
                    Tree.ScrollToPoint(rect.X, rect.Y);
                }
    
            }            
        }
		
		
		public override bool removeRows(List<VirtualGridRow> rows)
        {
			Gtk.TreeStore ts = Tree.Model as Gtk.TreeStore;
            if (ts != null)
            {
                Gtk.TreeIter iter;
				
				foreach (VirtualGridRow rw in rows)
				{
					if (getIter(rw, out iter))
                	{
                    	ts.Remove(ref iter);
					}
				}
				
				base.removeRows(rows);
			}
			return true; //TODO return value
		}
		
        /// <summary>
        /// removes the row from the grid and the treemodel
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public override bool removeRow(VirtualGridRow row)
        {
            Gtk.TreeStore ts = Tree.Model as Gtk.TreeStore;
            if (ts != null)
            {
                Gtk.TreeIter iter;
                if (getIter(row, out iter))
                {
                    ts.Remove(ref iter);
                    return base.removeRow(row);
                }
            }
            return false;
        }

		
	}
}

