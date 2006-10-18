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
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;

namespace Cxs.Configuration
{
	public class DirectoriesSectionHandler : IConfigurationSectionHandler
	{
		#region IConfigurationSectionHandler Members

		public object Create(object parent, object context, XmlNode section)
		{
			IDictionary directories;
			NameValueSectionHandler nameValueSectionHandler;
			XmlNodeList nodes;
			DirectoryConfiguration directory;
			NameValueCollection properties;

			directories = new ListDictionary();

			nameValueSectionHandler = new NameValueSectionHandler();

			nodes = section.SelectNodes("directory");

			foreach(XmlElement element in nodes) {
				if(element.GetAttributeNode("name") == null)
					throw(new ConfigurationException("Name not specified.", element));

				if(element.GetAttributeNode("type") == null)
					throw(new ConfigurationException("Type not specified.", element));

				if(element.SelectSingleNode("properties") == null)
					properties = new NameValueCollection();
				else
					properties = (NameValueCollection) nameValueSectionHandler.Create(null, context, element.SelectSingleNode("properties"));

				directory = new DirectoryConfiguration(element.GetAttribute("name"), element.GetAttribute("type"), properties);

				directories.Add(directory.Name, directory);
			}

			return(directories);
		}

		#endregion
	}
}