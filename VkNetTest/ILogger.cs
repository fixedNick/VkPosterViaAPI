using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkNetTest
{
    interface ILogger
    {
        string logPath { get; }
        void Print(string text);
    }
}
