//
// PlaceholderWindow.cs
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

//
// PlaceholderWindow.cs
//
// Author:
//   Lluis Sanchez Gual
//

//
// Copyright (C) 2007 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using Gdk;
using Gtk;

#if !GTK2
namespace Sharpend.GtkSharp.Docking
{
	public class PlaceholderWindow: Gtk.Window
	{
		//Gdk.GC redgc;
		//Cairo.Context redgc;
		//uint anim;
		//int rx, ry, rw, rh;
		bool allowDocking;
		
		public bool AllowDocking {
			get {
				return allowDocking;
			}
			set {
				allowDocking = value;
			}
		}
		
		public ItemAlignment Alignment {
			get;
			set;
		}
		
		public PlaceholderWindow (): base (Gtk.WindowType.Popup)
		{
			SkipTaskbarHint = true;
			Decorated = false;
			//TransientFor = (Gtk.Window) frame.Toplevel;
			TypeHint = WindowTypeHint.Utility;
			
			// Create the mask for the arrow
			
			Realize ();
			this.Opacity = 0.5;
//			redgc = new Gdk.GC (GdkWindow);
//	   		redgc.RgbFgColor = frame.Style.Background (StateType.Selected);
		}
		
		void CreateShape (int width, int height)
		{
			Gdk.Color black, white;
			black = new Gdk.Color (0, 0, 0);
			black.Pixel = 1;
			white = new Gdk.Color (255, 255, 255);
			white.Pixel = 0;
			
//			Gdk.Pixmap pm = new Pixmap (this.GdkWindow, width, height, 1);
//			Gdk.GC gc = new Gdk.GC (pm);
//			gc.Background = white;
//			gc.Foreground = white;
//			pm.DrawRectangle (gc, true, 0, 0, width, height);
//			
//			gc.Foreground = black;
//			pm.DrawRectangle (gc, false, 0, 0, width - 1, height - 1);
//			pm.DrawRectangle (gc, false, 1, 1, width - 3, height - 3);
//			
//			this.ShapeCombineMask (pm, 0, 0);
		}
		
		protected override void OnSizeAllocated (Rectangle allocation)
		{
			base.OnSizeAllocated (allocation);
			CreateShape (allocation.Width, allocation.Height);
		}
		
//		protected override bool OnDrawn (Cairo.Context cr)
//		{
//			return base.OnDrawn (cr);
//		}
		
//		protected override bool OnExposeEvent (Gdk.EventExpose args)
//		{
//			//base.OnExposeEvent (args);
//			int w, h;
//			this.GetSize (out w, out h);
//			this.GdkWindow.DrawRectangle (redgc, false, 0, 0, w-1, h-1);
//			this.GdkWindow.DrawRectangle (redgc, false, 1, 1, w-3, h-3);
//	  		return true;
//		}
		
//		public void Relocate (int x, int y, int w, int h, bool animate)
//		{
//			if (x != rx || y != ry || w != rw || h != rh) {
//				Move (x, y);
//				Resize (w, h);
//				
//				rx = x; ry = y; rw = w; rh = h;
//				
//				if (anim != 0) {
//					GLib.Source.Remove (anim);
//					anim = 0;
//				}
//				if (animate && w < 150 && h < 150) {
//					int sa = 7;
//					Move (rx-sa, ry-sa);
//					Resize (rw+sa*2, rh+sa*2);
//					anim = GLib.Timeout.Add (10, RunAnimation);
//				}
//			}
//		}
//		
//		bool RunAnimation ()
//		{
//			int cx, cy, ch, cw;
//			GetSize (out cw, out ch);
//			GetPosition	(out cx, out cy);
//			
//			if (cx != rx) {
//				cx++; cy++;
//				ch-=2; cw-=2;
//				Move (cx, cy);
//				Resize (cw, ch);
//				return true;
//			}
//			anim = 0;
//			return false;
//		}
	}
}
#endif