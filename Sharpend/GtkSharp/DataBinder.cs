//
// DataBinder.cs
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
using System.Reflection;
using System.Collections.Generic;

#if XWTSUPPORT
using Xwt;
#endif

namespace Sharpend.GtkSharp
{
	/// <summary>
	/// some helper functions for databinding
	/// </summary>
	public static class DataBinder
	{

		/// <summary>
		/// you can use this to handle exceptions while databinding
		/// if this is not specified the exception will be throwed
		/// </summary>
		public static event EventHandler OnBindException;

		/// <summary>
		/// get a value from various controls or objects
		/// </summary>
		/// <returns>
		/// The value.
		/// </returns>
		/// <param name='dataholder'>
		/// Dataholder.
		/// </param>
		private static object getValue(object dataholder)
		{
			//TODO more controls

			if (dataholder == null)
			{
				throw new ArgumentNullException("dataholder");
			}

			#if XWTSUPPORT
			//XWT
			if (dataholder is Xwt.TextEntry)
			{
				return (dataholder as Xwt.TextEntry).Text;
			}

			if (dataholder is Xwt.CheckBox)
			{
				return (dataholder as Xwt.CheckBox).Active;
			}
			#endif


			//GTK
			if (dataholder is Gtk.Entry)
			{
				return (dataholder as Gtk.Entry).Text;
			}

			if (dataholder is Gtk.CheckButton)
			{
				return (dataholder as Gtk.CheckButton).Active;
			}

			throw new Exception("unknown dataholder: " + dataholder);
		}

		/// <summary>
		/// set a control value
		/// </summary>
		/// <param name='dataholder'>
		/// Dataholder.
		/// </param>
		/// <param name='value'>
		/// Value.
		/// </param>
		private static void setValue(object dataholder, object value)
		{
			//TODO more controls
			if (dataholder == null)
			{
				throw new ArgumentNullException("dataholder");
			}

			#if XWTSUPPORT
			//XWT
			if (dataholder is Xwt.TextEntry)
			{
				(dataholder as Xwt.TextEntry).Text = (value as String);
				return;
			}

			if (dataholder is Xwt.CheckBox)
			{
				(dataholder as Xwt.CheckBox).Active = Convert.ToBoolean(value);
				return;
			}
			#endif


			//GTK
			if (dataholder is Gtk.Entry)
			{
				(dataholder as Gtk.Entry).Text = (value as String);
				return;
			}

			if (dataholder is Gtk.CheckButton)
			{
				(dataholder as Gtk.CheckButton).Active = Convert.ToBoolean(value);
				return;
			}

			throw new Exception("Dataholder not found: " + dataholder);
		}


		/// <summary>
		/// returns the name of the target property
		/// </summary>
		/// <returns>
		/// The target property name.
		/// </returns>
		/// <param name='property'>
		/// Property.
		/// </param>
		private static String getTargetPropertyName(string property)
		{
			String ret = property;
			if (property.Contains(","))
			{
				ret = ret.Split(',')[0];
			}

			if (ret.Contains("."))
			{
				string[] dt = ret.Split('.');
				ret = dt[dt.Length-1];
			}

			return ret;
		}

		/// <summary>
		/// returns the key used in DataObjects
		/// </summary>
		/// <returns>
		/// The data object key.
		/// </returns>
		/// <param name='property'>
		/// Property.
		/// </param>
		private static String getDataObjectKey(string property)
		{
			return property.Substring(0,property.LastIndexOf('.'));
		}

		/// <summary>
		/// binds data from a gtk widget (e.g. entry) to a custom object property
		/// </summary>
		/// <param name='container'>
		/// a container with a "DataObjects" property that contains the target objects which receive data
		/// </param>
		/// <param name='dataholder'>
		/// a object (control) containing the source data
		/// </param>
		/// <param name='property'>
		/// propertyname wich recieves the date
		/// </param>
		public static void BindData(object container,object dataholder,string property)
		{
			try
			{
				PropertyInfo pi = container.GetType().GetProperty("DataObjects");
				if (pi == null)
				{
					throw new Exception("Container does not have a DataObjects property");
				}

				DataObjectContainer dataobjects = (pi.GetValue(container,null) as DataObjectContainer);
				if (dataobjects == null)
				{
					throw new Exception("Could not get DataObjects Property, this must be a  Dictionary<String,object> ");
				}

				object target = null;
				String key = getDataObjectKey(property);
				if (dataobjects.ContainsKey(key))
				{
					target = dataobjects.Get(key);
				}

				if (target == null)
				{
					throw new Exception("Could not find target object: " + key);
				}


				if (target is BindableData)
				{
					//check if we are binding in the other direction
					if ((target as BindableData).IsBinding)
					{
						return;
					}
				}

				String tp = getTargetPropertyName(property);
				PropertyInfo pi2 = target.GetType().GetProperty(tp);
				if (pi2 == null)
				{
					throw new Exception("Container does not have a DataObjects property");
				}

				object val = getValue(dataholder);
				pi2.SetValue(target,val,null);

			} catch (Exception ex)
			{
				if (OnBindException != null)
				{
					OnBindException(ex,new EventArgs());
				} else
				{
					throw;
				}
			}
		}

//		public static void BindData(object sender, EventArgs args)
//		{
//
//		}

		/// <summary>
		/// Binds the data.
		/// </summary>
		/// <param name='source'>
		/// Source.
		/// </param>
		/// <param name='target'>
		/// Target.
		/// </param>
		/// <param name='propertyname'>
		/// Propertyname.
		/// </param>
		/// <param name='targetname'>
		/// Targetname.
		/// </param>
		public static void BindData(object source,object target,String propertyname, String targetname)
		{
			try
			{
				PropertyInfo pi = source.GetType().GetProperty(propertyname);
				if (pi == null)			{
					throw new Exception("Could not find property: " + propertyname);
				}

				PropertyInfo pi2 = target.GetType().GetProperty(targetname);
				if (pi2 == null)
				{
					throw new Exception("Could not find property: " + targetname);
				}

				object val = pi.GetValue(source,null);


				object dest = pi2.GetValue(target,null);


				setValue(dest,val);
			} catch (Exception ex)
			{
				if (OnBindException != null)
				{
					OnBindException(ex,new EventArgs());
				} else
				{
					throw;
				}
			}
		}


	}
}

