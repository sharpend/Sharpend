//
// VirtualTreeRow.cs
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
using System.Linq;

namespace Sharpend.Utils
{
	public class VirtualTreeRow
	{
		/// <summary>
		/// Nodename
		/// </summary>
		public String Name {
			get;
			protected set;
		}
		
		public String NodeValue {
			get;
			set;
		}

        public String CustomOption { get; set; }

        public object AsImage
        {
            get
            {
                try
                {
                    byte[] buf = VirtualGridRow.getImage(NodeValue,this.Grid);
                    return new Gdk.Pixbuf(buf);
                } catch
                {
                    return null;
                }
            }
        }
		
		public VirtualGridRow BaseRow {
			get;
			private set;
		}
		
		public object[] Data
		{
			get 
			{
				if (this.Rows.Count < 2)
				{
					return BaseRow.Datas;
				}
				
				object[] ret = new object[BaseRow.ColumnCount];
				
				if (BaseRow != null)
				{
					int i=0;
					foreach (object o in BaseRow.Datas)
					{
						ret[i] = o;
						i++;
					}
					
					//swap
					int idx = BaseRow.getCellIndex(Parent.Name);
					object tmp = ret[0];
					ret[0] = ret[idx];
					ret[idx] = tmp;
				}
				return ret;
				
				
				//VirtualGridRow row = FirstRow;
				/* old stuff
				if (BaseRow != null)
				{
					ret = new object[BaseRow.ColumnCount + 1];
					//ret = new object[1];
					ret[0] = NodeValue;
					
					int i=1;
					foreach (object o in BaseRow.Datas)
					{
						ret[i] = o;
						i++;
					}
				} else
				{
					ret = new object[1];
					ret[0] = NodeValue;
				}
				*/
				
				
//				int x=0; //TODO debug only
//				foreach (object o in ret)
//				{
//					if (o is String)
//					{
//						ret[x] = (o as String) + x.ToString();
//					}
//					x++;
//				}
				
				
				//return ret;
			}	
		}
		
		public object[] DataWithImage
		{
			get 
			{
				object[] ret;
				//VirtualGridRow row = FirstRow;
				if (BaseRow != null)
				{
					ret = new object[BaseRow.ColumnCount];
					//ret = new object[1];
					//ret[0] = AsImage;
					
					int i=0;
					foreach (object o in BaseRow.Datas)
					{
						ret[i] = o;
						i++;
					}
				} else
				{
					ret = new object[1];
					ret[0] = NodeValue;
				}
				return ret;
			}
		}
		
		
		public VirtualGridHeaderColumn HeaderColumn {
			get;
			set;
		}
		
		//List<VirtualTreeRow> children = null;
		Dictionary<String, VirtualTreeRow> children = null;
		public Dictionary<String, VirtualTreeRow> Children {
			get
			{
				if (children == null)
				{
					children = new Dictionary<String, VirtualTreeRow>(2000);
					loadChildren();
				}
				return children;
			}	
		}
		
		private VirtualGridRow FirstRow
		{
			get
			{
				if (rows != null)
				{
					return rows.First();
				}
				return null;
			}
		}
		
		private List<VirtualGridRow> rows = null;
		public List<VirtualGridRow> Rows {
			get
			{
				if (Parent == null)
				{
					return Grid.Rows;
				}
				
				if (rows == null)
				{
					rows = new List<VirtualGridRow>(1000);
					loadRows();
				}
				return rows;
			}	
		}
		
		public VirtualTreeList Grid {
			get;
			set;
		}
		
		public VirtualTreeRow Parent {
			get;
			set;
		}
		
		public VirtualTreeRow (VirtualTreeList grid, String name, String nodevalue)
		{
			Name = name;
			Grid = grid;
			NodeValue = nodevalue;
			//Children = new List<VirtualTreeRow>(100);
			//Rows = new List<VirtualGridRow>(100);
		}
		
		public VirtualTreeRow (VirtualTreeList grid, String name)
		{
			Name = name;
			Grid = grid;
			//Children = new List<VirtualTreeRow>(100);
			//Rows = new List<VirtualGridRow>(100);
		}
		
		/*
		public void addChild(VirtualTreeRow row)
		{
			Children.Add(row);
			row.Parent = this;
		}
		*/
		
