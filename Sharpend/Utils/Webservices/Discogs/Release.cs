//
// Release.cs
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
using Newtonsoft.Json.Linq;

#if !GTK2
namespace Sharpend.Utils.Webservices.Discogs
{
  public class Release
  {

    public String Id { get; private set; }
    public String Title { get; private set; }
    public String Uri { get; private set; }
    public String Notes { get; private set; }
    public int Year { get; private set; }
    public String ResourceUrl { get; private set; }

    public List<Track> Tracklist { get; private set; }
    public List<Image> Images { get; private set; }

    private Release(JToken token)
    {
      Id = Convert.ToString(token["id"]);
      Title = (String)token["title"];
      Uri = (String)token["uri"];
      Notes = (String)token["notes"];
      Year = (int)token["year"];
      ResourceUrl = (String)token["resource_url"];

      var tracklist = from c in token["tracklist"].Children() select c;
      Tracklist = new List<Track>(tracklist.Count());

      foreach (JToken tok in tracklist)
      {
        Tracklist.Add(Track.CreateInstance(tok));
      }
			
	  if (token["images"] != null)
	  {
        var imagelist = from c in token["images"].Children() select c;
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

    public static Release CreateInstance(String json)
    {
      JObject data = JObject.Parse(json);
      return new Release(data); 
    }

  }
}
#endif