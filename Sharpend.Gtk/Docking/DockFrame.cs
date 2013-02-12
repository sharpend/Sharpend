//
// DockFrame.cs
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
using Gdk;
using System.Collections.Generic;
using System.Xml.Serialization;

#if !GTK2
namespace Sharpend.GtkSharp.Docking
{
	public enum ItemAlignment
	{
		Top   = 1,
		Left  = 2,
		Right = 3,
		Bottom = 4,
		Center = 5
	}
	
	/// <summary>
	/// Dock frame position.
	/// 
	/// Values where dockitemcontainers are placed
	/// </summary>
	public enum DockFramePosition
	{
		None = 0,
		Left = 1,
		Right = 2
	}
	
	/// <summary>
	/// Dock frame.
	/// 
	/// A Dockframe has two boxes for dockitems (left and right) splitted by GtkPaned
	/// 
	/// </summary>
	public class DockFrame : CustomWidget , IXmlSerializable
	{
		private Gtk.Paned paned1;
		private Gtk.Box box2;
		private Gtk.Box box1;
		
		private DockContainer dock1;
		private DockContainer dock2;
		
		private bool placeholderVisible;
		private PlaceholderWindow placeholderwindow;
		private DockContainer targetContainer = null;
		private ItemAlignment targetAlign = ItemAlignment.Center;
		
		private List<DockItemContainer> items;
		private Dictionary<String,DockFramePosition> lastposition;
		
		public List<DockItemContainer> Items
		{
			get 
			{
				return items;
			}
		}
		
		public DockContainer TargetContainer {
			get { return targetContainer; }
		}
		
		public ItemAlignment TargetAlign {
			get { return targetAlign; }
		}
		
		
		public DockFrame ()
		{				
			paned1 = new Gtk.Paned(Orientation.Horizontal);
			paned1.Name ="paned1";
			paned1.Expand = true;
			
			box2 = new Gtk.Box(Orientation.Vertical,0);
			box2.Name ="box2";
			//box2.Expand = true;
			
			box1 = new Gtk.Box(Orientation.Vertical,0);
			box1.Name ="box1";
			//box1.Expand = true;
			
			dock1 = new DockContainer(this);
			dock2 = new DockContainer(this);
			
			dock1.Name = "dock1";
			dock2.Name = "dock2";
			
			dock1.Expand = true;
			dock2.Expand = true;
			
			box1.PackEnd(dock1,true,true,0);
			box2.PackEnd(dock2,true,true,0);
			dock1.Expand = true;
			dock2.Expand = true;
			
			items = new List<DockItemContainer>(100);
			lastposition = new Dictionary<string, DockFramePosition>(100);
			
			paned1.Add1(box1);
			paned1.Add2(box2);
									
			paned1.Position = 400;
			
			dock1.OnItemAdded += HandleDock1Added;
			dock1.OnPageAdded += HandleDock1Added;
			
			dock2.OnItemAdded += HandleDock2Added;
			dock2.OnPageAdded += HandleDock2Added;
			
			this.Add(paned1);
		}

		void HandleDock1Added (object sender, EventArgs e)
		{
			String type = sender.GetType().ToString();

			if (lastposition.ContainsKey(type))
			{
				lastposition[type] = DockFramePosition.Left;
			} else
			{
				lastposition.Add(type,DockFramePosition.Left);
			}
		}
		
		void HandleDock2Added (object sender, EventArgs e)
		{
			String type = sender.GetType().ToString();
			if (lastposition.ContainsKey(type))
			{
				lastposition[type] = DockFramePosition.Right;
			} else
			{
				lastposition.Add(type,DockFramePosition.Right);
			}
		}

		
		public void addItem(DockItemContainer item,ItemAlignment align,bool left=false)
		{
			items.Add(item);
			
			DockFramePosition pos = DockFramePosition.None;
			if (lastposition.ContainsKey(item.CurrentWidget.GetType().ToString()))
			{
				pos = lastposition[item.CurrentWidget.GetType().ToString()];
			}
			
			switch (pos) {
				case DockFramePosition.Left:
					left = true;
					break;
				case DockFramePosition.Right:
					left = false;
					break;
			}
			
			if (left)
			{
				if (dock1.Mode == ViewMode.Notebook)
				{
					dock1.addPage(item,align);
				} else {
				  dock1.addItem(item,align);
				}
			} else
			{
				if (dock2.Mode == ViewMode.Notebook)
				{
					dock2.addPage(item,align);
				} else {
				  dock2.addItem(item,align);
				}
			}
		}
		
