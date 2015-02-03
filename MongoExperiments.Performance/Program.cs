using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MongoExperiments.Performance
{
    class Program
    {
        static void Main(string[] args)
        {

            var start = new ProcessStartInfo
            {
                Arguments = "",
                FileName = @"..\..\..\MongoExperiments.CS\bin\debug\MongoExperiments.cs.exe",
                //Arguments = @"..\..\..\MongoExperiments.JS\app.js",
                //FileName = @"""C:\Program Files (x86)\nodejs\node.exe""",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };

            var exitCode = 0;
            long peakPagedMem = 0,
                peakWorkingSet = 0,
                peakVirtualMem = 0;
            var processorTime = 0d;

            using (var proc = Process.Start(start))
            {
                do
                {
                    if (!proc.HasExited)
                    {
                        proc.Refresh();

                        peakPagedMem = proc.PeakPagedMemorySize64;
                        peakVirtualMem = proc.PeakVirtualMemorySize64;
                        peakWorkingSet = proc.PeakWorkingSet64;
                    }
                }
                while (!proc.WaitForExit(10));

                processorTime = proc.TotalProcessorTime.TotalMilliseconds;
                exitCode = proc.ExitCode;
            }

            Console.WriteLine("Exit code: " + exitCode);
            Console.WriteLine("Paged Memory: " + peakPagedMem);
            Console.WriteLine("Virtual Memory: " + peakVirtualMem);
            Console.WriteLine("Working Set: " + peakWorkingSet);
            Console.WriteLine("Processor Time (ms): " + processorTime);
            Console.Read();
        }
    }
}
