//
// Utils.cs
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
using System.IO;
using System.Security.Cryptography;
using System.Text;
using log4net.Config;

#if SHARPZIPLIB
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
#endif

namespace Sharpend.Utils
{
	public static class Utils
	{
		/// <summary>
		/// TODO
		/// </summary>
		/// <returns>
		/// The formated date time.
		/// </returns>
		/// <param name='datetime'>
		/// Datetime.
		/// </param>
		public static String getFormatedDateTime(DateTime datetime)
		{
			return datetime.ToString();
		}
		
		/// <summary>
		/// returns id for a directoryinfo (md5 hash from the directory string)
		/// </summary>
		/// <param name="directoryinfo">
		/// A <see cref="DirectoryInfo"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public static String getId(DirectoryInfo directoryinfo)
		{
			return getMd5Hash((String)directoryinfo.FullName);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public static String getId(String path)
		{
			return getMd5Hash((String)path);
		}
		
		/// <summary>
		/// Gibt einen MD5 Hash als String zurück
		/// </summary>
		/// <param name="TextToHash">string der Gehasht werden soll.</param>
		/// <returns>Hash als string.</returns>
		public static string getMd5Hash(string TextToHash)
		{
		  try {
			  //Prüfen ob Daten übergeben wurden.
			  if((TextToHash == null) || (TextToHash.Length == 0))
			  {
			    return string.Empty;
			  }
			
			  //MD5 Hash aus dem String berechnen. Dazu muss der string in ein Byte[]
			  //zerlegt werden. Danach muss das Resultat wieder zurück in ein string.
			  MD5 md5 = new MD5CryptoServiceProvider();
			  byte[] textToHash = Encoding.Default.GetBytes (TextToHash);
			  byte[] result = md5.ComputeHash(textToHash); 
			  
			  //return result.ToString();
			  //return System.BitConverter.ToString(result); 
				
				// Create a new Stringbuilder to collect the bytes
	            // and create a string.
	            StringBuilder sBuilder = new StringBuilder();
				
				// Loop through each byte of the hashed data 
	            // and format each one as a hexadecimal string.
	            for (int i = 0; i < result.Length; i++)
	            {
	                sBuilder.Append(result[i].ToString("x2"));
	            }
	
	            // Return the hexadecimal string.
	            return "x" + sBuilder.ToString();
			} catch  (Exception ex)
			{
				//log.Error(ex);
				throw ex;
			}
		} 
		
		public static string getMd5Hash(FileInfo fileinfo)
		{
			String result;
			using (FileStream fs = new FileStream(fileinfo.FullName,FileMode.Open,FileAccess.Read))
			{
				result = getMd5Hash(fs);	
			}
			return result;
		}

		/// <summary>
		/// returns the md5 hash of a filestream
		/// </summary>
		/// <param name="filestream">
		/// A <see cref="FileStream"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		private static string getMd5Hash(FileStream filestream)
        {
            try {
				// Create a new instance of the MD5CryptoServiceProvider object.
	            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
						
	            // Convert the input string to a byte array and compute the hash.
	            byte[] data = md5Hasher.ComputeHash(filestream);
	
	            // Create a new Stringbuilder to collect the bytes
	            // and create a string.
	            StringBuilder sBuilder = new StringBuilder();
				
				// Loop through each byte of the hashed data 
	            // and format each one as a hexadecimal string.
	            for (int i = 0; i < data.Length; i++)
	            {
	                sBuilder.Append(data[i].ToString("x2"));
	            }
	
	            // Return the hexadecimal string.
	            return "x" + sBuilder.ToString();
			} catch (Exception ex)
			{
				//log.Error(ex);
				throw ex;
			}
        }

		/// <summary>
		/// initialize log4net
		/// </summary>
		public static void initLog4Net()
		{
			FileInfo fi = Configuration.ConfigurationManager.getConfigFile("log4net.config");
			if ((fi != null) && (fi.Exists))
			{
				XmlConfigurator.Configure(fi);			
			}
		}

		/// <summary>
		/// Compresses all files in the given folder using sharpziplib
		/// 
		/// http://stackoverflow.com/questions/1679986/zip-file-created-with-sharpziplib-cannot-be-opened-on-mac-os-x
		/// 
		/// </summary>
		/// <param name='folder'>
		/// Folder.
		/// </param>
		/// <param name='outputfile'>
		/// name of the zip
		/// </param>
        #if SHARPZIPLIB
		public static void CompressFolder(String folder, String outputfile)
		{
			DirectoryInfo di = new DirectoryInfo(folder);

			if (di.Exists)
			{
				FileInfo[] files = di.GetFiles();
				using (var outStream = new FileStream(outputfile, FileMode.Create))
		        {
		            using (var zipStream = new ZipOutputStream(outStream))
		            {
		                Crc32 crc = new Crc32();

		                foreach (FileInfo fi in files)
		                {
		                    byte[] buffer = File.ReadAllBytes(fi.FullName);

		                    ZipEntry entry = new ZipEntry(fi.Name);
							entry.DateTime = fi.LastWriteTime;
		                    entry.Size = buffer.Length;

		                    crc.Reset();
		                    crc.Update(buffer);

		                    entry.Crc = crc.Value;

		                    zipStream.PutNextEntry(entry);
		                    zipStream.Write(buffer, 0, buffer.Length);
		                }

		                zipStream.Finish();

		                // I dont think this is required at all
		                zipStream.Flush();
		                zipStream.Close();
		            }
		        }
			} else
			{
				throw new Exception("the folder " + folder + " does not exist");
			}
		}
#endif

		public static String getDateTimeForIndex(DateTime datetime)
		{
			return datetime.ToString("yyyyMMddHHmmss");
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

		/// <summary>
		/// Returns the content of a embedded ressource from given assembly
		/// </summary>
		/// <returns>
		/// The resource string.
		/// </returns>
		/// <param name='assembly'>
		/// Assembly.
		/// </param>
		/// <param name='ressourcename'>
		/// Ressourcename.
		/// </param>
		public static String getResourceString(System.Reflection.Assembly assembly, String ressourcename)
		{
			String s = String.Empty;

			using (Stream stream = assembly.GetManifestResourceStream(ressourcename))
			{
				if (stream != null)
				{
				    using (StreamReader _textStreamReader = new StreamReader(assembly.GetManifestResourceStream(ressourcename)))
					{	 
						s = _textStreamReader.ReadToEnd();
					}
				}
			}

			return s;
		}

		/// <summary>
		/// returns the content of a embedded resource using the System.Reflection.Assembly.GetEntryAssembly()
		/// </summary>
		/// <returns>
		/// The resource string.
		/// </returns>
		/// <param name='ressourcename'>
		/// Ressourcename.
		/// </param>
		public static String getResourceString(String ressourcename)
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
			if (assembly != null)
			{
				return getResourceString(assembly,ressourcename);
			}
			return String.Empty;
		}

		/// <summary>
		/// Executes a shell/commandline command
		/// 
		/// If an exception occurs the exception is catched and the function returns false !
		/// 
		/// </summary>
		/// <returns>
		/// The command.
		/// </returns>
		/// <param name='path'>
		/// If set to <c>true</c> path.
		/// </param>
		/// <param name='commandname'>
		/// If set to <c>true</c> commandname.
		/// </param>
		/// <param name='username'>
		/// If set to <c>true</c> username.
		/// </param>
		/// <param name='arguments'>
		/// If set to <c>true</c> arguments.
		/// </param>
		/// <param name='output'>
		/// the output of the commandline command or the exception string if an exception occurs
		/// </param>
		/// <param name='RedirectStandardOutput'>
		/// If set to <c>true</c> redirect standard output.
		/// </param>
		/// <param name='UseShellExecute'>
		/// If set to <c>true</c> use shell execute.
		/// </param>
		public static bool executeCommand(String path, String commandname, String username, String arguments, 
		                           out string output, bool RedirectStandardOutput=true, bool UseShellExecute=false)
		{
			output = String.Empty;
			try
			{
				//System.Diagnostics.Process p = new System.Diagnostics.Process();
				using (System.Diagnostics.Process p = new System.Diagnostics.Process())
				{
					p.StartInfo.FileName = commandname;
					p.StartInfo.UserName = username;
					//p.StartInfo.Arguments = "\"" + arguments + "\"";
					p.StartInfo.Arguments = arguments;

					p.StartInfo.RedirectStandardOutput = RedirectStandardOutput;
					p.StartInfo.UseShellExecute = UseShellExecute;
					//p.StartInfo.CreateNoWindow = true;
					
					p.Start();
					output = p.StandardOutput.ReadToEnd();
					p.WaitForExit();
					//p.Close();
					return true;
				}
			} catch (Exception ex)
			{
				output = ex.ToString();
				return false;
			}
		}


		
	}//class
}//namespace

