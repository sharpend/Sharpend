//
// DockContainer.cs
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
using System.Collections.Generic;
using Gdk;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;


#if !GTK2
namespace Sharpend.GtkSharp.Docking
{
	
	public enum ViewMode
	{
		PanedBox = 0,
		Notebook = 1
	}
	
	

	/// <summary>
	/// Dock container.
	/// 
	/// A dockcontainer can contain several dockitemcontainer items
	/// </summary>
	public class DockContainer : PanedBox
	{
		private DockFrame frame;
		
		//private Box dockItemAlignment;
		//private List<DockItemContainer> items;
		private Notebook notebook; //TODO switch to notebook mode
		
		public Notebook FrameNotebook
		{
			get
			{
				return notebook;
			}
		}
			
		public ViewMode Mode {
			get;
			set;
		}
		
		public event EventHandler OnItemAdded;
		public event EventHandler OnPageAdded;
		
		public DockContainer (DockFrame dockframe) : base(Orientation.Vertical)
		{
			//dockItemAlignment = new Box();
			//dockItemAlignment.Orientation = Orientation.Vertical;
			frame = dockframe;
			//items = new List<DockItemContainer>(10);
			notebook = new Notebook();
			notebook.Visible = false;	
		}
		
		/// <summary>
		/// adds a new item to the itemcontainer
		/// </summary>
		/// <param name='itemcontainer'>
		/// Itemcontainer.
		/// </param>
		/// <param name='align'>
		/// Align.
		/// </param>
		public void addItem(DockItemContainer itemcontainer, ItemAlignment align)
		{
			if ((itemcontainer.CurrentWidget != null) && (itemcontainer.CurrentWidget is DockcontainerWidget))
			{
				if (this == (itemcontainer.CurrentWidget as DockcontainerWidget).Dock)
				{
					return;
				}
			}

			//itemcontainer.ParentWindow
			if ((itemcontainer.CurrentContainer != null) && (itemcontainer.CurrentContainer == this))
			{
				//containing this item
				if (align != ItemAlignment.Center)
				{
					if (notebook.Visible)
					{
						hideNotebook(align);
						setAlign(align);
						return;
					}
				}
				
				if (align == ItemAlignment.Center)
				{
					addPages();
					return;
				}	
			} else
			{
				//Console.WriteLine("add new item: " + itemcontainer.Name);
				//add a new item
				itemcontainer.CurrentContainer = this;
				AddItem(itemcontainer);	
				setAlign(align);
			}
			
			if (OnItemAdded != null)
			{
				OnItemAdded(itemcontainer.CurrentWidget,new EventArgs());
			}
		} //addItem
		
		
		/// <summary>
		/// sets the align for all panedboxes
		/// </summary>
		/// <param name='align'>
		/// Align.
		/// </param>
		private void setAlign(ItemAlignment align)
		{
			switch (align) 
			{
				case ItemAlignment.Top:
					if (Orientation == Orientation.Horizontal)
					{
						Orientation = Orientation.Vertical;
					}
					//PackStart(itemcontainer,true,true,0);
					break;
				case ItemAlignment.Left:
					if (Orientation == Orientation.Vertical)
					{
						Orientation = Orientation.Horizontal;
					}				
					//PackStart(itemcontainer,true,true,0);
					break;
				case ItemAlignment.Right:
					if (Orientation == Orientation.Vertical)
					{
						Orientation = Orientation.Horizontal;
					}				
					//PackEnd(itemcontainer,true,true,0);
					break;
				case ItemAlignment.Bottom:
					if (Orientation == Orientation.Horizontal)
					{
						Orientation = Orientation.Vertical;
					}
					//PackEnd(itemcontainer,true,true,0);
					break;
				case ItemAlignment.Center:
					addPages();
					break;
				default:
					break;
			}
		}
		
