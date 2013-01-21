//
// Reflection.cs
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

namespace Sharpend.Utils
{
	/// <summary>
	/// helper functions for reflection
	/// </summary>
	public static class Reflection
	{

		/// <summary>
		/// unhook a delegate
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='target'>
		/// Target.
		/// </param>
		/// <param name='eventname'>
		/// Eventname.
		/// </param>
		/// <param name='functionname'>
		/// Functionname.
		/// </param>
		/// <param name='multi1'>
		/// Multi1.
		/// </param>
		/// <param name='multi2'>
		/// Multi2.
		/// </param>
		public static void unHookDelegate(object sender, object target, String eventname, String functionname, String multi1, String multi2)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            try
            {
                FieldInfo fi = sender.GetType().GetField(eventname, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                if (fi != null)
                {
                    Delegate del = (Delegate)fi.GetValue(sender);
                    if (del != null)
                    {                                       
                        if (del.GetInvocationList().Length > 0)
                        {
                            Delegate[] dl = del.GetInvocationList();
                            foreach (Delegate d in dl)
                            { 
                                EventInfo evClick2 = sender.GetType().GetEvent(fi.Name);
                                MethodInfo remHandler = evClick2.GetRemoveMethod();
                                Object[] remHandlerArgs = { d };
                                remHandler.Invoke(sender, remHandlerArgs);                                         
                            }
                        }
                    }
                } 
            }
            catch (Exception ex)
            {
                //log.Error(ex);
				//throw ex; //TODO make some logging and dont throw
				throw new Exception("Error in hookDelegate: " + eventname + " - " + functionname + " - " + sender.ToString() + " - " + target.ToString() + " - "  + ex.Message);
            }
        }


        /// <summary>
        /// hook delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="target"></param>
        /// <param name="eventname"></param>
        /// <param name="functionname"></param>
        public static void hookDelegate(object sender, object target, String eventname, String functionname, String multi1, String multi2)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }


            try
            {
                EventInfo evClick = sender.GetType().GetEvent(eventname);
                Type tDelegate = evClick.EventHandlerType;

				//check if a delegate is already hooked
                if (multi1.Equals("Single",StringComparison.OrdinalIgnoreCase))
                {
                    FieldInfo fi = sender.GetType().GetField(eventname, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                    if (fi != null)
                    {
                        Delegate del = (Delegate)fi.GetValue(sender);
                        if (del != null)
                        {             
                            if (del.GetInvocationList().Length > 0)
                            {
								Console.WriteLine("delegate already hooked, and only one is allowed! ->" + eventname + " sender: " + sender + " target: " + target);
                                return;
                            }
                        }
                    }
                }

				if (multi1.Equals("Multi",StringComparison.OrdinalIgnoreCase))
                {
					//Console.WriteLine("do multi hook: " + eventname + " sender: " + sender + " target: " + target);
                    FieldInfo fi = sender.GetType().GetField(eventname, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                    if (fi != null)
                    {
                        Delegate del = (Delegate)fi.GetValue(sender);
                        if (del != null)
                        {             
                            Delegate[] lst = del.GetInvocationList();
							if (lst.Length > 0)
							{
								foreach (Delegate hd in lst)
								{
									//Console.WriteLine("delegate: " + hd.Target.GetType());
									if (hd.Target.GetType().ToString() == target.GetType().ToString())
									{
										Console.WriteLine("delegate already hooked to target: " + eventname + " sender: " + sender + " target: " + target);
										return;
									}
								}
							}	
                        }
                    }
                }


                MethodInfo miHandler =
                target.GetType().GetMethod(functionname,
                    BindingFlags.Public | BindingFlags.Instance);
				
				if (miHandler == null)
				{
					Console.WriteLine("Target does not contain: " + functionname);
				}

                Delegate d = Delegate.CreateDelegate(tDelegate, target, miHandler);

                
                MethodInfo addHandler = evClick.GetAddMethod();
                Object[] addHandlerArgs = { d };
                addHandler.Invoke(sender, addHandlerArgs);
            }
            catch (Exception ex)
            {
                //log.Error(ex);
				//throw ex; //TODO make some logging and dont throw
				throw new Exception("Error in hookDelegate: " + eventname + " - " + functionname + " - " + sender.ToString() + " - " + target.ToString() + " - "  + ex.Message);
            }

        }

		/// <summary>
        /// hook delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="target"></param>
        /// <param name="eventname"></param>
        /// <param name="functionname"></param>
        public static void hookDelegate(object sender, EventHandler handler, String eventname, String multi1, String multi2)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender");
            }

            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }


            try
            {
                EventInfo evClick = sender.GetType().GetEvent(eventname);
                
				//check if a delegate is already hooked
                if (multi1.Equals("Single",StringComparison.OrdinalIgnoreCase))
                {
                    FieldInfo fi = sender.GetType().GetField(eventname, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                    if (fi != null)
                    {
                        Delegate del = (Delegate)fi.GetValue(sender);
                        if (del != null)
                        {                                       
                            if (del.GetInvocationList().Length > 0)
                            {
                                return;
                            }
                        }
                    }
                }
        
                MethodInfo addHandler = evClick.GetAddMethod();
                Object[] addHandlerArgs = { handler };
                addHandler.Invoke(sender, addHandlerArgs);
            }
            catch (Exception ex)
            {
                //log.Error(ex);
				//throw ex; //TODO make some logging and dont throw
				throw new Exception("Error in hookDelegate: " + eventname + " - " + " - " + sender.ToString() + " - " + handler.ToString() + " - "  + ex.Message);
            }

        }
		
		/// <summary>
		/// creates a instance of the given type using a standard consturctor with 0 params
		/// </summary>
		/// <returns>
		/// The instance.
		/// </returns>
		/// <param name='type'>
		/// Type.
		/// </param>
		public static object createInstance(Type type)
		{
			ConstructorInfo ci = type.GetConstructor(new Type[0]);
            object o = ci.Invoke(new object[0]);
			if (o != null)
			{
				return o;
			}
			return null;	
		}
		
		/// <summary>
		/// creates a new instance from a parameterset
		/// </summary>
		/// <returns>
		/// The instance.
		/// </returns>
		/// <param name='param'>
		/// Parameter.
		/// </param>
		public static object createInstance(ParameterSet param)
		{
			ConstructorInfo ci = param.BaseType.GetConstructor(param.Types);
            object o = ci.Invoke(param.Data);
			return o;  //TODO logging ... try catch ??
		}
		
		
//		public static void hookDelegate(DelegateData dd)
//		{
//			hookDelegate(dd.SourceName,dd.Target,dd.EventName,dd.FunctionName,"Single","Single");
//		}
		
	}
}

