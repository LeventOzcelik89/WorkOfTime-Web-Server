using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.CLI
{
    class Program
    {
        static void Main(string[] args)
        {

            var path = @"D:\TFS\infoline\AkilliProjeler\Infoline.WorkOfTime";
            var newPath = @"D:\TFS\infoline\AkilliProjeler\Infoline.TEB";
            var key = "Infoline.WorkOfTime";
            var replacer = "Infoline.TEB";
            var ext = new string[] { ".sln", ".csproj", ".cs", ".config", ".cshtml", ".asax" };
            var directories = Directory.GetDirectories(path, "*", System.IO.SearchOption.AllDirectories).ToList();
            directories.Add(path);
            foreach (var directory in directories.OrderBy(a => a))
            {
                var directoryInfo = new DirectoryInfo(directory);
                var newDirectory = directory.Replace(path, newPath).Replace(key, replacer);
                Directory.CreateDirectory(newDirectory);

                Console.WriteLine(directory + " => " + newDirectory);
                var files = directoryInfo.GetFiles();
                foreach (var fileInfo in files)
                {
                    var newFilePath = Path.Combine(newDirectory, fileInfo.Name.Replace(key, replacer));
                    Console.WriteLine(fileInfo.FullName + " => " + newFilePath);
                    fileInfo.CopyTo(newFilePath);
                    if (ext.Contains(fileInfo.Extension))
                    {
                        string text = File.ReadAllText(newFilePath, System.Text.Encoding.UTF8);
                        text = text.Replace(key, replacer);
                        File.WriteAllText(newFilePath, text, System.Text.Encoding.UTF8);
                    }
                }
            }
            Console.WriteLine("Bitti");
            Console.ReadLine();




        }
    }
}
