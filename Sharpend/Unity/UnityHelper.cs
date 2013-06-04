//
// UnityHelper.cs
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
using System.IO;
#if LINUX
using Mono.Posix;

namespace Sharpend
{
	/// <summary>
	/// This static class contains some useful functions if you are using unity
	/// </summary>
	public static class UnityHelper
	{
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		/// <param name='application'>
		/// Application.
		/// </param>
		/// <param name='fullfilename'>
		/// Fullfilename.
		/// </param>
		/// <param name='iconname'>
		/// Iconname.
		/// </param>
		/// <param name='applicationname'>
		/// Applicationname.
		/// </param>
		public static void createLauncherEntry(String path, String application, String fullfilename, String iconname, String applicationname)
		{
			TextWriter tw = new StreamWriter(fullfilename);
									
			tw.WriteLine("[Desktop Entry]");
			tw.WriteLine("Name="+applicationname);
			tw.WriteLine("Comment="+applicationname);
			tw.WriteLine("Exec="+path+application);
			tw.WriteLine("TryExec="+path+application);
			tw.WriteLine("Icon="+iconname);
			tw.WriteLine("Terminal=false");
			tw.WriteLine("Type=Application");
			tw.WriteLine("StartupNotify=true");
			
			tw.Close();
			
			//check the file permissions
			Mono.Unix.UnixFileInfo fi = new Mono.Unix.UnixFileInfo(fullfilename);
			if (fi.Exists)
			{
				if (! fi.FileAccessPermissions.HasFlag(Mono.Unix.FileAccessPermissions.UserExecute))
				{
					fi.FileAccessPermissions = fi.FileAccessPermissions | Mono.Unix.FileAccessPermissions.UserExecute;
				}	
				//Mono.Unix.FileAccessPermissions.
			}
			
		}
		
		/// <summary>
		/// This creates a {appname}.desktop file ... TODO
		/// </summary>
		/// <param name='iconname'>
		/// Iconname.
		/// </param>
		public static void createLauncherEntry(String iconname)
		{
			String app = System.AppDomain.CurrentDomain.FriendlyName;			
			String fn = Configuration.ConfigurationManager.AppSettings["launcherpath"];
			
			if (String.IsNullOrEmpty(fn))
			{
				fn = "~/.local/share/applications";
			}
			
			if (fn.Contains("~")) {							
				fn = fn.Replace("~",Environment.GetFolderPath(Environment.SpecialFolder.Personal));
			}
			
			if (!fn.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				fn += Path.DirectorySeparatorChar;
			}
			
			fn += app;			
			fn = fn.Replace(".exe",".desktop");
			
			String dir = System.AppDomain.CurrentDomain.BaseDirectory;			
			if (!dir.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				dir += Path.DirectorySeparatorChar;
			}
			
			if (!File.Exists(fn))
			{
				createLauncherEntry(dir, app,fn,dir + iconname,app);
			}			
		}
		
		
		
	}
}

#endif