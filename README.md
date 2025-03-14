# Explorlight

The ExplorlightSln directory implements a recursive Windows Explorer.

![image](https://github.com/user-attachments/assets/2450e55c-17c5-4049-b5f0-f0158a85a8ba)

Simply select a target in the tree view to display all files (and their sizes) in the directory and its subdirectories.

A filter allows you to retrieve only specific files, and you can copy the results to the clipboard or export them to a file.

It is also possible to open a target (file or directory) or its containing folder.

The primary objective of this application is to perform an inventory.

## Use

![image](https://github.com/user-attachments/assets/4f09ccb7-d4e2-4396-96e4-e720b6a64725)


### Commands

- A : When checked, display a separator between each group of directory files 
- B1: When checked, apply filter (B2) when searching for files
- B2: Filter to apply when searching for files (see details below)
- C : Local folder tree, select your search target by left-clicking on it here (see details below)
- D : Click here to export the search result to a file (this will open a file save dialog)
- E : Click here to copy the search result to the clipboard
- F : Click here to re-search files for the current target

#### Filter

It is possible to specify multiple filters by separing them with a `:`.

When multiple filters are provided, a file must match each of them to appear in the search result.

The first filter only applies to the file name (not the full path) and can contain wildcards (* and ?), but it does not support regular expressions.


The following filters apply to the full file path but do not handle wildcards (nor regular expressions).

Tip : to search on directory paths, regardless of file names, use this kind of filter `*:{directory name filter}`

#### Folder tree

The folder tree is not updated in real time ; the list of its subfolders is refreshed each time a directory node is expanded.

Left-click on a folder to select it. When a new folder is selected, a file search is triggered.

Middle-click on a folder to open it in windows explorer.

Right-click on a folder to open its parent in windows explorer.


### Search results

![image](https://github.com/user-attachments/assets/883ea483-8816-43a0-bad5-10e8ea8a7142)

The search results area displays the found files, based on the search parameters.

Middle-click on a result file to open it as windows explorer would.

Right-click on a result file to open its containing folder in windows explorer.


### Duplicate file names

Once a search is done, you can extract the list of duplicate file names by clicking on the relevant button.

This will open the Duplicate File Names pop-up window, displaying the files, grouped by file name, and the number of groups.

![image](https://github.com/user-attachments/assets/478d4271-00d5-4988-b504-867776d29cdf)

From now on, you can :
- A : Copy the list of duplicate file names (and their instances) to the clipboard 
- B : Remove instances that no longer exist
- C : Select instances you want to delete
- D : Delete selected instances

During a cleanup, instances that no longer exist are removed. If a file name has only one instance after this cleanup, it will be removed from the list of duplicate file names.

Note that a cleanup is automatically performed after the "Delete selected" operation.
