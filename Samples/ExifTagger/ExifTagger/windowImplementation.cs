using System;
using Gtk;
using Sharpend.GtkSharp;
using System.IO;
using Sharpend.Utils;
using System.Text.RegularExpressions;
using ExifLibrary;
using TagLib.Jpeg;

namespace window{	public partial class window: Gtk.Window	{

		public GtkListTreeView  TreeList {
			get;
			private set;
		}
		public void init() 
		{
			TreeList = new GtkListTreeView(treeview1);
			TreeList.loadStructureFromRessource("ExifTagger.grid.xml");

			btnLoadFiles.Clicked += HandleClicked;
			EntryRegEx.Text = "(\\/.*)";
			filechooserbutton1.Action = FileChooserAction.SelectFolder;
			filechooserbutton1.SelectionChanged += HandleSelectionChanged;

			btnWriteTags.Clicked += HandleWriteTagsClicked;
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
			entryPath.Text = filechooserbutton1.CurrentFolder;
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

					row.addGridColumn("fullfilename",fi.FullName);
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