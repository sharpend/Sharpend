////
////  SoundIndicatorProxy.cs
////
////  Author:
////       Dirk Lehmeier <sharpend_develop@yahoo.de>
////
////  Copyright (c) 2012 Dirk Lehmeier
////
////  This program is free software: you can redistribute it and/or modify
////  it under the terms of the GNU Lesser General Public License as published by
////  the Free Software Foundation, either version 3 of the License, or
////  (at your option) any later version.
////
////  This program is distributed in the hope that it will be useful,
////  but WITHOUT ANY WARRANTY; without even the implied warranty of
////  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
////  GNU Lesser General Public License for more details.
////
////  You should have received a copy of the GNU Lesser General Public License
////  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//using System;
//
//#if DBUS
//using NDesk.DBus;
//
//namespace Sharpend.Utils.Applications.Canonical
//{
//	public class SoundIndicatorProxy : DBusBaseProxy<ISoundIndicator>
//	{
//		public event SoundStateUpdateUpdateHandler OnSoundStateUpdate;
//
//		public int Volume {
//			get;
//			private set;
//		}
//		
//		public SoundIndicatorProxy () : base("com.canonical.indicator.sound","/com/canonical/indicator/sound/service")
//		{
//			this.Register();
//		}
//
//		public override void Register ()
//		{
//			base.Register ();	
//			RemoteInterface.SoundStateUpdate += SoundChanged;
//		}
//
//		public int GetVolume()
//		{
//			return RemoteInterface.GetSoundState();
//		}
//
//		private void SoundChanged(int value)
//		{
//			Volume = value;
//			if (OnSoundStateUpdate != null)
//			{
//				OnSoundStateUpdate(Volume);
//			}
//		}
//	}
//}
//#endif
