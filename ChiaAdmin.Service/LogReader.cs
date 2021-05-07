using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiaAdmin.Service
{
    internal class PlotLogReader 
    {
        private readonly string logDir;

        internal class Match
        {
            public const string TempDir = "Starting plotting progress into temporary dirs: ";
            internal const string Id = "ID: ";
            internal const string PlotSize = "Plot size is: ";
            internal const string MemoryBufferSize = "Buffer size is: ";
            internal const string Buckets = " buckets";
            internal const string ThreadsOfStripe = "threads of stripe size ";
            internal const string Phase1Start = "Starting phase 1/4: Forward Propagation into tmp files... ";
        }

        public PlotLogReader(string logDir)
        {
            this.logDir = logDir;
        }

        internal async Task<List<PlotLogFile>> Read()
        {
            // Plotting log file consists of the following sections:
            // 0. Info from the process
            // 1. Start plotting configuration
            // 2. Phase 1
            // 3. Phase 2
            // 4. Phase 3
            // 5. Phase 4 + summary

            var results = new List<PlotLogFile>();
            foreach (var enumerateFile in Directory.EnumerateFiles(logDir, "*.log"))
            {
                var plot = new PlotLogFile();
                results.Add(plot);
                var fileContent = await File.ReadAllLinesAsync(enumerateFile, Encoding.UTF8);
                plot.TempDirectories = fileContent.GrabLine(Match.TempDir).Split(" and ");
                plot.Id = fileContent.GrabLine(Match.Id);
                plot.PlotSize = int.Parse(fileContent.GrabLine(Match.PlotSize));
                var tmpMem = fileContent.GrabLine(Match.MemoryBufferSize);
                plot.MemoryBuffer = new Storage
                {
                    TempSpaceUsed = double.Parse(new string(tmpMem.Where(char.IsDigit).ToArray())),
                    TempSpaceUnit = new string(tmpMem.Where(char.IsLetter).ToArray())
                };
                
                plot.Buckets =
                    int.Parse(new string(fileContent.GrabLine(Match.Buckets).Where(char.IsDigit).ToArray()));
                var tmpThreads = fileContent.GrabLine(Match.ThreadsOfStripe).Split(" ");
                plot.Threads = int.Parse(tmpThreads[1]);
                plot.StripeSize = int.Parse(tmpThreads[2]);

                if (fileContent.Any(s => s.Contains(Match.Phase1Start)))
                {
                    var phase = new PlotPhase
                    {
                        StartTime =  DateTime.ParseExact(fileContent.GrabLine(Match.Phase1Start),
                            "ddd MMM  d hh:mm:ss yyyy", CultureInfo.InvariantCulture)
                    };
                    plot.Phases.Add(phase);

                }
            }

            return results;
        }
    }
}
