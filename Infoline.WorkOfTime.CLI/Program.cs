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
            var sourceProjectPath = @"C:\Github\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server";
            var destinationProjectPath = @"C:\Github\Infoline-Bilgi-Teknolojileri\WorkOfTime-Web-Server-Datacenter";
            var sourceProjectName = "Infoline.WorkOfTime";
            var destinationProjectName = "Infoline.DataCenter";
            var ext = new string[] { ".sln", ".csproj", ".cs", ".config", ".cshtml", ".asax" };
            var directories = Directory.GetDirectories(sourceProjectPath, "*", System.IO.SearchOption.AllDirectories).ToList();
            directories.Add(sourceProjectPath);
            foreach (var directory in directories.OrderBy(a => a))
            {
                var directoryInfo = new DirectoryInfo(directory);
                var newDirectory = directory.Replace(sourceProjectPath, destinationProjectPath).Replace(sourceProjectName, destinationProjectName);
                Directory.CreateDirectory(newDirectory);

                Console.WriteLine(directory + " => " + newDirectory);
                var files = directoryInfo.GetFiles();
                foreach (var fileInfo in files)
                {
                    var newFilePath = Path.Combine(newDirectory, fileInfo.Name.Replace(sourceProjectName, destinationProjectName));
                    Console.WriteLine(fileInfo.FullName + " => " + newFilePath);
                    fileInfo.CopyTo(newFilePath);
                    if (ext.Contains(fileInfo.Extension))
                    {
                        string text = File.ReadAllText(newFilePath, System.Text.Encoding.UTF8);
                        text = text.Replace(sourceProjectName, destinationProjectName);
                        File.WriteAllText(newFilePath, text, System.Text.Encoding.UTF8);
                    }
                }
            }
            Console.WriteLine("Bitti");
            Console.ReadLine();
        }
    }
}
