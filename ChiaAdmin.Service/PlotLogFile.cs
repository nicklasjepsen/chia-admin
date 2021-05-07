using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiaAdmin.Service
{
    internal class PlotLogFile
    {
        internal string[] TempDirectories { get; set; }
        internal string Id { get; set; }
        internal int PlotSize { get; set; }
        internal Storage MemoryBuffer { get; set; }
        internal int Buckets { get; set; }
        internal int Threads { get; set; }
        internal int StripeSize { get; set; }
        internal List<PlotPhase> Phases { get; set; } = new List<PlotPhase>();
        public Storage TempSpaceUsed { get; set; }
        public Storage FinalFileSize { get; set; }
        public double TotalTime { get; set; }
        public double Cpu { get; set; }
        public string TempFinalFilePath { get; set; }
        public string FinalPlotPath { get; set; }
    }

    internal class Storage
    {
        internal double TempSpaceUsed { get; set; }
        internal string TempSpaceUnit { get; set; }
    }

    internal class PlotPhase
    {
        internal DateTime StartTime { get; set; }
        internal DateTime? EndTime { get; set; }
        internal double ElapsedTime { get; set; }
        internal double Cpu { get; set; }
    }
}
