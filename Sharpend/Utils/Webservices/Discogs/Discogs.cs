//
// Discogs.cs
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
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Collections.Generic;

#if !GTK2
namespace Sharpend.Utils.Webservices.Discogs
{
	/// <summary>
	/// Discogs Webservice Client
	/// </summary>
	public class Discogs
	{
		private String baseUrl = "http://api.discogs.com/";
		
		public Discogs ()
		{
		}
		
		/// <summary>
		/// Get Artist with the specified artistname
		/// </summary>
		/// <returns>
		/// The artist.
		/// </returns>
		/// <param name='artist'>
		/// Artist.
		/// </param>
		public Artist GetArtist(String artist)
		{
			String data = getData(this.baseUrl + "artist/" + artist);
			return Artist.CreateInstance(data);
		}

	    public Release GetRelease(String id)
	    {
	      id = Uri.EscapeDataString(id);
	      String data = getData(this.baseUrl + "releases/" + id);
	      return Release.CreateInstance(data);      
	    }

		/// <summary>
		/// Search the specified query.
		/// </summary>
		/// <param name='query'>
		/// Query.
		/// </param>
		public List<SearchResult> Search(String query)
		{
			query = Uri.EscapeDataString(query);
			String data = getData(this.baseUrl + "search?q=" + query);
			return SearchResult.CreateInstance(data);			
		}
		
		private String getData(String uri)
		{
			using (WebClient wc = new WebClient())
			{
				//wc.Headers["Accept"] = "application/xml";
            	wc.Headers["Accept-Encoding"] = "gzip";
				//return wc.DownloadString(uri);
				
				byte[] data = wc.DownloadData(uri);
	            if (wc.ResponseHeaders["Content-Encoding"] == "gzip")
	            {
	                using (MemoryStream inputStream = new MemoryStream(data))
	                {
	                    GZipStream gzip = new GZipStream(inputStream, CompressionMode.Decompress);
	                    using (StreamReader reader = new StreamReader(gzip, Encoding.UTF8))
	                    {
	                        return reader.ReadToEnd();
	                    }
	                }
	            }
	            return Encoding.UTF8.GetString(data);
				
			}
		}

    internal static byte[] GetImage(string uri)
    {
      using (WebClient wc = new WebClient())
      {
        //wc.Headers["Accept"] = "application/xml";
        //wc.Headers["Accept-Encoding"] = "gzip";
        //return wc.DownloadString(uri);

        return wc.DownloadData(uri);
      }
    }

    //internal static System.Drawing.Image GetImage(string filePath)
    //{
    //  WebClient l_WebClient = new WebClient();
    //  byte[] l_imageBytes = l_WebClient.DownloadData(filePath);

    //  System.Drawing.Image l_image = null;
    //  using (MemoryStream l_MemStream = new MemoryStream(l_imageBytes, 0, l_imageBytes.Length))
    //  {
    //    l_MemStream.Write(l_imageBytes, 0, l_imageBytes.Length);
    //    l_image = System.Drawing.Image.FromStream(l_MemStream, true);
    //    l_MemStream.Close();
    //  }

    //  return l_image;
    //}


	}
}
#endif
