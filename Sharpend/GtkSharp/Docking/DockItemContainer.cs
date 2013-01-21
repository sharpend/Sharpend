//
// DockItemContainer.cs
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

//
// DockItemContainer.cs
//
// Author:
//   Lluis Sanchez Gual
//

//
// Copyright (C) 2007 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#if !GTK2
using System;
using Gtk;
using System.Xml.Serialization;
using System.Xml;
using Cairo;
using Sharpend.Extensions;

namespace Sharpend.GtkSharp.Docking
{
	
	
	
	/// <summary>
	/// Dock item container.
	/// 
	/// A Dockitemcontainer can contain a dockable widget
	/// The Dockitemcontainer can added to a dockcontainer.
	/// </summary>
	public class DockItemContainer: VBox, IXmlSerializable
	{
		Gtk.Label title;
		Gtk.Button btnClose;
		Gtk.Button btnDock;
		string txt;
		Gtk.EventBox header;
		Gtk.Alignment headerAlign;
		DockFrame frame;

		/// <summary>
		/// The assigned DockFrame
		/// </summary>
		/// <value>
		/// The frame.
		/// </value>
		public DockFrame Frame
		{
			get 
			{
				if (frame == null)
				{
					setFrame();
				}
				return frame;
			}
		}

		public Box HeaderBox {
			get;
			private set;
		}

//		public Alignment HeaderAlignment {
//			get;
//			private set;
//		}

		private Pango.Layout layout;
		
		static Gdk.Cursor fleurCursor = new Gdk.Cursor (Gdk.CursorType.Fleur);
		static Gdk.Cursor handCursor = new Gdk.Cursor (Gdk.CursorType.Hand2);
		
		public DockContainer CurrentContainer { get; set; }
		
		public String CurrentAssemblyName 
		{
			get
			{
				String name = this.GetType().Assembly.FullName;
				String[] l = name.Split(',');
				return l[0];
			}
		}
		
		private Pango.Layout Layout {
			get {
				if (layout == null)
					layout = CreatePangoLayout (title.Text);
				return layout;
			}
		}
		
		static DockItemContainer ()
		{
		}
		
		public Widget CurrentWidget {
			get;
			private set;
		}

		/// <summary>
		/// If true, the header will be faded out to 10 px height after some seconds
		/// </summary>
		/// <value>
		/// <c>true</c> if fade out header; otherwise, <c>false</c>.
		/// </value>
		public bool FadeOutHeader {
			get;
			set;
		}

		/// <summary>
		/// only for serialization
		/// </summary>
		public DockItemContainer () : this(null, null)
		{
		}
		
		public DockItemContainer (DockFrame dockframe) : this(dockframe,null)
		{
			CurrentContainer = null;
			FadeOutHeader = true;
			//Console.WriteLine("create new DockItemContainer");
		}

		public void removeCurrentWidget()
		{
			Remove(CurrentWidget);
			CurrentWidget = null;
		}
		
