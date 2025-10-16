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
        ['-a', '-c', '-p', '-f', '-m', '-A', '-P', '-u']

### Option 
        - -a - type the name of the file that you wanted to add changes into.
        - -c - type the message for the commit.
        - -p - you need to type the branch to where you will push your work. This pushes your updates to the remote branch.
        - -f - you need to type the branch that you want to fetch. This fetches updates from the remote branch.
        - -m - you need to type the branch that you want to merge with. This merges your local repository with the updates from the remote branch.
        - -A - you need to declare what file name to add, commit message, branch name to push to. this will add file, commit, and push using a single command.
        - -P - you need to type the branch that you want to fetch and merge. This fetches updates and merges it from the remote branch to your local repository.
        - -u - you need to type the branch that you want to use. This uses the branch version and makes you edit the content of that branch without harming or editing the other branches.
        - -b - you need to type the file to add, comment, branch to push to, and branch that should be updated too. this will add, commit, push, use the other branch, fetch and push to the branch. 

### Usage:
        - update -a [filename] / . (to add all changes)
        - update -c [comment/message]
        - update -p [branch]
        - update -f [branch]
        - update -m [branch]
        - update -A [filename] [comment] [to_branch]
        - update -P [branch]
        - update -u [branch]
        - update -b [filename/.] [comment] [branch] [toBranch]

### Proper usage:
        - update -f [branch] -m [branch]
        - update -a [filename/.] -c [comment/message] -p [branch]
        - update -A [filename] [comment] [to_branch]
        - update -P [branch]
        - update -u [branch]
        - update -b [filename/.] [comment] [branch] [toBranch]

#### Important note:
Make sure to fetch and merge before you work on any file.<br>
Push everytime you finish a file<br>