//
// builderImplementation.cs
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
using Sharpend.Utils;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using Xwt;
using Xwt.Formats;
using Sharpend.Databinding;

namespace GladeBuilder{	public partial class builder: Window	{

		/// <summary>
		/// Container for the bindable DataObjects
		/// </summary>
		/// <value>
		/// The data objects.
		/// </value>
		public DataObjectContainer DataObjects {
			get;
			private set;
		}

		//The Datafields for the tree
		private DataField<String> dfFilename = new DataField<String>();
		private DataField<GladeFile> dfGladefile = new DataField<GladeFile>();

		/// <summary>
		/// Treestore containing data from the glade files
		/// </summary>
		/// <value>
		/// The ts data.
		/// </value>
		public TreeStore TsData {
			get;
			private set;
		}

		/// <summary>
		/// The current .gladebuilder file 
		/// </summary>
		/// <value>
		/// The current file.
		/// </value>
		public String CurrentFile {
			get;
			private set;
		}

		/// <summary>
		/// init control
		/// </summary>		public void init() 
		{
			//DataBinder.OnBindException += HandleOnBindException;
			DataObjects = new DataObjectContainer();

			BtnOpenFile.Clicked += HandleOpenFileClicked;
			BtnGenerate.Clicked += BtnGenerateClicked;
			BtnNew.Clicked += HandleBtnNewClicked;
			BtnAddFile.Clicked += HandleBtnAddClicked;
			BtnRemoveSelected.Clicked += HandleBtnRemoveClicked;
			BtnSaveAs.Clicked += HandleBtnSaveAsClicked;

			//the tree (not used as tree here)
			TsData = new TreeStore(dfFilename,dfGladefile);
			Treeview1.DataSource = TsData;
			Treeview1.Columns.Add ("Filename", dfFilename);
			Treeview1.Columns.Add ("GladeFile", dfGladefile);
			Treeview1.SelectionChanged += TreeViewSelectionChangend;
			Treeview1.ButtonPressed += HandleTreeViewButtonPressed;

			CurrentFile = String.Empty;

			GladeFile.connect(typeof(GladeFile),this);
			restoreSettings();
		}

		void HandleBtnSaveAsClicked (object sender, EventArgs e)
		{
			saveAs();
		}

		/// <summary>
		/// remove the selected files from the treestore
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleBtnRemoveClicked (object sender, EventArgs e)
		{
//			TreePosition[] tp = Treeview1.SelectedRows;
//			foreach (TreePosition p in tp)
//			{
//				TsData.GetNavigatorAt(p).Remove();
//			}

			TreeNavigator tn = TsData.GetFirstNode();
			do
			{
				tn.Remove();
			} while (tn.MoveNext());
			clearEntries();
		}

		/// <summary>
		/// Add a new glade file in the datastore
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleBtnAddClicked (object sender, EventArgs e)
		{
			OpenFileDialog fc = new OpenFileDialog("select a glade file");
			fc.Filters.Add(new FileDialogFilter("Glade","*.glade"));
			if (fc.Run())
			{
				GladeFile gf = new GladeFile(fc.FileName);
				gf.WindowName = "window1";
				gf.Namespace = "window1";
				gf.ClassName = "window1";
				gf.CreateImplementation = true;
				gf.OutputPath = new FileInfo(fc.FileName).Directory.FullName;
				TsData.AddNode().SetValue(dfFilename,gf.Filename).SetValue(dfGladefile,gf);
			}
		}

		//clear all entries ... kind of workaround
		private void clearEntries()
		{	
			GladeFile gf = new GladeFile("");
			gf.ClassName = String.Empty;
			gf.CreateImplementation = true;
			gf.CustomWidgetName = String.Empty;
			gf.Filename = String.Empty;
			gf.Filename = String.Empty;
			gf.Namespace = String.Empty;
			gf.OutputPath = String.Empty;
			gf.Target = String.Empty;
			gf.UseGtk2 = false;
			gf.WindowName = String.Empty;
			gf.XwtOutput = false;
			DataObjects.Add(gf);
			gf.Bind(this);
			DataObjects.DataObjects.Clear();
		}

