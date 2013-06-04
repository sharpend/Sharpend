//
//  SoundProxy.cs
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
using Sharpend.Utils;
using Sharpend.Utils.Applications.Canonical;
using System.Collections.Generic;

#if DBUS
namespace Sharpend.Utils.Applications.Canonical
{

	public delegate void VolumeChangedHandler(double volume);

	/// <summary>
	/// Sound proxy for Ubuntu
	/// </summary>
	public class SoundProxy : DBusBaseProxy<ISoundMenu>
	{
		public VolumeChangedHandler OnVolumeChanged;

		public Double Volume {
			get;
			private set;
		}

		public SoundProxy () : base("com.canonical.indicator.sound","/com/canonical/indicator/sound/menu")
		{
			this.Register();
		}

		public override void Register ()
		{
			base.Register ();
			RemoteInterface.ItemsPropertiesUpdated += HandleItemsPropertiesUpdated;
			//RemoteInterface.ItemActivationRequested += HandleItemActivationRequested;
			GetVolume();
		}

		void HandleItemActivationRequested (int data1, uint data2)
		{
			throw new NotImplementedException("HandleItemActivationRequested");
		}

		void HandleItemsPropertiesUpdated (EventHandlerData1[] data1, EventHandlerData2[] data2)
		{
			//Console.WriteLine("HandleItemsPropertiesUpdated");
			if (data1 != null)
			{
				if (data1[0].data2.Length > 0)
				{
					if (data1[0].data2[0].subdata1 == "x-canonical-ido-volume-level")
					{
						double vol = Convert.ToDouble(data1[0].data2[0].subdata2);
						Volume = vol;
						if (OnVolumeChanged != null)
						{
							OnVolumeChanged(vol);
						}
					}
				}
			}
		}

		/// <summary>
		/// set the volume
		/// </summary>
		/// <param name='volume'>
		/// value between 0 and 100
		/// </param>
		public void SetVolume(double volume)
		{
			//Dictionary<string, object> dt = new Dictionary<string, object>(100);

			EventGroupData data = new EventGroupData
			{
				data1 = 3,
				data2 = "change-volume",
				data3 = volume,
				data4 = 0
			};

			RemoteInterface.EventGroup(new EventGroupData[]{data});
		}

		public double GetVolume()
		{
			object o = RemoteInterface.GetProperty(3,"x-canonical-ido-volume-level");
			if (o != null)
			{
				Volume = Convert.ToDouble(o);
				return Volume;
			}
			return 0;
		}

		/// <summary>
		/// if sound is not muted sound will be muted
		/// if the sound is muted it will be unmuted
		/// </summary>
		public void Mute()
		{
			EventGroupData data = new EventGroupData
			{
				data1 = 2,
				data2 = "clicked",
				data3 = 0,
				data4 =  18227741
			};

			RemoteInterface.EventGroup(new EventGroupData[]{data});
		}

	}
}

#endif