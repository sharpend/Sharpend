//
// GladeFile.cs
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
//using Sharpend.GtkSharp;
using System.Xml;
using Sharpend.Extensions;
using Sharpend.Databinding;

namespace GladeBuilder
{
	public class GladeFile : BindableData
	{
		/// <summary>
		/// Windowname used in glade
		/// </summary>
		/// <value>
		/// The name of the window.
		/// </value>
		[BindableProperty("EntryWindowName","Changed")]
		public String WindowName {
			get;
			set;
		}

		/// <summary>
		/// outputpath for the generated files
		/// </summary>
		/// <value>
		/// The output path.
		/// </value>
		[BindableProperty("EntryOutputPath","Changed")]
		public String OutputPath {
			get;
			set;
		}

		/// <summary>
		/// .glade file
		/// </summary>
		/// <value>
		/// The filename.
		/// </value>
		[BindableProperty("EntryFilename","Changed")]
		public String Filename {
			get;
			set;
		}

		/// <summary>
		/// should i create a partial implementation class
		/// </summary>
		/// <value>
		/// <c>true</c> if create implementation; otherwise, <c>false</c>.
		/// </value>
		[BindableProperty("ChkCreateImplementation","Toggled")]
		public bool CreateImplementation {
			get;
			set;
		}

		/// <summary>
		/// namespace for the window
		/// </summary>
		/// <value>
		/// The namespace.
		/// </value>
		[BindableProperty("EntryNamespace","Changed")]
		public String Namespace {
			get;
			set;
		}

		/// <summary>
		/// classname from window
		/// </summary>
		/// <value>
		/// The name of the class.
		/// </value>
		[BindableProperty("EntryClassName","Changed")]
		public String ClassName {
			get;
			set;
		}

		/// <summary>
		/// should i derive from Gtk.Window or a custom widget 
		/// </summary>
		/// <value>
		/// <c>true</c> if use custom widget; otherwise, <c>false</c>.
		/// </value>
		[BindableProperty("ChkUseCustomWidget","Toggled")]
		public bool UseCustomWidget {
			get;
			set;
		}

		/// <summary>
		/// classname from the custom widget
		/// </summary>
		/// <value>
		/// The name of the custom widget.
		/// </value>
		[BindableProperty("EntryCustomWidgetName","Changed")]
		public String CustomWidgetName {
			get;
			set;
		}

		/// <summary>
		/// Create gtk-sharp code for Gtk2
		/// </summary>
		/// <value>
		/// <c>true</c> if use gtk2; otherwise, <c>false</c>.
		/// </value>
		[BindableProperty("ChkUseGtk2","Toggled")]
		public bool UseGtk2 {
			get;
			set;
		}

		/// <summary>
		/// Create Mono XWT Code
		/// </summary>
		/// <value>
		/// <c>true</c> if xwt output; otherwise, <c>false</c>.
		/// </value>
		[BindableProperty("ChkXwtOutput","Toggled")]
		public bool XwtOutput {
			get;
			set;
		}

		private GladeFile ()
		{
		}

		public GladeFile (String filename)
		{
			Filename = filename;
		}

		public static GladeFile CreateInstance(XmlNode node)
		{
			GladeFile ret = new GladeFile();

			ret.WindowName = node.AttributeValue("windowname");
			ret.OutputPath = node.AttributeValue("outputpath");
			ret.Filename = node.AttributeValue("filename");
			ret.CreateImplementation = node.AttributeValueBool("createimplementation");
			ret.Namespace = node.AttributeValue("namespace");
			ret.ClassName = node.AttributeValue("class");
			ret.UseCustomWidget = node.AttributeValueBool("customwidget");
			ret.CustomWidgetName = node.AttributeValue("customwidgetclass");
			ret.UseGtk2 = node.AttributeValueBool("usegtk2");
			ret.XwtOutput = node.AttributeValueBool("xwtoutput");
			return ret;
		}

		public XmlElement save(XmlNode parentnode)
		{
			XmlElement data = parentnode.OwnerDocument.CreateElement("gladefile");

			data.AddAttributeValue("windowname",WindowName);
			data.AddAttributeValue("outputpath",OutputPath);
			data.AddAttributeValue("filename",Filename);
			data.AddAttributeValue("createimplementation",CreateImplementation.ToString());
			data.AddAttributeValue("namespace",Namespace);
			data.AddAttributeValue("class",ClassName);
			data.AddAttributeValue("customwidgetclass",CustomWidgetName);
			data.AddAttributeValue("customwidget",UseCustomWidget.ToString());
			data.AddAttributeValue("usegtk2",UseGtk2.ToString());
			data.AddAttributeValue("xwtoutput",XwtOutput.ToString());

			parentnode.AppendChild(data);

			return data;
		}


	}
}

