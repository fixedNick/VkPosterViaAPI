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
            using (var sw = new System.IO.StreamWriter(logPath))
            {
                sw.WriteLine(text);
            }
        }
    }
}
