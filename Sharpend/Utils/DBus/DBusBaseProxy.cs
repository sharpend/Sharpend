//
// DBusBaseProxy.cs
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
#if DBUS
using NDesk.DBus;
using org.freedesktop.DBus;

namespace Sharpend.Utils
{
	/// <summary>
	/// 
	/// </summary>
	/// <exception cref='Exception'>
	/// Represents errors that occur during application execution.
	/// </exception>
	public class DBusBaseProxy<T> where T:class
	{
		//private string DBusInterface = "org.gnome.Tomboy";
        //private string DBusPath = "/org/gnome/Tomboy/RemoteControl";
		public String DBusInterface {
			get;
			private set;
		}
		
		public String DBusPath {
			get;
			private set;
		}
		
		protected T remoteinterface;
		
		public DBusBaseProxy (String dbusinterface, String dbuspath)
		{
			DBusInterface = dbusinterface;
			DBusPath = dbuspath;
		}
		
		public T RemoteInterface  {
			get
			{
				if (remoteinterface == null) {       
					if (!Bus.Session.NameHasOwner (DBusInterface)) {
                        //Console.WriteLine("111aaa" + DBusInterface);
						return null;
                    }
					
					remoteinterface = Bus.Session.GetObject<T> (DBusInterface, new ObjectPath (DBusPath));
					
                    if (remoteinterface == null) {
						throw new Exception ("The {0} object could not be located on the DBus interface {1} " + 
                            DBusPath + "-" + DBusInterface); //TODO logging
                    }
                }
				
                return remoteinterface;	
			}
		}
		
		public virtual bool UnRegister()
		{
			//Bus.Session.Unregister(DBusInterface, new ObjectPath (DBusPath));
			try 
			{
				remoteinterface = null;
				if (Bus.Session.NameHasOwner (DBusInterface)) {
					Bus.Session.ReleaseName(DBusInterface);
					//Bus.Session.
					
					//Bus.Session.Unregister(new ObjectPath(DBusPath));
					return true;
				}
				return false;
			} catch (Exception ex)
			{
				Console.WriteLine("UnRegister: " + ex.ToString());
				return false;
			}	
		}

		/// <summary>
		/// release specified dbus interface
		/// </summary>
		/// <returns>
		/// The register.
		/// </returns>
		/// <param name='DBusInterface'>
		/// If set to <c>true</c> D bus interface.
		/// </param>
		public static bool ReleaseName(String DBusInterface)
		{
			if (Bus.Session.NameHasOwner (DBusInterface)) 
			{
				ReleaseNameReply rp = Bus.Session.ReleaseName(DBusInterface);

				Console.WriteLine(rp.ToString());
				if (rp == ReleaseNameReply.Released)
				{
					return true;
				}
			}
			return false;
		}
	
		/// <summary>
		/// unregister specified dbusinterface
		/// </summary>
		/// <param name='dbusinterface'>
		/// Dbusinterface.
		/// </param>
		/// <param name='DBusPath'>
		/// D bus path.
		/// </param>
		public static void UnRegister(String DBusPath)
		{
			Bus.Session.Unregister(new ObjectPath(DBusPath));
		}

		public virtual void Register()
		{
			if (RemoteInterface == null)
			{
				throw new Exception("Could not register interface");
			}
		}
		
		public static T Register<T>(string dbusinterface, string dbuspath) where T:class
		{
			//BusG.Init ();
						
			T remoteObject = Activator.CreateInstance<T>();

			Bus.Session.Register(new ObjectPath(dbuspath),remoteObject);
			if (Bus.Session.RequestName (dbusinterface) != RequestNameReply.PrimaryOwner)
			{
				return null;
			}
						
			return remoteObject;
		}
		
		
		
	}
}
#endif
