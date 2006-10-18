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

			command = new SqlCommand("select WorkPhoneNum, MobilePhoneNum from dbo.ContactExportView where EntryGUID = convert(uniqueidentifier, @id)", connection);

			command.Parameters.Add("@id", id);

			reader = null;

			try {
				reader = command.ExecuteReader();

				if(reader.Read()) {
					addTelephoneNumber(collection, reader, "Work", "WorkPhoneNum");
					addTelephoneNumber(collection, reader, "Mobile", "MobilePhoneNum");
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