using System;
using System.Diagnostics;

class Update
{
    static ConsoleKeyInfo key;
    static ProcessStartInfo p = new ProcessStartInfo();
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
            }
            else if(index == 2)
            {
                print("Choose a method for pull\n");
                print("--rebase\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                print("--abort <=\n");
                Console.ForegroundColor = ConsoleColor.White;
                print("None\n");
            } 
            else if(index == 3)
            {
                print("Choose a method for pull\n");
                print("--rebase\n");
                print("--abort\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                print("None <=\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }        
    }
    static bool pull(string branch)
    {
        string query = $"/c git pull origin {branch}";
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
        if(i == 1) query = query + " --rebase";
        else if(i == 2) query = query + " --abort";
        
        p.FileName = "cmd.exe";
        p.Arguments = query;
        p.Verb = "runas";
        try
        {
            Process.Start(p);
        } 
        catch
        {
            return false;
        }

        return true;
    }

    static void Main(String[] args)
    {
        if(pull("main"))
        {
            print("Success");
        }
    }
}