//
// VirtualGridRow.cs
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
	public class VirtualGridRow
	{
		
		private Dictionary<String,object> dataCache = null;
		
		public VirtualTreeList Grid {
			get;
			protected set;
		}
		
		public List<VirtualGridCell> Cells {
			get;
			protected set;
		}
		
		public List<VirtualGridRow> ChildRows {
			get;
			set;
		}
		
		public VirtualGridRow ParentRow
		{
			get;
			set;
		}
		
		public String GroupValue {
			get;
			set;
		}

        /// <summary>
        /// returns true when a cell was changed
        /// </summary>
        public bool Changed
        {
            get
            {
                foreach (VirtualGridCell c in Cells)
                {
                    if (c.Changed)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// sets all cells to changed
        /// </summary>
        /// <returns></returns>
        public void setChanged()
        {
            foreach (VirtualGridCell c in Cells)
            {
                c.Changed = true;                
            }            
        }

        //public String[] Values {
        //    get 
        //    {
        //        String[] ret = new String[Grid.HeaderColumns.Count + 1];
				
        //        int i=0;
        //        foreach (VirtualGridHeaderColumn hc in  Grid.getHeaderColumns())
        //        {
        //            ret[i] = getData(hc);
        //            i++;
        //        }
					
        //        /*
        //        for (int i=0;i<Colums.Count;i++)
        //        {
        //            ret[i] = Colums[i].Data;
        //        }*/
        //        return ret;
        //    }	
        //}

        public static System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            //a holder for the result
            System.Drawing.Bitmap result = new System.Drawing.Bitmap(width, height);

            //use a graphics object to draw the resized image into the bitmap
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);                
            }

            //return the resulting bitmap
            return result;
        }


        public static byte[] getImage(String filename,VirtualTreeList list)
        {
            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(filename);
                if (fi.Exists)
                {
                    //byte[] buf = new byte[fi.Length];
                    using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open,System.IO.FileAccess.Read))
                    {
                        //fs.Read(buf, 0, buf.Length);
                        System.Drawing.Image img = System.Drawing.Image.FromStream(fs);

                        System.IO.MemoryStream ms = new System.IO.MemoryStream();

                        //System.Drawing.Imaging.EncoderParameters
                        
						System.Drawing.Image thumb = img;
						if (list.Parameter.ImageScaleing == ImageScaleings.Scale)
						{
							thumb = ResizeImage(img, 100, 100);
						}

                        thumb.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        return ms.ToArray();
                    }
                }
                return new byte[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); //TODO logger
                return new byte[0];
            }
        }
		
		public int ColumnCount
		{
			get
			{
				return Grid.HeaderColumns.Count + 1;
			}
		}

        /// <summary>
        /// all values includeing this row as last entry (for selection)
        /// </summary>
        public object[] Datas
        {
            get
            {
                object[] ret = new object[Grid.HeaderColumns.Count + 1];

                int i = 0;
                //Grid.HeaderColumns.Count
                foreach (VirtualGridHeaderColumn hc in Grid.getHeaderColumns())
                {
                    object data = getData(hc);
                    ret[i] = data;

                    if (hc.CustomOption.Equals("pixbuf", StringComparison.OrdinalIgnoreCase))
                    {
                        ret[i] = null;
                        if (Grid.LoadPixBuf)
                        {
                            try
                            {
                                if (data != null)
                                {
//                                    byte[] buf = getImage((String)data,this.Grid);
//                                    ret[i] = new Gdk.Pixbuf(buf);

									ret[i] = Sharpend.Utils.CachedPictureList.Instance.GetPicture((String)data);
                                }
                            }
                            catch (Exception ex)
                            {
								//TODO logging
                                Console.WriteLine(ex.ToString());
                                ret[i] = null;
								//throw ex;
                            }
                        }
                    }
                    
                    i++;
                }

                ret[i] = this;                
                return ret;
            }
        }

        /// <summary>
        /// for external use ... if you need some parameters for the row
        /// </summary>
        public String CustomOption { get; set; }
		
		public VirtualGridRow (VirtualTreeList grid)
		{
			Grid = grid;
			Cells = new List<VirtualGridCell>(1000);
			ChildRows = new List<VirtualGridRow>(1000);
			dataCache = new Dictionary<string, object>(100);
            CustomOption = String.Empty;
		}
		
		public virtual void addCell(VirtualGridCell column)
		{
			Cells.Add(column);
		}
		
		public virtual VirtualGridCell addGridColumn(String headername, object data)
		{
			VirtualGridHeaderColumn hc = Grid.getHeaderByName(headername);
			if (hc != null)
			{
				VirtualGridCell cell = new VirtualGridCell(Grid,this,hc,data);
				dataCache.Add(headername.ToLower(),cell.Data);
				addCell(cell);
				return cell;
				//hc.addRow(this);
			}
			return null;
		}
		
		public VirtualGridCell getCell(VirtualGridHeaderColumn header)
		{
			foreach (VirtualGridCell cell in Cells)
			{
				if (cell.HeaderColumn == header)
				{
					if (!dataCache.ContainsKey(header.ColumnName.ToLower()))
					{
					  dataCache.Add(header.ColumnName.ToLower(),cell.Data);
					}
					return cell;
				}
			}
			return null;
		}
		
		/// <summary>
		/// returns the cell with the given columnName
		/// </summary>
		/// <returns>
		/// The cell.
		/// </returns>
		/// <param name='columnName'>
		/// Name.
		/// </param>
		public VirtualGridCell getCell(String columnName)
		{
			foreach (VirtualGridCell cell in Cells)
            {
                if (cell.HeaderColumn.ColumnName.Equals(columnName,StringComparison.OrdinalIgnoreCase))
                {
					return cell;
				}
			}
			return null;
		}
		
		public int getCellIndex(String columnname)
		{
			int ret = 0;
			foreach (VirtualGridCell cell in Cells)
			{
				if (cell.HeaderColumn.ColumnName == columnname)
				{
					return ret;
				}
				ret++;
			}
			return -1;
		}
		
		public String getColumnName(int index)
		{
			return Cells[index].HeaderColumn.ColumnName;
		}
		
		
		public object getData(VirtualGridHeaderColumn header)
		{
			
			if (dataCache.ContainsKey(header.ColumnName.ToLower()))
			{
				return dataCache[header.ColumnName.ToLower()];
			}
			
			VirtualGridCell cell = getCell(header);
			if (cell != null)
			{
				return cell.Data;
			}
			
			return null;
		}
		
		
		public object getData(String columnName)
		{
			
			if (dataCache.ContainsKey(columnName.ToLower()))
			{
				return dataCache[columnName.ToLower()];
			}
			
						
			VirtualGridHeaderColumn h = Grid.getHeaderByName(columnName);
			if (h != null)
			{
				return getData(h);
			}
			return null;
		}

        public virtual void setData(String columnName, object data, bool changed)
        {
			VirtualGridCell cell = getCell(columnName);
			if (cell != null)
			{
				cell.Data = data;
                cell.Changed = changed;
                if (dataCache.ContainsKey(columnName.ToLower()))
                {
                    dataCache.Remove(columnName.ToLower());
                    dataCache.Add(columnName.ToLower(), data);
                }
                return;
			} else
			{
				addGridColumn(columnName,data);	
			}
			
//            foreach (VirtualGridCell cell in Cells)
//            {
//                if (cell.HeaderColumn.ColumnName.Equals(columnName,StringComparison.OrdinalIgnoreCase))
//                {
//                    
//                }
//            }
        }
		
        public virtual void setData(String columnName, object data)
        {
            setData(columnName, data, false);
        }

		public String[] getValues(VirtualGridHeaderColumn without)
		{
			String[] ret = new String[Cells.Count];
			for (int i=0;i<Cells.Count;i++)
			{
				if (! Cells[i].HeaderColumn.isParentOf(without))
				{
					ret[i] = (String)Cells[i].Data;
				}
			}
			return ret;
		}
		
		
		
		
		public void addChildRow(VirtualGridRow row)
		{
			if (!ChildRows.Contains(row))
			{
				ChildRows.Add(row);
				row.ParentRow = this;
			}
		}
		 
	}
}

