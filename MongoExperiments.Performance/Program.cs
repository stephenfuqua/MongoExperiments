using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MongoExperiments.Performance
{
    class Program
    {
        static void Main(string[] args)
        {
            var iterations = 10;
            //var fileName = @"..\..\..\MongoExperiments.CS\bin\debug\MongoExperiments.cs.exe";
            var fileName = @"""C:\Program Files (x86)\nodejs\node.exe""";
            //var arguments = "";
            var arguments = @"..\..\..\MongoExperiments.JS\app.js";
            var reportName = ".\\Mongo.JS.10.csv";

            var start = new ProcessStartInfo
            {
                Arguments = arguments,
                FileName = fileName,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };

            var report = new PerformanceReport
            {
                FileName = reportName
            };

            for (int i = 0; i < iterations; i++)
            {
                var result = new PerformanceResult
                {
                    RunNumber = i,
                    FileName = fileName,
                    Arguments = arguments
                };

                using (var proc = Process.Start(start))
                {
                    do
                    {
                        if (!proc.HasExited)
                        {
                            proc.Refresh();

                            result.PeakPagedMemory = proc.PeakPagedMemorySize64;
                            result.PeakVirtualMemory = proc.PeakVirtualMemorySize64;
                            result.PeakWorkingSet = proc.PeakWorkingSet64;
                        }
                    }
                    while (!proc.WaitForExit(10));

                    result.ProcessorTime = (long) Math.Round(proc.TotalProcessorTime.TotalMilliseconds,0);
                    result.ExitCode = proc.ExitCode;
                }
                report.Add(result);
            }

            report.WriteFile();

        }

    }

    public class PerformanceResult
    {
        public int RunNumber { get; set; }
        public string FileName { get; set; }
        public string Arguments { get; set; }
        public int ExitCode { get; set; }
        public long PeakPagedMemory { get; set; }
        public long PeakWorkingSet { get; set; }
        public long PeakVirtualMemory { get; set; }
        public long ProcessorTime { get; set; }

        public override string ToString() => "\{RunNumber}, \{FileName}, \{Arguments}, \{ExitCode}, \{PeakPagedMemory}, \{PeakVirtualMemory}, \{PeakWorkingSet}, \{ProcessorTime}";
    }

    public class PerformanceReport
    {
        private List<PerformanceResult> _results = new List<PerformanceResult>();

        public string FileName { get; set; }

        public PerformanceReport Add(PerformanceResult result)
        {
            _results.Add(result);
            return this;
        }

        public void WriteFile()
        {
            var header = "Run Number, filename, arguments, exit code, paged memory, virtual memory, working set, processor time";
            

            var builder = new StringBuilder()
                                .AppendLine(header)
                                .AppendDetailLines(_results)
                                .AppendLine(GenerateStatistics());

            File.WriteAllText(FileName, builder.ToString());
        }

        private string GenerateStatistics()
        {
            var footer = "Averages, , , , {0}, {1}, {2}, {3}";

            long iterations = _results.Count();
            var avgPaged = CalculateAverageFor(x => x.PeakPagedMemory);
            var avgVirtual = CalculateAverageFor(x => x.PeakVirtualMemory);
            var avgWorking = CalculateAverageFor(x => x.PeakWorkingSet);
            var avgtime = CalculateAverageFor(x => x.ProcessorTime);

            return string.Format(footer, avgPaged, avgVirtual, avgWorking, avgtime);
        }
        
        private string CalculateAverageFor(Func<PerformanceResult, long> expression)
        {
            var avg = _results.Where(x => x.ExitCode == 0)
                           .Select(expression)
                           .Sum() / (long)_results.Count();

            return avg.ToString();
        }
    }

    public static class ReportStringBuilder
    {
        public static StringBuilder AppendLine(this StringBuilder builder, string line)
        {
            builder.AppendLine(line);
            return builder;
        }


        public static StringBuilder AppendDetailLines(this StringBuilder builder, IEnumerable<PerformanceResult> results)
        {
            foreach (var item in results)
            {
                builder.AppendLine(item.ToString());
            }

            return builder;
        }
    }
}
