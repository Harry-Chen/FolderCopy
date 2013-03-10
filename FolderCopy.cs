using System;
using System.IO;

namespace com.harry
{
    class FolderCopy
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("USAGE : foldercopy.exe SOURCE DESTINATION\nThis program will copy all files and directories in SOURCE to DESTINATION\nVER 0.1 Harry Chen 2013/03/10\n\nPress Enter to continue...");
                Console.ReadLine();
                Environment.Exit(-1);
            }
            else if (!Directory.Exists(args[0]) || !Directory.Exists(args[1]))
            {
                Console.WriteLine("ERROR: sourcce or destination does not exist or can not access");
                Environment.Exit(-1);
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                copyDir(args[0],args[1]);
                Console.ResetColor();
            }

        }

        private static void copyDir(string source, string destination)
        {
            string[] sourceFiles = Directory.GetFiles(source);
            foreach (string s in sourceFiles)
            {
                copyFile(source, s.Substring(source.Length+1), destination);
                
            }

            Console.WriteLine("================================================");

            string[] sourceDirs = Directory.GetDirectories(source);
            foreach (string sourceDir in sourceDirs)
            {
                string dirName=sourceDir.Substring(source.Length + 1);
                string destDir = destination + "\\" + dirName;
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.White;
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                    Console.WriteLine("Creating and entering " + dirName);
                }
                else
                {
                    Console.WriteLine("Entering " + dirName);
                }
                Console.ResetColor();
                copyDir(sourceDir, destDir);
                //TODO 递归
            }
        }

        private static void copyFile(string sourceDir,string fileName, string destDir)
        {
            string sourceFile = sourceDir + "\\" + fileName;
            string destFile = destDir + "\\" + fileName;
            if (File.Exists(destFile))
            {
                int diff = File.GetLastWriteTime(sourceFile).CompareTo(File.GetLastWriteTime(destFile));
                if (diff == 0)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(fileName + " is the same in BOTH dir!");
                    Console.ResetColor();
                }
                else if (diff > 0)
                {
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.ForegroundColor = ConsoleColor.White;
                    File.Copy(sourceFile, destFile, true);
                    Console.WriteLine(fileName + " is overwrittern by the newer version!");
                    Console.ResetColor();
                }
                else if (diff < 0)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(fileName + " in DESTINATION dir is newer than that in SOURCE dir! ");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                File.Copy(sourceFile, destFile);
                Console.WriteLine(fileName + " is copied!");
                Console.ResetColor();
            }
            
        }
    }
}
