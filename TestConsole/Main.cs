//
// Main.cs
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
using GLib;
using NDesk.DBus;
using Sharpend.Utils.Applications.FreeDesktop;
using Sharpend;
using Sharpend.Utils.Webservices.Discogs;
using Sharpend.Utils.Applications.Tomboy;
using Sharpend.Utils.Applications.Canonical;
using Gtk;

namespace TestConsole
{
	public class DBusProxy : Sharpend.Utils.DBusBaseProxy<IDBus>
		{
			public DBusProxy() 
				: base("org.freedesktop.DBus","/org/freedesktop/DBus")
			{
				Register();
			}
	
			
		}
	
	class MainClass
	{
		
		static SoundProxy sp;
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");
			
			//Discogs dg = new Discogs();
			//dg.GetArtist("Guano Apes");
			//dg.Search("Guano Apes");

			//TomboyProxy tp = new TomboyProxy();

			//return;

			//Console.WriteLine(si.GetVolume());

			//sp.SetVolume();
			//sp.Mute();
			//sp.SetVolume(30);
//			GType.Init();
//			
//			GLib.MainLoop mainLoop = new GLib.MainLoop();		
//			
//			GLib.Timeout.Add (500, () => {
//				TestScope();
//				return false;
//			});
//			
//			mainLoop.Run();


			Application.Init();

			GLib.Timeout.Add (500, () => {
				mainLoop();
				return true;
			});

			Application.Run();

			//Console.WriteLine(si.Volume);

			Console.WriteLine ("Ende");
		}

		static void HandleOnSoundStateUpdate (int state)
		{
			Console.WriteLine(state);
		}

		private static bool init=false;
		private static double x=50.0;
		private static void mainLoop()
		{
			if (!init)
			{
				sp = new SoundProxy();
				sp.OnVolumeChanged += HandleVolumeChanged;

				//Console.WriteLine(sp.RemoteInterface.Status);
//				SoundIndicatorProxy si = new SoundIndicatorProxy();
//				si.OnSoundStateUpdate += HandleOnSoundStateUpdate;
//
//				Console.WriteLine(si.GetVolume());

				init = true;
				//sp.SetVolume(x);
				//sp.GetVolume();
				Console.WriteLine("Vol:" + sp.Volume);
			}

			//Console.Write(sp.RemoteInterface.Status);

//			Console.WriteLine(x);
//			x--;
//			if (x == 40.0)
//			{
//				sp.SetVolume(20);
//			}


//			if (init)
//			{
//				sp.SetVolume(30);
//			}

			Console.WriteLine(sp.GetVolume());
			if (x < 20)
			{
				//x=50;
			}
		}

		private static void HandleVolumeChanged(double vol)
		{
			Console.WriteLine("vol" + vol);
		}

		public static void TestScope()
		{
			BusG.Init ();
			
			DBusProxy dp = new DBusProxy();
			
			//
			uint x = dp.RemoteInterface.RequestName("net.launchpad.scope.RemoteVideos",0x4);
			
			if (x != 1)
			{
				throw new Exception("already connected");
			}
			
			//UnityScope scope = new UnityScope("/net/launchpad/scope/remotevideos");

			
		}

		
		public static void changed(object o, NotifyArgs args)
		{
			Console.WriteLine("changed");
		}
		

		
		
	}
}
