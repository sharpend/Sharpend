//
// IUnityScope.cs
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
using DBus;

namespace Sharpend
{
	public delegate void RemoteDeletedHandler (string uri, string title);
	public delegate void RemoteAddedHandler (string uri);
	public delegate void RemoteSavedHandler (string uri);
	
	public delegate void ScopeChangedHandler (
		string data1, object data2, bool data3, string data4, string data5, string data6, string data7,
		Dictionary<string, object> data8);
	
	public struct ActivationData
	{
		public string data1; 
		public uint data2; 
		public Dictionary<string, object> data3;
	}
	
	public struct PreviewData
	{
		public string data1; 
		public string data2; 
		public Dictionary<string, object> data3;
	}
	
	
	public interface IUnityScope
	{
			
	 	void InfoRequest();
		Dictionary<string, object> Search(string search_string, Dictionary<string, object> hints);
		Dictionary<string, object> GlobalSearch(string search_string, Dictionary<string, object> hints);
		PreviewData Preview(string uri);
		void SetViewType(uint view_type);
		void SetActiveSources(string[] sources);
		event ScopeChangedHandler Changed;
		
		ActivationData Activate(string uri, uint action_type);
		
		void Test();
	    //event EventHandler<string, Dictionary<string, object>> SearchFinished;
	    //event EventHandler<string, Dictionary<string, object>> GlobalSearchFinished;
	
	}
}

#endif