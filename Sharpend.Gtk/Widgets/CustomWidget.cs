//
// CustomWidget.cs
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

using GLib;
using Gtk;
using System;
using Gdk;
using System.Xml.Serialization;
using System.Xml;

#if !GTK2
namespace Sharpend.GtkSharp
{
	
	
	
	public class CustomWidget : Bin, IXmlSerializable
	{
		private Pango.Layout layout;
				
		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
//		public String Title {
//			get
//			{
//				return label;
//			}
//			set
//			{
//				label = value;
//				Layout.SetText (label);
//			}
//		}
			
		public CustomWidget (String name) : base ()
		{
			HasWindow = false;
			this.Name = name;
		}
			
		public CustomWidget () : base ()
		{
			layout = null;
			HasWindow = false;
		}
	
//		private Gdk.Pixbuf Icon {
//			get {
//				if (icon == null)
//					icon = RenderIconPixbuf (stockid, IconSize.Menu);
//				return icon;
//			}
//		}
		
		public String CurrentAssemblyName {
			get
			{
				String name = this.GetType().Assembly.FullName;
				String[] l = name.Split(',');
				return l[0];
			}
		}
	
//		public string Label {
//			get {
//				return label;
//			}
//			set {
//				label = value;
//				Layout.SetText (label);
//			}
//		}
//	
//		private Pango.Layout Layout {
//			get {
//				if (layout == null) {
//					layout = CreatePangoLayout (Title);
//					
//				}
//				return layout;
//			}
//		}
	
//		public string StockId {
//			get {
//				return stockid;
//			}
//			set {
//				stockid = value;
//				icon = RenderIconPixbuf (stockid, IconSize.Menu);
//			}
//		}
	
//		private Gdk.Rectangle TitleArea {
//			get {
//				Gdk.Rectangle area;
//				area.X = Allocation.X + (int)BorderWidth;
//				area.Y = Allocation.Y + (int)BorderWidth;
//				area.Width = (Allocation.Width - 2 * (int)BorderWidth);
//				
//				int layoutWidth, layoutHeight;
//				Layout.GetPixelSize (out layoutWidth, out layoutHeight);
//				area.Height = Math.Max (layoutHeight, icon.Height);
//				
//				return area;
//			}
//		}
	
//		private Gdk.Rectangle TitleArea {
//			get {
//				Gdk.Rectangle area;
//				area.X = Allocation.X + (int)BorderWidth;
//				area.Y = Allocation.Y + (int)BorderWidth;
//				area.Width = (Allocation.Width - 2 * (int)BorderWidth);
//				
//				int layoutWidth, layoutHeight;
//				Layout.GetPixelSize (out layoutWidth, out layoutHeight);
//				area.Height = Math.Max (layoutHeight, icon.Height);
//				
//				return area;
//			}
//		}
		
		
//		protected override bool OnDrawn (Cairo.Context cr)
//		{
//			Gdk.Rectangle titleArea = TitleArea;
//	
//			Gdk.CairoHelper.SetSourcePixbuf (cr, Icon, 0, 0);
//			cr.Paint ();
//				
//			int layout_x = icon.Width + 1;
//			titleArea.Width -= icon.Width - 1;
//			
//			int layoutWidth, layoutHeight;
//			Layout.GetPixelSize (out layoutWidth, out layoutHeight);
//			
//			int layout_y = (titleArea.Height - layoutHeight) / 2;
//			
//			StyleContext.RenderLayout (cr, layout_x, layout_y, Layout);
//			Console.WriteLine("drawn");							
//			return base.OnDrawn (cr);
//		}
	
//		protected override bool OnDrawn (Cairo.Context cr)
//		{
//			//Gdk.Rectangle titleArea = TitleArea;
//	
//			Gdk.CairoHelper.SetSourcePixbuf (cr, Icon, 0, 0);
//			cr.Paint ();
//			
//			//int layout_x = icon.Width + 1;
//			//titleArea.Width -= icon.Width - 1;
//			
//			int layoutWidth, layoutHeight;
//			Layout.GetPixelSize (out layoutWidth, out layoutHeight);
//			
//			int layout_y = (titleArea.Height - layoutHeight) / 2;
//			
//			StyleContext.RenderLayout (cr, layout_x, layout_y, Layout);
//		
//			return base.OnDrawn (cr);
//		}
		
//		protected override void OnSizeAllocated (Gdk.Rectangle allocation)
//		{
//			base.OnSizeAllocated (allocation);
//		
//			int bw = (int)BorderWidth;
//	        //Console.WriteLine("OnSizeAllocated");	
//			//Gdk.Rectangle titleArea = TitleArea;
//	
//			if (Child != null) {
//				Gdk.Rectangle childAllocation;
//				childAllocation.X = allocation.X + bw;
//				childAllocation.Y = allocation.Y + bw; //+ titleArea.Height;
//				childAllocation.Width = allocation.Width - 2 * bw;
//				childAllocation.Height = allocation.Height - 2 * bw;// - titleArea.Height;
//				Child.SizeAllocate (childAllocation);
//			}
//		}
		
