Lexer example C# - Notepad++ plugin
===================================

This is an example of a Lexer, it is meant to explore how to setup a C# project with custom lexer code.
It applies highlighting to EDIFACT data files. The syntax highlighting colors and keywords are based on user configuration in the [EdifactLexer.xml](/config/EdifactLexer.xml) file.
This is just an example code project, it's not meant to be a full EDIFACT editor.

[EdifactLexer.xml](/config/EdifactLexer.xml)

Lexer datafiles
---------------
The lexer example is slightly different compared to a Lexer for a programming or scripting files.
But the code project can technically be used to create any lexer in C#.

For data files, the highlighting rules can be the same for all data files, like segment names in Edifact which can be set as keywords in [Settings > Style configurator](/config/EdifactLexer.xml).
For other data files like character separated data, the separator character can be differ per file and the user has to somehow set it at run-time.

Goal
----
The goal is to create a Lexer plugin project, as a starting point or template for plugin creation.

It is meant for finding out how to set it up and apply it [my other plugin](https://github.com/BdR76/CSVLint).
Also see this [forum post](https://community.notepad-plus-plus.org/topic/21124/c-adding-a-custom-styler-or-lexer-in-c-for-scintilla-notepad/6)

See screenshot below, the plugin works and this is what the current plug-in project can do

![preview screenshot](/mockup_preview.png?raw=true "Lexer plug-in example, preview")

How to install
--------------
You can try the file `EdifactLexer.dll`. In your \Notepad++\plugins\ folder,
create a new folder `EdifactLexer` and place the .dll file there, so:

* copy the file [.\LexerPlugin\NppManagedPluginDemo\bin\Release\EdifactLexer.dll](/LexerPlugin/NppManagedPluginDemo/bin/Release/)
* to new folder .\Program Files (x86)\Notepad++\plugins\EdifactLexer\EdifactLexer.dll

For the 64-bit version it is the same, except the output file is in the
[Release-x64](/LexerPlugin/NppManagedPluginDemo/bin/Release-x64/) folder and Notepad++ is
in the `.\Program Files\Notepad++\` folder.

Also copy the file [EdifactLexer.xml](./config/) to your `.\Program Files\Notepad++\plugins\Config\` folder, this file contains the settings for the  text styling.

See also
--------
This plugin is based on the [NotepadPlusPlusPluginPack.Net](https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net),
with help from members of the [Notepad++ development community](https://community.notepad-plus-plus.org/topic/21124/c-adding-a-custom-styler-or-lexer-in-c-for-scintilla-notepad/11)

History
-------
21-may-2021 - additional code
08-may-2021 - first test release

BdR©2021 Free to use - send questions or comments: Bas de Reuver - bdr1976@gmail.com
