//
// VirtualTreeList.cs
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


namespace Sharpend.Utils
{
	
	public enum ImageScaleings
	{
		NoScale = 0,
		Scale = 1
	}
	
	public class TreeListParameter
	{
		public TreeListParameter()
		{
			ImageScaleing = ImageScaleings.NoScale;
		}
		
		public ImageScaleings ImageScaleing {
			get;
			set;
		}
		
	}
	
	
	/// <summary>
	/// implements a virtual grid with columns, headers and rows
	/// </summary>

	public delegate void GetDataHandler(String columnname,object input, out object data);
	public delegate void GetComparisionDataHandler(String columnname,object input, out string data);
	public class VirtualTreeList
	{
		/// <summary>
		/// contains the header columns
		/// </summary>
		public List<VirtualGridHeaderColumn> HeaderColumns {
			get;
			set;
		}
		
		/// <summary>
		/// contains all rows in the grid
		/// </summary>
		public List<VirtualGridRow> Rows {
			get;
			set;
		}
		
		private VirtualGridHeaderColumn root = null;
		public VirtualGridHeaderColumn Root {
			get
			{
				return root;
			}			
			set
			{
				root = value;
                if (root != null)
                {
                    Root.RootRow = new VirtualTreeRow(this, value.ColumnName, String.Empty); //TODO constructor ?
                }
			}
		}

        /// <summary>
        /// soll pixbuf geladen werden
        /// </summary>
        //public bool LoadPixBuf { get; set; }
		
		private object locker = new object(); 
		//private System.Diagnostics.Stopwatch stopwatch;
		
		private Dictionary<String,System.Diagnostics.Stopwatch> watches = null;
		private List<String> logmessages = null;
		
		//TODO
		public void addLogMessage(String s)
		{
			Console.WriteLine(s);
			return;
			
//			if (logmessages == null)
//			{
//				logmessages = new List<string>(10000);
//			}
//			logmessages.Add(s);
		}
		
		public void showLog()
		{
			foreach (String s in logmessages)
			{
				Console.WriteLine(s);
			}
		}

		public void startStopWatch(String key)
		{
			addLogMessage("start: " +  key);
			
			if (watches == null)
			{
				watches = new Dictionary<string, System.Diagnostics.Stopwatch>(10);
			}
			
			if (watches.ContainsKey(key))
			{
				watches[key].Reset();
				watches[key].Start();	
			} else
			{
				System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
				watches.Add(key,sw);
				sw.Start();
			}
		}
		
		public void stopStopWatch(String key)
		{
			if (watches.ContainsKey(key))
			{
				System.Diagnostics.Stopwatch sw = watches[key];
				sw.Stop();
			
				addLogMessage(key + "Eplapsed: " + sw.ElapsedMilliseconds.ToString() + " ms");
			} else
			{
				addLogMessage("Stopwatch " + key + "does not exist");
			}
		}
		
		public TreeListParameter Parameter {
			get;
			set;
		}

		/// <summary>
		/// called before data will added in a tree cell
		/// e.g. if you have a gridcell containing a pixbuf <column type=type="Gdk.Pixbuf,gdk-sharp"... but you have
		/// loaded an xml with the filename inside you have to load the pixbuf
		/// from the filename in this handler
		/// </summary>
		public GetDataHandler OnGetData=null;
		/// <summary>
		/// this is used if you want to group columns for a TreeList
		/// if you have other objects then strings (e.g pixbuf see the above event)
		/// then you have to return a string for comparision for the TreeList
		/// e.g the filename of the pixbuf
		/// </summary>
		public GetComparisionDataHandler OnGetComparisionData=null;

		public VirtualTreeList ()
		{
			HeaderColumns = new List<VirtualGridHeaderColumn>(100);
			Rows = new List<VirtualGridRow>(100);
			Parameter = new TreeListParameter();
		}
		
		/// <summary>
		/// add a new headercolumn into the headercolumns list
		/// </summary>
		/// <param name="headercolumn">
		/// A <see cref="VirtualGridHeaderColumn"/>
		/// </param>
		protected virtual void addHeaderColumn(VirtualGridHeaderColumn headercolumn)
		{
			HeaderColumns.Add(headercolumn);
		}
		
		/// <summary>
		/// creates a new row and add's it to the grid
		/// </summary>
		/// <returns>
		/// The row.
		/// </returns>
		public virtual VirtualGridRow newRow()
		{
            if (Rows == null)
            {
                Rows = new List<VirtualGridRow>(1000);
            }
            
            VirtualGridRow row = new VirtualGridRow(this);
			addRow(row);
			return row;
		}
		
		/// <summary>
		/// Adds a row to the grid
		/// </summary>
		/// <param name='row'>
		/// Row.
		/// </param>
		protected virtual void addRow(VirtualGridRow row)
		{
			Rows.Add(row);
		}
		
		public VirtualGridHeaderColumn getHeaderByName(String name)
		{
			foreach (VirtualGridHeaderColumn col in HeaderColumns)
			{
				if (col.ColumnName.Equals(name,StringComparison.OrdinalIgnoreCase))
				{
					return col;
				}
			}
			return null;
		}

        public virtual VirtualGridRow getSelectedRow()
        {
            return null;
        }

        public virtual List<VirtualGridRow> getSelectedRows()
        {
            return null;
        }

        public virtual void selectRow(VirtualGridRow row)
        {
        }

        public virtual void selectRows(List<VirtualGridRow> rows)
        {
        }

        /// <summary>
        /// only used to override it in a derived class
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="data"></param>
        public virtual void setData(VirtualGridRow row, String columnName, String data)
        {
            row.setData(columnName, data);
        }
        
		/// <summary>
		/// called after row.setData to inform a derived grid about changes
		/// </summary>
		/// <param name='row'>
		/// Row.
		/// </param>
		/// <param name='columnName'>
		/// Column name.
		/// </param>
		/// <param name='data'>
		/// Data.
		/// </param>
		public virtual void afterSetData(VirtualGridRow row, String columnName, object data)
		{	
		}

		public List<VirtualGridHeaderColumn> getHeaderColumns()
		{
			List<VirtualGridHeaderColumn> lst = null;
			if (Root != null)
			{
				lst = Root.getChildColumns();
			}
			
			if (lst == null)
			{
				lst = new List<VirtualGridHeaderColumn>(100);
			}
			
			foreach (VirtualGridHeaderColumn c in HeaderColumns)
			{
				if (!lst.Contains(c))
				{
					lst.Add(c);
				}
			}
			return lst;						
		}

        public virtual bool removeRow(VirtualGridRow row)
        {
            return Rows.Remove(row);
        }
		
		public virtual bool removeRows(List<VirtualGridRow> rows)
		{
			lock (locker)
			{
				foreach (VirtualGridRow rw in rows)
				{
					removeRow(rw);
				}
			}
			return true;
		}
		
		
		/// <summary>
		/// returns the index of the column with the given name ... -1 if not found
		/// </summary>
		/// <returns>
		/// The column index.
		/// </returns>
		/// <param name='columnname'>
		/// Columnname.
		/// </param>
		public int getColumnIndex(string columnname)
		{
			for (int i=0;i<HeaderColumns.Count;i++)
			{
				VirtualGridHeaderColumn c = HeaderColumns[i];
				if (c.ColumnName.Equals(columnname,StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}
		
	}
}

