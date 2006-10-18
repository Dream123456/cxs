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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cxs.WebControls
{
	public class PhoneMenu : System.Web.UI.WebControls.WebControl
	{
		private string title;
		private string prompt;
		private IList items;

		public class MenuItem
		{
			private string name;
			private string uri;

			public MenuItem(string name, string uri)
			{
				this.name = name;
				this.uri = uri;
			}

			public string Name
			{
				get {
					return(name);
				}

				set {
					name = value;
				}
			}

			public string Uri
			{
				get {
					return(uri);
				}

				set {
					uri = value;
				}
			}
		}

		public PhoneMenu()
		{
			title = string.Empty;
			prompt = string.Empty;

			items = new ArrayList();
		}

		public string Title
		{
			get {
				return(title);
			}

			set {
				title = value;
			}
		}

		public string Prompt
		{
			get {
				return(prompt);
			}

			set {
				prompt = value;
			}
		}

		public IList Items
		{
			get {
				return(items);
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			MenuItem menuItem;

			writer.WriteFullBeginTag("CiscoIPPhoneMenu");

			writer.WriteFullBeginTag("Title");
			writer.Write(title);
			writer.WriteEndTag("Title");

			writer.WriteFullBeginTag("Prompt");
			writer.Write(prompt);
			writer.WriteEndTag("Prompt");

			foreach(object item in items) {
				if(item is MenuItem) {
					menuItem = (MenuItem) item;

					writer.WriteFullBeginTag("MenuItem");

					writer.WriteFullBeginTag("Name");
					writer.Write(menuItem.Name);
					writer.WriteEndTag("Name");

					writer.WriteFullBeginTag("URL");
					writer.Write(menuItem.Uri);
					writer.WriteEndTag("URL");

					writer.WriteEndTag("MenuItem");
				}
			}

			writer.WriteEndTag("CiscoIPPhoneMenu");
		}
	}
}