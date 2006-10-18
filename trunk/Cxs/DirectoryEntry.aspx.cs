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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;

using Cxs.WebControls;

namespace Cxs
{
	public class DirectoryEntry : CiscoPage
	{
		protected Cxs.WebControls.PhoneDirectory Directory;

		public static string GetUri(string directory, Contact contact)
		{
			return(string.Format("http://192.168.1.64/Cisco/DirectoryEntry.aspx?Directory={0}&amp;Id={1}", HttpUtility.UrlEncode(directory), HttpUtility.UrlEncode(contact.Id)));
		}

		private void Page_Load(object sender, System.EventArgs arguments)
		{
			IDirectory directory;
			NameValueCollection telephoneNumbers;

			//
			directory = DirectoryFactory.GetByName(Request["Directory"]);

			//
			telephoneNumbers = directory.GetTelephoneNumbers( HttpUtility.UrlDecode( Request["Id"] ) );

			foreach(string key in telephoneNumbers.Keys)
				Directory.Entries.Add(new PhoneDirectory.DirectoryEntry(key, telephoneNumbers[key]));
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}