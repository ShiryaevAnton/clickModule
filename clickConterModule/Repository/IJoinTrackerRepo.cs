using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using clickConterModule.Entity;

namespace clickConterModule.Repository
{
    public interface IJoinTrackerRepo
    {
        void Create(JoinTracker joinTracker);
        void DeleteAll();
        List<JoinTracker> GetByTime(uint joinNumber, DateTime from, DateTime to);
    }
}