		private void setFrame(Widget widget=null)
		{
			if (widget == null)
			{
				if (this.Parent != null)
				{
					setFrame(this.Parent);
				}
			}
			
			if (widget != null)
			{
				if (widget is DockFrame)
				{
					frame = (DockFrame)widget;
				} else
				{
					if (widget.Parent != null)
					{
						setFrame(widget.Parent);
					}
				}
			}
		}
		
		
		public DockItemContainer (DockFrame dockframe, Widget widget)
		{
			this.Expand = true;
			this.FadeOutHeader = true;
			//initIcons();
			
			frame = dockframe;
			CurrentContainer = null;
			
			ResizeMode = Gtk.ResizeMode.Queue;
			Spacing = 0;
		
			title = new Gtk.Label();
			title.Justify = Justification.Left;
									
			Gtk.Image img1 = new Gtk.Image("gtk-zoom-out",IconSize.Menu);
			btnDock = new Button(img1); 
			btnDock.Relief = ReliefStyle.None;
			btnDock.Clicked += HandleBtnDockClicked; 
			btnDock.EnterNotifyEvent += HandleButtonEnterNotifyEvent;

			Gtk.Image img = new Gtk.Image("gtk-close",IconSize.SmallToolbar);	
			btnClose = new Button(img);
			btnClose.Visible = true;
			btnClose.Clicked += HandleBtnCloseClicked;
			btnClose.Relief = ReliefStyle.None;
			btnClose.EnterNotifyEvent += HandleButtonEnterNotifyEvent;

			HeaderBox = new HBox (false, 0);
			HeaderBox.PackStart (title, true, false, 0);
			HeaderBox.PackEnd (btnClose, false, false, 0);
			HeaderBox.PackEnd (btnDock, false, false, 0);
									
			headerAlign = new Alignment (0.0f, 0.0f, 1.0f, 1.0f);
			headerAlign.TopPadding = headerAlign.BottomPadding = headerAlign.RightPadding = headerAlign.LeftPadding = 0;
			headerAlign.Add (HeaderBox);
			
			header = new EventBox ();
			header.Events |= Gdk.EventMask.KeyPressMask | Gdk.EventMask.KeyReleaseMask;
			header.ButtonPressEvent += HeaderButtonPress;
			header.ButtonReleaseEvent += HeaderButtonRelease;


//			header.MotionNotifyEvent += HeaderMotion;
//			header.KeyPressEvent += HeaderKeyPress;
//			header.KeyReleaseEvent += HeaderKeyRelease;
			header.Add (headerAlign);
			//header.Drawn += HandleHeaderDrawn;
						
			//header.Add(new Gtk.Button("gtk-close"));
			
			header.Realized += delegate {
				header.Window.Cursor = handCursor;
			};
			
			foreach (Widget w in new Widget [] { header, btnDock, btnClose }) {
				w.EnterNotifyEvent += HeaderEnterNotify;
				w.LeaveNotifyEvent += HeaderLeaveNotify;
			}
			
			PackStart (header, false, false, 0);
			ShowAll ();

			if (widget != null)
			{
				CurrentWidget = widget;
				widget.Visible = true;
				PackEnd(widget,true,true,0);
				
				if (widget is CustomWidget)
				{
					title.Text = (widget as DockableWidget).Title;
				}
			}

			doRemove = true;
			GLib.Timeout.Add(5000,new GLib.TimeoutHandler(removeHeader));
		}

		void HandleButtonEnterNotifyEvent (object o, EnterNotifyEventArgs args)
		{
			//Console.WriteLine("HandleButtonEnterNotifyEvent");
			doRemove = false;
		}

		void HandleBtnDockClicked (object sender, EventArgs e)
		{
			Gtk.Widget w;
			if (CurrentContainer.FrameNotebook.Visible == true)
			{
				 w = CurrentContainer.removePage(this);	//remove Page
				//Console.WriteLine(w.GetType().ToString());
			} else
			{
				w = PanedBox.removeItem(this); //remove Item
				//Console.WriteLine(w.GetType().ToString());				
			}
			
			DockItemContainer container = (w as DockItemContainer);
			MainWindow.Instance.removeWidget(container.CurrentWidget as DockableWidget);

			DockItemContainer dc = w as DockItemContainer;

			Widget cw = dc.CurrentWidget;
			dc.removeCurrentWidget();
			cw.Parent = null;
			MainWindow.Instance.showAsPopupWindow(cw as DockableWidget);

			w.Destroy();
		}


		void HandleHeaderDrawn (object o, DrawnArgs args)
		{
			Cairo.Context cr = args.Cr;
			cr.Save ();

            int wd = Allocation.Width - 48;
			Cairo.Color c1 = new Gdk.Color(65,65,65).ToCairoColor();
			Cairo.Color c2 = new Gdk.Color(255,255,255).ToCairoColor();
						
			Cairo.Gradient linpat = new LinearGradient(0,0,wd,Allocation.Height);
			linpat.AddColorStop(0.00, c1);
			linpat.AddColorStop(1.00, c2);
			
			cr.Rectangle (0, 0, wd, Allocation.Height);
            cr.Source = linpat;
			//cr.PaintWithAlpha(0.5);
			cr.Fill();
			
			cr.Color = new Color(1, 1, 1);
			cr.SelectFontFace("Arial", FontSlant.Normal, FontWeight.Bold);
			cr.SetFontSize(15.2);
			
			String txt = String.Empty;
			
			if (CurrentWidget != null)
			{
				if (CurrentWidget is  DockableWidget)
				{
					txt = (CurrentWidget as DockableWidget).Title;
				}
			}
			
			//TextExtents te = cr.TextExtents(txt);

			
			cr.MoveTo(4,20);			
			cr.ShowText(txt);
			
            cr.Restore ();
			
			cr=null; 			
		}
			

