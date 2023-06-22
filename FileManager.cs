using CsvHelper;
using FileWatcherApp.EfContext;
using FileWatcherApp.Model;
using FileWatcherApp.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherApp
{
    public class FileManager
    {
        public static void FileWatcher()
        {
            LoggerInstance.Logger.Information("Started Runnning Program");        

            using FileSystemWatcher watcher = new FileSystemWatcher(ConstantFilePaths.folderPath);
            watcher.NotifyFilter = NotifyFilters.FileName;
            watcher.Filter = ConstantFilePaths.targetFileName;
            watcher.Created += (sender, e) => OnFileCreated(e, ConstantFilePaths.targetFolderPath, ConstantFilePaths.folderPath);
            watcher.EnableRaisingEvents = true;
            Console.WriteLine($"Monitoring file: {ConstantFilePaths.targetFileName} in folder: {ConstantFilePaths.folderPath}");
            Console.WriteLine("Press 'q' to quit.");
            while (Console.Read() != 'q') ;
        }
        static void OnFileCreated(FileSystemEventArgs e, string targetFolderPath, string folderPath)
        {
            LoggerInstance.Logger.Information("File Has be Added in Folder");
            string sourceFilePath = e.FullPath;
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string targetFileName = $"{Path.GetFileNameWithoutExtension(sourceFilePath)}_{timestamp}{Path.GetExtension(sourceFilePath)}";
            string targetFilePath = Path.Combine(targetFolderPath, targetFileName);            
            File.Copy(sourceFilePath, targetFilePath);
            LoggerInstance.Logger.Information("File Copied to Archieve Folder");
            SaveDataToTable();
            DeleteOldFiles(folderPath);

            Console.WriteLine($"File saved to: {targetFilePath}");
        }
        static void DeleteOldFiles(string folderPath)
        {
            LoggerInstance.Logger.Information("File Has Deleted In Folder");
            DirectoryInfo directory = new DirectoryInfo(folderPath);
            FileInfo[] files = directory.GetFiles("*.csv");
            foreach (FileInfo file in files)
            {
                file.Delete();
                Console.WriteLine($"Deleted file: {file.Name}");
            }
        }

        static void SaveDataToTable()
        {
            using (var context = new MyDbContext())
            {
               

                using (var reader = new StreamReader(ConstantFilePaths.csvFilePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Trade>().ToList();
                    LoggerInstance.Logger.Information("Converted CSV to List");
                    foreach (var record in records)
                    {

                        var data = context.TradeDatas.Where(x => x.TradeID == record.TradeID).ToList();
                        if (data.Any())
                        {
                            context.TradeDatas.RemoveRange(data);

                        }
                        context.TradeDatas.Add(record);
                        context.SaveChanges();
                    }

                }

                LoggerInstance.Logger.Information("File Saved Into Database");


            }
        }
    }
}