		/// <summary>
		/// removes the page containing the given widget
		/// </summary>
		/// <returns>
		/// The page.
		/// </returns>
		/// <param name='widget'>
		/// Widget.
		/// </param>
		public Widget removePage(Widget widget)
		{
			for (int i=0;i<notebook.NPages;i++)
			{
				Widget w = notebook.GetNthPage(i);
				if (w == widget)
				{
					notebook.RemovePage(i);
					return w;
				}
			}
			return null;
		}
		
		public void addPage(DockItemContainer itemcontainer, ItemAlignment align)
		{
			if ((itemcontainer.CurrentContainer != null) && (itemcontainer.CurrentContainer == this))
			{
				if (align != ItemAlignment.Center)
				{
					if (notebook.Visible)
					{
						hideNotebook(align);
						//setAlign(align);
						return;
					}
				}
				
//				if (align == ItemAlignment.Center)
//				{
//					addPages();
//					return;
//				}		
			} else
			{
				itemcontainer.CurrentContainer = this;
				
				String title = itemcontainer.Name;
				if (itemcontainer.CurrentWidget != null)
				{
					if (itemcontainer.CurrentWidget is DockableWidget)
					{
						if (!String.IsNullOrEmpty(((DockableWidget)itemcontainer.CurrentWidget).Title))
						{
							title = ((DockableWidget)itemcontainer.CurrentWidget).Title;
						}
					}
				}
				
				Gtk.Label lbl = new Gtk.Label(title);
				notebook.InsertPage(itemcontainer,lbl,notebook.NPages+1);
			}
			
			if (OnItemAdded != null)
			{
				OnItemAdded(itemcontainer.CurrentWidget,new EventArgs());
			}
		}
		
		/// <summary>
		/// Hides the notebook and switches to paned mode
		/// </summary>
		/// <param name='newalign'>
		/// Newalign.
		/// </param>
		public void hideNotebook(ItemAlignment newalign)
		{
			List<Widget> widgets = new List<Widget>(20);
			
			for (int i=(notebook.NPages-1);i>-1;i--)
			{
				Widget w = notebook.GetNthPage(i);
				notebook.RemovePage(i);	
				w.Parent = null;
				widgets.Add(w);
			}
			
			
			notebook.Visible = false;
			PanedBox.removeItem(notebook);
			
			foreach (Widget w in widgets)
			{
				DockItemContainer dc = w as DockItemContainer;
				if (dc != null)
				{
					dc.CurrentContainer = null;
					addItem(dc,newalign);
				}
			}
			
			Mode = ViewMode.PanedBox;
		}
		
