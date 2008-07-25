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
		Gtk.Menu menu;
		bool submenu_built;

		public override void Initialize ()
		{
			submenu_built = false;

			menu = new Gtk.Menu ();
			menu.Hidden += OnMenuHidden;
			menu.ShowAll ();
			menu_item = new Gtk.ImageMenuItem (
			        Catalog.GetString ("Word count"));
			menu_item.Image = new Gtk.Image (Gtk.Stock.JumpTo, Gtk.IconSize.Menu);
			menu_item.Submenu = menu;
			menu_item.Activated += OnMenuItemActivated;
			menu_item.Show ();
			AddPluginMenuItem (menu_item);
		}

		public override void Shutdown ()
		{
			// The following two lines are required to prevent the plugin
			// from leaking references when the plugin is disabled.
			menu.Hidden -= OnMenuHidden;
			menu_item.Activated -= OnMenuItemActivated;
		}

		public override void OnNoteOpened ()
		{
		}

		void OnMenuItemActivated (object sender, EventArgs args)
		{
			if (submenu_built == true)
				return; // submenu already built.  do nothing.

			UpdateMenu ();
		}

		void OnMenuHidden (object sender, EventArgs args)
		{
			// FIXME: Figure out how to have this function be called only when
			// the whole Tools menu is collapsed so that if a user keeps
			// toggling over the "What links here?" menu item, it doesn't
			// keep forcing the submenu to rebuild.

			// Force the submenu to rebuild next time it's supposed to show
			submenu_built = false;
		}

		void UpdateMenu ()
		{
			//
			// Clear out the old list
			//
			foreach (Gtk.MenuItem old_item in menu.Children) {
				menu.Remove (old_item);
			}

			//
			// Build a new list
			//
			foreach (WordcountMenuItem item in GetWordcountMenuItems ()) {
				item.ShowAll ();
				menu.Append (item);
			}

			// If nothing was found, add in a "dummy" item
			if (menu.Children.Length == 0) {
				Gtk.MenuItem blank_item = new Gtk.MenuItem (Catalog.GetString ("(none)"));
				blank_item.Sensitive = false;
				blank_item.ShowAll ();
				menu.Append (blank_item);
			}

			submenu_built = true;
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