		protected override void OnSizeAllocated (Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated (allocation);
		
			int bw = (int)BorderWidth;
	
			//Gdk.Rectangle titleArea = TitleArea;
	
			if (Child != null) {
				Gdk.Rectangle childAllocation;
				childAllocation.X = allocation.X + bw;
				childAllocation.Y = allocation.Y + bw;//titleArea.Height;
				childAllocation.Width = allocation.Width - 2 * bw;
				childAllocation.Height = allocation.Height - 2 * bw;// - titleArea.Height;
				Child.SizeAllocate (childAllocation);
			}
		}
	
//		protected override void OnGetPreferredWidth (out int minimum_width, out int natural_width)
//		{
//			minimum_width = natural_width = (int)BorderWidth * 2 + 1; //+Icon.Width
//			int layoutWidth, layoutHeight;
//			Layout.GetPixelSize (out layoutWidth, out layoutHeight);
//			
//			if (Child != null && Child.Visible) {
//				int child_min_width, child_nat_width;
//				Child.GetPreferredWidth (out child_min_width, out child_nat_width);
//				
//				minimum_width += Math.Max (layoutWidth, child_min_width);
//				natural_width += Math.Max (layoutWidth, child_nat_width);
//			} else {
//				minimum_width += layoutWidth;
//				natural_width += layoutWidth;
//			}
//		}
//	
//		protected override void OnGetPreferredHeight (out int minimum_height, out int natural_height)
//		{
//			minimum_height = natural_height = (int)BorderWidth * 2;
//			
//			int layoutWidth, layoutHeight;
//			Layout.GetPixelSize (out layoutWidth, out layoutHeight);
//			minimum_height += layoutHeight;
//			natural_height += layoutHeight;
//			
//			if (Child != null && Child.Visible) {
//				int child_min_height, child_nat_height;
//				Child.GetPreferredHeight (out child_min_height, out child_nat_height);
//				
//				minimum_height += Math.Max (layoutHeight, child_min_height);
//				natural_height += Math.Max (layoutHeight, child_nat_height);
//			}
//		}
		
	protected override void OnGetPreferredWidth (out int minimum_width, out int natural_width)
	{
		minimum_width = natural_width = (int)BorderWidth * 2 + 1; // + Icon.Width
		//int layoutWidth, layoutHeight;
		//Layout.GetPixelSize (out layoutWidth, out layoutHeight);
		
		if (Child != null && Child.Visible) {
			int child_min_width, child_nat_width;
			Child.GetPreferredWidth (out child_min_width, out child_nat_width);
			
			//minimum_width += Math.Max (layoutWidth, child_min_width);
			//natural_width += Math.Max (layoutWidth, child_nat_width);
		} else {
			//minimum_width += layoutWidth;
			//natural_width += layoutWidth;
		}
	}
	
		protected override void OnGetPreferredHeight (out int minimum_height, out int natural_height)
		{
			minimum_height = natural_height = (int)BorderWidth * 2;
			
//			int layoutWidth, layoutHeight;
//			Layout.GetPixelSize (out layoutWidth, out layoutHeight);
//			minimum_height += layoutHeight;
//			natural_height += layoutHeight;
			
			if (Child != null && Child.Visible) {
				int child_min_height, child_nat_height;
				Child.GetPreferredHeight (out child_min_height, out child_nat_height);
				
				//minimum_height += Math.Max (layoutHeight, child_min_height);
				//natural_height += Math.Max (layoutHeight, child_nat_height);
			}
		}
		
		protected virtual void doWriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("assembly",CurrentAssemblyName);
			//writer.WriteAttributeString("title",Title);
			writer.WriteAttributeString("Expand",Expand.ToString());
		}
		
		protected virtual void doReadXml(XmlReader reader)
		{
			//this.Title = reader["title"];
			this.Expand = Convert.ToBoolean(reader["Expand"]);
		}
		
		#region IXmlSerializable implementation
		public System.Xml.Schema.XmlSchema GetSchema ()
		{
			return null;
		}

		public void ReadXml (XmlReader reader)
		{
			if (reader.LocalName == "CustomWidget")
			{
				reader.Read();
			}
			//reader.ReadStartElement(this.GetType().ToString());
			
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

	}//class
} //namespace
#endif