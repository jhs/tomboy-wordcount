TOMBOY_DIR=$(HOME)/.tomboy/addins

Wordcount.dll:
	gmcs -debug -out:Wordcount.dll -define:DEBUG -target:library -pkg:tomboy-addins -r:Mono.Posix \
	WordcountMenuItem.cs WordcountNoteAddin.cs -resource:Wordcount.addin.xml \
	`pkg-config --libs tomboy-addins gnome-sharp-2.0`

install:
	cp Wordcount.dll $(TOMBOY_DIR)

clean:
	rm -vf Wordcount.dll Wordcount.dll.mdb
