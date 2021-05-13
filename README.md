Lexer example C# - Notepad++ plugin
===================================

This is an example of a Lexer, it is meant to explore how to setup a C# project with a custom lexer code.
The syntax highlighting is based on user configuration in a window which can change at run-time.
It should apply highlighting to EDIFACT data files.
This is just an example code project, it's not meant to be a full EDIFACT editor.

Lexer datafiles
---------------
The lexer example is slightly different compared to a Lexer for a programming or scripting files.
But the code project can technically be used to create any lexer in C#.

For data files, the highlighting rules can change depending on user input, and depending on the data files.
For example the segment names, separator character, etc. can differ, and the user can change these though a configuration window.

Goal
----
The goal is to create a Lexer plugin project, as a starting point or template for plugin creation.

It is meant for finding out how to set it up and apply it [my other plugin](https://github.com/BdR76/CSVLint).
Also see this [forum post](https://community.notepad-plus-plus.org/topic/21124/c-adding-a-custom-styler-or-lexer-in-c-for-scintilla-notepad/6)

See mockup below, the plug-in doesn't work like this yet, it is a goal mock-up screenshot

![preview screenshot](/mockup_preview.png?raw=true "Lexer plug-in example, goal preview")

See also
--------
This plugin is based on the [NotepadPlusPlusPluginPack.Net](https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net),
with help from members of the [Notepad++ development community](https://community.notepad-plus-plus.org/topic/21124/c-adding-a-custom-styler-or-lexer-in-c-for-scintilla-notepad/11)

History
-------
08-may-2021 - first test release

BdR©2021 Free to use - send questions or comments: Bas de Reuver - bdr1976@gmail.com
