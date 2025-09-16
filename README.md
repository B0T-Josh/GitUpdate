# GitUpdate

## Important note: Study git first

### Requirements:

* git https://github.com/git-for-windows/git/releases/download/v2.51.0.windows.1/Git-2.51.0-64-bit.exe
* Github Desktop https://central.github.com/deployments/desktop/desktop/latest/win32

### Git basic usage:

* git fetch origin //this connects your version to the online remote repository
* git merge main //this merges the repository version to your local repository
* git pull //is a command that fetches and merge with the online remote repository
* git add [file_name] //this loads the changes to be prepared to be committed
* git commit -m [comment] //this saves your changes on your local repository
* git push origin main //this uploads your changes to the online remote repository
* git checkout [branch] //use the branch that was indicated

### Syntax
        update [command] [option]
### Command 
        - ['-a', '-c', '-p', '-f', '-m', '-A']
### Option 
        -a is chosen, type the name of the file that you wanted to add changes into.
        -c is chosen, type the message for the commit.
        -p is chosen, you need to type the branch to where you will push your work. This pushes your updates to the remote branch
        -f is chosen, you need to type the branch that you want to fetch. This fetches updates from the remote branch
        -m is chosen, you need to type the branch that you want to merge with. This merges your local repository with the updates from the remote branch
        -A is chosen, you don't need to type anything after it. This will fetch, merge, add changes, commit and push with one command
### Usage:
        update -a [filename] / . (to add all changes)
        update -a [filename] / . (to add all changes)
        update -c [comment/message]
        update -p [branch]
        update -f [branch]
        update -m [branch]
### Proper usage:
        update -f -m
        update -a [filename/.] -c [comment/message] -p [branch]
        update -A

#### Important note:
Make sure to fetch and merge before you work on any file.<br>
Push everytime you finish a file<br>