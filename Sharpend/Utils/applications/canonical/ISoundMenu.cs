//
//  ISoundMenu.cs
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
#if DBUS
using NDesk.DBus;

namespace Sharpend.Utils.Applications.Canonical
{
	public struct GetLayoutResponse 
	{ 	
		public int data1;
		public Dictionary<string, object> data2;
		public object[] data3;
	}

	public struct GetGroupPropertiesResponse 
	{ 
		public int data1;
		public Dictionary<string, object> data2;
	}

	public struct EventGroupData
	{ 
		public int data1;
		public string data2;
		public object data3;
		public uint data4;
	}

	public struct Subdata
	{
		public string subdata1;
		public object subdata2;
	}

	public struct EventHandlerData1 
	{ 
		public int data1; 
		//public Dictionary<string, object> data;
		public Subdata[] data2;
	}

	public struct EventHandlerData2
	{ 
		public int data1; 
		public string[] data2; 
	}

	public delegate void ItemsPropertiesUpdatedEventHandler(EventHandlerData1[] data1, EventHandlerData2[] data2);
	public delegate void ItemActivationRequestedHandler(int data1, uint data2);

	[Interface("com.canonical.dbusmenu")]
	public interface ISoundMenu
	{
	    GetLayoutResponse GetLayout(int parentId, int recursionDepth, string[] propertyNames);
	    GetGroupPropertiesResponse[] GetGroupProperties(int[] ids, string[] propertyNames);
	    object GetProperty(int id, string name);
	    void Event(int id, string eventId, object data, uint timestamp);
	    int[] EventGroup(EventGroupData[] events);
	    bool AboutToShow(int id);
	    int[] AboutToShowGroup(int[] ids);
	    event ItemsPropertiesUpdatedEventHandler ItemsPropertiesUpdated;
	    //event EventHandler<uint, int> LayoutUpdated;
	    event ItemActivationRequestedHandler ItemActivationRequested;
	    uint Version { get; }
	    string TextDirection { get; }
	    string Status { get; }
	    string[] IconThemePath { get; }
	}


}
#endif
