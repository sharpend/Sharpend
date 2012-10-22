//
// FileChooserWrapper.cs
//
//  Author:
//       Dirk Lehmeier <sharpend_develop@yahoo.de>
//
//  Coptyright (c) 2012 Dirk Lehmeier
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

using GLib;
using Gtk;
using System;
using Gdk;
using System.Xml.Serialization;
using System.Xml;


namespace Sharpend.GtkSharp
{
	/// <summary>
	/// File chooser wrapper.
	/// 
	/// The FileChooserWrapper contains some extentsions for a FileChooser and
	/// connects it optional to a Entry Widget
	/// </summary>
	public class FileChooserButtonWrapper
	{
		/// <summary>
		/// Wrapped FileChooserButton
		/// </summary>
		/// <value>
		/// The chooser.
		/// </value>
		public FileChooserButton Chooser {
			get;
			private set;
		}

		/// <summary>
		/// Entry Widget (optional)
		/// </summary>
		/// <value>
		/// The entry path.
		/// </value>
		public Entry EntryPath {
			get;
			private set;
		}

		/// <summary>
		/// Current File Filter
		/// </summary>
		/// <value>
		/// The current filter.
		/// </value>
		public FileFilter CurrentFilter {
			get;
			private set;
		}

		/// <summary>
		/// Occurs when SelectionChanged event from the FilechooserButton is fired
		/// </summary>
		public event EventHandler OnSelectionChanged;

		/// <summary>
		/// Store data in specified configfile
		/// default this is null, then the application.config file ist used
		/// </summary>
		/// <value>
		/// The config file.
		/// </value>
		public String ConfigFile 
		{
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Sharpend.GtkSharp.FileChooserWrapper"/> class.
		/// </summary>
		/// <param name='title'>
		/// Title.
		/// </param>
		/// <param name='chooser'>
		/// Chooser.
		/// </param>
		/// <param name='action'>
		/// Action.
		/// </param>
		public FileChooserButtonWrapper (String title, FileChooserButton chooser,FileChooserAction action)
			: this (title,chooser,null,action)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Sharpend.GtkSharp.FileChooserWrapper"/> class.
		/// 
		/// </summary>
		/// <param name='title'>
		/// Title.
		/// </param>
		/// <param name='chooser'>
		/// Chooser.
		/// </param>
		/// <param name='entryPath'>
		/// Entry to display selected path (optional, can be null)
		/// </param>
		/// <param name='action'>
		/// Action.
		/// </param>
		public FileChooserButtonWrapper (String title, FileChooserButton chooser, Entry entryPath,FileChooserAction action)
		{
			Chooser = chooser;
			EntryPath = entryPath;
			Chooser.Action = action;
			ConfigFile = null;
			CurrentFilter = null;
			Chooser.Title = title;
			init();
		}

		/// <summary>
		/// add a new filepattern to the filechooserbutton (e.g. *.xml)
		/// </summary>
		/// <param name='pattern'>
		/// Pattern.
		/// </param>
		public void AddPattern(String pattern)
		{
			if (CurrentFilter == null)
			{
				CurrentFilter = new Gtk.FileFilter();
				CurrentFilter.AddPattern(pattern);
				Chooser.Filter = CurrentFilter;
			} else
			{
				CurrentFilter.AddPattern(pattern);
			}
		}

		/// <summary>
		/// initialize wrapper
		/// </summary>
		private void init()
		{
			if (EntryPath != null)
			{
				Chooser.SelectionChanged += HandleFileChooserSelectionChanged;
			}
			restoreData();
		}

		void HandleFileChooserSelectionChanged (object sender, EventArgs e)
		{
			if (Chooser.Filename == null)
			{
				return;
			}

			switch (Chooser.Action) {
				case FileChooserAction.SelectFolder:
					EntryPath.Text = Chooser.CurrentFolder;
					break;
				default:
					EntryPath.Text = Chooser.Filename;
				break;
			}
			storeData();

			if (OnSelectionChanged != null)
			{
				OnSelectionChanged(sender,e);
			}
		}

		/// <summary>
		/// save path of selected file or folder in the config
		/// </summary>
		private void storeData()
		{
			if (!String.IsNullOrEmpty(EntryPath.Text))
			{
				if (String.IsNullOrEmpty(ConfigFile))
				{
					System.IO.FileInfo fi = Sharpend.Configuration.ConfigurationManager.getApplicationConfig();
					if (fi == null)
					{
						fi = Sharpend.Configuration.ConfigurationManager.createApplicationConfig();
					}
				} else
				{
					System.IO.FileInfo fi = Sharpend.Configuration.ConfigurationManager.getConfigFile(ConfigFile);
					if (fi == null)
					{
						throw new Exception("configfile: " + ConfigFile + " does not exist! ");
					}
				}

				String xp = "/configuration/chooser/chooser_" + Chooser.Name;
				Sharpend.Configuration.ConfigurationManager.setValue(xp,EntryPath.Text,true);
			}
		}

		/// <summary>
		/// restore path from config file
		/// </summary>
		private void restoreData()
		{
			String xp = "/configuration/chooser/chooser_" + Chooser.Name;
			String cp = Sharpend.Configuration.ConfigurationManager.getString(xp,true);
			if (!String.IsNullOrEmpty(ConfigFile))
			{
				cp = Sharpend.Configuration.ConfigurationManager.getString(ConfigFile,xp,true);
			} 
		
			if (!String.IsNullOrEmpty(cp))
			{
				switch (Chooser.Action) 
				{
					case FileChooserAction.SelectFolder:
						Chooser.SetCurrentFolder(cp);
						break;
					default:
						Chooser.SetFilename(cp);
					break;
				}
			}
		}
	}
}