		/// <summary>
		/// Clear all entries and remove all glade files
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleBtnNewClicked (object sender, EventArgs e)
		{
			TsData.Clear();
			CurrentFile = String.Empty;
			clearEntries();
			TextviewMessages.LoadText(String.Empty,TextFormat.Markdown);
		}

		void HandleTreeViewButtonPressed (object sender, ButtonEventArgs e)
		{
			if (e.MultiplePress == 2)
			{
				TreePosition tp = Treeview1.SelectedRow;

				GladeFile gf = (GladeFile)Treeview1.DataSource.GetValue(tp,1);
				if (gf != null)
				{
					DataObjects.Add(gf);
					gf.Bind(this);
				}
			}
		}


		void HandleOpenFileClicked (object sender, EventArgs e)
		{
			OpenFileDialog fc = new OpenFileDialog("select a gladebuilder file");
			fc.Filters.Add(new FileDialogFilter("Gladebuilder","*.gladebuilder"));
			if (fc.Run())
			{
				loadData(fc.FileName);
			}
		}

		void HandleOnBindException (object sender, EventArgs e)
		{
			String txt = "no .glade file selected ! \n";
			Notebook1.CurrentTabIndex = 0;
			Exception ex = sender as Exception;
			if (ex != null)
			{
				txt = ex.ToString();
			}

			TextviewMessages.LoadText(txt,TextFormat.Markdown);
		}

		/// <summary>
		/// gnerate c# code from .gladebuilder file
		/// </summary>
		/// <param name='filename'>
		/// Filename.
		/// </param>
		private void generateCode(String filename)
		{
			try
			{
				TextviewMessages.LoadText("start generating c# code.\n",TextFormat.Markdown);
				if (String.IsNullOrEmpty(filename))
				{
					throw new Exception("parameter filename must not be empty");
				}

				Sharpend.Glade.GladeBuilder.generateCode(filename,true);
				TextviewMessages.LoadText("done.",TextFormat.Markdown);
			} 
			catch (Exception ex)
			{
				String txt = "error while generating c# code: \n";
				txt += ex.ToString();
				TextviewMessages.LoadText(txt,TextFormat.Markdown);
			}
		}

		/// <summary>
		/// generate files clicked
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void BtnGenerateClicked (object sender, EventArgs e)
		{
			TreeNavigator tn = TsData.GetFirstNode();
			if (tn.CurrentPosition == null)
			{
				TextviewMessages.LoadText("no glade files in list!\n",TextFormat.Markdown);
				return;
			}

			if (String.IsNullOrEmpty(CurrentFile))
			{
				saveAs();
			} else
			{
				save();
			}

			generateCode(CurrentFile);		
		}

		protected override bool OnCloseRequested ()
		{
			saveSettings();
			return base.OnCloseRequested ();
		}

		/// <summary>
		/// save settings in the config file
		/// </summary>
		private void saveSettings()
		{
			FileInfo fi = Sharpend.Configuration.ConfigurationManager.getApplicationConfig();

			if (fi == null)
			{
				fi = Sharpend.Configuration.ConfigurationManager.createApplicationConfig();
			}

			Sharpend.Configuration.ConfigurationManager.setValue("/configuration/lastloaded",CurrentFile,true);
			Sharpend.Configuration.ConfigurationManager.setValue("/configuration/pane",paned1.Position.ToString(),true);
		}

		/// <summary>
		/// restore settings from config file
		/// </summary>
		private void restoreSettings()
		{
			FileInfo fi = Sharpend.Configuration.ConfigurationManager.getApplicationConfig();

			if (fi == null)
			{
				fi = Sharpend.Configuration.ConfigurationManager.createApplicationConfig();
			}

			String s = Sharpend.Configuration.ConfigurationManager.getString("lastloaded");
			if (!String.IsNullOrEmpty(s))
			{
				loadData(s);
			}

			String p = Sharpend.Configuration.ConfigurationManager.getString("pane");
			if (!String.IsNullOrEmpty(p))
			{
				paned1.Position = Convert.ToInt32(p);
			}
		}