		/// <summary>
		/// switch to notebook mode
		/// </summary>
		/// <exception cref='Exception'>
		/// Represents errors that occur during application execution.
		/// </exception>
		protected void addPages()
		{
			if (notebook.Visible)
			{
				throw new Exception("this should not happen"); //TODO			
			} else
			{	
				//Console.WriteLine("addPages()");
				List<PanedBox> lst = ChildBoxes;
				
				int pos = 0;
				
				notebook.Visible = false;
				
				foreach (PanedBox pb in lst)
				{
					Widget w = PanedBox.removeItem(pb.Widget1);
					if (w != null)
					{
						String title = w.Name;
						DockItemContainer dc = w as DockItemContainer;
						if (dc != null)
						{
							if (dc.CurrentWidget != null)
							{
								if (dc.CurrentWidget is DockableWidget)
								{
									if (!String.IsNullOrEmpty(((DockableWidget)dc.CurrentWidget).Title))
									{
										title = ((DockableWidget)dc.CurrentWidget).Title;
									}
								}
							}
						}
						
						Gtk.Label lbl2 = new Gtk.Label(title);
						//w.Parent = null;
						//w.Reparent(notebook);
						Gtk.Container c = (w.Parent as Gtk.Container);
						if (c != null)
						{
							c.Remove(w);
						}

						//Console.WriteLine("insert page: " + w.Name + " visible: " + w.Visible);
						//Console.WriteLine("notebook: " + notebook.Visible);
						notebook.InsertPage(w,lbl2,pos);
						
						pos++;
					} else
					{
						throw new Exception("item " + w.Name+ "is to in the panedbox");
					}
				}
						
				Widget first = removeItem(Widget1);
				
				String tt2 = first.Name;
				if (first is DockItemContainer)
				{
					DockItemContainer dc = first as DockItemContainer;
					
					if (!String.IsNullOrEmpty(((DockableWidget)dc.CurrentWidget).Title))
					{
						tt2 = ((DockableWidget)dc.CurrentWidget).Title;
					}
				}
				
				Gtk.Label lbl = new Gtk.Label(tt2);

				notebook.Visible = true;
				//Console.WriteLine("insert page: " + first.Name + " visible: " + first.Visible);
				//Console.WriteLine("notebook: " + notebook.Visible);
				notebook.InsertPage(first,lbl,pos);
								
				AddItem(notebook);
				notebook.Page = 0;
				Mode = ViewMode.Notebook;

			}
		}
		
		
		protected override void doReadXml (XmlReader reader)
		{
			String mode = reader["Mode"];
			ViewMode md = ViewMode.PanedBox;
			Enum.TryParse<ViewMode>(mode,out md);
			this.Mode = md;
			
			base.doReadXml (reader);
			
			if (this.Widget1 != null)
			{
				DockItemContainer container = (this.Widget1 as DockItemContainer);
				if (container != null)
				{
					container.CurrentContainer = this;
				}
			}
			
			foreach (Widget w in ChildWidgets)
			{
				DockItemContainer container = (w as DockItemContainer);
				if (container != null)
				{
					container.CurrentContainer = this;
				}
			}
			
			
			while(reader.LocalName != "pages" && reader.Read());
			if (reader.LocalName == "pages")
	        {
				if (!reader.IsEmptyElement)
				{
					int pos=0;
					while (reader.Read()) // Skip ahead to next node
					//while (reader.LocalName != "page" && reader.Read());
					
					//while (reader.LocalName == "page") 
					{
						String label = reader["label"];
						
						reader.Read(); //move to content element
						if (!reader.IsEmptyElement)
						{
							String assembly = reader["assembly"];
							if (!String.IsNullOrEmpty(assembly))
							{
								String tp = reader.LocalName + "," + assembly;						
								Widget w = (Widget)Sharpend.Utils.Reflection.createInstance(Type.GetType(tp));
								
								DockItemContainer container = (w as DockItemContainer);
								if (container != null)
								{
									container.CurrentContainer = this;
								}
									
								
								IXmlSerializable xs = w as IXmlSerializable;
								if (xs != null)
								{
									xs.ReadXml(reader);
								}
								notebook.InsertPage(w,new Gtk.Label(label),pos);
								pos++;
							} else
							{
								break;
							}
							while (reader.LocalName != "page" && reader.Read());
						}
						
					} 
					
					AddItem(notebook);
					notebook.Page = 0;
					Mode = ViewMode.Notebook;
					notebook.Visible = true;
					container1.Visible = true;
					//container2.Visible  = true;
					
				} else
				{
					reader.Read(); //skip pages
				}
			}
		}
		
		protected override void doWriteXml (XmlWriter writer)
		{
			writer.WriteAttributeString("Mode",this.Mode.ToString());			
						
			base.doWriteXml (writer);
			
			writer.WriteStartElement("pages");
			if (this.Mode == ViewMode.Notebook)
			{
				for (int i=(notebook.NPages-1);i>-1;i--)
				{
					writer.WriteStartElement("page");
					
					Widget w = notebook.GetNthPage(i);
					if ((w != null) && (w is IXmlSerializable))
					{
						writer.WriteAttributeString("label",notebook.GetTabLabelText(w));
						(w as IXmlSerializable).WriteXml(writer);
					}
					writer.WriteEndElement();
				}	
			}
			
			writer.WriteEndElement();
		}
		
		
		
	} //class
} //namespace
#endif
