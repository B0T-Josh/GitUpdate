using System;
using System.Diagnostics;

class Update
{
    static ConsoleKeyInfo key;
    static void print(string msg)
    {
        Console.Write(msg);
    }

    static void setChoice(string method, int index)
    {
        Console.Clear();
        if(method == "pull")
        {
            if(index == 1)
            {
                print("Choose a method for pull\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                print("--rebase <=\n");
                Console.ForegroundColor = ConsoleColor.White;
                print("--abort\n");
                print("None\n");
                print("Press enter to confirm.\n");
            }
            else if(index == 2)
            {
                print("Choose a method for pull\n");
                print("--rebase\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                print("--abort <=\n");
                Console.ForegroundColor = ConsoleColor.White;
                print("None\n");
                print("Press enter to confirm.\n");
            } 
            else if(index == 3)
            {
                print("Choose a method for pull\n");
                print("--rebase\n");
                print("--abort\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                print("None <=\n");
                Console.ForegroundColor = ConsoleColor.White;
                print("Press enter to confirm.\n");
            }
        }        
    }
    static bool pull()
    {
        Process proc = new Process();
        string query = "";
        int i = 1;
        setChoice("pull", i);
        do {
            key = Console.ReadKey(true);
            if(key.Key == ConsoleKey.DownArrow) i++;
            else if(key.Key == ConsoleKey.UpArrow) i--;
            if(i > 3) i = 1;
            else if(i < 1) i = 3;
            setChoice("pull", i);
        } while(key.Key != ConsoleKey.Enter);
        if(i == 1)
        {
            Console.Clear();
            print("Enter branch name: ");
            string branch = Console.ReadLine() ?? "";
            query = $"/c git pull origin {branch}";
        }
        else if(i == 2) query = "/c git merge --abort";
        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = query;
        proc.StartInfo.Verb = "runas";
        try
        {
            proc.Start();
            proc.WaitForExit();
            if(proc.ExitCode == 0) return true;
            else return false;
        } 
        catch
        {
            return false;
        }
    }

    static bool fetch(string branch)
    {
        if(branch == null)
        {
            print("Missing branch name\n");
            return false;
        }
        Process proc = new Process();
        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = $"/c git fetch origin {branch}";
        proc.StartInfo.Verb = "runas";
        try
        {
            proc.Start();
            proc.WaitForExit();
            if(proc.ExitCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    static bool merge(string branch)
    {
        if(branch == null)
        {
            print("Missing branch name\n");
            return false;
        }
        Process proc = new Process();
        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = $"/c git merge origin/{branch}";
        proc.StartInfo.Verb = "runas";
        try
        {
            proc.Start();
            proc.WaitForExit();
            if(proc.ExitCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    static bool add(string file)
    {
        if(file == null)
        {
            print("Missing filename\n");
            return false;
        }
        Process proc = new Process();
        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = $"/c git add \"{file}\"";
        proc.StartInfo.Verb = "runas";
        try
        {
            proc.Start();
            proc.WaitForExit();
            if(proc.ExitCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    static bool commit(string file)
    {
        if(file == null)
        {
            print("Missing filename\n");
            return false;
        }
        Process proc = new Process();
        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = $"/c git commit -m \"{file}\"";
        proc.StartInfo.Verb = "runas";
        try
        {
            proc.Start();
            proc.WaitForExit();
            if(proc.ExitCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    static bool push(string branch)
    {
        if(branch == null)
        {
            print("Missing branch name\n");
            return false;
        }
        Process proc = new Process();
        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = $"/c git push -u origin  \"{branch}\"";
        proc.StartInfo.Verb = "runas";
        try
        {
            proc.Start();
            proc.WaitForExit();
            if(proc.ExitCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    static bool use(string branch)
    {
        if(branch == null)
        {
            print("Missing branch name\n");
            return false;
        }
        Process proc = new Process();
        proc.StartInfo.FileName = "cmd.exe";
        proc.StartInfo.Arguments = $"/c git checkout \"{branch}\"";
        proc.StartInfo.Verb = "runas";
        try
        {
            proc.Start();
            proc.WaitForExit();
            if(proc.ExitCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    static void printErr()
    {
        print(@"Syntax:
    update [command] [option]
Command
    ['-a', '-c', '-p', '-f', '-m', '-A', '-P', '-u', '-b', '-g']
Option
    - -a - type the name of the file that you wanted to add changes into.
    - -c - type the message for the commit.
    - -p - you need to type the branch to where you will push your work. This pushes your updates to the remote branch.
    - -f - you need to type the branch that you want to fetch. This fetches updates from the remote branch.
    - -m - you need to type the branch that you want to merge with. This merges your local repository with the updates from the remote branch.
    - -A - you need to declare what file name to add, commit message, branch name to push to. this will add file, commit, and push using a single command.
    - -P - you need to type the branch that you want to fetch and merge. This fetches updates and merges it from the remote branch to your local repository.
    - -u - you need to type the branch that you want to use. This uses the branch version and makes you edit the content of that branch without harming or editing the other branches.
    - -b - you need to type the file to add, comment, branch to push to, and branch that should be updated too. this will add, commit, push, use the other branch, fetch and push to the branch.
    - -g - you need to type if it is --rebase or --abort by typing 're' or 'ab'. if you'll just do normal pull, just type nothing after. you also must declare a branch to pull from after setting the option.
Usage:
    - update -a [filename] / . (to add all changes)
    - update -c [comment/message]
    - update -p [branch]
    - update -f [branch]
    - update -m [branch]
    - update -A [filename] [comment] [branch]
    - update -P [branch]
    - update -b [filename/.] [comment] [branch] [toBranch]
    - update -g [branch]
Proper usage:
    - update -f [branch] -m [branch]
    - update -a [filename/.] -c [comment/message] -p [branch]
    - update -A [filename] [comment] [branch]
    - update -P [branch]
    - update -u [branch]
    - update -b [filename/.] [comment] [branch] [toBranch]
    - update -g [branch]

Important note:
Make sure to fetch and merge before you work on any file.
Push everytime you finish a file
");
    }

    static void Main(String[] args)
    {
        if(args.Length < 2)
        {
            printErr();
        }
        else
        {
            for(int i = 0; i < args.Length; i++)
            {
                if(args[i] == "-a")
                {
                    if(args[i+1] != null)
                    {
                        if(add(args[i+1]))
                        {
                            print("Add successful\n");   
                        } 
                        else
                        {
                            print("Add unsuccessful\n");
                        }
                    }
                    else
                    {
                        print("Syntax error.\n");
                        printErr();
                        break;
                    }
                } 
                if(args[i] == "-c")
                {
                    if(args[i+1] != null)
                    {
                        if(commit(args[i+1]))
                        {
                            print("Commit successful\n");   
                        } 
                        else
                        {
                            print("Commit unsuccessful\n");
                        }
                    }
                    else
                    {
                        print("Syntax error.\n");
                        printErr();
                        break;
                    }
                }
                if(args[i] == "-f")
                {
                    if(args[i+1] != null)
                    {
                        if(fetch(args[i+1]))
                        {
                            print("Fetch successful\n");   
                        } 
                        else
                        {
                            print("Fetch unsuccessful\n");
                        }
                    }
                    else
                    {
                        print("Syntax error.\n");
                        printErr();
                        break;
                    }
                }
                if(args[i] == "-m")
                {
                    if(args[i+1] != null)
                    {
                        if(merge(args[i+1]))
                        {
                            print("Commit successful\n");   
                        } 
                        else
                        {
                            print("Commit unsuccessful\n");
                        }
                    }
                    else
                    {
                        print("Syntax error.\n");
                        printErr();
                        break;
                    }
                }
                if(args[i] == "-p")
                {
                    if(args[i+1] != null)
                    {
                        if(push(args[i+1]))
                        {
                            print("Push successful\n");   
                        } 
                        else
                        {
                            print("Push unsuccessful\n");
                        }
                    }
                    else
                    {
                        print("Syntax error.\n");
                        printErr();
                        break;
                    }
                }
                if(args[i] == "-u")
                {
                    if(args[i+1] != null)
                    {
                        if(use(args[i+1]))
                        {
                            print("Use successful\n");   
                        } 
                        else
                        {
                            print("Use unsuccessful\n");
                        }
                    }
                    else
                    {
                        print("Syntax error.\n");
                        printErr();
                        break;
                    }
                }
                if(args[i] == "-P")
                {
                    if(args[i+1] != null)
                    {
                        if(fetch(args[i+1]))
                        {
                            if(merge(args[i+1]))
                            {
                                print("Pull successful\n");   
                            } 
                            else
                            {
                                print("Merge unsuccessful\n");
                            }
                        } 
                        else
                        {
                            print("Fetch unsuccessful\n");
                        }
                    } 
                    else
                    {
                        print("Syntax error.\n");
                        printErr();
                        break;
                    }
                }
                if(args[i] == "-A")
                {
                    if(args[i+1] != null)
                    {
                        if(add(args[i+1]))
                        {
                            if(commit(args[i+2]))
                            {
                                if(push(args[i+3]))
                                {
                                    print("Push successful\n");   
                                } 
                                else
                                {
                                    print("Push unsuccessful\n");
                                }
                            } 
                            else
                            {
                                print("Commit unsuccessful\n");
                            }
                        } 
                        else
                        {
                            print("Add unsuccessful\n");
                        }
                    } 
                    else
                    {
                        print("Syntax error.\n");
                        printErr();
                        break;
                    }
                }
                if(args[i] == "-b")
                {
                    if(args[i+1] != null)
                    {
                        if(use(args[i+3]))
                        {
                            if(add(args[i+1]))
                            {
                                if(commit(args[i+2]))
                                {
                                    if(push(args[i+3]))
                                    {
                                        if(use(args[i+4]))
                                        {
                                            if(fetch(args[i+3]))
                                            {
                                                if(merge(args[i+3]))
                                                {
                                                    if(push(args[i+4]))
                                                    {
                                                        if(use(args[i+3]))
                                                        {
                                                            print("Use successful\n");
                                                        }
                                                        else
                                                        {
                                                            print("Use unsuccessful\n");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        print("Push unsuccessful\n");
                                                    }
                                                }
                                                else
                                                {
                                                    print("Merge unsuccessful\n");
                                                }
                                            } 
                                            else
                                            {
                                                print("Fetch unsuccessful\n");
                                            }  
                                        } 
                                        else
                                        {
                                            print("Push unsuccessful\n");
                                        }
                                    } 
                                    else
                                    {
                                        print("Push unsuccessful\n");
                                    }
                                } 
                                else
                                {
                                    print("Commit unsuccessful\n");
                                }
                            } 
                            else
                            {
                                print("Add unsuccessful\n");
                            }
                        }
                        else
                        {
                            print("Use unsuccessful");
                        }
                    } 
                    else
                    {
                        print("Syntax error.\n");
                        printErr();
                        break;
                    }
                }
                if(args[i] == "-g")
                {
                    if(args[i+1] != null)
                    {
                        if(pull())
                        {
                            print("Pull successful");
                        } 
                        else
                        {
                            print("Pull unsuccessful\n");
                        }
                    } 
                    else
                    {
                        print("Syntax error.\n");
                        printErr();
                        break;
                    }
                }
                if(args[i] == "-u")
                {
                    if(args[i+1] != null)
                    {
                        if(use(args[i+1]))
                        {
                            print("Use successful");
                        } 
                        else
                        {
                            print("Use unsuccessful\n");
                        }
                    }
                    else
                    {
                        print("Syntax error.\n");
                        printErr();
                        break;
                    }
                }
            }
        }
    }
}