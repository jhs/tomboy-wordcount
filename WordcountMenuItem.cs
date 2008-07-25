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
using Tomboy;

namespace Tomboy.Wordcount
{
	public class WordcountMenuItem : Gtk.ImageMenuItem, System.IComparable
	{
		Note note;
		string title_search;

		static Gdk.Pixbuf note_icon;

		static WordcountMenuItem ()
		{
			note_icon = GuiUtils.GetIcon ("note", 16);
		}

public WordcountMenuItem (Note note, string title_search) :
		base (note.Title)
		{
			this.note = note;
			this.title_search = title_search;
			this.Image = new Gtk.Image (note_icon);
		}

		protected override void OnActivated ()
		{
			if (note == null)
				return;

			// Show the title of the note
			// where the user just came from.
			NoteFindBar find = note.Window.Find;
			find.ShowAll ();
			find.Visible = true;
			find.SearchText = title_search;

			note.Window.Present ();
		}

		public Note Note
		{
			get {
				return note;
			}
		}

		// IComparable interface
		public int CompareTo (object obj)
		{
			WordcountMenuItem other_item = obj as WordcountMenuItem;
			return note.Title.CompareTo (other_item.Note.Title);
		}
	}
}
