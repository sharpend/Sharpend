//
// DelegateSet.cs
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
using System.Xml;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sharpend.Utils
{
	public class DelegateSet :  IEnumerable<DelegateData>, IXmlSerializable
	{
//		public String Target {
//			get;
//			private set;
//		}
//		
//		public String ClassName {
//			get;
//			private set;
//		}
//		
//		public String AssemblyName {
//			get;
//			private set;
//		}
		
		private List<DelegateData> delegates;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Sharpend.DelegateSet"/> class.
		/// </summary>
		public DelegateSet ()
		{
			delegates = new List<DelegateData>(10);
		}
		
		public void addDelegate(String target, String functionname, String eventname, String sourcename)
		{
			delegates.Add(new DelegateData(target,functionname,eventname,sourcename));
		}

		public void addDelegate(String target, String functionname, String eventname, String sourcename, String multi)
		{
			delegates.Add(new DelegateData(target,functionname,eventname,sourcename,multi));
		}

		public static DelegateSet CreateInstance(XmlNode node)
		{
			DelegateSet ds = new DelegateSet();
			
			XmlNodeList lst = node.SelectNodes("./delegates//delegate");	
						
			foreach(XmlNode nd in lst)
			{
				String target = XmlHelper.getAttributeValue(nd,"target");
				String functionname = XmlHelper.getAttributeValue(nd,"function");
				String eventname = XmlHelper.getAttributeValue(nd,"event");
				String sourcename = XmlHelper.getAttributeValue(nd,"source");
				String multi = XmlHelper.getAttributeValue(nd,"multi");
				
				if (!String.IsNullOrEmpty(multi))
				{
					ds.addDelegate(target,functionname,eventname,sourcename,multi);
				} else
				{
					ds.addDelegate(target,functionname,eventname,sourcename);
				}
			}
			
			return ds;
		}
		
				
		
		#region IXmlSerializable implementation
		public System.Xml.Schema.XmlSchema GetSchema ()
		{
			return null;
		}

		public void ReadXml (XmlReader reader)
		{
			//reader.ReadStartElement(this.GetType().ToString());
			//while((reader.LocalName != "delegates") && reader.Read());
			reader.Read();  //skip delegate set node
			
			if ((reader.LocalName == "delegates") && (! reader.IsEmptyElement))
			{
				reader.Read();  //skip delegates node
				do
				{
					DelegateData dd = new DelegateData();
					dd.ReadXml(reader);
					delegates.Add(dd);
					//reader.Read();
				} while ((reader.LocalName != "delegates"));
			}
			reader.Read(); //skip delegates node
			
			reader.ReadEndElement();
			
			
			//while((reader.LocalName != "delegates") && reader.Read());			
//		    if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "delegates")
//		    {			
//		        reader.Read(); // Skip ahead to next node
//		         				
//				while (reader.MoveToContent() == XmlNodeType.Element && Type.GetType(reader.LocalName) == (typeof(DelegateData)))
//		        {
//		            //reader.Read(); 
//					DelegateData dd = new DelegateData();
//					dd.ReadXml(reader);
//					delegates.Add(dd);
//					reader.Read();
//				}
//			}
//			reader.Read();
//			reader.ReadStartElement("Sharpend.Utils.DelegateSet");
			
			//reader.ReadStartElement("delegates");
			
//			while ((reader.MoveToContent() == XmlNodeType.Element) && Type.GetType(reader.LocalName) == (typeof(DelegateData)))
//			{
//				DelegateData dd = new DelegateData();
//				dd.ReadXml(reader);
//				delegates.Add(dd);	
//			}
			
//			while((reader.LocalName != "delegates") && reader.Read()); //forward to delegates			
//			
//			reader.ReadToFollowing((typeof(DelegateData)).ToString());
//		    do {
//		       DelegateData dd = new DelegateData();
//			   dd.ReadXml(reader);
//			   delegates.Add(dd); 
//		    } while (reader.ReadToNextSibling((typeof(DelegateData)).ToString()));
			
//			reader.Read();
//			while(Type.GetType(reader.LocalName) == (typeof(DelegateData)))
//			{
//				if (reader.IsStartElement())
//				{
//					DelegateData dd = new DelegateData();
//					dd.ReadXml(reader);
//					delegates.Add(dd);
//				}
//				reader.ReadToNextSibling(typeof(DelegateData).ToString());
//			}
			//reader.ReadEndElement();
		}

		public void WriteXml (XmlWriter writer)
		{
			writer.WriteStartElement(this.GetType().ToString());
			
			writer.WriteStartElement("delegates");
			foreach(DelegateData dd in delegates)
			{
				dd.WriteXml(writer);
			}
			writer.WriteEndElement();
			
			writer.WriteEndElement();
		}
		#endregion

		#region IEnumerable[DelegateData] implementation
		public IEnumerator<DelegateData> GetEnumerator ()
		{
			return delegates.GetEnumerator();
		}
		#endregion

		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
		}
		#endregion
	}
}

