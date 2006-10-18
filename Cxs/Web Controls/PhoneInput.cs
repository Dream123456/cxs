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
	public class PhoneInput : System.Web.UI.WebControls.WebControl
	{
		private string title;
		private string prompt;
		private string uri;
		private IList items;

		public class InputItem
		{
			private string name;
			private string parameter;
			private string flags;
			private string value;

			public InputItem(string name, string parameter, string flags, string value)
			{
				this.name = name;
				this.parameter = parameter;
				this.flags = flags;
				this.value = value;
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

			public string Parameter
			{
				get {
					return(parameter);
				}

				set {
					parameter = value;
				}
			}

			public string Flags
			{
				get {
					return(flags);
				}

				set {
					flags = value;
				}
			}

			public string Value
			{
				get {
					return(value);
				}

				set {
					this.value = value;
				}
			}
		}

		public PhoneInput()
		{
			title = string.Empty;
			prompt = string.Empty;
			uri = string.Empty;

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

		public string Uri
		{
			get {
				return(uri);
			}

			set {
				uri = value;
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
			InputItem inputItem;

			writer.WriteFullBeginTag("CiscoIPPhoneInput");

			writer.WriteFullBeginTag("Title");
			writer.Write(title);
			writer.WriteEndTag("Title");

			writer.WriteFullBeginTag("Prompt");
			writer.Write(prompt);
			writer.WriteEndTag("Prompt");

			writer.WriteFullBeginTag("URL");
			writer.Write(uri);
			writer.WriteEndTag("URL");

			foreach(object item in items) {
				if(item is InputItem) {
					inputItem = (InputItem) item;

					writer.WriteFullBeginTag("InputItem");

					writer.WriteFullBeginTag("DisplayName");
					writer.Write(inputItem.Name);
					writer.WriteEndTag("DisplayName");

					writer.WriteFullBeginTag("QueryStringParam");
					writer.Write(inputItem.Parameter);
					writer.WriteEndTag("QueryStringParam");

					writer.WriteFullBeginTag("InputFlags");
					writer.Write(inputItem.Flags);
					writer.WriteEndTag("InputFlags");

					// writer.WriteFullBeginTag("DefaultValue");
					// writer.Write(inputItem.Value);
					// writer.WriteEndTag("DefaultValue");

					writer.WriteEndTag("InputItem");
				}
			}

			writer.WriteEndTag("CiscoIPPhoneInput");
		}
	}
}