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
using System.Collections;
using System.Configuration;

using Cxs.Configuration;

namespace Cxs
{
	public class DirectoryFactory
	{
		private static IDictionary getAll()
		{
			return((IDictionary) ConfigurationSettings.GetConfig("cxs/directories"));
		}

		public static string[] GetAll()
		{
			IDictionary configurations;

			configurations = getAll();

			return((string[]) Utility.ToArray(configurations.Keys, typeof(string)));
		}

		public static IDirectory GetByName(string name)
		{
			IDictionary configurations;
			DirectoryConfiguration configuration;
			IDirectory directory;

			configurations = getAll();

			configuration = (DirectoryConfiguration) configurations[name];

			directory = (IDirectory) Activator.CreateInstance(Type.GetType(configuration.Type));

			directory.Initialize(configuration.Properties);

			return(directory);
		}
	}
}