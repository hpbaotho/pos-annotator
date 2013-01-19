# Created on January 19th, 2013
# Author: bulbul

DESCRIPTION
This is a lightweight tool for annotation of corpus data with POS tags designed for ease of use and speed.

PREREQUISITES FOR RUNNING
 1. .NET Framework 4.5
 2. A tagset file with all the POS tags, one tag per line.(default extension is *.list, see  the provided example file "tagset.list" [1]).
 3. A vertical text file, i.e. one token per line (default extension is *.vrt, see the provided example file "corpus.vrt"). 
 The file can already be preannotated (all of it or just some tokens), in which case the tag must be separated from the token by a tab.  If the tag is in the tagset, it will be copied to the editing interface.

HOW TO USE
1. Click File > Open tagset file... to read in the list of tags. If you try to open a vertical file first, you will get a warning to open the tagset file first. All the tags will be read into a list  area ("POS tags") on the right.
2. Click File > Open vertical file... to read in the tokens. The tokens are displayed in the first column ("Token", which is read only), any pre-existing tags are read into the column on the right. 
3. You will use the right column ("Tag") to add tags. When the vertical file is read in, the list scrolls automatically to the first token without a tag and sets editing focus on the cell in the "Tag" column.
4. To add a tag you can:
- start typing;
- use arrows to pick a tag of the list in the cell;
- use your mouse to pick a tag out of the list in the cell; or
- doubleclick a POS tag in the list on the right.
5. To move between cells in the "Tag" column:
- press Enter to move down by one cell;
- press PageDown to scroll down;
- press PageUp to scroll down;
- when you doubleclick on a POS tag in the "POS tags" list area, the tag is add to the tag column and focus jumps to the next empty cell.
6. To save an annotated file (even not all tokens have been assigned a tag), click File > Save annotated file ... A file is saved with the same format as the initial vertical text file, i.e. token + tab + tag. 
Additionally, every time the application is closed, a backup file with the same structure is created in the same directory (extension *.processed).

Notes:
[1] See "A Universal Part-of-Speech Tagset" by Slav Petrov, Dipanjan Das and Ryan McDonald for more details: http://arxiv.org/abs/1104.2086