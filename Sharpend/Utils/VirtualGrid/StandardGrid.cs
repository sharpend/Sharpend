//
// StandardGrid.cs
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

﻿using System;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using System.Text;

namespace Sharpend.Utils
{
	/// <summary>
	/// implements a "standard grid" which is able to load
	/// the gridstructure from an xmlfile at the moment
	/// </summary>
	public class StandardGrid : VirtualTreeList
	{
		//the xml document containing the grid structure	
		private XmlDocument gridStructure = null;
		private XmlDocument gridData = null;
		private XslCompiledTransform transform = null; //TODO an den Anfang
		private String currentXsldoc = string.Empty;

		public StandardGrid () : base()
		{
		}
		
		/// <summary>
		/// create a new grid
		/// </summary>
		/// <param name="xmlstructurefile">
		/// the full filename to the xml file
		/// </param>
		public StandardGrid(String xmlstructurefile) : base()
		{
			loadStructure(xmlstructurefile);
		}
		
		protected override void addHeaderColumn(VirtualGridHeaderColumn column)
		{
			base.addHeaderColumn(column);
		}
		
		public void loadStructureXml(String xmldata)
		{
			gridStructure = new XmlDocument();
			gridStructure.LoadXml(xmldata);
			loadStructure(gridStructure);
		}
		
		public void loadStructure(String filename)
		{
			gridStructure = new XmlDocument();
			gridStructure.Load(filename);
			loadStructure(gridStructure);
		}

		public void loadStructureFromRessource(String ressourcename)
		{
			String xml = Sharpend.Utils.Utils.getResourceString(ressourcename);
			if (!String.IsNullOrEmpty(xml))
			{
				loadStructureXml(xml);
			}
		}

			
		/// <summary>
		/// load the structure from an xmlfile
		/// </summary>
		/// <param name="filename">
		/// A <see cref="String"/>
		/// </param>
		public void loadStructure(XmlDocument gridStructure)
		{
			//gridStructure = new XmlDocument();
			//gridStructure.Load(filename);
			
			XmlNodeList columns = gridStructure.SelectNodes("//column");
			foreach(XmlNode nd in columns)
			{
				String columnname = nd.SelectSingleNode("name").InnerText;				                
                VirtualGridHeaderColumn c = new VirtualGridHeaderColumn(this,columnname);

                XmlAttribute at = nd.Attributes["visible"];
                if ((at != null) && (!String.IsNullOrEmpty(at.Value)))
                {
                    if (at.Value.Equals("false", StringComparison.OrdinalIgnoreCase))
                    {
                        c.Visible = false;
                    }
                }

                at = nd.Attributes["editable"];
                if ((at != null) && (!String.IsNullOrEmpty(at.Value)))
                {
                    if (at.Value.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        c.Editable = true;
                    }
					
					c.EditableColumn = XmlHelper.getAttributeValue(nd,"editablecolumn");
					
                }
				c.Renderer = XmlHelper.getAttributeValue(nd,"renderer");

                at = nd.Attributes["customoption"];
                if ((at != null) && (!String.IsNullOrEmpty(at.Value)))
                {
                    c.CustomOption = at.Value;                    
                }
				
				c.ColumnType = XmlHelper.getAttributeValue(nd,"type");
				
				addHeaderColumn(c);
			}	
		}

