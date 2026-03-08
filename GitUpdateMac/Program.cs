using System.Text.RegularExpressions;
using System.Diagnostics;

class Update
{
    static ConsoleKeyInfo key;
    static void Print(string msg)
    {
        Console.Write(msg);
    }

    static void SetChoice(string method, int index)
    {
        Console.Clear();
        if(method == "pull")
        {
            if(index == 1)
            {
                Print("Choose a method for pull\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                Print("--rebase <=\n");
                Console.ForegroundColor = ConsoleColor.White;
                Print("--abort\n");
                Print("None\n");
                Print("Press enter to confirm.\n");
            }
            else if(index == 2)
            {
                Print("Choose a method for pull\n");
                Print("--rebase\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                Print("--abort <=\n");
                Console.ForegroundColor = ConsoleColor.White;
                Print("None\n");
                Print("Press enter to confirm.\n");
            } 
            else if(index == 3)
            {
                Print("Choose a method for pull\n");
                Print("--rebase\n");
                Print("--abort\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                Print("None <=\n");
                Console.ForegroundColor = ConsoleColor.White;
                Print("Press enter to confirm.\n");
            }
        }        
    }

    static string SelectBranch()
    {
        int i = 0;
        Process process = new();
        process.StartInfo.FileName = "/bin/zsh";
        process.StartInfo.Arguments = "-c \"git branch\"";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        
        process.Start();

        string temp = process.StandardOutput.ReadToEnd();
        string[] branches = Regex.Replace(temp, @"\* ", "")
        .Trim()
        .Split("\n")
        .Select(b => b.Trim())        
        .Where(b => b != string.Empty)
        .ToArray();
        int size = branches.Length;

        do
        {
            Console.Clear();
            Print("----------- Select Branch -----------\n");

            for(int j = 0; j < size; j++)
            {
                if(i == j)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Print(branches[j] + " <\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Print(branches[j] + "\n");
                }
            }
            Print("Press enter to confirm.");

            ConsoleKeyInfo key = Console.ReadKey(true);
            if(key.Key == ConsoleKey.DownArrow) i++;
            else if(key.Key == ConsoleKey.UpArrow) i--;
            if(i >= size) i = 0;
            else if(i < 0) i = size - 1;

        } while(key.Key != ConsoleKey.Enter);

        Console.Clear();

        return branches[i];
    }

    static string Branch()
    {
        Process process = new();
        process.StartInfo.FileName = "/bin/zsh";
        process.StartInfo.Arguments = "-c \"git branch\"";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        
        process.Start();

        string[] branches = process.StandardOutput.ReadToEnd().Split("\n");
        foreach(string branch in branches)
        {
            if(branch.StartsWith("*"))
            {
                return branch.Replace("* ", "");
            }
        }
        return "";
    }
    static bool Pull(string to_branch)
    {
        Process proc = new Process();
        string query = "";
        int i = 1;
        SetChoice("pull", i);
        do {
            key = Console.ReadKey(true);
            if(key.Key == ConsoleKey.DownArrow) i++;
            else if(key.Key == ConsoleKey.UpArrow) i--;
            if(i > 3) i = 1;
            else if(i < 1) i = 3;
            SetChoice("pull", i);
        } while(key.Key != ConsoleKey.Enter);
        if(i == 1)
        {
            string branch;
            if(to_branch.Length < 1)
            {
                do
                {
                    Console.Clear();
                    Print("Enter branch name: ");   
                    branch = Console.ReadLine() ?? "";
                } while(branch == "");
            }
            else
            {
                branch = to_branch;
            }
            query = $"-c \"git pull origin {branch} --rebase\"";
        }
        else if(i == 2) query = "-c \"git merge --abort\"";
        else if(i == 3)
        {
            string branch;
            if(to_branch.Length < 1)
            {
                do
                {
                    Console.Clear();
                    Print("Enter branch name: ");   
                    branch = Console.ReadLine() ?? "";
                } while(branch == "");
            }
            else
            {
                branch = to_branch;
            }
            query = $"-c \"git pull origin {branch}\"";
        }
        proc.StartInfo.FileName = "/bin/zsh";
        proc.StartInfo.Arguments = query;
        proc.StartInfo.UseShellExecute = false;
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

    static bool Fetch(string branch)
    {
        if(branch == null)
        {
            Print("Missing branch name\n");
            return false;
        }
        Process proc = new Process();
        proc.StartInfo.FileName = "/bin/zsh";
        proc.StartInfo.Arguments = $"-c \"git fetch origin {branch}\"";
        proc.StartInfo.UseShellExecute = false;
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

    static bool Merge(string branch)
    {
        if(branch == null)
        {
            Print("Missing branch name\n");
            return false;
        }
        Process proc = new Process();
        proc.StartInfo.FileName = "/bin/zsh";
        proc.StartInfo.Arguments = $"-c \"git merge origin/{branch}\"";
        proc.StartInfo.UseShellExecute = false;
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

    static bool Add(string file)
    {
        if(file == null)
        {
            Print("Missing filename\n");
            return false;
        }
        Process proc = new Process();
        proc.StartInfo.FileName = "/bin/zsh";
        proc.StartInfo.Arguments = $"-c \"git add '{file}'\"";
        proc.StartInfo.UseShellExecute = false;
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

    static bool Commit(string msg)
    {
        if(msg == null)
        {
            Print("Missing filename\n");
            return false;
        }
        Process proc = new Process();
        proc.StartInfo.FileName = "/bin/zsh";
        proc.StartInfo.Arguments = $"-c \"git commit -m '{msg}'\"";
        proc.StartInfo.UseShellExecute = false;
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

    static bool Push(string branch)
    {
        if(branch == null)
        {
            Print("Missing branch name\n");
            return false;
        }
        Process proc = new Process();
        proc.StartInfo.FileName = "/bin/zsh";
        proc.StartInfo.Arguments = $"-c \"git push -u origin  \"{branch}\"\"";
        proc.StartInfo.UseShellExecute = false;
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

    static bool Use(string branch)
    {
        if(branch == null)
        {
            Print("Missing branch name\n");
            return false;
        }
        Process proc = new Process();
        proc.StartInfo.FileName = "/bin/zsh";
        proc.StartInfo.Arguments = $"-c \"git checkout \"{branch}\"\"";
        proc.StartInfo.UseShellExecute = false;
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

    static void PrintErr()
    {
        Print(@"Syntax:
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
    - -g - you will be asked if you want to do a --rebase, --abort, or None . if you chose --rebase, you will be asked for input of what branch that you want to pull. if --abort, you will
          undo the merge that you have done earlier. if None, you will be asked what branch do you want to pull. 
Usage:
    - up -a [filename] / . (to add all changes)
    - up -c [comment/message]
    - up -p [branch]
    - up -f [branch]
    - up -m [branch]
    - up -A [filename] [comment] [branch]
    - up -P [branch]
    - up -b [filename/.] [comment] [branch] [toBranch]
    - up -g [branch]
Proper usage:
    - up -f [branch] -m [branch]
    - up -a [filename/.] -c [comment/message] -p [branch]
    - up -A [filename] [comment] [branch]
    - up -P [branch]
    - up -u [branch]
    - up -b [filename/.] [comment] [branch] [toBranch]
    - up -g 

Important note:
Make sure to fetch and merge before you work on any file.
Push everytime you finish a file
");
    }

    static void Main(String[] args)
    {
        if(args.Length < 1)
        {
            PrintErr();
        }
        else
        {
            for(int i = 0; i < args.Length; i++)
            {
                if(args[i] == "-a")
                {
                    if(args[i+1] != null)
                    {
                        if(Add(args[i+1]))
                        {
                            Print("Add successful\n");   
                        } 
                        else
                        {
                            Print("Add unsuccessful\n");
                        }
                    }
                    else
                    {
                        Print("Syntax error.\n");
                        PrintErr();
                        break;
                    }
                } 
                if(args[i] == "-c")
                {
                    if(args[i+1] != null)
                    {
                        if(Commit(args[i+1]))
                        {
                            Print("Commit successful\n");   
                        } 
                        else
                        {
                            Print("Commit unsuccessful\n");
                        }
                    }
                    else
                    {
                        Print("Syntax error.\n");
                        PrintErr();
                        break;
                    }
                }
                if(args[i] == "-f")
                {
                    string branch;
                    try
                    {
                        branch = args[i+1];
                    }
                    catch
                    {
                        branch = SelectBranch();
                    }
                    if(Fetch(branch))
                    {
                        Print("Fetch successful\n");   
                    } 
                    else
                    {
                        Print("Fetch unsuccessful\n");
                    }
                }
                if(args[i] == "-m")
                {
                    string branch;
                    try
                    {
                        branch = args[i+1];
                    }
                    catch
                    {
                        branch = SelectBranch();
                    }
                    if(Merge(branch))
                    {
                        Print("Commit successful\n");   
                    } 
                    else
                    {
                        Print("Commit unsuccessful\n");
                    }
                }
                if(args[i] == "-p")
                {
                    string branch;
                    try
                    {
                        branch = args[i+1];
                    }
                    catch
                    {
                        branch = Branch();
                    }
                    if(Push(branch))
                    {
                        Print("Push successful\n");   
                    } 
                    else
                    {
                        Print("Push unsuccessful\n");
                    }
                }
                if(args[i] == "-P")
                {
                    string branch;
                    try
                    {
                        branch = args[i+1];
                    }
                    catch
                    {
                        branch = Branch();
                    }
                    if(Fetch(branch))
                    {
                        if(Merge(branch))
                        {
                            Print("Pull successful\n");   
                        } 
                        else
                        {
                            Print("Merge unsuccessful\n");
                        }
                    } 
                    else
                    {
                        Print("Fetch unsuccessful\n");
                    };
                }
                if(args[i] == "-A")
                {
                    string branch;
                    try
                    {
                        branch = args[i+3];
                    }
                    catch
                    {
                        branch = Branch();
                    }
                    if(Use(branch))
                    {
                        if(Add(args[i+1]))
                        {
                            if(Commit(args[i+2]))
                            {
                                if(Push(branch))
                                {
                                    Print("Push successful\n");   
                                } 
                                else
                                {
                                    Print("Push unsuccessful\n");
                                }
                            } 
                            else
                            {
                                Print("Commit unsuccessful\n");
                            }
                        } 
                        else
                        {
                            Print("Add unsuccessful\n");
                        }
                    } 
                    else
                    {
                        Print("Use unsuccessful");
                    }
                }
                if(args[i] == "-b")
                {
                    string branch;
                    string to_branch;
                    try
                    {
                        branch = args[i+3];
                        to_branch = args[i+4];
                    }
                    catch
                    {
                        branch = Branch();
                        to_branch = SelectBranch();
                    }
                    if(Use(branch))
                    {
                        if(Add(args[i+1]))
                        {
                            if(Commit(args[i+2]))
                            {
                                if(Push(branch))
                                {
                                    if(Use(to_branch))
                                    {
                                        if(Fetch(branch))
                                        {
                                            if(Merge(branch))
                                            {
                                                if(Push(to_branch))
                                                {
                                                    if(Use(branch))
                                                    {
                                                        Print("Use successful\n");
                                                    }
                                                    else
                                                    {
                                                        Print("Use unsuccessful\n");
                                                    }
                                                }
                                                else
                                                {
                                                    Print("Push unsuccessful\n");
                                                }
                                            }
                                            else
                                            {
                                                Print("Merge unsuccessful\n");
                                            }
                                        } 
                                        else
                                        {
                                            Print("Fetch unsuccessful\n");
                                        }  
                                    } 
                                    else
                                    {
                                        Print("Push unsuccessful\n");
                                    }
                                } 
                                else
                                {
                                    Print("Push unsuccessful\n");
                                }
                            } 
                            else
                            {
                                Print("Commit unsuccessful\n");
                            }
                        } 
                        else
                        {
                            Print("Add unsuccessful\n");
                        }
                    }
                    else
                    {
                        Print("Use unsuccessful");
                    }
                }
                if(args[i] == "-g")
                {
                    string branch;
                    try
                    {
                        branch = args[i+1];
                    }
                    catch
                    {
                        branch = "";
                    }
                    if(Pull(branch))
                    {
                        Print("Pull successful");
                    } 
                    else
                    {
                        Print("Pull unsuccessful\n");
                    }
                }
                if(args[i] == "-u")
                {
                    string branch;
                    try
                    {
                        branch = args[i+1];
                    }
                    catch
                    {
                        branch = SelectBranch();
                    }
                    if(Use(branch))
                    {
                        Print("Use successful");
                    } 
                    else
                    {
                        Print("Use unsuccessful\n");
                    }
                }
            }
        }
    }
}