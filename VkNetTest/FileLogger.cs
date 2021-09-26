using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkNetTest
{
    class FileLogger : ILogger
    {
        public string logPath => "log.txt";

        public void Print(string text)
        {
            if (System.IO.File.Exists(logPath) == false)
                System.IO.File.Create(logPath);

            System.IO.File.AppendAllText(logPath, text + Environment.NewLine);
        }
    }
}
