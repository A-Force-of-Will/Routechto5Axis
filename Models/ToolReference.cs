using Mastercam.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutechToFiveAxis.Models
{
    public class SizeBooleans
    {
        public bool EighthExists { get; set; }
        public bool QuarterExists { get; set; }
        public bool Three_EigthExists { get; set; }
        public bool HalfExists { get; set; }
        public bool Three_QuarterExists { get; set; }
        public bool TwentyMMExists { get; set; }
        public bool VFoldExists { get; set; }

    }

    public class ToolReference
    {
        

        public string Name { get; set; }

        public Tool Tool { get; set; }
        /// <summary>
        /// Constructor for initializing values for tool reference
        /// </summary>
        /// <param name="name">The name you want to give the tool</param>
        /// <param name="tool">The tool you wish to store</param>
        public ToolReference(string name, Tool tool)
        {
            this.Name = name;
            this.Tool = tool;
        }
    }
}
