using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using clickConterModule.Entity;

namespace clickConterModule.Repository
{
    public interface IJoinTrackerLogRepo
    {
        void Create(JoinTrackerDaily joinTrackerDaily);
        void Delete(string uuid);
        List<JoinTrackerDaily> GetByTime(uint joinNumber, string day, string time);
        List<JoinTrackerDaily> GetByTime(uint joinNumber, DateTime from, DateTime to);
        List<JoinTrackerDaily> GetByTime(DateTime from, DateTime to);
    }
}