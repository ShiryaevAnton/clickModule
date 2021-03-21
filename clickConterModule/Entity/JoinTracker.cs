using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace clickConterModule.Entity
{
    public class JoinTracker
    {
        public string UUID { get; set; }
        public uint JoinNumber { get; set; }
        public string TimeStamp { get; set; }

        public JoinTracker()
        {

        }
    }
}