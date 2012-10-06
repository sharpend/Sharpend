//
// GladeBuilder.cs
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
using System.Xml.Xsl;
using System.Xml;
using System.IO;
using System.Text;
using System.Reflection;

namespace Sharpend.Glade
{
	public class GladeBuilder
	{
		public GladeBuilder ()
		{
		}
		
		
		private static void generateCode(String inputdocument, String outputpath, String windowname, String namespacename, String outputname, String xsldoc,String classname="",bool isCustomWidget=false,string customwidgetclass=null,bool usegtk2=false,string ressourceId=null)
		{
			XslCompiledTransform transform = new XslCompiledTransform();
            
			if (String.IsNullOrEmpty(xsldoc)) //no xsl doc specified ... then we check if we have embedded ressources
			{
				Assembly asm = Assembly.GetCallingAssembly();
				String xsl = Sharpend.Utils.Utils.getResourceString(asm,ressourceId);

				using (StringReader sr = new StringReader(xsl))
				{
					using(XmlTextReader reader = new XmlTextReader(sr))
					{
						transform.Load(reader);
					}
				}
			} else
			{
				transform.Load(xsldoc);
			}



            //MemoryStream stm = new MemoryStream();
            //transform.Transform(inputdocument, null, stm);

            //handle the output stream
            StringBuilder stringBuilder = new StringBuilder();
            TextWriter textWriter = new StringWriter(stringBuilder);
			
			XsltArgumentList args = new XsltArgumentList();
            args.AddParam("windowid", "", windowname);
            args.AddParam("namespace", "", namespacename);
			args.AddParam("iscustomwidget", "", isCustomWidget);
			args.AddParam("classname", "", classname);
			
			if (!String.IsNullOrEmpty(customwidgetclass))
			{
				args.AddParam("customwidgetclass", "", customwidgetclass);
			}
			
			if (usegtk2)
			{
				args.AddParam("usegtk2", "", "true");
			} 
			
            //do the transform
            transform.Transform(inputdocument, args, textWriter);
			//transform.Transform(
			
            String xml = stringBuilder.ToString();
            
			Console.WriteLine("creating " + outputpath + outputname);
			File.WriteAllText(outputpath + outputname,xml);
		}
		
		
		public static void generateCode(String configfile,bool fullname=false)
		{
			XmlNodeList lst=null;
			if (fullname)
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(configfile);
				lst = doc.SelectNodes("//gladefile");
			} else {
		    	lst = Sharpend.Configuration.ConfigurationManager.getValues(configfile,"//gladefile");
			}

		  foreach (XmlNode nd in lst)
		  {
				String fn = XmlHelper.getAttributeValue(nd,"filename");
				String outputpath = XmlHelper.getAttributeValue(nd,"outputpath");
				String windowname = XmlHelper.getAttributeValue(nd,"windowname");
				String namespacename = XmlHelper.getAttributeValue(nd,"namespace");
				String classname = XmlHelper.getAttributeValue(nd,"class");
				String customwidgetclass = XmlHelper.getAttributeValue(nd,"customwidgetclass");
				bool createImplementaionClass = Convert.ToBoolean(XmlHelper.getAttributeValue(nd,"createimplementation"));
				bool iscustomwidget = false;
				String cw = XmlHelper.getAttributeValue(nd,"customwidget");
				if (!String.IsNullOrEmpty(cw))	
				{
					Boolean.TryParse(cw,out iscustomwidget);
				}
				
				bool usegtk2 = false;
				String gtk2 = XmlHelper.getAttributeValue(nd,"usegtk2");
				if (!String.IsNullOrEmpty(gtk2))	
				{
					Boolean.TryParse(gtk2,out usegtk2);
				}
				
				if (File.Exists(fn))
				{
					String filename = windowname;
					if (!String.IsNullOrEmpty(classname))
					{
						filename = classname;
					}
					
					if ((!String.IsNullOrEmpty(outputpath)) && (! outputpath.EndsWith(Path.DirectorySeparatorChar.ToString())))
					{
						outputpath += Path.DirectorySeparatorChar;
					}
					
					//String xsldoc = Sharpend.Configuration.ConfigurationManager.AppSettings["glade_transform"];		
					FileInfo xsldoc = Configuration.ConfigurationManager.getConfigFile("glade_transform.xsl");

					String xslname = String.Empty;
					if (xsldoc != null)
					{
						xslname = xsldoc.FullName;
					}

					generateCode(fn, outputpath, windowname,namespacename, filename + ".cs", xslname,classname,iscustomwidget,customwidgetclass,usegtk2,"Sharpend.Glade.glade_transform.xsl");
					
					if (createImplementaionClass)
					{
						if (!File.Exists(outputpath + filename + "Implementation.cs"))
						{
							xsldoc = Configuration.ConfigurationManager.getConfigFile("glade_transform2.xsl");
							if (xsldoc != null)
							{
								xslname = xsldoc.FullName;
							}
							generateCode(fn, outputpath, windowname,namespacename, filename + "Implementation.cs", xslname,classname,iscustomwidget,customwidgetclass,usegtk2,"Sharpend.Glade.glade_transform2.xsl");	
						}
					}
				}
		  }
		}
		
	}
}

