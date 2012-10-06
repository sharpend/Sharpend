//
// SearchResult.cs
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
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

#if !GTK2
namespace Sharpend.Utils.Webservices.Discogs
{

	public class SearchResult
	{
		public bool ExactResult {
			get;
			private set;
		}

    public String Title { get; private set; }
    public String Uri   { get; private set; }
    public String Thumb { get; private set; }
    public String Type { get; private set; }
    public String Summary { get; private set; }

    public String Id
    {
      get
      {
        if (!String.IsNullOrEmpty(Uri))
        {
          String[] lst = Uri.Split('/');
          return lst[lst.Length - 1];
        }
        return String.Empty;
      }
    }

		private SearchResult (JToken token, bool exactResult)
		{
      		Title = (String)token["title"];
     		Uri = (String)token["uri"];
     		Thumb = (String)token["thumb"];
      		Type = (String)token["type"];
      		Summary = (String)token["summary"];
      		ExactResult = exactResult;
		}

    	private SearchResult(JToken token)
      		: this(token, false)
		{
    	}

    
		public static List<SearchResult> CreateInstance(String json)
		{
			JObject data = JObject.Parse(json);

	  		var results = from c in data["resp"]["search"]["searchresults"]["results"].Children() select c;
			List<SearchResult> ret = new List<SearchResult>(results.Count() + 1);
			
			if (data["resp"]["search"]["exactresults"] != null)
			{
	  			var exactresults = from c in data["resp"]["search"]["exactresults"].Children() select c;
				foreach (JToken tok in exactresults)
			      {
			        ret.Add(new SearchResult(tok,true));
			      }
			}
			
			foreach (JToken tok in results)
	      	{
	        	ret.Add(new SearchResult(tok));
	      	}

            			
			return ret;					
		}

    
	}
}
#endif
