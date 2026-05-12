using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Formats.Asn1;

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
        process.StartInfo.FileName = "git";
        process.StartInfo.Arguments = "branch";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        
        process.Start();

        string temp = process.StandardOutput.ReadToEnd();
        string[] branches = temp
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
                    if(branches[j].StartsWith("* "))
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Print(branches[j] + " <\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Print(branches[j] + " <\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    if(branches[j].StartsWith("* "))
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Print(branches[j] + " <\n");
                        Console.ResetColor();
                    } 
                    else
                    {
                        Print(branches[j] + "\n");   
                    }
                }
            }
            Print("Press enter to confirm.");

            key = Console.ReadKey(true);
            if(key.Key == ConsoleKey.DownArrow) i++;
            else if(key.Key == ConsoleKey.UpArrow) i--;
            if(i >= size) i = 0;
            else if(i < 0) i = size - 1;

        } while(key.Key != ConsoleKey.Enter);

        Console.Clear();

        return Regex.Replace(branches[i], @"[\s*]", "");
    }

    static string Branch()
    {
        Process process = new();
        process.StartInfo.FileName = "git";
        process.StartInfo.Arguments = "branch";
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
            query = $"pull origin {branch} --rebase";
        }
        else if(i == 2) query = "merge --abort";
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
            query = $"pull origin {branch}";
        }
        proc.StartInfo.FileName = "git";
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
        proc.StartInfo.FileName = "git";
        proc.StartInfo.Arguments = $"fetch origin {branch}";
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
        proc.StartInfo.FileName = "git";
        proc.StartInfo.Arguments = $"merge origin/{branch}";
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
        proc.StartInfo.FileName = "git";
        proc.StartInfo.Arguments = $"add \"{file}\"";
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
        proc.StartInfo.FileName = "git";
        proc.StartInfo.Arguments = $"commit -m \"{msg}\"";
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
        proc.StartInfo.FileName = "git";
        proc.StartInfo.Arguments = $"push -u origin {branch}";
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
        proc.StartInfo.FileName = "git";
        proc.StartInfo.Arguments = $"checkout {branch}";
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
            int command_size = args[0].Length - 1;
            string commands = args[0].Replace("-", "");
            int argument_size = args.Length - 2;
            string[] arguments = args;
            int index = argument_size;
            

            foreach(char com in commands)
            {   
                if(com == 'a')
                {
                    try
                    {
                        string arg = arguments[index];
                        if(Add(arg))
                        {
                            Print("Add file successful\n");
                            index++;
                        }
                        else
                        {
                            throw new Exception("Add file failed");
                        }
                    } 
                    catch(Exception e)
                    {
                        if(e is IndexOutOfRangeException)
                        {
                            if(Add("."))
                            {
                                Print("Add file successful\n");
                                index++;
                                continue;
                            } 
                            else
                            {
                                throw new Exception("Add file failed");
                            }
                        }
                        PrintErr();
                        Print(e.Message);
                    }
                }
                else if(com == 'c')
                {
                    try
                    {
                        string arg = arguments[index];
                        if(Commit(arg))
                        {
                            index++;
                            Print("Commit successful\n");
                            continue;
                        }
                        else
                        {
                            throw new Exception("Commit failed");
                        }
                    } 
                    catch(Exception e)
                    {
                        if(e is IndexOutOfRangeException)
                        {
                            if(Commit("New update"))
                            {
                                Print("Commit successful\n");
                                index++;
                                continue;
                            }
                            else
                            {
                                throw new Exception("Commit failed");
                            }
                        }
                        PrintErr();
                        Print(e.Message);
                    }
                }
                else if(com == 'p')
                {
                    try
                    {
                        string arg = arguments[index];
                        if(Push(arg))
                        {
                            index++;
                            Print("Push successful\n");
                            continue;
                        }
                        else
                        {
                            throw new Exception("Push failed");
                        }
                    } 
                    catch(Exception e)
                    {
                        if(e is IndexOutOfRangeException)
                        {
                            if(Push(Branch()))
                            {
                                Print("Push successful\n");
                                index++;
                                continue;
                            }
                            else
                            {
                                throw new Exception("Push failed");
                            }
                        } 
                        PrintErr();
                        Print(e.Message);
                    }
                }
                else if(com == 'f')
                {
                    try
                    {
                        string arg = arguments[index];
                        if(Fetch(arg))
                        {
                            index++;
                            Print("Fetch successful\n");
                            continue;
                        }
                        else
                        {
                            throw new Exception("Fetch failed\n");
                        }
                    } 
                    catch(Exception e)
                    {
                        if(e is IndexOutOfRangeException)
                        {
                            if(Fetch(SelectBranch()))
                            {
                                Print("Fetch successful\n");
                                index++;
                                continue;
                            }
                            else
                            {
                                throw new Exception("Fetch failed\n");
                            }
                        }
                        PrintErr();
                        Print(e.Message);
                    }
                }
                else if(com == 'm')
                {
                    try
                    {
                        string arg = arguments[index];
                        if(Merge(arg))
                        {
                            Print("Merge successful\n");
                            index++;
                            continue;
                        }
                        else
                        {
                            throw new Exception("Merge failed");
                        }
                    } 
                    catch(Exception e)
                    {
                        if(e is IndexOutOfRangeException)
                        {
                            if(Merge(SelectBranch()))
                            {
                                Print("Merge successful");
                                index++;
                                continue;
                            }
                            else
                            {
                                throw new Exception("Merge failed");
                            }
                        }
                        PrintErr();
                        Print(e.Message);
                    }
                }
                else if(com == 'u')
                {
                    try
                    {
                        string arg = arguments[index];
                        if(Use(arg))
                        {
                            Print("Checkout successful");
                            index++;
                            continue;
                        }
                        else
                        {
                            throw new Exception("Checkout failed");
                        }
                    } 
                    catch(Exception e)
                    {
                        if(e is IndexOutOfRangeException)
                        {
                            if (Use(SelectBranch()))
                            {
                                Print("Checkout successful");
                                index++;
                                continue;
                            }
                            else
                            {
                                throw new Exception("Checkout failed");
                            }
                        }
                        PrintErr();
                        Print(e.Message);
                    }
                }
                else if(com == 'g')
                {
                    try
                    {
                        string arg = arguments[index];
                        if(Pull(arg))
                        {
                            Print("Pull succesful\n");
                            index++;
                            continue;
                        }
                        else
                        {
                            throw new Exception("Pull failed");
                        }
                    } 
                    catch(Exception e)
                    {
                        if(e is IndexOutOfRangeException)
                        {
                            if(Pull(SelectBranch()))
                            {
                                Print("Pull successful");
                                index++;   
                                continue;
                            }
                            else
                            {
                                throw new Exception("Pull failed");
                            }
                        }
                        PrintErr();
                        Print(e.Message);
                    }
                }
                else if(com == 'A')
                {
                    try
                    {
                        string arg = arguments[index];
                        if(Use(Branch()))
                        {
                            string file = arg;
                            if(Add(file))
                            {
                                index++;
                                string comment = arguments[index];
                                if(Commit(comment))
                                {
                                    index++;
                                    string branch = arguments[index];
                                    if(Push(branch))
                                    {
                                        Print("Update successful\n");
                                        index++;
                                        continue;
                                    }
                                    else
                                    {
                                        Print("Push failed\n");
                                        throw new Exception("");
                                    }
                                }
                                else
                                {
                                    Print("Commit failed\n");
                                    throw new Exception("");
                                }
                            }
                            else
                            {
                                Print("Add file failed\n");
                                throw new Exception("");
                            }
                        } 
                        else
                        {
                            Print("Use failed\n");
                            throw new Exception("");
                        }
                    } 
                    catch(Exception e)
                    {
                        if(e is IndexOutOfRangeException)
                        {
                            if(Use(Branch()))
                            {
                                string file = ".";
                                if(Add(file))
                                {
                                    index++;
                                    Print("Enter commit message: ");
                                    string comment = Console.ReadLine() ?? "New update";
                                    if(Commit(comment))
                                    {
                                        index++;
                                        string branch = arguments[index];
                                        if(Push(branch))
                                        {
                                            Print("Update successful\n");
                                            index++;
                                            continue;
                                        }
                                        else
                                        {
                                            Print("Push failed\n");
                                            throw new Exception("");
                                        }
                                    }
                                    else
                                    {
                                        Print("Commit failed\n");
                                        throw new Exception("");
                                    }
                                }
                                else
                                {
                                    Print("Add file failed\n");
                                    throw new Exception("");
                                }
                            } 
                            else
                            {
                                Print("Use failed\n");
                                throw new Exception("");
                            }
                        }
                        PrintErr();
                        Print(e.Message);
                    }
                }
            }
        }
    }
}