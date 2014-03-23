//
//  ConfigurationManager.cs
//
//  Author:
//       Dirk Lehmeier <sharpend_develop@yahoo.de>
//
//  Copyright (c) 2014 Dirk Lehmeier
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

namespace Sharpend.Configuration
{
	public class ConfigurationManager2 :ISharpendConfigurationManager
	{
		public ConfigurationManager2 ()
		{
		}

		private static ConfigurationManager2 _instance;

		public static ISharpendConfigurationManager getConfigurationManager()
		{
			if (_instance == null) {
				_instance = new ConfigurationManager2();
			}
			return _instance;
		}

		/*
		public static ConfigurationManager2 Instance
		{
			get {
				if (_instance == null)
				{
					//_instance = new Sharpend.ConfigurationManager2();
					Type tp = Type.GetType("Sharpend.Configuration.ConfigurationManager2,Sharpend");
					_instance = (ConfigurationManager2)Activator.CreateInstance(tp);
				}
				return _instance;
			}
		}
		*/

		#region IConfigurationManager implementation

		public string getString (string elementName)
		{
			return Sharpend.Configuration.ConfigurationManager.getString(elementName);
		}

		#endregion
	}
}

