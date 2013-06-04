//
// Image.cs
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

using System.Drawing.Imaging;
using System.IO;

#if JSON
namespace Sharpend.Utils.Webservices.Discogs
{
  public class Image
  {

    public String Uri { get; private set; }
    public int Height { get; private set; }
    public int Width { get; private set; }
    public String ResourceUrl { get; private set; }
    public String Type { get; private set; }
    public String Uri150 { get; private set; }

    private Image(JToken token)
    {
      Uri = (String)token["uri"];
      Height = (int)token["height"];
      Width = (int)token["width"];
      ResourceUrl = (String)token["resource_url"];
      Type = (String)token["type"];
      Uri150 = (String)token["uri150"];
    }

    public static void saveToFile(String uri, String fullname)
    {
      try
      {        
        //path += ".jpg";

        //Copy.Save(path,ImageFormat.Jpeg);
        byte[] data = Discogs.GetImage(uri);
        using (FileStream fs = new FileStream(fullname, FileMode.OpenOrCreate))
        {
          fs.Write(data, 0, data.Length);

          //fs.Close();
        }        
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString()); //TODO logging
      }
    }
    
	public byte[] getData()
	{
		return Discogs.GetImage(Uri);	
	}
				
	public static System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
	{
	     MemoryStream ms = new MemoryStream(byteArrayIn);
	     System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
	     return returnImage;
	}
		
	public static System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
    {
        //a holder for the result
        System.Drawing.Bitmap result = new System.Drawing.Bitmap(width, height);

        //use a graphics object to draw the resized image into the bitmap
        using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(result))
        {
            //set the resize quality modes to high quality
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //draw the image into the target bitmap
            graphics.DrawImage(image, 0, 0, result.Width, result.Height);                
        }

        //return the resulting bitmap
        return result;
    }
		
	public byte[] getData(int width, int height)
    {
        
            //byte[] buf = new byte[fi.Length];
            
                //fs.Read(buf, 0, buf.Length);
                System.Drawing.Image img = byteArrayToImage(getData());

                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                //System.Drawing.Imaging.EncoderParameters
                System.Drawing.Image thumb = img;
				
				thumb = ResizeImage(img, width, height);
				
                thumb.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg); //TODO imageformat
                return ms.ToArray();
            
        
        //return new byte[0];
    }
		
		
    internal static Image CreateInstance(JToken token)
    {
      return new Image(token);
    }

  }
}
#endif