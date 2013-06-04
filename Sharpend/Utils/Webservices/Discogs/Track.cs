//
// Track.cs
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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if JSON
using Newtonsoft.Json.Linq;
#endif

#if JSON
namespace Sharpend.Utils.Webservices.Discogs
{
  public class Track
  {

    public String Duration { get; private set; }
    public String Title { get; private set; }
    public int Position { get; private set; }

    

    private Track(JToken token)
    {
      Title = (String)token["title"];
      Duration = (String)token["duration"];

      String pos = (String)token["position"];
      if (!String.IsNullOrEmpty(pos))
      {
		int p;
    	if (Int32.TryParse(pos,out p))
		{
			Position = p;
		}
      }  
    }

    public static Track CreateInstance(JToken token)
    {
      return new Track(token);
    }

    //public static Track CreateInstance(String json)
    //{
    //}

    //public static List<Track> CreateInstance(JToken token)
    //{
    //}


  }
}
#endif