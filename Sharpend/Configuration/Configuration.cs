//
// Configuration.cs
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
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Sharpend.Extensions;

namespace Sharpend.Configuration
{
	/// <summary>
	/// ConfigurationManager for accessing the app.config or custom xml config files as well
	/// 
	/// The ConfigurationManager will look in some standard paths for the configuration files
	/// 
	/// e.g.
	/// 
	/// - Application Directory
	/// - Home Directory (.applicationname)  all lower case
	/// - some windows specific paths as the plan is to be platform undepended
	/// 
	/// </summary>
	public static class ConfigurationManager
	{	
		private static IAppSettingsProvider appSettings;
		private static Dictionary<String,XmlDocument> configs = new Dictionary<String, XmlDocument>(20);
		
		/// <summary>
		/// Standard app settings.
		/// </summary>
		private class StandardAppSettings : IAppSettingsProvider
		{
			public String this[String index]
			{
				get 
				{
					return System.Configuration.ConfigurationManager.AppSettings[index];
				}
				set
				{
					throw new NotImplementedException("StandardAppSettings.setter");
				}
			}	
		}
		
		/// <summary>
		/// Gets the app settings.
		/// </summary>
		/// <value>
		/// The app settings.
		/// </value>
		public static IAppSettingsProvider AppSettings
		{
			get {
				return appSettings;
			}
		}
		
		
		/// <summary>
		/// Initializes the <see cref="Sharpend.Configuration.ConfigurationManager"/> class.
		/// </summary>
		static ConfigurationManager()
		{
			appSettings = new StandardAppSettings();
		}


		/// <summary>
		/// loads a custom config the function will look in some standard pathes for the file
		/// </summary>
		/// <param name='filename'>
		/// Filename.
		/// </param>
		public static void loadConfigFile(String filename)
		{
			if (configs.ContainsKey(filename))
			{
				return;
			}
			
			FileInfo fi = getConfigFile(filename);
			if (fi != null)
			{
				loadFromFile(fi);
			}
			
			// trying some paths for a config file
//			bool loaded = false;
//			String path = Environment.CurrentDirectory;
//			if (!loaded)
//				loaded = loadFromFile(path,filename);
//			
//			//the home directory
//			path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
//			if (!loaded)
//				loaded = loadFromFile(path,filename);
//			
//			String app = System.AppDomain.CurrentDomain.FriendlyName.ToLower();
//			app = app.Replace(".exe",String.Empty);
//						
//			path += "./" + app;
//			if (!loaded)
//				loaded = loadFromFile(path,filename);
//
//            path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
//            if (!loaded)
//                loaded = loadFromFile(path,filename);
//
//            path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
//            if (!loaded)
//                loaded = loadFromFile(path,filename);
//
//            path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
//            if (!loaded)
//                loaded = loadFromFile(path,filename);	
			
			if (fi == null)
				throw new Exception("could not load configuration");
			
		}

		/// <summary>
		/// returns the appication config file ... e.g. "myapplication.config"
		/// </summary>
		/// <returns>
		/// The application config.
		/// </returns>
		public static FileInfo getApplicationConfig()
		{
			String app = System.AppDomain.CurrentDomain.FriendlyName.ToLower();

			String configfile = app.Replace(".exe",".config");
			configfile = configfile.ToLower();
			return getConfigFile(configfile);
		}

		/// <summary>
		/// creates a new application config in the directory of GetEntryAssembly
		/// </summary>
		/// <returns>
		/// The application config.
		/// </returns>
		public static FileInfo createApplicationConfig()
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();

			FileInfo fi = new FileInfo(assembly.Location);
			if (fi.Directory.Exists)
			{
				XmlDocument doc = new XmlDocument();
				XmlElement elem = doc.CreateElement("configuration");
				doc.AppendChild(elem);

				String app = System.AppDomain.CurrentDomain.FriendlyName.ToLower();
				String configfile = app.Replace(".exe",".config");

				String fn = fi.Directory.FullName + Path.DirectorySeparatorChar + configfile;
				doc.Save(fn);

				return getApplicationConfig();
			}

			return null;
		}


		/// <summary>
		/// returns the fileinfo for a configfile ... will look in some standard pathes
		/// </summary>
		/// <returns>
		/// The config file.
		/// </returns>
		/// <param name='filename'>
		/// Filename.
		/// </param>
		public static FileInfo getConfigFile(String filename)
		{
			// trying some paths for a config file
			FileInfo fi = null;

			FileInfo entry = new FileInfo (System.Reflection.Assembly.GetEntryAssembly ().Location);
			if (fi == null)
				fi = getFileInfo (entry.Directory.FullName, filename);

			//TODO ??  System.Reflection.Assembly.GetExecutingAssembly().Location;

			//CurrentDirectory ... 
			String path = Environment.CurrentDirectory;
			if (fi == null)
				fi = getFileInfo(path,filename);
		
			//the home directory
			path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			if (fi == null)
				fi = getFileInfo(path,filename);
			
			String app = System.AppDomain.CurrentDomain.FriendlyName.ToLower();
			app = app.Replace(".exe",String.Empty);

			path += "/." + app;
			if (fi == null)
				fi = getFileInfo(path,filename);

            path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (fi == null)
                fi = getFileInfo(path,filename);

            path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            if (fi == null)
                fi = getFileInfo(path,filename);

            path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (fi == null)
                fi = getFileInfo(path,filename);
			
			return fi;
		}
		
