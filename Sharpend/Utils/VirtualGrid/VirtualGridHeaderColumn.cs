//
// VirtualGridHeaderColumn.cs
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
    /// <summary>
    /// a header column for the virtual grid ... a header column can have child columns to implement a tree in the grid
    /// </summary>
	public class VirtualGridHeaderColumn
	{
		public String ColumnName {
			get;
			protected set;
		}
		
				
		public VirtualGridHeaderColumn ParentColumn {
			get;
			set;		
		}
		
		protected VirtualTreeRow rootrow = null;
		public VirtualTreeRow RootRow {
			get
			{
				return rootrow;
			}
			set 
			{
				rootrow = value;
				rootrow.HeaderColumn = this;
				rootrow.Grid = this.Grid;
			}
		}
		
		public String ColumnType {
			get;
			set;
		}
		
		private VirtualGridHeaderColumn childcolumn;
		public VirtualGridHeaderColumn ChildColumn {
			get
			{
				return childcolumn;
			}
			set
			{
				childcolumn = value;
				value.ParentColumn = this;
			}
		}
		
		public VirtualTreeList Grid {
			get;
			protected set;
		}

        private bool visible = true;
        public bool Visible { get { return visible; } set { visible = value; } }

        private bool editable = false;
        public bool Editable { get { return editable; } set { editable = value; } }
		
		/// <summary>
		/// Name of column which define if editable or not
		/// </summary>
		/// <value>
		/// The editable column.
		/// </value>
		public String EditableColumn {
			get;
			set;
		}
		
        /// <summary>
        /// for external use ... if you have some custom options
        /// </summary>
        public String CustomOption { get; set; }
		
		public String Renderer {
			get;
			set;
		}
		
		public VirtualGridHeaderColumn (VirtualTreeList grid, String columnName)
		{
			Grid = grid;
			ColumnName = columnName;
            CustomOption = String.Empty;
			//Rows = new List<VirtualGridRow>(1000);
			
		}
		
		/*
		public void addRow(VirtualGridRow row)
		{
			if (!Rows.Contains(row))
			{
				Rows.Add(row);
			}
		}
		*/
		
		public bool isParentOf(VirtualGridHeaderColumn col)
		{
			if (col == this)
			{
				return true;
			}
			
			if (col.ParentColumn != null)
			{
				if (col.ParentColumn == this)
				{
					return true;
				}
				else 
				{
					return isParentOf(col.ParentColumn);
				}
			}
			
			return false;	
		}
		
		private static void addHeaderColumns(ref List<VirtualGridHeaderColumn> lst, VirtualGridHeaderColumn parent)
		{
			if (parent.ChildColumn != null)
			{
				lst.Add(parent.ChildColumn);
				addHeaderColumns(ref lst, parent.ChildColumn);
			}		
		}
		
		
		public List<VirtualGridHeaderColumn> getChildColumns()
		{
			List<VirtualGridHeaderColumn> ret = new List<VirtualGridHeaderColumn>(20);
			
			ret.Add(this);
			addHeaderColumns(ref ret, this);
			
			return ret;
		}
		
		
		
	}//class
} //namespace

