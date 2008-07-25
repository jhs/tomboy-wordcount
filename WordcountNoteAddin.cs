/* This file is part of Tomboy-Wordcount.
 *
 * Tomboy-Wordcount is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License,
 * version 3, as published by the Free Software Foundation.
 *
 * Tomboy-Wordcount is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License in the file gpl-3.0.txt
 * for more details.
 *
 * You should have received a copy of the GNU General
 * Public License along with Tomboy-Wordcount; if not, write to the Free
 * Software Foundation, Inc., 59 Temple Place, Suite 330,
 * Boston, MA 02111-1307 USA.
 */

using System;
using System.Collections;
using System.Text.RegularExpressions;

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
			menu_item.Activated += OnButtonClicked;
			menu_item.Show ();

			AddPluginMenuItem (menu_item);
		}

		public override void Shutdown ()
		{
		}

		public override void OnNoteOpened ()
		{
		}

                static Regex wordSplitter = new Regex("\\s+");

                /* Return the word count of an arbitrary multi-line text string. */
                private int CountWords (string s)
                {
                    return wordSplitter.Split(s).Length;
                }

		void OnButtonClicked (object sender, EventArgs args)
		{
                        int lines, words, chars;    // Like wc.

                        // For now, the char count simply omits the top-level XML tag.
                        string header = "<note-content version=\"0.1\">";
                        string footer = "</note-content>";

                        chars = Note.Data.Text.Length - header.Length - footer.Length;
                        string realText = Note.Data.Text.Substring(header.Length, chars);

                        /* The word count is straightforward, but remember to omit the note title. */
                        words = CountWords(realText);
                        words -= CountWords(Note.Title);

                        lines = realText.Split('\n').Length;
                        lines -= 2; // Omit the Title and blank line beneath it.

                        // Logger.Log("{0}:\n{1}", Note.Title, realText);
                        Logger.Log("{0}: {1} {2} {3}", Note.Title, lines, words, chars);
		}
	}
}
