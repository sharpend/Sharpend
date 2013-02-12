//
// BindableData.cs
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

namespace Sharpend.GtkSharp
{
	/// <summary>
	/// Base class for bindable data
	/// 
	/// use this as base class if you want to bind your data to gtk controls
	/// 
	/// </summary>
	public class BindableData
	{
		public object Target {
			get;
			set;
		}

		/// <summary>
		/// this is true if we are doing a binding call from the object to the controls
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is binding; otherwise, <c>false</c>.
		/// </value>
		public bool IsBinding {
			get;
			private set;
		}

		public BindableData ()
		{
			Target = null;
		}

		public BindableData (object target)
		{
			Target = target;
		}

		/// <summary>
		/// Bind the specified target.
		/// </summary>
		/// <param name='target'>
		/// Target.
		/// </param>
		protected virtual void bind(object target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}

			try 
			{
				IsBinding = true;

				PropertyInfo[] properties = this.GetType().GetProperties();
				foreach(PropertyInfo property in properties)
				{
					BindableProperty b = Attribute.GetCustomAttribute(property,typeof(BindableProperty)) as BindableProperty;
					if (b != null)
					{
						Sharpend.GtkSharp.DataBinder.BindData(this,target,property.Name ,b.ControlName);
					}
				}
			} 
			finally
			{
				IsBinding = false;
			}
		}

		/// <summary>
		/// Bind to a specified target.
		/// </summary>
		/// <param name='target'>
		/// Target.
		/// </param>
		public virtual void Bind(object target)
		{
			bind(target);
		}

		/// <summary>
		/// bind to Target
		/// </summary>
		public virtual void Bind()
		{
			bind(Target);
		}

		/// <summary>
		/// Connect the specified sender, targettype and dataprovider.
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='targettype'>
		/// Targettype.
		/// </param>
		/// <param name='dataprovider'>
		/// Dataprovider.
		/// </param>
		public static void connect(object sender, Type targettype,object dataprovider)
		{
			object currentsender = sender;

			PropertyInfo[] properties = targettype.GetProperties();
			foreach(PropertyInfo property in properties)
			{
				BindableProperty b = Attribute.GetCustomAttribute(property,typeof(BindableProperty)) as BindableProperty;
				if (b != null)  //TODO error if null ??
				{
					if (!String.IsNullOrEmpty(b.Action))
					{
						if (b != null)
						{
							String pn = targettype.ToString() + "." + property.Name;

							if (currentsender == null)
							{
								currentsender = getSender(dataprovider,b.ControlName);
							}

							Sharpend.Utils.Reflection.hookDelegate(currentsender,delegate(object send, EventArgs e) {
								Sharpend.GtkSharp.DataBinder.BindData(dataprovider,send,pn);},b.Action,"false","");

							currentsender = null;
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets the sender.
		/// </summary>
		/// <returns>
		/// The sender.
		/// </returns>
		/// <param name='dataprovider'>
		/// Dataprovider.
		/// </param>
		/// <param name='sendername'>
		/// Sendername.
		/// </param>
		private static object getSender(object dataprovider, String sendername)
		{
			PropertyInfo pi = dataprovider.GetType().GetProperty(sendername);
			if (pi == null)
			{
				throw new Exception("Container does not have a DataObjects property");
			}

			return pi.GetValue(dataprovider,null);
		}

		/// <summary>
		/// Connect the specified targettype and dataprovider.
		/// </summary>
		/// <param name='targettype'>
		/// Targettype.
		/// </param>
		/// <param name='dataprovider'>
		/// Dataprovider.
		/// </param>
		public static void connect(Type targettype,object dataprovider)
		{
			connect(null,targettype,dataprovider);
		}

	}
}

