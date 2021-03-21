using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace clickConterModule.Entity
{
    public class JoinTrackerDaily
    {
        public string UUID { get; set; }
        public uint JoinNumber { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public uint Quantity { get; set; }

        public JoinTrackerDaily()
        {

        }
    }
}