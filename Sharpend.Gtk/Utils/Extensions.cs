using System;

namespace Sharpend.GtkSharp
{
	public static class Extensions
	{
		public static Cairo.Color ToCairoColor (this Gdk.Color color)
		{
		        return new Cairo.Color ((double)color.Red / ushort.MaxValue,
				(double)color.Green / ushort.MaxValue, (double)color.Blue /
				ushort.MaxValue);
		}


		public static Gdk.Color ToGdkColor (this Cairo.Color color)
		{
		        Gdk.Color c = new Gdk.Color ();
		        c.Blue = (ushort)(color.B * ushort.MaxValue);
		        c.Red = (ushort)(color.R * ushort.MaxValue);
		        c.Green = (ushort)(color.G * ushort.MaxValue);
		
		        return c;
		}
	}
}

