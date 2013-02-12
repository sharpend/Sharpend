//
// CachedPictureList.cs
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
using System.IO;

namespace Sharpend.Utils
{
	public class CachedPictureList
	{
		//private Dictionary<String,Gdk.Pixbuf> files;
		private Dictionary<String,String> files;
		private HashSet<String> empty;

		/// <summary>
		/// path where temp images will be stored
		/// </summary>
		/// <value>
		/// The data path.
		/// </value>
		public String DataPath {
			get;
			private set;
		}

		/// <summary>
		/// placeholder image for not existing pictures
		/// </summary>
		/// <value>
		/// The place holder.
		/// </value>
		public String PlaceHolder
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Sharpend.Utils.CachedPictureList"/> class.
		/// </summary>
		private CachedPictureList ()
		{
			init();
			load();
		}

		/// <summary>
		/// Init this instance.
		/// </summary>
		private void init()
		{
			//files = new Dictionary<string, Gdk.Pixbuf>(100);
			files = new Dictionary<string, string>(1000);

			empty = new HashSet<string>();
			DataPath = Sharpend.Configuration.ConfigurationManager.getString("picturepath");
			PlaceHolder = Sharpend.Configuration.ConfigurationManager.getString("placeholderimage");
		}

		/// <summary>
		/// load data from file
		/// </summary>
		private void load()
		{
			if (String.IsNullOrEmpty(DataPath))
			{
				throw new Exception("loadPicture: no datapath available");
			}

			FileInfo fi = Sharpend.Configuration.ConfigurationManager.getApplicationConfig();

			char sp = '|';
			if (!String.IsNullOrEmpty(fi.FullName))
			{
				String pt = fi.Directory.FullName + "/" + "cache.txt";

				if (File.Exists(pt))
				{
					string line;
					using (System.IO.StreamReader file =  new System.IO.StreamReader(pt))
					{
						while((line = file.ReadLine()) != null)
						{
							String[] dt = line.Split(sp);
							files.Add(dt[0],dt[1]);
						}
					}
				} else
				{
					File.Create(pt);
				}
			}

		}


		/// <summary>
		/// save data to file
		/// </summary>
		public void Save()
		{
			FileInfo fi = Sharpend.Configuration.ConfigurationManager.getApplicationConfig();

			if (!String.IsNullOrEmpty(fi.FullName))
			{
				String pt = fi.Directory.FullName + "/" + "cache.txt";

				using (System.IO.StreamWriter file = new System.IO.StreamWriter(pt))
				{
					foreach (KeyValuePair<String,String> kp in files)
					{
						file.WriteLine(kp.Key+"|"+kp.Value);
					}
				}
			}
		}

		/// <summary>
		/// update the given path recursively ... does not call Save!
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		public void UpdatePath(String path)
		{
			List<string> keys = new List<string>(100);
			foreach (KeyValuePair<String,String> kp in files)
			{
				if (kp.Key.StartsWith(path))
				{
					keys.Add(kp.Key);
				}
			}

			foreach (String s in keys)
			{
				files.Remove(s);
			}

			scanPath(path);
		}

		/// <summary>
		/// scans the path recursively for new image
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		private void scanPath(String path)
		{
			DirectoryInfo di = new DirectoryInfo(path);
			if (di.Exists)
			{
				recursiveScan(di);
			}
		}

