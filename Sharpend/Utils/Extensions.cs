//
// Extensions.cs
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
using System.IO;


namespace Sharpend.Extensions
{
	public static class Extensions
	{
#if !GTK2
		public static Cairo.Color ToCairoColor (this Gdk.Color color)
		{
		        return new Cairo.Color ((double)color.Red / ushort.MaxValue,
				(double)color.Green / ushort.MaxValue, (double)color.Blue /
				ushort.MaxValue);
		}


		public static Gdk.Color ToGdkColor (this Cairo.Color color)
		{
		        Gdk.Color c = new Gdk.Color ();
		        c.Blue = (ushort)(color.B * ushort.MaxValue);
		        c.Red = (ushort)(color.R * ushort.MaxValue);
		        c.Green = (ushort)(color.G * ushort.MaxValue);
		
		        return c;
		}
#endif
		public static void AddAttributeValue(this XmlNode node, String name, String val)
		{
			XmlAttribute at = node.OwnerDocument.CreateAttribute(name);	
			node.Attributes.Append(at);
			at.Value = val;	
		}
		
		public static void AddAttributeValue(this XmlDocument doc, XmlNode node, String name, String val)
		{
			//node.AddAttributeValue(node,name,val);
			//TODO ??
			throw new NotImplementedException("AddAttributeValue");
		}
		
		//TODO comments
		//http://stackoverflow.com/questions/230128/best-way-to-copy-between-two-stream-instances-c-sharp
		
		public static String CDataValue(this XmlNode node)
		{	
			if ((node.ChildNodes != null) && (node.ChildNodes.Count == 1))
			{
			  XmlCDataSection dt = (XmlCDataSection)(node.ChildNodes[0]);
			  return dt.Data;	
			}
			return String.Empty;
		}

		/// <summary>
		/// returns the attributevalue ... string.empty if this attribute does not exist
		/// </summary>
		/// <returns>
		/// The value.
		/// </returns>
		/// <param name='node'>
		/// Node.
		/// </param>
		/// <param name='attributename'>
		/// Attributename.
		/// </param>
		public static String AttributeValue(this XmlNode node,String attributename)
		{
			XmlAttribute at = node.Attributes[attributename];
			if (at != null)
			{
				return at.Value ?? String.Empty;
			}
			return string.Empty;
		}

		/// <summary>
		/// returns the attributevalue as boolean ... if the attribute does not exist the function returns false!
		/// </summary>
		/// <returns>
		/// The value bool.
		/// </returns>
		/// <param name='node'>
		/// If set to <c>true</c> node.
		/// </param>
		/// <param name='attributename'>
		/// If set to <c>true</c> attributename.
		/// </param>
		public static bool AttributeValueBool(this XmlNode node,String attributename)
		{
			XmlAttribute at = node.Attributes[attributename];
			if (at != null)
			{
				if (!String.IsNullOrEmpty(at.Value))
				{
					return Convert.ToBoolean(at.Value);
				}
			}
			return false;
		}

		public static void AppendTextValue(this XmlNode node,String element,String value)
		{	
			XmlElement e = node.OwnerDocument.CreateElement(element);
			node.AppendChild(e);
			e.InnerText = value;
		}


		/// <summary>
		/// Copy stream into the destination stream
		/// </summary>
		/// <param name='src'>
		/// Source.
		/// </param>
		/// <param name='dest'>
		/// Destination.
		/// </param>
		public static void CopyTo(this Stream src, Stream dest)
	    {
	        int size = (src.CanSeek) ? Math.Min((int)(src.Length - src.Position), 0x2000) : 0x2000;
	        byte[] buffer = new byte[size];
	        int n;
	        do
	        {
	            n = src.Read(buffer, 0, buffer.Length);
	            dest.Write(buffer, 0, n);
	        } while (n != 0);           
	    }

		/// <summary>
		/// Copy stream into the destination stream
		/// </summary>
		/// <param name='src'>
		/// Source.
		/// </param>
		/// <param name='dest'>
		/// Destination.
		/// </param>
	    public static void CopyTo(this MemoryStream src, Stream dest)
	    {
	        dest.Write(src.GetBuffer(), (int)src.Position, (int)(src.Length - src.Position));
	    }

		/// <summary>
		/// Copy stream into the destination stream
		/// </summary>
		/// <param name='src'>
		/// Source.
		/// </param>
		/// <param name='dest'>
		/// Destination.
		/// </param>
	    public static void CopyTo(this Stream src, MemoryStream dest)
	    {
	        if (src.CanSeek)
	        {
	            int pos = (int)dest.Position;
	            int length = (int)(src.Length - src.Position) + pos;
	            dest.SetLength(length); 
	
	            while(pos < length)                
	                pos += src.Read(dest.GetBuffer(), pos, length - pos);
	        }
	        else
	            src.CopyTo((Stream)dest);
	    }

		
		
	}
}

