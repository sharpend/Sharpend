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
using Gtk;
using Sharpend.GtkSharp;
using Sharpend.Utils;
using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace GladeBuilder{	public partial class builder: Gtk.Window	{
		/// <summary>
		/// List for files
		/// </summary>
		/// <value>
		/// The tree list.
		/// </value>
		public GtkListTreeView TreeList {
			get;
			private set;
		}

		public DataObjectContainer DataObjects {
			get;
			private set;
		}

		/// <summary>
		/// init control
		/// </summary>		public void init() 
		{
			DataBinder.OnBindException += HandleOnBindException;
			DataObjects = new DataObjectContainer();

			TreeList = new GtkListTreeView(treeview1);
			TreeList.loadStructureFromRessource("GladeBuilder.list.xml");
			treeview1.Selection.Mode = SelectionMode.Multiple;
			treeview1.RowActivated += HandleRowActivated;

			btnAddFiles.SelectionChanged += HandleSelectionChanged;
			//btnAddFiles.SelectMultiple = true;
		    //btnAddFiles.Shown += HandleShown;

//			Gtk.FileFilter ff = new Gtk.FileFilter();
//			ff.AddPattern("*.glade");
//			btnAddFiles.Filter = ff;

			FileChooserButtonWrapper fc1 = new FileChooserButtonWrapper("Please select a .glade file",BtnAddFiles,FileChooserAction.Open);
			fc1.OnSelectionChanged += HandleSelectionChanged;
			fc1.AddPattern("*.glade");

			btnAddFiles.Action = FileChooserAction.Open;
			btnRemoveSelected.Clicked += RemoveSelectedClicked;

			//BtnSelectOutputPath.Action = FileChooserAction.SelectFolder;
			//BtnSelectOutputPath.SelectionChanged += BtnSelectOutputPathChanged;
			FileChooserButtonWrapper fc2 = new FileChooserButtonWrapper("Please select an output path",BtnSelectOutputPath,EntryOutputPath,FileChooserAction.SelectFolder);


			btnSaveAs.Clicked += BtnSaveAsClicked;

			//config files
			//btnSelectConfigFile.SelectionChanged += BtnConfigFileSelectionChanged;

			FileChooserButtonWrapper fc = new FileChooserButtonWrapper("Please select a .gladebuilder file", BtnSelectConfigFile,EntryConfigFile,FileChooserAction.Open);
			fc.AddPattern("*.gladebuilder");
			fc.OnSelectionChanged += BtnConfigFileSelectionChanged;

			BtnGenerate.Clicked += BtnGenerateClicked;

			btnExit.Clicked += delegate(object sender, EventArgs e) {
				Application.Quit();
			};

			GladeFile.connect(typeof(GladeFile),this);
			restoreSettings();
		}

		void HandleOnBindException (object sender, EventArgs e)
		{
			textviewMessages.Buffer.Text = "no .glade file selected ! \n";
			notebook1.CurrentPage = 0;
//			textviewMessages.Buffer.Clear();
//			Exception ex = sender as Exception;
//			if (ex != null)
//			{
//				textviewMessages.Buffer.Text = ex.ToString();
//			}
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
			try
			{
				if (TreeList.Rows.Count < 1)
				{
					textviewMessages.Buffer.Text += "there are no .glade files in the list !\n";
					return;
				}

				textviewMessages.Buffer.Clear();
				textviewMessages.Buffer.Text += "start generating...\n";
				String fn = EntryConfigFile.Text.Trim();
				if (String.IsNullOrEmpty(fn))
				{
					saveAs();
				}

				fn = EntryConfigFile.Text.Trim();
				if (saveFile(fn))
				{
					Sharpend.Glade.GladeBuilder.generateCode(fn,true);
					textviewMessages.Buffer.Text += "done\n";
				} else
				{
					textviewMessages.Buffer.Text += "error, could not save file '" + fn + "' \n";
				}
			} catch (Exception ex)
			{
				textviewMessages.Buffer.Text = ex.ToString();
			}
		}


		protected override void OnDestroyed ()
		{
			saveSettings();
			base.OnDestroyed ();
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

			//Sharpend.Configuration.ConfigurationManager.setValue("/configuration/lastloaded",EntryConfigFile.Text.Trim(),true);
			Sharpend.Configuration.ConfigurationManager.setValue("/configuration/pane",paned1.Position.ToString(),true);
		}

		/// <summary>
		/// restore settings from config file
		/// </summary>
		private void restoreSettings()
		{
//			String s = Sharpend.Configuration.ConfigurationManager.getString("lastloaded");
//			if (!String.IsNullOrEmpty(s))
//			{
//				EntryConfigFile.Text = s;
//				loadData(entryConfigFile.Text.Trim());
//			}

			String p = Sharpend.Configuration.ConfigurationManager.getString("pane");
			if (!String.IsNullOrEmpty(p))
			{
				paned1.Position = Convert.ToInt32(p);
			}
		}

//		void BtnSelectOutputPathChanged (object sender, EventArgs e)
//		{
//			entryOutputPath.Text = BtnSelectOutputPath.CurrentFolder;
//		}

		void BtnSaveAsClicked (object sender, EventArgs e)
		{
			saveAs();
		}

		/// <summary>
		/// remove selected rows from the filelist
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void RemoveSelectedClicked (object sender, EventArgs e)
		{
			List<VirtualGridRow> lst = TreeList.getSelectedRows(); 
			for (int i=lst.Count-1;i>-1;i--)
            {
				VirtualGridRow rw = lst[i];
				TreeList.removeRow(rw);
			}
		}

		/// <summary>
		/// load
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
//		void LoadConfigFile (object sender, EventArgs e)
//		{
//			loadData(entryConfigFile.Text.Trim());
//		}

		void BtnConfigFileSelectionChanged (object sender, EventArgs e)
		{
			loadData(entryConfigFile.Text.Trim());
		}

		void HandleShown (object sender, EventArgs e)
		{
			btnAddFiles.SelectMultiple = true;	
		}

		/// <summary>
		/// load data from a config file
		/// </summary>
		/// <param name='filename'>
		/// Filename.
		/// </param>
		private void loadData(String filename)
		{
			TreeList.Rows.Clear();

			XmlDocument doc = new XmlDocument();
			doc.Load(filename);

			XmlNodeList lst = doc.SelectNodes("//gladefile");
			foreach (XmlNode nd in lst)
			{
				GladeFile f = GladeFile.CreateInstance(nd);

				VirtualGridRow row = TreeList.newRow();
				row.setData("Filename",f.Filename);
				row.setData("Object",f);	
			}

			TreeList.reload();
		}

		/// <summary>
		/// Row in the treelist is activated (selected)
		/// </summary>
		/// <param name='o'>
		/// O.
		/// </param>
		/// <param name='args'>
		/// Arguments.
		/// </param>
		void HandleRowActivated (object o, RowActivatedArgs args)
		{
			VirtualGridRow row = TreeList.getSelectedRow();
			if (row != null)
			{
				GladeFile gf = (row.getData("object") as GladeFile);
				DataObjects.Add(gf);
				gf.Bind(this);
			}
		}

		/// <summary>
		/// file selection changed
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// E.
		/// </param>
		void HandleSelectionChanged (object sender, EventArgs e)
		{
			//TreeList.Rows.Clear();

			foreach (GLib.File f in btnAddFiles.Files)
			{
				VirtualGridRow row = TreeList.newRow();
				FileInfo fi = new FileInfo(f.Path);
				row.setData("Filename",fi.Name);
				row.setData("Path",fi.Directory.FullName);
				GladeFile gf = new GladeFile(fi.FullName);
				row.setData("Object",gf);
				DataObjects.Add(gf);
				setDefaults(gf);
				gf.Bind(this);
			}
			TreeList.reload();
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
		private void doSave(String fullname)
		{
			XmlDocument d = new XmlDocument();

			XmlElement config = d.CreateElement("configuration");
			d.AppendChild(config);

			XmlElement glade = d.CreateElement("glade");
			config.AppendChild(glade);

			foreach (VirtualGridRow row in TreeList.Rows)
			{
				GladeFile gf = (row.getData("Object") as GladeFile);
				gf.save(glade);
			}

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
			if (fi.Exists) 
			{
//				save = false;
//				var dialog = new MessageDialog(this,DialogFlags.Modal,MessageType.Info,ButtonsType.OkCancel,
//				                               "File " + fi.FullName + " exists, overwrite ?",new object[0]); 	
//				int res = dialog.Run ();
//				if ((ResponseType) res == ResponseType.Ok) {
//					save = true;
//					fi.Delete();
//				}
//
//				dialog.Destroy();
				save = false;
				if (Message.ShowInfoMessage("File " + fi.FullName + " exists, overwrite ?",this) == ResponseType.Ok)
				{
					save = true;
					fi.Delete();
				}
			}

			if (save)
			{
				doSave(fullname);
				return true;
			}

			return false;
		}

		/// <summary>
		/// do a "save as"
		/// </summary>
		private void saveAs()
		{
			if (TreeList.Rows.Count < 1)
			{
				textviewMessages.Buffer.Text += "there are no .glade files in the list !\n";
				return;
			}

			Gtk.FileChooserDialog fc = new FileChooserDialog("Save As",this,FileChooserAction.Save,Stock.Cancel, ResponseType.Cancel,
                Stock.Open, ResponseType.Ok);

			int response = fc.Run();

			if ((ResponseType) response == ResponseType.Ok) {
				saveFile(fc.Filename);
				entryFilename.Text = fc.Filename;
			}

			fc.Destroy();
		}

	} //class} //namespace