		public static void flushConfigfile(String filename)
		{
			if (configs.ContainsKey(filename))
			{
				configs.Remove(filename);
			}
		}
		
		public static XmlNodeList getValues(String configfile, String xpath)
		{		
			if (!configs.ContainsKey(configfile))
			{
				loadConfigFile(configfile);
			}
			
			XmlDocument d = configs[configfile];
			if (d != null)
			{
				return d.SelectNodes(xpath);
			}
			return null;
		}

		/// <summary>
		/// sets a config value in the application config file
		/// </summary>
		/// <returns>
		/// The value.
		/// </returns>
		/// <param name='xpath'>
		/// If set to <c>true</c> xpath.
		/// </param>
		/// <param name='value'>
		/// If set to <c>true</c> value.
		/// </param>
		public static bool setValue(String xpath, String value,bool createIfnotExist=false)
		{
			FileInfo fi = getApplicationConfig();
			return setValue(fi.Name,xpath,value,createIfnotExist);
		}


		public static bool setValue(String configfile, String xpath, String value,bool createIfnotExist=false)
		{		
			if (!configs.ContainsKey(configfile))
			{
				loadConfigFile(configfile);
			}

			XmlDocument d = configs[configfile];
			if (d != null)
			{
				XmlNode node = d.SelectSingleNode(xpath);

				if ((node == null) && createIfnotExist)
				{
					node = XmlHelper.makeXPath(d,xpath);
				}
				//node.Value = value;
				node.InnerText = value;

				FileInfo fi = getConfigFile(configfile);
				d.Save(fi.FullName);
				return true;
			}
			return false;
		}

		public static XmlNode getValue(String xpath)
		{
			String app = System.AppDomain.CurrentDomain.FriendlyName.ToLower();
			String configfile = app.Replace(".exe",".config");
			configfile = configfile.ToLower();
			return getValue(configfile,xpath);
		}

		/// <summary>
		/// returns an elementvalue as string
		/// if usexpath=true elementname must be an xpath
		/// otherwise elementname must be an child of /configuration 
		/// </summary>
		/// <returns>
		///  an elementvalue if exist, otherwise String.Empty
		/// </returns>
		/// <param name='elementname'>
		/// Elementname.
		/// </param>
		/// <param name='usexpath'>
		/// Usexpath.
		/// </param>
		public static String getString(String elementname, bool usexpath)
		{

			String xp = "/configuration/" + elementname;
			if (usexpath)
			{
				xp = elementname;
			}

			XmlNode nd = getValue(xp);
			if (nd != null)
			{
				return nd.InnerText;
			}
			return String.Empty;
		}

		/// <summary>
		/// returns string element value from specified configfile
		/// </summary>
		/// <returns>
		/// The string.
		/// </returns>
		/// <param name='configfile'>
		/// Configfile.
		/// </param>
		/// <param name='elementname'>
		/// Elementname.
		/// </param>
		/// <param name='usexpath'>
		/// Usexpath.
		/// </param>
		public static String getString(String configfile, String elementname, bool usexpath)
		{
			String xp = "/configuration/" + elementname;
			if (usexpath)
			{
				xp = elementname;
			}

			XmlNode nd = getValue(configfile,xp);
			if (nd != null)
			{
				return nd.InnerText;
			}
			return String.Empty;
		}


		/// <summary>
		/// returns an elementvalue as string where the element 
		/// must be a childnode from the /configuration node
		/// </summary>
		/// <returns>
		/// The string.
		/// </returns>
		/// <param name='elementname'>
		/// Elementname.
		/// </param>
		public static String getString(String elementname)
		{
			return getString(elementname,false);
		}

		public static bool getBool(String elementname)
		{
			String val = getString(elementname);
			if (!String.IsNullOrEmpty(val))
			{
				return Convert.ToBoolean(val);
			}

			return false;
		}
				
		public static XmlNode getValue(String configfile, String xpath)
		{
			if (!configs.ContainsKey(configfile))
			{
				loadConfigFile(configfile);
			}
			
			XmlDocument d = configs[configfile];
			if (d != null)
			{
				return d.SelectSingleNode(xpath);
			}
			return null;
		}
		
		private static FileInfo getFileInfo(String path, String filename)
		{
			if (! path.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				path += Path.DirectorySeparatorChar.ToString();
			}
			
			if (File.Exists(path  + filename))
			{
				return new FileInfo(path + filename);
			}
			
			return null;
		}
		
		private static bool loadFromFile(FileInfo fi)
		{
//			if (! path.EndsWith(Path.DirectorySeparatorChar.ToString()))
//			{
//				path += Path.DirectorySeparatorChar.ToString();
//			}
//			
//			if (File.Exists(path  + filename))
//			{
//				XmlDocument doc = new XmlDocument();
//				doc.Load(path + filename);
//				configs.Add(filename,doc);
//				return true;
//			}
			
			if (fi.Exists)
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(fi.FullName);

				if (configs.ContainsKey(fi.Name))
				{
					configs.Remove(fi.Name);
				}
				configs.Add(fi.Name,doc);

				return true;
			}	
			
			return false;
		}
		
		
		
	}
}

