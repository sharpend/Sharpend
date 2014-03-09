//
//  IWebserviceControl.cs
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
using System.ServiceModel;
using System.ServiceModel.Web;

namespace TaskManager
{
    [ServiceContract]
	public interface IWebserviceControl
	{
		//[OperationContract]
		//[WebGet(ResponseFormat = WebMessageFormat.Json)]
		[OperationContract,WebGet(UriTemplate = "/start/{classname}/{parameters}", ResponseFormat = WebMessageFormat.Json)]
		string StartTask(string classname, string parameters);

		//[OperationContract]
		[OperationContract, WebGet(UriTemplate = "/class/{classname}", ResponseFormat = WebMessageFormat.Json)]
		TestData GetTaskStatus(String classname);

		[OperationContract,WebGet(UriTemplate = "/waitfor/{classname}", ResponseFormat = WebMessageFormat.Json)]
		string WaitForTask (String classname);
	}

	public class TestData {
		public String Daten {
			get;
			set;
		}
	}
}

