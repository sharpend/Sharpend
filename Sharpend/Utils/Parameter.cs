//
// Parameter.cs
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

namespace Sharpend
{
	public class Parameter
	{
		
		public object Data
		{
			get;
			private set;
		}
				
		public String Name {
			get;
			private set;
		}
		
		public Type ParameterType {
			get;
			private set;
		}
		
		public Parameter (String name, String typename, object data)
		{
			Data = data;
			Name = name;
			ParameterType = Type.GetType(typename);
		}
		
		public Parameter (Type type,object data)
		{
			Data = data;
			Name = String.Empty;
			ParameterType = type;
		}
		
		
	}
}