		void HandleBtnCloseClicked (object sender, EventArgs e)
		{
			Gtk.Widget w;
			if (CurrentContainer.FrameNotebook.Visible == true)
			{
				 w = CurrentContainer.removePage(this);	//remove Page
				//Console.WriteLine(w.GetType().ToString());
			} else
			{
				w = PanedBox.removeItem(this); //remove Item
				//Console.WriteLine(w.GetType().ToString());
				
			}
			
			DockItemContainer container = (w as DockItemContainer);
			MainWindow.Instance.removeWidget(container.CurrentWidget as DockableWidget);
			w.Destroy();
		}
		
		
		void HeaderButtonPress (object ob, Gtk.ButtonPressEventArgs args)
		{
//			if (args.Event.TriggersContextMenu ()) {
//				item.ShowDockPopupMenu (args.Event.Time);
//			} else if (args.Event.Button == 1) {
			if (args.Event.Button == 1) {
				//frame.	 ();
				header.Window.Cursor = fleurCursor;
				//frame.Toplevel.KeyPressEvent += HeaderKeyPress;
				//frame.Toplevel.KeyReleaseEvent += HeaderKeyRelease;
				//allowPlaceholderDocking = true;
				Frame.ShowPlaceholder();
			}
		}
		
		void HeaderButtonRelease (object ob, Gtk.ButtonReleaseEventArgs args)
		{
			//Console.WriteLine("HeaderButtonRelease");
			if (args.Event.Button == 1) {
				Frame.HidePlaceholder ();
				
				if (header.Window != null)
					header.Window.Cursor = handCursor;
				
				if (Frame.TargetContainer != null)
				{	
					//check if we are in notebook mode
//					if ((CurrentContainer != null) && (CurrentContainer.FrameNotebook.Visible == true))
//					{
//						//notebok mode
//						if ((frame.TargetContainer == CurrentContainer) && (frame.TargetAlign != ItemAlignment.Center))
//						{
//							//switch to paned mode
//							CurrentContainer.hideNotebook(frame.TargetAlign);
//						} else
//						{
//							CurrentContainer.removePage(this);
//						}
//						
//						
//						
//					}  else
//					{
//						//panedbox mode
//						
//						PanedBox.removeItem(this);
//					
//					
//					}
					
					//check if we have to remove something
					if ((CurrentContainer != null) && (Frame.TargetContainer != CurrentContainer))
					{
						if (CurrentContainer.FrameNotebook.Visible == true)
						{
							CurrentContainer.removePage(this);	//remove Page
						} else
						{
							PanedBox.removeItem(this); //remove Item
						}
					}

					//Console.WriteLine("alg:" + frame.TargetAlign);
					//add the new item
					if (Frame.TargetContainer.FrameNotebook.Visible == true)
					{
						this.Reparent(null);
						this.Parent = null;
						//Console.WriteLine("add1 " + this.Name + " to "  + Frame.TargetContainer.Name);
						frame.TargetContainer.addPage(this,Frame.TargetAlign);
						this.Visible = true;
					
					} else
					{
						this.Reparent(null);
						//Console.WriteLine("add2 " + this.Name + " to "  + Frame.TargetContainer.Name);
						Frame.TargetContainer.addItem(this,Frame.TargetAlign);
						this.Visible = true;
					}
				}
			}
		}
		
//		void HeaderMotion (object ob, Gtk.MotionNotifyEventArgs args)
//		{
//			//frame.UpdatePlaceholder (item, Allocation.Size, allowPlaceholderDocking);
//		}
		
//		[GLib.ConnectBeforeAttribute]
//		void HeaderKeyPress (object ob, Gtk.KeyPressEventArgs a)
//		{
////			if (a.Event.Key == Gdk.Key.Control_L || a.Event.Key == Gdk.Key.Control_R) {
////				allowPlaceholderDocking = false;
////				frame.UpdatePlaceholder (item, Allocation.Size, false);
////			}
////			if (a.Event.Key == Gdk.Key.Escape) {
////				frame.HidePlaceholder ();
////				frame.Toplevel.KeyPressEvent -= HeaderKeyPress;
////				frame.Toplevel.KeyReleaseEvent -= HeaderKeyRelease;
////				Gdk.Pointer.Ungrab (0);
////			}
//		}
//				
//		[GLib.ConnectBeforeAttribute]
//		void HeaderKeyRelease (object ob, Gtk.KeyReleaseEventArgs a)
//		{
////			if (a.Event.Key == Gdk.Key.Control_L || a.Event.Key == Gdk.Key.Control_R) {
////				allowPlaceholderDocking = true;
////				frame.UpdatePlaceholder (item, Allocation.Size, true);
////			}
//		}
		