		protected override bool OnMotionNotifyEvent (EventMotion evnt)
		{
			if (placeholderVisible)
			{	
				int sx,sy;
				this.Window.GetOrigin (out sx, out sy);
				int rx = (int)evnt.XRoot - sx;
				int ry = (int)evnt.YRoot - sy;
					
				int xDragDif=0;
				int yDragDif=0;
				
				ShowPlaceholder(false,rx,ry,xDragDif, yDragDif);
				setPlaceholderWindowsize(rx,ry);
			}
			return base.OnMotionNotifyEvent (evnt);
		}

		/// <summary>
		/// recursive add all children
		/// </summary>
		/// <param name='parent'>
		/// Parent.
		/// </param>
		/// <param name='list'>
		/// List.
		/// </param>
		protected void addChildren(Container parent, List<Widget> list)
		{
			foreach (Widget w in parent.AllChildren)
			{
				list.Add(w);
				//Console.WriteLine("xxx:" + w.Name);
				Container c = w as Container;
				if (c != null)
				{
					//Console.WriteLine("-->:" + w.Name);
					addChildren(c,list);
				}
			}	
		}
		
		protected void setPlaceholderWindowsize(int cX, int cY)
		{
		  
		  List<Widget> lst = new List<Widget>(100);
		  addChildren(this,lst); //recursiv add all children in the list
		  targetContainer = null; 
			bool done = false;
		  
			//foreach(Widget w in lst)
			for (int i=lst.Count-1;i>-1;i--)
			  {	 
					Widget w = lst[i];

				//Console.WriteLine("fe: " + w.Name);
				int rx = 0;
				int ry = 0;
					
				int sx,sy;
				this.Window.GetOrigin (out sx, out sy);
				if (w.Window != null)
				{
					//Console.WriteLine("window:" + w.Window.ToString() + " sx: " + sx + " sy: " + sy);
					w.Window.GetOrigin(out rx,out ry);	
					rx = rx - sx; //x of widget relativ to window
					ry = ry - sy; //y of widget relative to window
					
					 if ( (rx <= cX) && (cX <= (rx+w.Allocation.Width)))
					 {
						if ((ry <= cY) && (cY <= (ry+w.Allocation.Height)))
						{
						  if (w is DockContainer)
						  {	
							//Console.WriteLine("tc:" + w.Name);
							targetContainer = (DockContainer)w;
							if (placeholderwindow != null)
							{
				    		  int wd = w.Allocation.Width;
							  int wh = w.Allocation.Height / 2;
							  int wx = rx + sx;
							  int wy = ry + sy;
							  			
							  targetAlign = getAlignment(rx,ry,cX,cY,w.Allocation.Width,w.Allocation.Height);
							  
							  //bool alignChanged = placeholderwindow.Alignment != targetAlign;			
							  placeholderwindow.Alignment = targetAlign;		
							  switch (targetAlign) 
							  {
										case ItemAlignment.Top:
											wh = w.Allocation.Height / 3;
											break;
										case ItemAlignment.Left:
											wh = w.Allocation.Height;
											wd = w.Allocation.Width /3;
											break;
										case ItemAlignment.Right:
											wh = w.Allocation.Height;
											wd = w.Allocation.Width /3;
											wx = wx + (w.Allocation.Width -wd);
											break;
										case ItemAlignment.Bottom:
											wh = w.Allocation.Height / 3;
											wy = wy + (w.Allocation.Height - wh);
											break;
										case ItemAlignment.Center:
											wh = w.Allocation.Height / 3;
											wd = w.Allocation.Width / 3;
											wx = wx + wd;
											wy = wy + wh;
											break;
										default:
											break;
							  }
							  
							  //if (alignChanged) {		
							  placeholderwindow.Move(wx,wy);
							  placeholderwindow.Resize(wd,wh);
							  done = true;
							//		}
							}
						  }
						}
					 }
				}	

				if (done)
				{
					break;
				}
			  } //foreach		
		}
		
		
		private ItemAlignment getAlignment(int rx, int ry, int cX, int cY, int width, int height)
		{
			int hg = height / 3;
			int wd = width  / 3;
			
			//center
			if ((cX >= rx+wd) && (cX <= rx+(2*wd)) &&
				(cY >= ry+hg) && (cY <= ry+(2*hg)))
			{
				return ItemAlignment.Center;
			}
			
			//left
			if ((cX >= rx) && (cX <= rx+wd))
			{
				return ItemAlignment.Left;
			}
			
			//right
			if ((cX >= (rx + (2*wd))) && (cX <= rx+width))
			{
				return ItemAlignment.Right;
			}
			
			//top
			if ((cY >= ry) && (cY <= ry+hg))
			{
				return ItemAlignment.Top;
			}
			
			//bottom
			if ((cY >= (ry + (2*hg))) && (cY <= ry+height))
			{
				return ItemAlignment.Bottom;
			}
			
			return ItemAlignment.Center;			
		}
		
		
		public void ShowPlaceholder()
		{
			if (placeholderwindow == null)
			{
				placeholderwindow = new PlaceholderWindow();
			}
			placeholderwindow.Show();
			placeholderVisible = true;
		}
		