		/*
		private bool containsItem(String nodevalue)
		{
			foreach(VirtualTreeRow row in Children)
			{
				if (row.NodeValue == nodevalue)
				{
					return true;
				}
			}
			return false;
		}
		*/
		
		private void addData(String nodename, String nodevalue, VirtualGridHeaderColumn headercolumn,VirtualGridRow baserow)
		{
			
            if ((!String.IsNullOrEmpty(nodevalue)) &&  (!children.ContainsKey(nodevalue)))
			{
				VirtualTreeRow tr = new VirtualTreeRow(Grid, headercolumn.ColumnName, nodevalue);
				tr.HeaderColumn = headercolumn; //TODO in constructor
				tr.Parent = this;
				tr.BaseRow = baserow;
				Children.Add(nodevalue,tr);
			}
		}
		
//		private void addRow(VirtualGridRow row)
//		{
//			if (!Rows.Contains(row))
//			{
//				//String val = row.getData(this.HeaderColumn);
//				//if (dt.Equals(
//				//Rows.Add(row);
//			}
//		}
		
		private static void clearCache(VirtualTreeRow row, bool recursiv)
		{
			row.clearRows();
			row.clearChildren();
			
			if (recursiv)
			{
				if (row.Children != null)
				{
					foreach (KeyValuePair<String, VirtualTreeRow> kp in row.Children)
					{
						VirtualTreeRow rw = kp.Value;
						clearCache(rw,recursiv);
					}
				}
			}	
		}
		
		public void clearChache(bool recursiv)
		{	
			clearCache(this,recursiv);
		}
		
		public void clearRows()
		{
			rows = null;
		}
		
		public void clearChildren()
		{
			children = null;
		}
		
		private void loadRows()
		{
			if (Parent != null)
			{
				//Console.WriteLine("prnt");
				foreach (VirtualGridRow row in Parent.Rows)
				{
					String val = (String)row.getData(Name); //TODO convert
					//Console.WriteLine("val2:" + val + "-" + NodeValue);
                    if (val != null)
                    {
                        if (val.Equals(NodeValue, StringComparison.OrdinalIgnoreCase))
                        {
                            Rows.Add(row);
                        }
                    }
				}
			} else
			{
				foreach (VirtualGridRow row in Grid.Rows)
				{
					String val = String.Empty;
					if (Parent == null)
					{
						val = (String)row.getData(Name); //TODO convert
					} else
					{
						val = (String)row.getData(Parent.Name); //TODO convert
					}
					
					//Console.WriteLine("val:" + val + "-" + NodeValue);
					if (val.Equals(NodeValue,StringComparison.OrdinalIgnoreCase))
					{
						Rows.Add(row);
					}
				}
			}	
		}
		
		private void loadChildren()
		{
			Grid.startStopWatch("loadChildren");
			if (Parent != null)
			{
				//Console.WriteLine("--->" + Rows.Count);
                if (Parent.Rows != null)
                {
                    Grid.startStopWatch("childrows");
                    foreach (VirtualGridRow row in Parent.Rows)
                    {
                        //Grid.startStopWatch("singlerow");
                        if (HeaderColumn.ChildColumn != null)
                        {
                            String check = (String)row.getData(this.HeaderColumn); //TODO convert ?
                            if (!String.IsNullOrEmpty(check) && (check.Equals(NodeValue)))
                            {
                                String data = (String)row.getData(this.HeaderColumn.ChildColumn); //TODO convert ?
                                addData(Name, data, this.HeaderColumn.ChildColumn,row);
                            }
                        }
                        //Grid.stopStopWatch("singlerow");
                    }
                    Grid.stopStopWatch("childrows");
                }
			} else
			{		
				//Console.WriteLine("->" + Grid.Rows.Count);
                if (Grid.Rows != null)
                {
                    Grid.startStopWatch("allrows");
                    foreach (VirtualGridRow row in Grid.Rows)
                    {
                        //Grid.startStopWatch("singlerow");
                        String data = (String)row.getData(this.HeaderColumn); //TODO convert ?
                        addData(Name, data, this.HeaderColumn,row);
                        //Grid.stopStopWatch("singlerow");
                    }
                    Grid.stopStopWatch("allrows");
                }
			}
			Grid.stopStopWatch("loadChildren");
		}	
		
		
	}
}

