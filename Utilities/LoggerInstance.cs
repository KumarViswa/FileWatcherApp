using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherApp.Utilities
{
    public class LoggerInstance
    {
        public static readonly ILogger Logger = new LoggerConfiguration()
        .WriteTo.Console()
            .WriteTo.File(ConstantFilePaths.LogFilePath)
        .CreateLogger();
    }
}
