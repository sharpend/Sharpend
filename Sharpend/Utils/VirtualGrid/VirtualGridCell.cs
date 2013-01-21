//
// VirtualGridCell.cs
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
namespace Sharpend.Utils
{
    /// <summary>
    /// a Grid Cell containing Data
    /// </summary>
	public class VirtualGridCell
	{
		public VirtualGridHeaderColumn HeaderColumn {
			get;
			protected set;
		}
		
		private object data;
		public object Data {
			get
			{
				return data;
			}
			set
			{
				if ((!String.IsNullOrEmpty(HeaderColumn.ColumnType)) && (value != null))
				{
					Type tp = Type.GetType(HeaderColumn.ColumnType);
					data = Convert.ChangeType(value,tp);
					
				} else
				{
					data = value;
				}
			}
		}
		
		public VirtualGridRow Row {
			get;
			protected set;
		}
		
		public VirtualTreeList Grid {
			get;
			protected set;
		}

        /// <summary>
        /// you can use this if you have editable cells ... you have to set it manually
        /// </summary>
        public bool Changed { get; set; }

        /// <summary>
        /// for external use ... if you have some custom options
        /// </summary>
        public String CustomOption { get; set; }

		public VirtualGridCell (VirtualTreeList grid, VirtualGridRow row, VirtualGridHeaderColumn headercolumn, object data)
		{
			HeaderColumn = headercolumn;
			Row = row;
			Grid = grid;
			Data = data;
            Changed = false;
            CustomOption = String.Empty;
		}
	}
}

