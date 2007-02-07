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
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Specialized;

namespace Cxs
{
	public class OutlookBcmDirectory : IDirectory
	{
		SqlConnection connection;

		#region IDirectory Members

		public void Initialize(NameValueCollection properties)
		{
			if(!Utility.KeyExists(properties, "connectionString"))
				throw(new Exception("Connection string property not specified."));

			connection = new SqlConnection(properties["connectionString"]);

			connection.Open();
		}

		public Contact[] GetAll()
		{
			return(get(new SqlCommand("select convert(varchar(64), EntryGUID) as Id, FullName from dbo.ContactExportView", connection)));
		}

		public Contact[] Search(string name)
		{
			SqlCommand command;

			command = new SqlCommand("select convert(varchar(64), EntryGUID) as Id, FullName from dbo.ContactExportView where FullName like '%' + @name + '%'", connection);

			command.Parameters.Add("@name", name);

			return(get(command));
		}

		public NameValueCollection GetTelephoneNumbers(string id)
		{
			NameValueCollection collection;
			SqlCommand command;
			SqlDataReader reader;

			collection = new NameValueCollection();

			command = new SqlCommand("select C.WorkPhoneNum, C.MobilePhoneNum, SIP.PropertyValue as SIP, PCM.FullName as AccountFullName, PCT.WorkPhoneNum as AccountWorkPhoneNum from dbo.ContactExportView C left outer join dbo.ContactAdditionalPropertyBag SIP on SIP.ContactID = C.ContactServiceID and SIP.PropertyMAPIID = 41177 join dbo.ContactMainTable CM on CM.ContactServiceID = C.ContactServiceID left outer join dbo.ContactNamesTable PCM on PCM.ContactServiceID = CM.ParentContactServiceID left outer join dbo.ContactPhoneTable PCT on PCT.ContactServiceID = CM.ParentContactServiceID where C.EntryGUID = convert(uniqueidentifier, @id)", connection);

			command.Parameters.Add("@id", id);

			reader = null;

			try {
				reader = command.ExecuteReader();

				if(reader.Read()) {
					addTelephoneNumber(collection, reader, "Work", "WorkPhoneNum");
					addTelephoneNumber(collection, reader, "Mobile", "MobilePhoneNum");
					addTelephoneNumber(collection, reader, "SIP", "SIP");

					if(!reader.IsDBNull(reader.GetOrdinal("AccountFullName")))
						addTelephoneNumber(collection, reader, reader.GetString(reader.GetOrdinal("AccountFullName")), "AccountWorkPhoneNum");
				}
			} finally {
				if(reader != null)
					reader.Close();
			}

			return(collection);
		}

		#endregion

		private Contact[] get(SqlCommand command)
		{
			ArrayList contacts;
			SqlDataReader reader;

			contacts = new ArrayList();

			reader = null;

			try {
				reader = command.ExecuteReader();

				while(reader.Read())
					contacts.Add(new Contact(reader.GetString(reader.GetOrdinal("Id")), reader.GetString(reader.GetOrdinal("FullName"))));
			} finally {
				if(reader != null)
					reader.Close();
			}

			return((Contact[]) contacts.ToArray(typeof(Contact)));
		}

		private void addTelephoneNumber(NameValueCollection collection, SqlDataReader reader, string name, string columnName)
		{
			if(!reader.IsDBNull(reader.GetOrdinal(columnName)))
				collection.Add(name, Contact.NormaliseTelephoneNumber(reader.GetString(reader.GetOrdinal(columnName))));
		}
	}
}