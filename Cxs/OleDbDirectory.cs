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
using System.Data.OleDb;
using System.Collections;
using System.Collections.Specialized;

namespace Cxs
{
	public class OleDbDirectory : IDirectory
	{
		private OleDbConnection connection;

		#region IDirectory Members

		public void Initialize(NameValueCollection properties)
		{
			if(!Utility.KeyExists(properties, "connectionString"))
				throw(new Exception("Connection string property not specified."));

			connection = new OleDbConnection(properties["connectionString"]);

			connection.Open();
		}

		public Contact[] GetAll()
		{
			return(get(new OleDbCommand("select Id, Name from TelephoneNumbers", connection)));
		}

		public Contact[] Search(string name)
		{
			OleDbCommand command;

			command = new OleDbCommand("select Id, Name from TelephoneNumbers where Name like '%' + ? + '%'", connection);

			command.Parameters.Add("name", name);

			return(get(command));
		}

		public System.Collections.Specialized.NameValueCollection GetTelephoneNumbers(string id)
		{
			NameValueCollection collection;
			OleDbCommand command;
			OleDbDataReader reader;

			collection = new NameValueCollection();

			command = new OleDbCommand("select TelephoneNumber from TelephoneNumbers where Id = ?", connection);

			command.Parameters.Add("id", int.Parse(id));

			reader = null;

			try {
				reader = command.ExecuteReader();

				if(reader.Read())
					collection.Add("Telephone", Contact.NormaliseTelephoneNumber(reader.GetString(reader.GetOrdinal("TelephoneNumber"))));
			} finally {
				if(reader != null)
					reader.Close();
			}

			return(collection);
		}

		#endregion

		private Contact[] get(OleDbCommand command)
		{
			ArrayList contacts;
			OleDbDataReader reader;

			contacts = new ArrayList();

			reader = null;

			try {
				reader = command.ExecuteReader();

				while(reader.Read())
					contacts.Add(new Contact(reader.GetInt32(reader.GetOrdinal("Id")).ToString(), reader.GetString(reader.GetOrdinal("Name"))));
			} finally {
				if(reader != null)
					reader.Close();
			}

			return((Contact[]) contacts.ToArray(typeof(Contact)));
		}
	}
}