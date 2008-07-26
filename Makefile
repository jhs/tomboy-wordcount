TOMBOY_DIR=$(HOME)/.tomboy/addins

Wordcount.dll: WordcountNoteAddin.cs Wordcount.addin.xml
	gmcs -debug -out:Wordcount.dll -define:DEBUG -target:library -pkg:tomboy-addins -r:Mono.Posix \
	WordcountNoteAddin.cs -resource:Wordcount.addin.xml \
	`pkg-config --libs tomboy-addins gnome-sharp-2.0`

install: Wordcount.dll
	cp Wordcount.dll $(TOMBOY_DIR)

clean:
	rm -vf Wordcount.dll Wordcount.dll.mdb
