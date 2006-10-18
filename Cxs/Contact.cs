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

namespace Cxs
{
	public class Contact
	{
		private string id;
		private string name;

		public Contact(string id, string name)
		{
			this.id = id;
			this.name = name;
		}

		public string Id
		{
			get {
				return(id);
			}
		}

		public string Name
		{
			get {
				return(name);
			}
		}

		public static string NormaliseTelephoneNumber(string telephoneNumber)
		{
			string number;

			number = telephoneNumber;

			number = number.Replace("(", string.Empty);
			number = number.Replace(")", string.Empty);
			number = number.Replace("-", string.Empty);

			number = number.Replace(" ", string.Empty);

			if(number.StartsWith("+"))
				number = string.Format("00{0}", number.Substring(1));

			return(number);
		}
	}

	public class ContactComparer : IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
			return(string.Compare(((Contact) x).Name, ((Contact) y).Name));
		}

		#endregion
	}
}