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

using Novell.Directory.Ldap;

namespace Cxs
{
	public class LdapDirectory : IDirectory
	{
		private NameValueCollection properties;
		private LdapConnection connection;

		#region IDirectory Members

		public void Initialize(NameValueCollection properties)
		{
			this.properties = properties;

			if(!Utility.KeyExists(properties, "server"))
				throw(new Exception("Server property not specified."));

			if(!Utility.KeyExists(properties, "base"))
				throw(new Exception("Base property not specified."));

			connection = new LdapConnection();

			connection.Connect(properties["server"], Utility.KeyExists(properties, "port") ? int.Parse(properties["port"]) : LdapConnection.DEFAULT_PORT);
		}

		public Contact[] GetAll()
		{
			return(get("(objectClass=officePerson)"));
		}

		public Contact[] Search(string name)
		{
			return(get(string.Format("(&(cn=*{0}*)(objectClass=officePerson))", name)));
		}

		public NameValueCollection GetTelephoneNumbers(string id)
		{
			NameValueCollection collection;
			LdapEntry entry;

			collection = new NameValueCollection();

			entry = connection.Read(id);

			addTelephoneNumber(collection, entry, "Home", "homePhone");
			addTelephoneNumber(collection, entry, "Mobile", "mobile");
			addTelephoneNumber(collection, entry, "SIP", "sipPhone");

			return(collection);
		}

		#endregion

		private Contact[] get(string filter)
		{
			ArrayList contacts;
			LdapSearchResults results;
			LdapEntry[] entries;

			contacts = new ArrayList();

			results = connection.Search(properties["base"], LdapConnection.SCOPE_SUB, filter, null, false);

			entries = toArray(results);

			foreach(LdapEntry entry in entries)
				contacts.Add(new Contact(entry.DN, entry.getAttribute("cn").StringValue));

			return((Contact[]) contacts.ToArray(typeof(Contact)));
		}

		private void addTelephoneNumber(NameValueCollection collection, LdapEntry entry, string name, string attributeName)
		{
			LdapAttribute attribute;

			attribute = entry.getAttribute(attributeName);

			if(attribute != null)
				collection.Add(name, Contact.NormaliseTelephoneNumber(attribute.StringValue));
		}

		private static LdapEntry[] toArray(LdapSearchResults results)
		{
			ArrayList entries;

			entries = new ArrayList();

			while(results.hasMore())
				entries.Add(results.next());

			return((LdapEntry[]) entries.ToArray(typeof(LdapEntry)));
		}
	}
}