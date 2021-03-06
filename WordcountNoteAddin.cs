//
// WordcountNoteAddin.cs: Count Tomboy note contents a la /usr/bin/wc
//
// Author:
//   Jason Smith (jhs@proven-corporation.com)
//
// Copyright (C) 2008 Proven Corporation Co., Ltd., Thailand
// (http://www.proven-corporation.com)
//

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
		}

		public override void OnNoteOpened ()
		{
			// Add the menu item when the window is created.
			menu_item = new Gtk.ImageMenuItem (
				Catalog.GetString ("Word count"));
			menu_item.Image = new Gtk.Image (Gtk.Stock.Index,
				Gtk.IconSize.Menu);
			menu_item.Activated += OnMenuItemActivated;
			menu_item.AddAccelerator ("activate", Window.AccelGroup,
				(uint) Gdk.Key.o, Gdk.ModifierType.ControlMask,
				Gtk.AccelFlags.Visible);

			menu_item.Show ();
			AddPluginMenuItem (menu_item);
		}

		public override void Shutdown ()
		{
			if (menu_item != null)
				menu_item.Activated -= OnMenuItemActivated;
		}

		static Regex xml_tag    = new Regex ("<.*?>");
		static Regex boundaries = new Regex (@"[\s;,-]+");

		// Return the word count of an arbitrary multi-line text string.
		private int CountWords (string source)
		{
			// The idea is to condense all whitespace to one space,
			// remove the XML tags, and then count what's left.
			string final = source;

			final = xml_tag.Replace(final, "");
			final = final.Trim();

			return boundaries.Split (final).Length;
		}

		void OnMenuItemActivated (object sender, EventArgs args)
		{
			int lines, words, chars;// Like wc.

			string plain_text = xml_tag.Replace(Note.Data.Text, "");

			// The title and space beneath it do not count.
			string title = String.Format("{0}\n\n", Note.Title);
			chars = plain_text.Length - title.Length;

			// The word count is straightforward, but remember to
			// omit the note title.
			words = CountWords (plain_text);
			words -= CountWords (Note.Title);

			lines = plain_text.Split ('\n').Length;
			lines -= 2; // Omit the Title and blank line beneath it.

			ShowStats (Note.Title, lines, words, chars);
		}

		private Gtk.Window stat_win;

		private void ShowStats (string title, int lines, int words,
		                        int chars)
		{
			Logger.Log ("Wordcount: {0}: {1} {2} {3}", title, lines,
				words, chars);

			stat_win = new Gtk.Window (String.Format (
				"{0} - Word count", title));
			stat_win.Resize (200, 100);

			Gtk.VBox box = new Gtk.VBox (false, 0);

			Gtk.Label stat_label = new Gtk.Label ();
			stat_label.Text = String.Format (
				"{0}\n\nLines: {1}\nWords: {2}\nCharacters: {3}\n",
				title, lines, words, chars);

			box.PackStart (stat_label, true, true, 0);

			Gtk.Button ok = new Gtk.Button ("Close");
			ok.Clicked += new EventHandler (OkHandler);
			box.PackStart (ok, true, true, 0);

			stat_win.Add (box);
			stat_win.ShowAll ();
		}

		private void OkHandler (object o, EventArgs args)
		{
			stat_win.Destroy ();
		}
	}
}

// vim: sts=0 ts=8 sw=8 noet
