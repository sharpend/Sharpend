//using System;
//using Gtk;
//using Sharpend.GtkSharp;
//using Sharpend.GtkSharp.Docking;
//
//namespace DockApplication//{//	public partial class DockframeWindow
//						: DockableWidget//	{
//		//private DockFrame frame;
//		private DockContainer xxxdock;
//
//
//
////		public void init() 
//		{
////			if (frame == null)
////			{
////				frame = new DockFrame();
////				box1.PackStart(frame,true,true,0);
////				frame.Expand = true;
////			}
//
//			//testbox.Expand = true;
//			//testbox.Visible = true;
//			xxxdock = new DockContainer(null);
//			xxxdock.Name = "xxxdock";
//			xxxdock.Visible = true;
//			xxxdock.Expand = true;
//		testbox.PackEnd(xxxdock,true,true,0);
//		}
//
//		protected override void doReadXml (System.Xml.XmlReader reader)
//		{
//			base.doReadXml (reader);
//
//			while((reader.LocalName != "xxxdock") && reader.Read());
//			if ((reader.LocalName == "xxxdock") && !reader.IsEmptyElement)
//			{
//				reader.Read(); //skip dock1
//				xxxdock.ReadXml(reader);
//				xxxdock.Visible = true;
//			}
//
//			reader.Read();
//		}
//
//		protected override void doWriteXml (System.Xml.XmlWriter writer)
//		{
//			base.doWriteXml (writer);
//
//			writer.WriteStartElement("xxxdock");
//			xxxdock.WriteXml(writer);
//			writer.WriteEndElement();
//		}
////	
//	} //class//} //namespace