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
using System.Web;
using System.Web.UI;

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

		public static string BuildUri(string relativeUri, params string[] parameters)
		{
			HttpRequest request;
			Uri uri;

#if SITE_VANDELAY
			uri = new Uri(new Uri("http://192.168.1.64/Cxs/"), parameters.Length == 0 ? relativeUri : string.Format("{0}?{1}", relativeUri, string.Join("&", parameters)));
#else
			request = HttpContext.Current.Request;

			uri = new Uri(new Uri(request.Url.GetLeftPart(UriPartial.Path)), parameters.Length == 0 ? relativeUri : string.Format("{0}?{1}", relativeUri, string.Join("&", parameters)));
#endif

			return(uri.ToString());
		}
	}
}