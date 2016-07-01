using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace dirtree
{
    class Program
    {
        static void Main(string[] args)
        {
            var arg = string.Join(" ", args).Trim();
            if (string.IsNullOrWhiteSpace(arg))
            {
                Console.WriteLine("Usage: dirtree PATH");
                Console.WriteLine("");
                Console.WriteLine("  -E FolderName    Exclude certain folders from being traversed.");
                return;
            }



#if DEBUG
            arg += " -E test3";
            arg += " -E node_modules";
#endif

            //extract exclude directories
            string[] stringSeparators = new string[] { " -E" };
            var ExcludeDirectories = new List<string>();
            var keys = arg.Split(stringSeparators, StringSplitOptions.None);
            var dir = ""; //rebuilding k
            foreach (var k in keys)
            {
                if (k.StartsWith(" "))
                {
                    ExcludeDirectories.Add(k.Substring(1).Trim());
                }
                else
                {
                    dir = dir + k;
                }
            }

            if (dir.StartsWith("./") || dir.StartsWith(".\\"))
            {
                dir = Environment.CurrentDirectory + dir.Substring(2, dir.Length - 2).Replace("/", "\\");
            }

            Console.WriteLine(" Directory of " + dir);
            //if (ExcludeDirectories.Count > 0)
            //{
            //    Console.WriteLine("");
            //    Console.WriteLine("Excluding the Following Directories:");
            //    foreach (var ed in ExcludeDirectories)
            //    {
            //        Console.WriteLine("  " + ed + "\\");
            //    }
            //    Console.WriteLine("");
            //}

            if (Directory.Exists(dir))
            {
                DirSearch(dir, dir, "", ExcludeDirectories);
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("Error:  Specified directory does not exist.");
            }


#if DEBUG
            Console.ReadLine();
#endif
        }


        static void DirSearch(string sDir, string rootDir, string indent = "", List<string> ExcludeDirectories = null)
        {
            try
            {
                if (sDir.Equals(rootDir))
                {
                    foreach (string f in Directory.GetFiles(sDir))
                    {
                        Console.WriteLine(indent + "|-" + f.Replace(sDir, ""));
                    }
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    var dName = d.Replace(sDir.TrimEnd('\\') + "\\", "");
                    
                    if (ExcludeDirectories != null && ExcludeDirectories.Contains(dName))
                    {
                        Console.WriteLine(indent + "|-" + dName + "\\ [Excluded]");
                    }
                    else
                    {
                        Console.WriteLine(indent + "|-" + dName + "\\");
                        foreach (string f in Directory.GetFiles(d))
                        {
                            Console.WriteLine(indent + "  |-" + f.Replace(d + "\\", ""));
                        }
                        DirSearch(d, rootDir, indent + "  ", ExcludeDirectories);
                    }
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }
    }
}
