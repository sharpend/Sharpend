//
// PanedBox.cs
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
using Sharpend.GtkSharp.Docking;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

#if !GTK2
namespace Sharpend.GtkSharp
{
	/// <summary>
	/// A Box where you can add several widgets divided by a movable gutter (GtkPane)
	/// </summary>
	public class PanedBox : Paned, IXmlSerializable
	{
		protected Box container1;
		protected PanedBox container2;
		//private Paned paned;
		public PanedBox ParentBox { get; private set; }
		public PanedBox ChildBox { get { return container2;} }
		
		public Widget Widget1 {
			get
			{
				if (container1 != null)
				{
					if (container1.Children.Length > 0)
					{
						return container1.Children[0];
					}
				}
				return null;
			}	
		}
		
		public Widget Widget2
		{
			get
			{
				if (container2 != null)
				{
					return container2.Widget1;
				}
				return null;
			}
		}
		
		public List<Widget> ChildWidgets
		{
			get
			{
				List<Widget> ret = new List<Widget>(20);
				getChildWidgets(this,ret);
				return ret;
			}
		}
		
		public List<PanedBox> ChildBoxes
		{
			get 
			{
				List<PanedBox> ret = new List<PanedBox>(20);
				getChildBoxes(this,ret);
				return ret;
			}
		}
		
		public String CurrentAssemblyName {
			get
			{
				String name = this.GetType().Assembly.FullName;
				String[] l = name.Split(',');
				return l[0];
			}
		}
		
		//private Orientation orientation;
		
		/// <summary>
		/// Gets the orientation of the gutter
		/// </summary>
		/// <value>
		/// The orientation.
		/// </value>
//		public Orientation Orientation {
//			get
//			{
//				return orientation;
//			}
//			set
//			{
//				orientation = value;
//			}
//		}
		
		/// <summary>
		/// serialization only
		/// </summary>
		public PanedBox() : this(Orientation.Horizontal)
		{
		}
		
		public PanedBox(IntPtr raw) : base(raw)
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Sharpend.GtkSharp.PanedBox"/> class.
		/// </summary>
		/// <param name='orientation'>
		/// Orientation.
		/// </param>
		/// <param name='widget1'>
		/// Widget1.
		/// </param>
		public PanedBox (Orientation orientation, Widget widget1) : base(orientation)
		{
			init(orientation,widget1,null,false);
		}
		
