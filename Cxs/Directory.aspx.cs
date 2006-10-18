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
	public class Directory : CiscoPage
	{
		public static readonly int PageSize = 30;

		protected Cxs.WebControls.PhoneMenu Menu;

		public static string GetUri(string directory)
		{
			return(string.Format("http://192.168.1.64/Cisco/Directory.aspx?Directory={0}", HttpUtility.UrlEncode(directory)));
		}

		public static string GetUri(string directory, int start)
		{
			return(string.Format("http://192.168.1.64/Cisco/Directory.aspx?Directory={0}&amp;Start={1}", HttpUtility.UrlEncode(directory), start));
		}

		private void Page_Load(object sender, System.EventArgs arguments)
		{
			IDirectory directory;
			Contact[] contacts;
			int startIndex;
			Contact contact;
			int index;

			if(Request["Directory"] == null) {
				Menu.Title = "Directories";
				Menu.Prompt = "Select a directory";

				foreach(string directoryName in DirectoryFactory.GetAll())
					Menu.Items.Add(new PhoneMenu.MenuItem(directoryName, GetUri(directoryName)));
			} else {
				directory = DirectoryFactory.GetByName(Request["Directory"]);
				
				Menu.Title = Request["Directory"];
				Menu.Prompt = "Select a contact";

				Menu.Items.Add(new PhoneMenu.MenuItem("Search...", Search.GetUri(Request["Directory"])));

				contacts = directory.GetAll();

				Array.Sort(contacts, new ContactComparer());

				startIndex = 0;

				try {
					startIndex = int.Parse(Request["Start"]);
				} catch { }

				for(index = startIndex; index < (startIndex + PageSize > contacts.Length ? contacts.Length : startIndex + PageSize); index++) {
					contact = contacts[index];

					Menu.Items.Add(new PhoneMenu.MenuItem(contact.Name, DirectoryEntry.GetUri(Request["Directory"], contact)));
				}

				if(index < contacts.Length)
					Menu.Items.Add(new PhoneMenu.MenuItem("More...", GetUri(Request["Directory"], startIndex + PageSize)));
			}
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