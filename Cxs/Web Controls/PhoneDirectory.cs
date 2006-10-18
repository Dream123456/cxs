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

using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cxs.WebControls
{
	public class PhoneDirectory : System.Web.UI.WebControls.WebControl
	{
		private string title;
		private string prompt;
		private IList entries;

		public class DirectoryEntry
		{
			private string name;
			private string telephone;

			public DirectoryEntry(string name, string telephone)
			{
				this.name = name;
				this.telephone = telephone;
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

			public string Telephone
			{
				get {
					return(telephone);
				}

				set {
					telephone = value;
				}
			}
		}

		public PhoneDirectory()
		{
			title = string.Empty;
			prompt = string.Empty;

			entries = new ArrayList();
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

		public IList Entries
		{
			get {
				return(entries);
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			DirectoryEntry directoryEntry;

			writer.WriteFullBeginTag("CiscoIPPhoneDirectory");

			writer.WriteFullBeginTag("Title");
			writer.Write(title);
			writer.WriteEndTag("Title");

			writer.WriteFullBeginTag("Prompt");
			writer.Write(prompt);
			writer.WriteEndTag("Prompt");

			foreach(object entry in entries) {
				if(entry is DirectoryEntry) {
					directoryEntry = (DirectoryEntry) entry;

					writer.WriteFullBeginTag("DirectoryEntry");

					writer.WriteFullBeginTag("Name");
					writer.Write(directoryEntry.Name);
					writer.WriteEndTag("Name");

					writer.WriteFullBeginTag("Telephone");
					writer.Write(directoryEntry.Telephone);
					writer.WriteEndTag("Telephone");

					writer.WriteEndTag("DirectoryEntry");
				}
			}

			writer.WriteEndTag("CiscoIPPhoneDirectory");
		}
	}
}