		public PanedBox (Orientation orientation) : base(orientation)
		{
			init(orientation,null,null,false);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Sharpend.GtkSharp.PanedBox"/> class.
		/// </summary>
		/// <param name='orientation'>
		/// Orientation.
		/// </param>
		/// <param name='widget1'>
		/// Widget1.
		/// </param>
		/// <param name='widget2'>
		/// Widget2.
		/// </param>
		public PanedBox (Orientation orientation, Widget widget1, Widget widget2) : base(orientation)
		{
			init(orientation,widget1,widget2,true);
		}
		
		private PanedBox (PanedBox parent, Widget widget1) : base(parent.Orientation)
		{
			ParentBox = parent;
			init(ParentBox.Orientation,widget1,null,false);
		}
		
		/// <summary>
		/// add all widgets to list (recursiv)
		/// </summary>
		/// <param name='panedbox'>
		/// Panedbox.
		/// </param>
		/// <param name='list'>
		/// List.
		/// </param>
		private static void getChildWidgets(PanedBox panedbox, List<Widget> list)
		{
			if (panedbox.Widget1 != null)
			{
				list.Add(panedbox.Widget1);
			}
			if (panedbox.ChildBox != null)
			{
				getChildWidgets(panedbox.ChildBox,list);
			}
		}
		
		private static void getChildBoxes(PanedBox panedbox, List<PanedBox> list)
		{
			if (panedbox.ChildBox != null)
			{
				list.Add(panedbox.ChildBox);
			}
			if (panedbox.ChildBox != null)
			{
				getChildBoxes(panedbox.ChildBox,list);
			}
		}
		
		private void init(Orientation orientation, Widget widget1, Widget widget2,bool initChildPaned)
		{
			Orientation = orientation;
			
//			Gtk.Button bt = new Gtk.Button();
//			bt.Label = "asd§";
//			bt.Visible = true;
//			Add(bt);
			
//			paned = new Paned(Orientation);
//			paned.Visible = true;
//			Add(paned);
			container1 = new Box(Orientation,0);
			Pack1(container1,true,false);
			container1.Expand = true;
			container1.BorderWidth = 1;
			
			if (initChildPaned)
			{
				container2 = new PanedBox(this, widget2);
				Pack2(container2,true,false);
				container2.Visible = true;
			}

//			if(widget2 == null)
//			{
//				paned.Visible = false;
//			}
			
			if ((container1 != null) && (widget1 != null))
			{
				widget1.Reparent(container1);
				container1.PackStart(widget1,true,true,0);	
				container1.Visible = true;
			}
			//Add(paned);
			
			
			
		}
		
		private PanedBox getLastChild(PanedBox parent)
		{
			parent.Visible = true;
			if (parent.ChildBox == null)
			{
				return parent;
			} else
			{
				return getLastChild(parent.ChildBox);
			}
		}
		
//		protected override void OnRemoved (Widget widget)
//		{
//			base.OnRemoved (widget);
//		}
		
		public void removeChildbox()
		{
			//PanedBox tmp = container2;
			//container2 = null;
			Remove(container2);
			container2 = null;
//			Remove(tmp);
//			tmp.Reparent(null);
//			tmp = null;	
		}
				
		public static Widget removeItem(Widget w)
		{
			PanedBox pb = (w.Parent.Parent as PanedBox);
			if (pb != null)
			{
				if (pb.ParentBox != null)
				{
					Widget wdg = pb.Widget1;
					if (w == wdg)
					{
						pb.removeItem(w,true);
						wdg.Reparent(null);
						
						PanedBox parent = pb.ParentBox;
						PanedBox temp = pb.ChildBox;
						parent.removeChildbox();
						if (temp != null)
						{
							temp.Reparent(parent);
							parent.Append(temp);
							//temp.Visible = true;
						}
											
						pb.Dispose();
						pb = null;
						
						return w;
					}
				} else
				{
					return pb.removeItem(w,false);
				}
			}
			return null;
		}
		
		private Widget removeItem(Widget widget,bool x)
		{
			Widget w = container1.Children[0];
			if (w == widget)
			{
				container1.Remove(w);
				//w.Reparent(null); //TODO needed ?
				w.Parent = null;
				if (ParentBox != null)
				{	
					PanedBox temp = ChildBox;
					if (temp != null)
					{
						ParentBox.removeChildbox();
						temp.Reparent(this);
						ParentBox.Append(temp);
						//temp.Reparent(this);
						
					}
				} else
				{
					PanedBox temp = ChildBox;
					if (temp != null)
					{
						Widget chld = temp.Widget1;
						if (chld != null)
						{	
							container1.PackStart(chld,true,true,0);
							chld.Reparent(container1);
							container1.Visible = true;
							chld.Visible = true;
							PanedBox temp2 = temp.ChildBox;
							removeChildbox();
							if (temp2 != null)
							{
							    temp2.Reparent(this);
								Append(temp2);
							}
						}
					}
				}
				//Console.WriteLine(this.Name);
				return w;
			}
				return null;
		}
		
		public void Append(PanedBox box)
		{
			if (container2 == null)
			{
				container2 = box;
				Pack2(box,true,false);
				container2.Visible = true;
			} 
		}
		
		/// <summary>
		/// Adds a new Widget to the box
		/// </summary>
		/// <param name='widget'>
		/// Widget.
		/// </param>
		public void AddItem(Widget widget)
		{
			if (container1.Children.Length == 0)
			{
				if (widget.Parent != null)
				{
					widget.Reparent(container1); //TODO check if parent id not container1
				}
				
				container1.PackStart(widget,true,true,0);	
			} else 
			{
				PanedBox lb = getLastChild(this);		
				PanedBox newbox = new PanedBox(lb,widget);
				lb.Append(newbox);
			}
		}
		
//		public static PanedBox CreateInstance(XmlReader reader)
//		{
//			XmlSerializer xs = new XmlSerializer(type);								
//			PanedBox pb = (PanedBox)xs.Deserialize(reader);			
//			return pb;
//		}
		
		
		protected virtual void doReadXml(XmlReader reader)
		{
			if (reader["Orientation"].Equals("vertical",StringComparison.OrdinalIgnoreCase))
			{
				this.Orientation = Orientation.Vertical;
			} else
			{
				this.Orientation = Orientation.Horizontal;
			}
			
			this.Position = Convert.ToInt32(reader["Position"]);			
			//this.SetProperty("max-position",new GLib.Value(reader["MaxPosition"]));
			//this.SetProperty("min-position",new GLib.Value(reader["MinPosition"]));
			this.Expand = Convert.ToBoolean(reader["Expand"]);
			
			
			while((reader.LocalName != "item") && reader.Read());
			if (reader.LocalName == "item")
	        {
	            if (!reader.IsEmptyElement)
				{
					reader.Read(); //move to next node
					
					String tp = reader.LocalName + "," + reader["assembly"];
					Widget w = (Widget)Sharpend.Utils.Reflection.createInstance(Type.GetType(tp));
					container1.Visible = true;
					AddItem(w);
					w.Visible = true;
					
					IXmlSerializable xs = w as IXmlSerializable;
					if (xs != null)
					{
						xs.ReadXml(reader);
					}
				} else
				{
					reader.Read();
				}
	        }
			
			while(reader.LocalName != "childbox" && reader.Read());
			
			if (reader.LocalName == "childbox")
	        {
				if (!reader.IsEmptyElement)
				{
					reader.Read();
					String tp = reader.LocalName + "," + reader["assembly"];
					//this is the child panedbox
					Widget w = (Widget)Sharpend.Utils.Reflection.createInstance(Type.GetType(tp));
					//AddItem(w);
					//container2 = (PanedBox)w;
					Append((PanedBox)w);
					
					container2.Visible = true;
					w.Visible = true;
					
					IXmlSerializable xs = w as IXmlSerializable;
					if (xs != null)
					{
						xs.ReadXml(reader);
					}
				} else
				{
					reader.Read();
				}
			}
		}
		
		protected virtual void doWriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("assembly",CurrentAssemblyName);
			writer.WriteAttributeString("Orientation",this.Orientation.ToString());
			writer.WriteAttributeString("Position",this.Position.ToString());
			writer.WriteAttributeString("MaxPosition",this.MaxPosition.ToString());
			writer.WriteAttributeString("MinPosition",this.MinPosition.ToString());
			writer.WriteAttributeString("Expand",this.Expand.ToString());
			
			writer.WriteStartElement("item");
			IXmlSerializable xmlitm = Widget1 as IXmlSerializable;
			if (xmlitm != null)
			{
				xmlitm.WriteXml(writer);
			}
			writer.WriteEndElement();
			
			writer.WriteStartElement("childbox");
			if (container2 != null)
			{
				container2.WriteXml(writer);
			}
			writer.WriteEndElement();
		}
		
		#region IXmlSerializable implementation
		public System.Xml.Schema.XmlSchema GetSchema ()
		{
			return null;
		}
		
		public void ReadXml (XmlReader reader)
		{
			if (reader.LocalName == "PanedBox")
			{
				reader.Read();
			}
			
			doReadXml(reader);
			reader.ReadEndElement();
		}

		public void WriteXml (XmlWriter writer)
		{
			writer.WriteStartElement(this.GetType().ToString());
			
			doWriteXml(writer);
						
			writer.WriteEndElement();
		}
		#endregion
	} //class
}//namespace

#endif