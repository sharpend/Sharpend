using System;
using Gtk;
using Sharpend.GtkSharp;
using System.IO;
using Sharpend.Utils;
using System.Text.RegularExpressions;
using ExifLibrary;
using TagLib.Jpeg;
using System.Collections.Generic;

namespace window{	public partial class window: Gtk.Window	{

		public GtkListTreeView  TreeList {
			get;
			private set;
		}

		public FileChooserButtonWrapper Wrapper {
			get;
			private set;
		}
		public void init() 
		{
			TreeList = new GtkListTreeView(treeview1);
			TreeList.loadStructureFromRessource("ExifTagger.grid.xml");
			TreeList.OnGetData += GetCellData;
			TreeList.AfterEdit += AfterEdit;

			btnLoadFiles.Clicked += HandleClicked;
			EntryRegEx.Text = "(\\/.*)";
			//filechooserbutton1.Action = FileChooserAction.SelectFolder;
			//filechooserbutton1.SelectionChanged += HandleSelectionChanged;

			Wrapper = new Sharpend.GtkSharp.FileChooserButtonWrapper("",Filechooserbutton1,EntryPath,FileChooserAction.SelectFolder);
			Wrapper.OnSelectionChanged += HandleSelectionChanged;
			
			btnWriteTags.Clicked += HandleWriteTagsClicked;

			EntryPath.Text = Wrapper.Chooser.CurrentFolder;
			treeview1.RowActivated += HandleRowActivated;
			this.WidthRequest = 800;
			this.HeightRequest = 600;
		}

		public void GetCellData(String column,object input, out object o)
		{
			o = null;
			if (column.ToLower().Equals("preview"))
			{
				if (input is Gdk.Pixbuf)
				{
					o = input;
				} else
				{
					String fn = Convert.ToString(input);
					if (!String.IsNullOrEmpty(fn))
					{
						Gdk.Pixbuf pb = null;

						try  //TODO try catch gar nicht gut
						{
							//pb = Sharpend.Utils.CachedPictureList.Instance.GetPicture(fn);
							pb = GetPixbuf(fn);
						} catch (Exception ex)
						{
							//log.Error(ex);
							o = null;
							return;
						}

						if (pb == null)
						{
							pb = new Gdk.Pixbuf(fn);
						}
						//PixbufList.Add(pb,fn);
						o = pb;
					} else
					{
						o = null;
					}
				}
			} else
			{
				o = input;
			}
		}