		private Gdk.Rectangle TitleArea {
			get {
				Gdk.Rectangle area;
				area.X = Allocation.X + (int)BorderWidth;
				area.Y = Allocation.Y + (int)BorderWidth;
				area.Width = (Allocation.Width - 2 * (int)BorderWidth);
				
				int layoutWidth, layoutHeight;
				Layout.GetPixelSize (out layoutWidth, out layoutHeight);
				//area.Height = Math.Max (layoutHeight, icon.Height);
				
				return area;
			}
		}

		protected override bool OnMotionNotifyEvent (Gdk.EventMotion evnt)
		{
//			Console.WriteLine("mn");
//			header.Show();
			return base.OnMotionNotifyEvent (evnt);
		}

		private bool removeHeader()
		{
			//Console.WriteLine("removeHeader" + FadeOutHeader);
			if ((doRemove) && FadeOutHeader)
			{
				//Console.WriteLine("removehandler");
				header.HeightRequest = header.AllocatedHeight;
				removeing = true;
				header.Remove(headerAlign);
				GLib.Timeout.Add (50, delegate {
					//Console.WriteLine(header.HeightRequest);
					if (header.HeightRequest > 10) 
					{
						header.HeightRequest = header.HeightRequest-4;
						return true;
					}
					removeing = false;
				    return false;
	             });
			}

			return false;
		}

		bool removeing = false;
		bool doRemove = false;
		private void HeaderLeaveNotify (object ob, EventArgs a)
		{
			//pointerHover = false;
			//header.Hide();
			//Console.WriteLine("leave" + ob + "--");
			doRemove = true;
			if (header.Children.Length == 1)
			{
				GLib.Timeout.Add(600,new GLib.TimeoutHandler(removeHeader));
			} else
			{
				header.QueueDraw ();
			}
		}

		private bool showHeader()
		{
			if (!doRemove)
			{
				header.Add(headerAlign); 
			}
			return false;
		}

		private void HeaderEnterNotify (object ob, EventArgs a)
		{
			//pointerHover = true;
			if (removeing)
				return;

			//Console.WriteLine("add");
			doRemove = false;
			GLib.Timeout.Add(300,new GLib.TimeoutHandler(showHeader));
			header.QueueDraw ();
		}
				
		public string Label {
			get { return txt; }
			set {
				title.Markup = "<small>" + value + "</small>";
				txt = value;
			}
		}
		
		#region IXmlSerializable implementation
		public System.Xml.Schema.XmlSchema GetSchema ()
		{
			return null;
		}

		public void ReadXml (XmlReader reader)
		{
			if (reader.LocalName == "DockItemContainer")
			{
				reader.Read();
			}
			
			title.Text = reader["title"]; //TODO title ??
			Expand = Convert.ToBoolean(reader["Expand"]);	
			

			while((reader.LocalName != "widget") && reader.Read());

			reader.Read(); //skip widget node
			do
			{
				String tp = reader.LocalName + "," + reader["assembly"];
				Widget w = (Widget)Sharpend.Utils.Reflection.createInstance(Type.GetType(tp));
				
				IXmlSerializable xs = w as IXmlSerializable;
				if (xs != null)
				{
					xs.ReadXml(reader);
				}
				
				CurrentWidget = w;
				PackEnd(w,true,true,0);
				w.Visible = true;
				//reader.Read();
			} while ((reader.LocalName != "widget"));
			reader.Read(); //skip widget node
			
			reader.ReadEndElement();	
		}

		public void WriteXml (XmlWriter writer)
		{
			writer.WriteStartElement(this.GetType().ToString());
			writer.WriteAttributeString("assembly",CurrentAssemblyName);
			writer.WriteAttributeString("title",title.Text);
			writer.WriteAttributeString("Expand",Expand.ToString());
			
			writer.WriteStartElement("widget");
			IXmlSerializable xmlitm = CurrentWidget as IXmlSerializable;
			if (xmlitm != null)
			{
				xmlitm.WriteXml(writer);
			}
			writer.WriteEndElement();
			
			writer.WriteEndElement();
		}
		#endregion
	} //class
}//namespace

#endif