//
// ISoundIndicator.cs
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
using NDesk.DBus;

namespace Sharpend.Utils.Applications.Canonical
{
	
	public delegate void SoundStateUpdateUpdateHandler(int state);
	
	[Interface("com.canonical.indicator.sound")]
	public interface ISoundIndicator
	{
		bool BlacklistMediaPlayer(string player_desktop_name, bool blacklist);
	    bool IsBlacklisted(string player_desktop_name);
	    int GetSoundState();
	    //void EnableTrackSpecificItems((Err) player_object_path, string player_desktop_id);
	    //void EnablePlayerSpecificItems((Err) player_object_path, string player_desktop_id);
	    //event EventHandler<int> SoundStateUpdate;
		event SoundStateUpdateUpdateHandler SoundStateUpdate;
	}
}