		public void AfterEdit(object o, EventArgs args)
		{
			try
			{
				Console.WriteLine("AfterEdit");
				RowChangedEventArgs ra = args as RowChangedEventArgs;
				if (ra != null)
				{
					if (ra.ChangedRow != null)
					{
						String fullname = (String)ra.ChangedRow.Datas[1];
						//String tag = (String)ra.ChangedRow.Datas[1];
						String value = String.Empty;

						ExifTag et = ExifTag.GPSLatitude;
						if (ra.ColumnName == "lat")
						{
							value = (String)ra.ChangedRow.Datas[3];
						} else
						{
							value = (String)ra.ChangedRow.Datas[4];
							et = ExifTag.GPSLongitude;
						}

						//Console.WriteLine("tag: " + tag + "-value:" +value + "fn:" + fullname);

						ImageFile file = ImageFile.FromFile(fullname);

						//ExifTag et = (ExifTag)Enum.Parse(typeof(ExifTag),tag);
						value = value.Replace(",",".");
						GPSLatitudeLongitude latlon = GetGpsValue(value,ExifTag.GPSLatitude);

						Console.WriteLine("set "  +et.ToString());

						//file.Properties[et] = latlon;

						Console.WriteLine(file.Properties[et].GetType());
						if (file.Properties[et] != null)
						{
							(file.Properties[et] as GPSLatitudeLongitude).Minutes = latlon.Minutes;
							(file.Properties[et] as GPSLatitudeLongitude).Seconds = latlon.Seconds;
							(file.Properties[et] as GPSLatitudeLongitude).Degrees = latlon.Degrees;
						} else
						{
							file.Properties[et] = latlon;
						}

						file.Save(fullname);
					}
				}
			} catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		private GPSLatitudeLongitude GetGpsValue(String input,ExifTag tag)
		{
			//Console.WriteLine(input);
			float d = 0;
			float m = 0;
			float s = 0;

			String[] dp = input.Split('.');
			if (dp.Length > 1)
			{
				d = (float)Convert.ToDouble(dp[0]);

				String frac = "0," + dp[1];
				double dm = Convert.ToDouble(frac) * 60;
				//Console.WriteLine("frac:" + frac  + "dm:" + dm.ToString());
				m = (float)Math.Truncate(dm);

				double ds = dm - Math.Truncate(dm);
				//Console.WriteLine("ds:" + ds.ToString());
				s = (float)(ds * 60);
				//Console.WriteLine("ss:" + s.ToString());
			}
			Console.WriteLine("new: " + d.ToString() + "-" + m.ToString() + "-" + s.ToString());		
			return new GPSLatitudeLongitude(tag,d,m,s);
		}

		private Gdk.Pixbuf GetPixbuf(String filename)
		{
			Gdk.Pixbuf ret = new Gdk.Pixbuf(filename);

			{
				//Gdk.Pixbuf ret = new Gdk.Pixbuf(cover);

				double fac = 1;
				if (ret.Width > ret.Height)
				{
					fac = Convert.ToDouble(ret.Width) / Convert.ToDouble(ret.Height);
				} else
				{
					fac = Convert.ToDouble(ret.Height) / Convert.ToDouble(ret.Width);
				}

				int hg = 120;
				int newWidth =Convert.ToInt32((fac * hg));


				return ret.ScaleSimple(newWidth,hg,Gdk.InterpType.Nearest);
				//return ret;
			}

			//return null;
		}

		void HandleRowActivated (object o, RowActivatedArgs args)
		{
			List<VirtualGridRow> lst = TreeList.getSelectedRows();
			if (lst.Count > 0)
			{
				VirtualGridRow row = lst[0];

				String lat = (String)row.getData("lat");
				String lon = (String)row.getData("lon");

				//http://www.openstreetmap.org/?lon=22.5739625&lat=57.6287332&zoom=12&mlat=57.6529102&mlon=22.5148625
				string data = "http://www.openstreetmap.org/?lon=" + lon;
				data += "&lat=" + lat;
				data += "&zoom=12";
				data += "&mlat=" + lat;
				data += "&mlon=" + lon;
				data = data.Replace(",",".");
//				String data = "http://www.openstreetmap.org/index.html?lat" + lat;
//				data += "&lon=" + lon;
				System.Diagnostics.Process.Start(data);

			}
		}

		void HandleWriteTagsClicked (object sender, EventArgs e)
		{
			textviewMessages.Buffer.Clear();
			textviewMessages.Buffer.Text += "writeing tags...\n";

			foreach (VirtualGridRow rw in TreeList.Rows)
			{
				String fn = (String)rw.getData("fullfilename");
				String tags = (String)rw.getData("tags");
				FileInfo fi = new FileInfo(fn);
				setExifData(fi,tags);
			}

			textviewMessages.Buffer.Text += "done\n";
		}

		void HandleSelectionChanged (object sender, EventArgs e)
		{
			entryPath.Text = Wrapper.Chooser.CurrentFolder;
		}

		void HandleClicked (object sender, EventArgs e)
		{
			loadData();
		}


		private void loadData()
		{
			textviewMessages.Buffer.Clear();
			textviewMessages.Buffer.Text += "reading directory\n";

			if (! String.IsNullOrEmpty(entryPath.Text))
			{
				DirectoryInfo di = new DirectoryInfo(entryPath.Text);
				if (di.Exists)
				{
					TreeList.Rows.Clear();
					recursiveLoadData(di);
					TreeList.reload();
				}
			}

			textviewMessages.Buffer.Text += "done";
		}


		private void recursiveLoadData(DirectoryInfo parent)
		{


			try
			{
				foreach (FileInfo fi in parent.GetFiles())
				{
					VirtualGridRow row = TreeList.newRow();



					try
					{
						row.addGridColumn("fullfilename",fi.FullName);
						row.setData("preview",fi.FullName);

						TagLib.File jpg = TagLib.Jpeg.File.Create(fi.FullName);
						TagLib.Xmp.XmpTag xmptag = (TagLib.Xmp.XmpTag)jpg.GetTag(TagLib.TagTypes.XMP);
						if (xmptag.Latitude != null)
						{
							row.setData("lat",xmptag.Latitude);
						}

						if (xmptag.Longitude != null)
						{
							row.setData("lon",xmptag.Longitude);
						}

					} catch  (Exception ex)
					{
						Console.WriteLine(ex.ToString());
					}

					try
					{
						ImageFile file = null;
						file = ImageFile.FromFile(fi.FullName);

						double lat = GetDecimalCoordinate(file.Properties[ExifTag.GPSLatitude]);
						double lon = GetDecimalCoordinate(file.Properties[ExifTag.GPSLongitude]);

						row.setData("lat",lat.ToString());
						row.setData("lon",lon.ToString());


					} catch (Exception ex) {
						Console.WriteLine(ex.ToString());
					}


					//row.addGridColumn("tags","tags");
					addTags(fi,row);
				}

				foreach (DirectoryInfo di in parent.GetDirectories())
				{
					recursiveLoadData(di);
				}
			} catch (Exception ex)
			{
				textviewMessages.Buffer.Text += ex.Message;
			}


		}

		private double GetDecimalCoordinate(ExifProperty item)
		{
			GPSLatitudeLongitude coord = (item as ExifLibrary.GPSLatitudeLongitude);
			
			double degrees = Convert.ToDouble(coord.Degrees.Numerator / coord.Degrees.Denominator);
			double minutes = Convert.ToDouble(coord.Minutes.Numerator / coord.Minutes.Denominator);
			double seconds = Convert.ToDouble(coord.Seconds.Numerator / coord.Seconds.Denominator);

			return  degrees + (minutes/60) + (seconds/3600);
		}

		private void addTags(FileInfo fi,VirtualGridRow row)
		{
			try
			{
				String tags = String.Empty;

				if (!String.IsNullOrEmpty(EntryRegEx.Text))
				{
					Regex rx = new Regex(EntryRegEx.Text);

					Match match = rx.Match(fi.FullName);

					tags += rx.Replace(fi.FullName,EntryPattern.Text);

					//TagLib.File tf = TagLib.Jpeg.File.Create(fi.FullName);

					 //tf.GetTag(TagLib.TagTypes.XMP);

//					for (int i=1;i<match.Groups.Count;i++)
//					{
//						Group gc = match.Groups[i];
//
//						tags += gc.Value + ",";
//
//					}
				}

				tags = tags.Trim(',');
				row.addGridColumn("tags",tags);
			} catch (Exception ex)
			{
				textviewMessages.Buffer.Text += ex.Message;
			}
		}
	
		private void setExifData(FileInfo fi, String csvtags)
		{
			try
			{
				TagLib.File fl = TagLib.Jpeg.File.Create(fi.FullName);
				//textviewMessages.Buffer.Text += fl.ToString();

				TagLib.Jpeg.File jpg = fl as TagLib.Jpeg.File;
				if (jpg != null)
				{
					TagLib.Xmp.XmpTag xmptag = (TagLib.Xmp.XmpTag)jpg.GetTag(TagLib.TagTypes.XMP);

					xmptag.Keywords = csvtags.Split(',');

					//xmptag.Latitude = 

					//TagLib.Xmp.XmpTag xmptag = (TagLib.Xmp.XmpTag)jpg.GetTag(TagLib.TagTypes.XMP);


				}

				jpg.Save();
			}catch (Exception ex)
			{
				TextviewMessages.Buffer.Text += ex.Message;
			}
		}

		private void setExifData2(FileInfo fi, String csvtags)
		{
			try
			{
				//ImageFile file = ImageFile.FromFile(fi.FullName);
				textviewMessages.Buffer.Text += " --- " + fi.FullName + " --- \n ";
				ImageFile file = JPEGFile.FromFile(fi.FullName);
//				if (ff is JPEGFile)
//				{
//					//(ff as JPEGFile).
//				}

				//Console.WriteLine(file.Properties[ExifTag.ip]);

				JPEGFile jf = (file as JPEGFile);



				foreach (JPEGSection sec in jf.Sections)
				{
					//Console.WriteLine(sec.Marker.ToString());
					textviewMessages.Buffer.Text += "\n\n ########### \n\n";

					String val = String.Empty;
					System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
				  	val = "-> Header: " +  enc.GetString(sec.Header);
					if (sec.EntropyData.Length > 0)
					{
						val += "---> Data: " + enc.GetString(sec.EntropyData);
						val += "\n\n\n";


							
						val += "\n";
						System.Text.UnicodeEncoding uc = new System.Text.UnicodeEncoding();
						val += " uc: " + uc.GetString(sec.EntropyData) + "\n";
					} else
					{
						val += " NO DATA \n";
					}

					//Console.WriteLine(val);
					//Console.WriteLine("---------- \n\n");
					textviewMessages.Buffer.Text += val +"\n\n";
				}

				textviewMessages.Buffer.Text += "\n\n EXIF PROPERTIES \n\n";
				foreach (ExifProperty p in file.Properties)
				{
					object data = p.Value;

					String val = String.Empty;

					if (p.Name == "UserComment")
					{
						Console.WriteLine("xx");
					}

					if (p is JFIFThumbnailProperty)
					{
						val = "xxxxxxxxxxxxxxxxxxxxxxx";
					}

					if (p is ExifEncodedString)
					{
						ExifEncodedString es = p as ExifEncodedString;
						val = es.Encoding.GetString(es.Interoperability.Data);
					}
					else if (data is JFIFThumbnail)
					{
						JFIFThumbnail tn = data as JFIFThumbnail;
						val = "tn";
					} else if (data is  byte[])
					{
					  System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
				  	  val = "bt: " +  enc.GetString((byte[])data);

				      System.Text.UnicodeEncoding uc = new System.Text.UnicodeEncoding();
					  val += " uc: " + uc.GetString((byte[]) data);
					} else
					{
						val = p.Value.ToString();
					}

					//Console.WriteLine(p.Name + " - " + val);
					textviewMessages.Buffer.Text += p.Name + " - " + val + "\n";
				}
			} catch (Exception ex)
			{
				textviewMessages.Buffer.Text += ex.Message + "\n";
			}
		}

	} //class} //namespace