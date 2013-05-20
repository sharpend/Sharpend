//
//  DBusServer.cs
//
//  Author:
//       Dirk Lehmeier <sharpend_develop@yahoo.de>
//
//  Copyright (c) 2013 Dirk Lehmeier
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
using System.Threading;
using DBus;
using org.freedesktop.DBus;

namespace Sharpend
{
	public class DBusServer<T> where T:class
	{
		public String BusName {
			get;
			private set;
		}

		public String Object_Path {
			get;
			private set;
		}

		private T serverObject;
		private Bus m_bus;
		private ObjectPath m_path;

		public T ServerObject {
			get
			{
				return serverObject;
			}
		}

		public Thread Listener {
			get;
			private set;
		}

		public bool Running {
			get;
			private set;
		}

		public DBusServer (String busname, String objectPath)
		{
			BusName = busname;
			Object_Path = objectPath;
			Running = false;
		}

		public bool Register()
		{	
			m_bus = Bus.Session;

			m_path = new ObjectPath (Object_Path);
			if (m_bus.RequestName (BusName) == RequestNameReply.PrimaryOwner) {
				Running = true;
				Listener = new Thread (new ThreadStart (this.Handle));
				Listener.Start();
				return true;
			} else {
				return false;
			}
		}

		public void UnRegister()
		{
			if (Listener != null) {
				Running = false;
				Register(); //little workaround for the blocking Iterate () call in Handle()

				m_bus.Close();
				//m_bus.ReleaseName(BusName);

				Listener.Join();
				Listener = null;
			}		
		}

		public T GetClient() {
			if ((m_bus != null) && (m_path != null)) {
				return m_bus.GetObject<T>(BusName,m_path);
			}
			return null;
		}

		private void Handle()
		{
			T serverObject = Activator.CreateInstance<T>();

			m_bus.Register (m_path, serverObject);


			//run the main loop
			while (Running) {
				m_bus.Iterate ();
			}
		}

	}
}