		void BtnSaveAsClicked (object sender, EventArgs e)
		{
			saveAs();
		}


		void TreeViewSelectionChangend (object sender, EventArgs e)
		{
			TreePosition tp = Treeview1.SelectedRow;
			GladeFile gf = (GladeFile)Treeview1.DataSource.GetValue(tp,1);
			if (gf != null)
			{
				DataObjects.Add(gf);
				gf.Bind(this);
			}
		}


		/// <summary>
		/// load data from a config file
		/// </summary>
		/// <param name='filename'>
		/// Filename.
		/// </param>
		private void loadData(String filename)
		{
			TsData.Clear();

			XmlDocument doc = new XmlDocument();
			doc.Load(filename);
			XmlNodeList lst = doc.SelectNodes("//gladefile");
			foreach (XmlNode nd in lst)
			{
				GladeFile f = GladeFile.CreateInstance(nd);
				TsData.AddNode().SetValue(dfFilename,f.Filename).SetValue(dfGladefile,f);
			}
			CurrentFile = filename;
		
			TreeNavigator tn = TsData.GetFirstNode();
			Treeview1.SelectRow(tn.CurrentPosition);
			TextviewMessages.LoadText("File " + CurrentFile + " loaded\n",TextFormat.Markdown);
		}


		/// <summary>
		/// set default values
		/// </summary>
		private void setDefaults(GladeFile gf)
		{
			FileInfo fi = new FileInfo(gf.Filename);

			gf.ClassName = fi.Name.Replace(fi.Extension,String.Empty);
			gf.WindowName = "window1"; //Glade default ?
			gf.Namespace = gf.ClassName;
			gf.CreateImplementation = true;
			gf.OutputPath = fi.Directory.FullName;
		}

		/// <summary>
		/// save to file
		/// </summary>
		/// <param name='fullname'>
		/// Fullname.
		/// </param>
		private void doSave(String fullname, bool clear)
		{
			XmlDocument d = new XmlDocument();

			if (clear)
			{
				d.Load(fullname);
				d.RemoveAll();
			}

			XmlElement config = d.CreateElement("configuration");
			d.AppendChild(config);

			XmlElement glade = d.CreateElement("glade");
			config.AppendChild(glade);

			TreeNavigator tn = TsData.GetFirstNode();
			do
			{
				GladeFile gf = (GladeFile)tn.GetValue(dfGladefile);
				gf.save(glade);
			} while (tn.MoveNext());

			d.Save(fullname);
		}

		private bool saveFile(String fullname)
		{
			if (String.IsNullOrEmpty(fullname))
			{
				return false;
			}

			FileInfo fi = new FileInfo(fullname);
			bool save = true;	
			bool clear = false;
			if (fi.Exists) 
			{
				save = false;
				//TODO XWT
//				if (Message.ShowInfoMessage("File " + fi.FullName + " exists, overwrite ?",this) == ResponseType.Ok)
//				{
//					save = true;
//					clear = true;
//				}
			}
		
			if (save)
			{
				doSave(fullname,clear);
				return true;
			}

			return false;
		}

		/// <summary>
		/// save the current file
		/// </summary>
		private void save()
		{
			doSave(CurrentFile,true);
		}

		/// <summary>
		/// do a "save as"
		/// </summary>
		private void saveAs()
		{
			TreeNavigator tn = TsData.GetFirstNode();
			if (tn.CurrentPosition == null)
			{
				TextviewMessages.LoadText("no glade files in list!\n",TextFormat.Markdown);
				return;
			}

			//FileDialogFilter fi = new FileDialogFilter(
			SaveFileDialog fc = new SaveFileDialog("save as");
			fc.Filters.Add(new FileDialogFilter("Gladebuilder","*.gladebuilder"));
			if (fc.Run())
			{
				CurrentFile = fc.FileName;
				doSave(CurrentFile,false);
			}	
		}

	} //class} //namespace