		public void HidePlaceholder()
		{
			placeholderwindow.Hide();
			placeholderVisible = false;
		}
		
				
		void ShowPlaceholder (bool horz, int x, int y, int w, int h)
		{
			if (placeholderwindow != null)
			{
				int sx, sy;
				this.Window.GetOrigin (out sx, out sy);
				//Console.WriteLine("mid ShowPlaceholder");
				sx += x;
				sy += y;
				
				int mg = -4;
				//placeholderwindow.Move (sx - mg, sy - mg);
				//Console.WriteLine((w + mg*2).ToString());
				int width = (w + mg*2);
				int height = (h + mg * 2);
				
				if ((width > 0) && (height > 0))
				{
					placeholderwindow.Resize (width,height);
				}
			}
		}
		
		
		protected override void doWriteXml (System.Xml.XmlWriter writer)
		{
			writer.WriteAttributeString("Position",paned1.Position.ToString());
			base.doWriteXml (writer);
			
			writer.WriteStartElement("dock1");
			dock1.WriteXml(writer);
			writer.WriteEndElement();
			
			writer.WriteStartElement("dock2");
			dock2.WriteXml(writer);
			writer.WriteEndElement();
			
			writer.WriteStartElement("positions");
			
			foreach(KeyValuePair<String,DockFramePosition> kp in lastposition)
			{
				writer.WriteStartElement("position");
				writer.WriteAttributeString("type",kp.Key);
				writer.WriteAttributeString("position",kp.Value.ToString());
				writer.WriteEndElement();
			}
			
			writer.WriteEndElement();
		}
		
		
		protected override void doReadXml (System.Xml.XmlReader reader)
		{
			paned1.Position = Convert.ToInt32(reader["Position"]);
			base.doReadXml (reader);
			
			while((reader.LocalName != "dock1") && reader.Read());
			if ((reader.LocalName == "dock1") && !reader.IsEmptyElement)
			{
				reader.Read(); //skip dock1
				dock1.ReadXml(reader);
				dock1.Visible = true;
			}
			
			while((reader.LocalName != "dock2") && reader.Read());
			if ((reader.LocalName == "dock2") && !reader.IsEmptyElement)
			{
				reader.Read(); //skip dock2
				dock2.ReadXml(reader);
				dock2.Visible = true;
			}
			
			while((reader.LocalName != "positions") && reader.Read());
			if ((reader.LocalName == "positions") && (! reader.IsEmptyElement))
			{
				reader.Read();  //skip positions node
				do
				{
					String type = reader["type"];
					String val = reader["position"];
					
					DockFramePosition pos = DockFramePosition.None;
					Enum.TryParse<DockFramePosition>(val,out pos);
					
					lastposition.Add(type,pos);
					reader.Read();
				} while ((reader.LocalName != "positions"));
			}
			reader.Read(); //skip positions node
			
		}
		
	} //class
} //namespace

#endif