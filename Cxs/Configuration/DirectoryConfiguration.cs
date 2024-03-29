#region Preamble
/*
 * $Id:$
 * 
 * 
 * Copyright 2006 Andrew Kay.  All rights reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License.  You may obtain a copy
 * of the License at
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations
 * under the License.
 */
#endregion

using System;
using System.Collections.Specialized;

namespace Cxs.Configuration
{
	public class DirectoryConfiguration
	{
		private string name;
		private string type;
		private NameValueCollection properties;

		public DirectoryConfiguration(string name, string type, NameValueCollection properties)
		{
			this.name = name;
			this.type = type;
			this.properties = properties;
		}

		public string Name
		{
			get {
				return(name);
			}
		}

		public string Type
		{
			get {
				return(type);
			}
		}

		public NameValueCollection Properties
		{
			get {
				return(properties);
			}
		}
	}
}