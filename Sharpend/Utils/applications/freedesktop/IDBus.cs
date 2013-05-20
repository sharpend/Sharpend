//
// IDBus.cs
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
using DBus;
using System.Collections.Generic;

namespace Sharpend.Utils.Applications.FreeDesktop
{
	public delegate void NameOwnerChangedHandler(String data1, String data2, String data3);
	public delegate void NameLostHandler(String data1);
	public delegate void NameAcquiredHandler(String data1);
	
	[Interface("org.freedesktop.DBus")]
	public interface IDBus {
    
    	string Hello();
	    uint RequestName(string data1, uint data2);
	    uint ReleaseName(string data1);
	    uint StartServiceByName(string data1, uint data2);
	    void UpdateActivationEnvironment(Dictionary<string, string> data);
	    bool NameHasOwner(string data);
	    string[] ListNames();
	    string[] ListActivatableNames();
	    void AddMatch(string data);
	    void RemoveMatch(string data);
	    string GetNameOwner(string data);
	    string[] ListQueuedOwners(string data);
	    uint GetConnectionUnixUser(string data);
	    uint GetConnectionUnixProcessID(string data);
	    Array GetAdtAuditSessionData(string data);
	    Array GetConnectionSELinuxSecurityContext(string data);
	    void ReloadConfig();
	    string GetId();
	    event NameOwnerChangedHandler NameOwnerChanged;
	    event NameLostHandler NameLost;
	    event NameAcquiredHandler NameAcquired;
	}
}

#endif