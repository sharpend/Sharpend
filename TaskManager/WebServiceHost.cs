//
//  WebServiceHost.cs
//
//  Author:
//       Dirk Lehmeier <sharpend_develop@yahoo.de>
//
//  Copyright (c) 2013 Dirk Lehmeier
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
using System.ServiceModel.Activation;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace TaskManager
{

	public interface ITaskManager
	{
		string startTask (String classname, String parameters);
	}


	public class WebServiceHostFactory : ServiceHostFactory
	{
		private readonly ITaskManager dep;
		
		public WebServiceHostFactory(ITaskManager dependency)
		{
			this.dep = dependency;
		}
		
		protected override ServiceHost CreateServiceHost(Type serviceType,
		                                                 Uri[] baseAddresses)
		{
			return new WebServiceHost(this.dep, serviceType, baseAddresses);
		}
	}


	public class WebServiceHost : ServiceHost
	{
		public WebServiceHost(ITaskManager dep, Type serviceType, params Uri[] baseAddresses)
			: base(serviceType, baseAddresses)
		{
			if (dep == null)
			{
				throw new ArgumentNullException("dep");
			}
			
			foreach (var cd in this.ImplementedContracts.Values)
			{
				cd.Behaviors.Add(new WebServiceHostProvider(dep));
			}
		}
	}

	/// <summary>
	/// Web service host provider.
	/// </summary>
	public class WebServiceHostProvider : IInstanceProvider, IContractBehavior
	{
		private readonly ITaskManager dep;
		
		public WebServiceHostProvider(ITaskManager dep)
		{
			if (dep == null)
			{
				throw new ArgumentNullException("dep");
			}
			
			this.dep = dep;
		}
		
		#region IInstanceProvider Members
		
		public object GetInstance(InstanceContext instanceContext, Message message)
		{
			return this.GetInstance(instanceContext);
		}
		
		public object GetInstance(InstanceContext instanceContext)
		{
			//return new MyService(this.dep);
			return new WebServiceControl (this.dep);
		}
		
		public void ReleaseInstance(InstanceContext instanceContext, object instance)
		{
		}
		
		#endregion
		
		#region IContractBehavior Members
		
		public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}
		
		public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
		}
		
		public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
		{
			dispatchRuntime.InstanceProvider = this;
		}
		
		public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
		{
		}
		
		#endregion
	}
}

