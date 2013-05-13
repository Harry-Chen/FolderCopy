using System;
using System.IO;

namespace com.harry
{
    class FolderCopy
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2) 
                /* 参数数目不正确 */
            {
                Console.WriteLine("USAGE : foldercopy.exe SOURCE DESTINATION\nThis program will copy all files and directories in SOURCE to DESTINATION\nVER 0.1 Harry Chen 2013/05/13\n\nPress Enter to continue...");
                Console.ReadLine(); //避免提示马上消失
                Environment.Exit(-1);
            }
            else if (!Directory.Exists(args[0]) || !Directory.Exists(args[1]))
                /* 源目录或者目标目录不存在,或者无权访问
                 会考虑以后目标目录不存在的时候直接创建 */
            {
                Console.WriteLine("ERROR: sourcce or destination does not exist or can not access.");
                Console.ReadLine();
                Environment.Exit(-1);
            }
            else
                /* 一切正常,开始工作 */
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue; //正常工作时候的背景颜色
                copyDir(args[0],args[1]); //进入第一层的目录复制
                Console.ResetColor(); //恢复默认,以防接下来Console还是这个颜色
            }

        }

        private static void copyDir(string source, string destination)
        /// <summary>
        /// 复制一个目录以及其中所有子目录中的文件,保持原结构
        /// 实质是递归调用本身,以及对copyFile的调用
        /// </summary>
        /// <param name="source">源目录绝对路径</param>
        /// <param name="destination">目标目录绝对路径</param>
        {
            string[] sourceFiles = Directory.GetFiles(source); //得到源目录中的文件列表
            foreach (string s in sourceFiles) //遍历所有文件
            {
                copyFile(source, s.Substring(source.Length+1), destination); //调用copyFile复制到目标目录中
                
            }

            Console.WriteLine("================================================"); //复制完文件以后显示分隔符

            string[] sourceDirs = Directory.GetDirectories(source); //得到源目录中的子目录列表
            foreach (string sourceDir in sourceDirs) //遍历所有子目录
            {
                string dirName=sourceDir.Substring(source.Length + 1); //得到子目录名(不含路径)
                string destDir = destination + "\\" + dirName; //要在目标目录创建的新目录的绝对路径
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.White; //颜色更改
                if (!Directory.Exists(destDir)) //如果要创建的目录不存在
                {
                    Directory.CreateDirectory(destDir); //创建该目录
                    Console.WriteLine("Creating and entering " + dirName); //提示目录的创建
                }
                else //如果目录存在
                {
                    Console.WriteLine("Entering " + dirName); //进入该目录的提示
                }
                /* 这里没有考虑无法创建的情况,需要补充 
                 * FIXME */ 
                Console.ResetColor(); //恢复颜色
                copyDir(sourceDir, destDir); //在子目录中再次递归调用本方法
            }
        }

        private static void copyFile(string sourceDir,string fileName, string destDir)
        /// <summary>
        /// 复制单个文件
        /// 实质是判断和对System.IO.File.Copy的引用
        /// </summary>
        /// <param name="sourceDir">源文件所处目录的绝对路径</param>
        /// <param name="fileName">源文件名</param>
        /// <param name="destDir">目标目录的绝对路径</param>
        {
            string sourceFile = sourceDir + "\\" + fileName; //得到源文件绝对路径
            string destFile = destDir + "\\" + fileName; //目标文件绝对路径
            if (File.Exists(destFile)) //如果目标文件已存在
            {
                int diff = File.GetLastWriteTime(sourceFile).CompareTo(File.GetLastWriteTime(destFile)); //比较修改时间
                /* 好吧,我承认这么比较有危险
                 * XXX */
                if (diff == 0)
                    /* 如果修改时间相同,提示并且不做改动 */
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(fileName + " is the same in BOTH dir!");
                    Console.ResetColor();
                }
                else if (diff > 0)
                /* 如果源文件较新相同,提示并且覆盖目标文件 */
                {
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.ForegroundColor = ConsoleColor.White;
                    File.Copy(sourceFile, destFile, true);
                    Console.WriteLine(fileName + " is overwrittern by the newer version!");
                    Console.ResetColor();
                }
                else if (diff < 0)
                /* 如果目标文件较新相同,警告并且不做改动 */
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(fileName + " in DESTINATION dir is newer than that in SOURCE dir! ");
                    Console.ResetColor();
                }
            }
            else //如果目标文件不存在,直接复制
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                File.Copy(sourceFile, destFile);
                Console.WriteLine(fileName + " is copied!");
                Console.ResetColor();
            }
            
        }
    }
}