        public void SaveStreamToFile(string fileFullPath, Stream stream)
        {
            if (stream.Length == 0) return;

            // Create a FileStream object to write a stream to a file
            using (FileStream fileStream = System.IO.File.Create(fileFullPath, (int)stream.Length))
            {
                // Fill the bytes[] array with the stream data
                byte[] bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, (int)bytesInStream.Length);

                // Use FileStream object to write to the specified file
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
        }
		

		
        /// <summary>
        /// load data with xmltransformation
        /// </summary>
        /// <param name="inputdocument"></param>
        /// <param name="xsldoc"></param>
        public virtual void loadData(XmlReader input, XmlDocument xsldoc, XsltArgumentList arguments)
        {
			Console.WriteLine("start Transformation");
			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			sw.Start();
			
			if ((transform == null) || (currentXsldoc != xsldoc.BaseURI))
			{
				//Console.WriteLine("creating new transform");
				currentXsldoc = xsldoc.BaseURI;
	            transform = new XslCompiledTransform();
	            transform.Load(xsldoc);
			}
						
            //MemoryStream stm = new MemoryStream();
            //transform.Transform(inputdocument, null, stm);

            //handle the output stream
            StringBuilder stringBuilder = new StringBuilder();
            TextWriter textWriter = new StringWriter(stringBuilder);

            //do the transform
            transform.Transform(input, arguments, textWriter);
            

			String xml = stringBuilder.ToString();
            xml = xml.Replace("﻿<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
            xml = xml.Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", "");
			
			sw.Stop();
			
			//Console.WriteLine("Transformation Time: " + sw.ElapsedMilliseconds.ToString());	
			
			
            XmlDocument newdoc = new XmlDocument();
            newdoc.LoadXml(xml);
			//newdoc.Save("test.xml");//debug ... TODO remove
			
            loadData(newdoc);
        }
		
		public virtual void loadDataFromString(String inputxml)
		{
			
			XmlDocument newdoc = new XmlDocument();
            newdoc.LoadXml(inputxml);
//			newdoc.Save("test.xml");//debug ... TODO remove
			
            loadData(newdoc);
		}
		
		private String convertToUtf8(String input)
		{
		  
		  byte[] unicode = Encoding.Unicode.GetBytes(input);
		  byte[] utf8 = Encoding.Convert(Encoding.Unicode,Encoding.UTF8,unicode);
		  UTF8Encoding enc = new UTF8Encoding();
		  
			return enc.GetString(utf8);
		}
		

        public virtual void loadData(String inputxml, XmlDocument xsldoc, XsltArgumentList arguments)
        {
			//throw new NotImplementedException("loadData -> must change to xml reader");

//			XmlDocument doc = new XmlDocument();
//
//            inputxml = inputxml.Replace("﻿<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
//            doc.LoadXml(inputxml);
//
//            System.Xml.XmlNamespaceManager xmlnsManager = new System.Xml.XmlNamespaceManager(doc.NameTable);
//            xmlnsManager.AddNamespace("db", doc.DocumentElement.NamespaceURI);
//
//            
//
			using (XmlReader xmlReader = XmlTextReader.Create(new StringReader(inputxml)))
			{
				loadData(xmlReader,xsldoc,arguments);
			}

			//loadData(doc, xsldoc, arguments);            
        }

        /// <summary>
        /// load data from inputdocumet
        /// </summary>
        /// <param name="inputdocument"></param>
        public virtual void loadData(XmlDocument inputdocument)
        {
            XmlNodeList rows = inputdocument.SelectNodes("//row");
            foreach (XmlNode nd in rows)
            {                
                VirtualGridRow row = this.newRow(); //TODO rename to NewRow ?

                if ((nd.Attributes["customoption"] != null) && (!String.IsNullOrEmpty(nd.Attributes["customoption"].Value)))
                {
                    row.CustomOption = nd.Attributes["customoption"].Value;
                }
                
                XmlNodeList columns = nd.SelectNodes(".//column");
                foreach (XmlNode nd2 in columns)
                {
                    String columnname = nd2.SelectSingleNode("name").InnerText;
                    String columndata = nd2.SelectSingleNode("data").InnerText;
										
                    VirtualGridCell cell = row.addGridColumn(columnname, columndata);
					if (cell != null)
					{
						cell.CustomOption = XmlHelper.getAttributeValue(nd2,"customoption");
					}
                }
            }
        }

		public virtual void loadData(XmlReader reader)
        {

		}

		/// <summary>
		/// load data from given filename
		/// </summary>
		/// <param name="filename"></param>
		public virtual void loadData(String filename)
		{
			gridData = new XmlDocument();
			gridData.Load(filename);
            loadData(gridData);
		}
		
		
		
	}
}

