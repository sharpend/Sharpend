//
// ParameterSet.cs
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

namespace Sharpend.Utils
{
	public class ParameterSet
	{
		public Type BaseType {
			get;
			private set;
		}
		
		private List<Parameter> parameter = new List<Parameter>(10);
				
		public ParameterSet (String classname, String assembly)
		{
			BaseType = Type.GetType(classname + "," + assembly);
		}
		
		public void addParameter(Parameter p)
		{
			parameter.Add(p);
		}
		
		public void addParameter(Type type, object data)
		{
			addParameter(new Parameter(type,data));	
		}
		
		public object[] Data {
			get
			{
				object[] obj = new object[parameter.Count];
				int i=0;
				foreach(Parameter p in parameter)
				{
					if (Types[i] == typeof(object))
					{
						obj[i] = p.Data;
					} else
					{
						obj[i] = Convert.ChangeType(p.Data,Types[i]);					
						i++;
					}
				}
				return obj;
			}
		}
		
		public Type[] Types 
		{
			get
			{
				Type[] tps = new Type[parameter.Count];
				int i=0;
				foreach(Parameter p in parameter)
				{
					tps[i] = p.ParameterType;	
					i++;
				}
				return tps;
			}
		}
		
		public static ParameterSet CreateInstance(XmlNode parent)
		{
			String classname = XmlHelper.getAttributeValue(parent,"class");
			String assembly = XmlHelper.getAttributeValue(parent,"assembly");
			
			ParameterSet ps = new ParameterSet(classname,assembly);
			
			XmlNodeList lst = parent.SelectNodes(".//param");
			foreach (XmlNode n in lst)
			{
				String paramname = XmlHelper.getAttributeValue(n,"name");	
				String paramtype = XmlHelper.getAttributeValue(n,"type");
				String paramvalue = n.InnerText;
				
				ps.addParameter(new Parameter(paramname,paramtype,paramvalue));
			}
			return ps;
		}
		
		public static ParameterSet CreateInstance(String classname, String assembly, object parameters)
		{	
			ParameterSet ps = new ParameterSet(classname, assembly);
			
//			XmlNodeList lst = parent.SelectNodes(".//param");
//			foreach (XmlNode n in lst)
//			{
//				String paramname = XmlHelper.getAttributeValue(n,"name");	
//				String paramtype = XmlHelper.getAttributeValue(n,"type");
//				String paramvalue = n.InnerText;
//				
//				ps.addParameter(new Parameter(paramname,paramtype,paramvalue));
//			}
			
//			foreach (String s in parameters)
//			{
//				ps.addParameter(typeof(String),s);
//			}
			ps.addParameter(parameters.GetType(),parameters);			
			
			return ps;
		}
		
		
		
	}
}