		/// <summary>
		/// returns true if given file is an image (jpg extension)
		/// </summary>
		/// <returns>
		/// The image.
		/// </returns>
		/// <param name='fi'>
		/// If set to <c>true</c> fi.
		/// </param>
		private bool isImage(FileInfo fi)
		{
			if (fi.Extension.ToLower().Equals(".jpg"))
			{
				return true;
			}

			if (fi.Extension.ToLower().Equals(".JPEG"))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// scan given path recursively for images
		/// </summary>
		/// <param name='parent'>
		/// Parent.
		/// </param>
		private void recursiveScan(DirectoryInfo parent)
		{
			foreach (FileInfo fi in parent.GetFiles())
			{
				if (isImage(fi))
				{
					loadPicture(fi.FullName);
				}
			}

			foreach (DirectoryInfo di in parent.GetDirectories())
			{
				recursiveScan(di);
			}
		}


		/// <summary>
		/// returns a new Gdk Pixbuf from given image path
		/// </summary>
		/// <returns>
		/// The picture.
		/// </returns>
		/// <param name='fullname'>
		/// Fullname.
		/// </param>
		/// <param name='addIfNotExist'>
		/// Add if not exist.
		/// </param>
		public Gdk.Pixbuf GetPicture(String fullname,bool addIfNotExist=true)
		{
			String fn = String.Empty;
			if (files.ContainsKey(fullname))
			{
				fn = files[fullname];

			} else
			{
				fn = loadPicture(fullname);
			}

			if (!String.IsNullOrEmpty(fn))
			{
				return getImage(fn);
			}

			return getImage(PlaceHolder);
		}

		/// <summary>
		/// checks if a image for this path exists
		/// </summary>
		/// <param name='fullname'>
		/// If set to <c>true</c> fullname.
		/// </param>
		public bool Exists(String fullname)
		{
			if (files.ContainsKey(fullname))
			{
				String check = files[fullname];

				if (check.Equals(PlaceHolder))
				{
					return false;
				}
				return true;
			}

			String fn = loadPicture(fullname);
			if (!String.IsNullOrEmpty(fn))
			{
				return true;
			}

			return false;
		}


		/// <summary>
		/// load a picture ... copy to temp path and insert into hashlist
		/// </summary>
		/// <returns>
		/// The picture.
		/// </returns>
		/// <param name='fullname'>
		/// Fullname.
		/// </param>
		private String loadPicture(String fullname)
		{
			if (fullname.ToLower().EndsWith(".mp3"))
			{
				throw new Exception("this is not very good");
			}

			Console.WriteLine("loadPicture" + fullname);
			if (String.IsNullOrEmpty(DataPath))
			{
				throw new Exception("loadPicture: no datapath available");
			}

			String dest = DataPath + fullname;

			//original file
			FileInfo fi = new FileInfo(fullname);

			if (fi.Exists)
			{
				if (!Directory.Exists(DataPath + fi.Directory.FullName))
				{
					Directory.CreateDirectory(DataPath + fi.Directory.FullName);
				}

				if (!File.Exists(dest))
				{
					fi.CopyTo(dest);
				}

				files.Add(fullname,dest);
				return dest;
			} else
			{
				files.Add(fullname,PlaceHolder);
				return String.Empty;
			}
		}


		/// <summary>
		/// Gets the image.
		/// </summary>
		/// <returns>
		/// The image.
		/// </returns>
		/// <param name='filename'>
		/// Filename.
		/// </param>
		private static Gdk.Pixbuf getImage(String filename)
        {
            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(filename);
                if (fi.Exists)
                {
                    //byte[] buf = new byte[fi.Length];
                    using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open,System.IO.FileAccess.Read))
                    {
                        //fs.Read(buf, 0, buf.Length);
                        System.Drawing.Image img = System.Drawing.Image.FromStream(fs);

                        System.IO.MemoryStream ms = new System.IO.MemoryStream();
						                        
//						System.Drawing.Image thumb = img;
//						if (list.Parameter.ImageScaleing == ImageScaleings.Scale)
//						{}
						System.Drawing.Image thumb = Utils.ResizeImage(img, 100, 100);
						

                        thumb.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        //return ms.ToArray();

						return new Gdk.Pixbuf(ms.ToArray());
                    }
                }
				return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); //TODO logger
                return null;
            }
        }

		private static CachedPictureList instance;
		/// <summary>
		/// Singleton Instance
		/// </summary>
		/// <value>
		/// The instance.
		/// </value>
		public static CachedPictureList Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new CachedPictureList();
				}

				return instance;
			}
		}

	}
}

