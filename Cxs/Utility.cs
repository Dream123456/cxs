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

namespace Cxs
{
	public class Utility
	{
		public static object[] ToArray(ICollection collection, Type type)
		{
			Array array;

			array = Array.CreateInstance(type, collection.Count);

			collection.CopyTo(array, 0);

			return((object[]) array);
		}

		public static bool KeyExists(NameValueCollection nameValueCollection, string key)
		{
			if(nameValueCollection.GetValues(key) != null)
				return(true);

			return(false);
		}
	}
}