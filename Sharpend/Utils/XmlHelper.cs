//
// XmlHelper.cs
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
using System.Linq;

namespace Sharpend
{
	public static class XmlHelper
	{
		public static string getAttributeValue(XmlNode node,String attributename)
		{
			XmlAttribute at = node.Attributes[attributename];
			if (at != null)
			{
				return at.Value ?? String.Empty;
			}
			return string.Empty;
		}

		//http://stackoverflow.com/questions/508390/create-xml-nodes-based-on-xpath

		public static XmlNode makeXPath(XmlDocument doc, string xpath)
		{
		    return makeXPath(doc, doc as XmlNode, xpath);
		}

		private static XmlNode makeXPath(XmlDocument doc, XmlNode parent, string xpath)
		{
		    // grab the next node name in the xpath; or return parent if empty
		    string[] partsOfXPath = xpath.Trim('/').Split('/');
		    string nextNodeInXPath = partsOfXPath.First();
		    if (string.IsNullOrEmpty(nextNodeInXPath))
		        return parent;

		    // get or create the node from the name
		    XmlNode node = parent.SelectSingleNode(nextNodeInXPath);
		    if (node == null)
		        node = parent.AppendChild(doc.CreateElement(nextNodeInXPath));

		    // rejoin the remainder of the array as an xpath expression and recurse
		    string rest = String.Join("/", partsOfXPath.Skip(1).ToArray());
		    return makeXPath(doc, node, rest);
		}

		
		
		
	}
}

