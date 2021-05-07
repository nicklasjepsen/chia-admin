using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiaAdmin.Service
{
    public static class StringArrayExtensions
    {
        public static string GrabLine(this string[] input, string searchString)
        {
            return input.First(s => s.Contains(searchString)).Replace(searchString, "");
        }
    }
}
