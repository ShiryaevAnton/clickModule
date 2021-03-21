using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Crestron.SimplSharp.Net.Http;
using clickConterModule.Repository;
using clickConterModule.Entity;

namespace clickConterModule.Service
{
    public class JoinTrackerService
    {

        private IJoinTrackerRepo repoTemp;
        private IJoinTrackerLogRepo repoLog;

        public JoinTrackerService(IJoinTrackerRepo repoTemp, IJoinTrackerLogRepo repoLog)
        {
            this.repoTemp = repoTemp;
            this.repoLog = repoLog;
        }

        public void PostToTempLog(uint joinNumber)
        {

            try
            {
                var joinTracker = new JoinTracker()
                {
                    UUID = Guid.NewGuid().ToString(),
                    JoinNumber = joinNumber,
                    TimeStamp = DateTime.Now.ToString()
                };

                repoTemp.Create(joinTracker);

            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public void ClearTempLog() 
        {
            try 
            {
                repoTemp.DeleteAll();
            }
            catch(Exception e)
            {
                throw (e);
            }
        }

        public void ClearLog()
        {
            try
            {
                var lastYear = DateTime.Today.AddYears(-1);
                var lastYearNextDay = DateTime.Today.AddDays(1);
                var jts = repoLog.GetByTime(lastYear, lastYearNextDay);
                if (jts != null)
                {
                    jts.RemoveAll(r => r != null);
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
 
        }

        public uint GetNumberOfClickForWeek(uint joinNumber)
        {
            var today = DateTime.Now;
            var lastWeek = today.AddDays(-7);
            var quantaty = repoLog.GetByTime(joinNumber, lastWeek, today).ToArray().Length;
            return (uint)quantaty;
        }

        public uint GetNumberOfClickForMonth(uint joinNumber)
        {
            var today = DateTime.Now;
            var lastMonth = today.AddMonths(-1);
            var quantaty = repoLog.GetByTime(joinNumber, lastMonth, today).ToArray().Length;
            return (uint)quantaty;
        }

        public uint GetNumberOfClickForYear(uint joinNumber)
        {
            var today = DateTime.Now;
            var lastYear = today.AddYears(-1);
            var quantaty = repoLog.GetByTime(joinNumber, lastYear, today).ToArray().Length;
            return (uint)quantaty;
        }

        public void PostToLog(uint joinNumber, string time)
        {
            try
            {
                uint quantaty = 0;

                switch (time)
                {
                    case "morning":
                        quantaty = (uint)GetForMorning(joinNumber).ToArray().Length;
                        break;
                    case "day":
                        quantaty = (uint)GetForDay(joinNumber).ToArray().Length;
                        break;
                    case "evening":
                        quantaty = (uint)GetForEvening(joinNumber).ToArray().Length;
                        break;
                    case "night":
                        quantaty = (uint)GetForNight(joinNumber).ToArray().Length;
                        break;
                    default:
                        break;
                }

                var joinTracker = new JoinTrackerDaily()
                {
                    UUID = Guid.NewGuid().ToString(),
                    JoinNumber = joinNumber,
                    Day = DateTime.Today.ToString(),
                    Time = time,
                    Quantity = quantaty
                };

                repoLog.Create(joinTracker);

            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private List<JoinTracker> GetForMorning(uint joinNumber)
        {
            try
            {
                var sixAM = DateTime.Today.AddHours(6);
                var noon = DateTime.Today.AddHours(12);
                return repoTemp.GetByTime(joinNumber, sixAM, noon);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private List<JoinTracker> GetForDay(uint joinNumber)
        {
            try
            {
                var noon = DateTime.Today.AddHours(12);
                var sixPM = DateTime.Today.AddHours(18);
                return repoTemp.GetByTime(joinNumber, noon, sixPM);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private List<JoinTracker> GetForEvening(uint joinNumber)
        {
            try
            {
                var sixPM = DateTime.Today.AddHours(18);
                var midnight = DateTime.Today.AddHours(24);
                return repoTemp.GetByTime(joinNumber, sixPM, midnight);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private List<JoinTracker> GetForNight(uint joinNumber)
        {
            try
            {
                var midnight = DateTime.Today;
                var sixAM = DateTime.Today.AddHours(6);
                return repoTemp.GetByTime(joinNumber, midnight, sixAM);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private List<JoinTracker> GetForToday(uint joinNumber)
        {
            try
            {
                var today = DateTime.Today;
                var tomorrow = DateTime.Today.AddDays(1);
                return repoTemp.GetByTime(joinNumber, today, tomorrow);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        //public Dictionary<uint, float> RangeByDay(List<uint> joinNumbers)
        //{
        //    try
        //    {
        //        var today = DateTime.Today.ToString();
        //        var tomorrow = DateTime.Today.AddDays(1).ToString();
        //        return Range(joinNumbers, today, tomorrow);
        //    }
        //    catch (Exception e)
        //    {
        //        CrestronConsole.PrintLine(e.ToString());
        //        throw (e);
        //    }

        //}

        //public Dictionary<uint, float> RangeByWeek(List<uint> joinNumbers)
        //{
        //    try
        //    {
        //        var today = DateTime.Today.ToString();
        //        var nextWeek = DateTime.Today.AddDays(7).ToString();
        //        return Range(joinNumbers, today, nextWeek);
        //    }
        //    catch (Exception e)
        //    {
        //        CrestronConsole.PrintLine(e.ToString());
        //        throw (e);
        //    }
        //}

        //public Dictionary<uint, float> RangeByMonth(List<uint> joinNumbers)
        //{
        //    try
        //    {
        //        var today = DateTime.Today.ToString();
        //        var nextMonth = DateTime.Today.AddMonths(1).ToString();
        //        return Range(joinNumbers, today, nextMonth);
        //    }
        //    catch (Exception e)
        //    {
        //        CrestronConsole.PrintLine(e.ToString());
        //        throw (e);
        //    }
        //}

        //private Dictionary<uint, float> Range(List<uint> joinNumbers, string from, string to)
        //{
        //    try
        //    {
        //        Dictionary<uint, int> preResult = new Dictionary<uint, int>();
        //        Dictionary<uint, float> result = new Dictionary<uint, float>();

        //        foreach (uint value in joinNumbers)
        //        {
        //            preResult.Add(value, repoTemp.GetByTime(value, from, to).ToArray().Length);
        //        }

        //        int totalPress = 0;

        //        foreach (KeyValuePair<uint, int> jtNumber in preResult)
        //        {
        //            totalPress += jtNumber.Value;
        //        }

        //        preResult.OrderBy(key => key.Value);

        //        foreach (KeyValuePair<uint, int> jtNumber in preResult)
        //        {
        //            result[jtNumber.Key] = (float)jtNumber.Value / totalPress * 100;
        //        }

        //        return result;
        //    }

        //    catch (Exception e)
        //    {
        //        throw (e);
        //    }
        //}

    }
}