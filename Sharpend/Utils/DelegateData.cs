//
// DelegateData.cs
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
using System.Xml.Serialization;

namespace Sharpend.Utils
{
	public class DelegateData : IXmlSerializable
	{
		public String Target {
			get;
			private set;
		}
		
		public String FunctionName {
			get;
			private set;
		}
		
		public String EventName {
			get;
			private set;
		}
		
		public String SourceName {
			get;
			private set;
		}
		
		/// <summary>
		/// Serialization only
		/// </summary>
		public DelegateData()
		{
		}
		
		public DelegateData (String target, String functionname, String eventname, String sourcename)
		{
			Target = target;
			FunctionName = functionname;
			EventName = eventname;
			SourceName = sourcename;
		}

		#region IXmlSerializable implementation
		public System.Xml.Schema.XmlSchema GetSchema ()
		{
			return null;
		}

		public void ReadXml (System.Xml.XmlReader reader)
		{	
			Target = reader.GetAttribute("Target");
			FunctionName = reader.GetAttribute("FunctionName");
			EventName = reader.GetAttribute("EventName");
			SourceName = reader.GetAttribute("SourceName");
			reader.Read(); //because there is no endelement
		}

		public void WriteXml (System.Xml.XmlWriter writer)
		{
			writer.WriteStartElement(this.GetType().ToString());
			writer.WriteAttributeString("Target",Target);
			writer.WriteAttributeString("FunctionName",FunctionName);
			writer.WriteAttributeString("EventName",EventName);
			writer.WriteAttributeString("SourceName",SourceName);
			
			writer.WriteEndElement();
		}
		#endregion
	}
}

