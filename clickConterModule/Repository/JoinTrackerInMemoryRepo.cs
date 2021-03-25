using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using clickConterModule.Entity;

namespace clickConterModule.Repository
{
    public class JoinTrackerInMemoryRepo : IJoinTrackerRepo
    {
        List<JoinTracker> joinTrackers;

        public JoinTrackerInMemoryRepo()
        {
            joinTrackers = new List<JoinTracker>();
        }
        public void Create(JoinTracker joinTracker)
        {
            try
            {
                joinTrackers.Add(joinTracker);
            }
            catch(Exception e)
            {
                throw (e);
            }
        }
        public void DeleteAll()
        {
            try
            {
                joinTrackers.RemoveAll(jt => jt != null);
            }
            catch(Exception e)
            {
                throw (e);
            }
        }
        public List<JoinTracker> GetByTime(uint joinNumber, DateTime from, DateTime to)
        {
            try
            {
                List<JoinTracker> joinTrackersTime = joinTrackers.Where(l => l.JoinNumber == joinNumber)
                                                                .Where(l => DateTime.Parse(l.TimeStamp) >= from)
                                                                .Where(l => DateTime.Parse(l.TimeStamp) <= to)
                                                                .ToList();


                return joinTrackersTime;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

    }
}