using System;
using System.Threading.Tasks;

namespace ChiaAdmin.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var plotLogReader = new PlotLogReader("SampleLogs");
            var plotLogs = await plotLogReader.Read();
        }
    }
}
