//
// DataObjectContainer.cs
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

namespace Sharpend.Databinding
{
	/// <summary>
	/// Container for bindable data objects
	/// 
	/// the auto binding function will look for a DataObjectContainer
	/// 
	/// </summary>
	public class DataObjectContainer
	{
		private Dictionary<String,object> dataobjects;
		/// <summary>
		/// Gets or sets the dataobjects.
		/// </summary>
		/// <value>
		/// The dataobjects.
		/// </value>
		public Dictionary<String, object> DataObjects {
			get {
				return dataobjects;
			}
			set
			{
				dataobjects = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Sharpend.GtkSharp.DataObjectContainer"/> class.
		/// </summary>
		public DataObjectContainer ()
		{
			dataobjects = new Dictionary<string, object>(10);
		}

		/// <summary>
		/// returns true if the key is already in the container
		/// </summary>
		/// <returns>
		/// The key.
		/// </returns>
		/// <param name='key'>
		/// If set to <c>true</c> key.
		/// </param>
		public bool ContainsKey(String key)
		{
			return dataobjects.ContainsKey(key);
		}

		/// <summary>
		/// returns the dataobject with specified key
		/// </summary>
		/// <param name='key'>
		/// Key.
		/// </param>
		public object Get(String key)
		{
			return dataobjects[key];
		}


		/// <summary>
		/// add's a new object or if exits replace an existing object with the same key
		/// 
		/// key is the typename of the object
		/// 
		/// </summary>
		/// <param name='o'>
		/// O.
		/// </param>
		public void Add(object o)
		{
			String name = o.GetType().ToString();

			if (dataobjects.ContainsKey(name))
			{
				dataobjects[name] = o;
			} else
			{
				dataobjects.Add(name,o);
			}
		}
	}
}

