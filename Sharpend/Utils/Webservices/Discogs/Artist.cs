//
// Artist.cs
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
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

#if !GTK2
namespace Sharpend.Utils.Webservices.Discogs
{
	public class Artist
	{
		private JToken data;
		
		
		public String Name {
			get
			{
			  return (String)data["resp"]["artist"]["name"];
			}
		}
		
		public String Profile {
			get
			{
			  return (String)data["resp"]["artist"]["profile"];
			}
		}
		
		public String DataQuality {
			get
			{
			  return (String)data["resp"]["artist"]["data_quality"];
			}
		}

		public List<Image> Images { get; private set; }

//		public String DataQuality {
//			get
//			{
//			  return data["resp"]["artist"]["data_quality"];
//			}
//		}
		
		
		private Artist (JToken data)
		{
			this.data = data;

			if (data["resp"]["artist"]["images"] != null)
			{
				var imagelist = from c in data["resp"]["artist"]["images"].Children() select c;
				Images = new List<Image>(imagelist.Count());

				foreach (JToken tok in imagelist)
				{
					Images.Add(Image.CreateInstance(tok));
				}
			} else
			{
				Images = new List<Image>(0);
			}

		}
				
		public static Artist CreateInstance(String json)
		{
			JObject data = JObject.Parse(json);	
			return new Artist(data);
		}
		
	}
}
#endif
