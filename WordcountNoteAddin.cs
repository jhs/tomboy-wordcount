/* This file is part of Tomboy-Wordcount.
 *
 * Tomboy-Wordcount is free software; you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License,
 * version 3, as published by the Free Software Foundation.
 *
 * Tomboy-Wordcount is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License in the file agpl-3.0.txt
 * for more details.
 *
 * You should have received a copy of the GNU Affero General
 * Public License along with Tomboy-Wordcount; if not, write to the Free
 * Software Foundation, Inc., 59 Temple Place, Suite 330,
 * Boston, MA 02111-1307 USA.
 */

using System;
using System.Collections;

using Mono.Unix;

using Tomboy;

namespace Tomboy.Wordcount
{
	public class WordcountNoteAddin : NoteAddin
	{
		Gtk.ImageMenuItem menu_item;

		public override void Initialize ()
		{
                        menu_item = new Gtk.ImageMenuItem(Catalog.GetString("Word count"));
			menu_item.Image = new Gtk.Image (Gtk.Stock.JumpTo, Gtk.IconSize.Menu);  /* TODO: correct this */
			menu_item.Activated += OnMenuItemActivated;
			menu_item.Show ();

			AddPluginMenuItem (menu_item);
		}

		public override void Shutdown ()
		{
		}

		public override void OnNoteOpened ()
		{
		}

		void OnMenuItemActivated (object sender, EventArgs args)
		{
                        Logger.Log("Word count activated!");
		}

		WordcountMenuItem [] GetWordcountMenuItems ()
		{
			ArrayList items = new ArrayList ();

			string search_title = Note.Title;
			string encoded_title = XmlEncoder.Encode (search_title.ToLower ());

			// Go through each note looking for
			// notes that link to this one.
			foreach (Note note in Note.Manager.Notes) {
				if (note != Note // don't match ourself
				                && CheckNoteHasMatch (note, encoded_title)) {
					WordcountMenuItem item =
					        new WordcountMenuItem (note, search_title);

					items.Add (item);
				}
			}

			items.Sort ();

			return items.ToArray (typeof (WordcountMenuItem)) as WordcountMenuItem [];
		}

		bool CheckNoteHasMatch (Note note, string encoded_title)
		{
			string note_text = note.XmlContent.ToLower ();
			if (note_text.IndexOf (encoded_title) < 0)
				return false;

			return true;
		}
	}
}
