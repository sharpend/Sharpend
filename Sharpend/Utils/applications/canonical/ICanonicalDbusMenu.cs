//
// ICanonicalDbusMenu.cs
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

namespace Sharpend.Utils.Applications.Canonical
{
	/// <summary>
	/// TODO description: LayoutResponse
	/// </summary>
	struct LayoutResponse
	{
		public int id;
		public Dictionary<string, object> data1;
		public object[] data2;
	}
	
	/// <summary>
	/// TODO description: GroupPropertiesResponse
	/// </summary>
	struct GroupPropertiesResponse
	{
		public int id;
		public Dictionary<string, object> data; 
	}
	
	
	/// <summary>
	/// This is experimentell
	/// </summary>
	public interface ICanonicalDbusMenu
	{
		//struct { int; Dictionary<string, object>; object[]; } GetLayout(int parentId, int recursionDepth, string[] propertyNames);
	    //struct { int; Dictionary<string, object>; }[] GetGroupProperties(int[] ids, string[] propertyNames);
	    object GetProperty(int id, string name);
	    void Event(int id, string eventId, object data, uint timestamp);
	    //int[] EventGroup(struct { int; string; object; uint; }[] events);
	    bool AboutToShow(int id);
	    int[] AboutToShowGroup(int[] ids);
	    //event EventHandler<struct { int; Dictionary<string, object>; }[], struct { int; string[]; }[]> ItemsPropertiesUpdated;
	    //event EventHandler<uint, int> LayoutUpdated;
	    //event EventHandler<int, uint> ItemActivationRequested;
	    uint Version { get; }
	    string TextDirection { get; }
	    string Status { get; }
	    string[] IconThemePath { get; }
	}
}

