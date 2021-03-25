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
        private uint numberOfInputs;
        private bool debug = false;

        public JoinTrackerService(IJoinTrackerRepo repoTemp, IJoinTrackerLogRepo repoLog, uint numberOfInputs)
        {
            this.repoTemp = repoTemp;
            this.repoLog = repoLog;
            this.numberOfInputs = numberOfInputs;
        }

        public void PostAndClear()
        {
            try
            {
                for (uint i = 1; i < numberOfInputs + 1; i++)
                {
                    PostToLog(i, "morning");
                    PostToLog(i, "day");
                    PostToLog(i, "evening");
                    PostToLog(i, "night");
                }

                if (debug) CrestronConsole.PrintLine("Post to log {0} inputs\n", numberOfInputs);

                repoTemp.DeleteAll();
                ClearLog();

                if (debug) CrestronConsole.PrintLine("Clear logs");


            }
            catch (Exception e)
            {
                throw(e);
            }

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

        private uint GetAllClickForWeek()
        {
            uint total = 0;

            for(uint i = 1; i < numberOfInputs + 1; i++)
            {
                total = total + GetNumberOfClickForWeek(i);
            }

            if (debug) CrestronConsole.PrintLine("Total click for week: {0}\n", total);

            return total;
        }

        private uint GetAllClickForMonth()
        {
            uint total = 0;

            for (uint i = 1; i < numberOfInputs + 1; i++)
            {
                total = total + GetNumberOfClickForMonth(i);
            }

            if (debug) CrestronConsole.PrintLine("Total click for month: {0}\n", total);

            return total;
        }

        private uint GetAllClickForYear()
        {
            uint total = 0;

            for (uint i = 1; i < numberOfInputs + 1; i++)
            {
                total = total + GetNumberOfClickForYear(i);
            }

            if (debug) CrestronConsole.PrintLine("Total click for year: {0}\n", total);

            return total;
        }

        private uint GetNumberOfClickForWeek(uint joinNumber)
        {
            var today2355 = DateTime.Today.AddHours(23).AddMinutes(55);
            var lastWeek = today2355.AddDays(-7);
            var jts = repoLog.GetByTime(joinNumber, lastWeek, today2355);
            uint quantaty = 0;

            foreach (var jt in jts)
            {
                quantaty = quantaty + jt.Quantity;
            }

            return quantaty;
        }

        private uint GetNumberOfClickForMonth(uint joinNumber)
        {
            var today2355 = DateTime.Today.AddHours(23).AddMinutes(55);
            var lastMonth = today2355.AddMonths(-1);

            var jts = repoLog.GetByTime(joinNumber, lastMonth, today2355);
            uint quantaty = 0;
            
            foreach (var jt in jts)
            {
                quantaty = quantaty + jt.Quantity;
            }

            return quantaty;
        }

        private uint GetNumberOfClickForYear(uint joinNumber)
        {
            var today2355 = DateTime.Today.AddHours(23).AddMinutes(55);
            var lastYear = today2355.AddYears(-1);

            var jts = repoLog.GetByTime(joinNumber, lastYear, today2355);
            uint quantaty = 0;

            foreach (var jt in jts)
            {
                quantaty = quantaty + jt.Quantity;
            }

            return quantaty;
        }

        public uint GetForWeek(uint joinNumber)
        {
            uint week = GetNumberOfClickForWeek(joinNumber);
            uint total = GetAllClickForWeek();
            uint result = 0;
            if (total != 0)
                result = (week * 100 / total);

            if (debug) CrestronConsole.PrintLine("Total click for input {0} for week {1}\n", joinNumber, week);
            if (debug) CrestronConsole.PrintLine("Total click for input {0} for week {1}%\n", joinNumber, result);

            return result;
        }

        public uint GetForMonth(uint joinNumber)
        {
            uint month = GetNumberOfClickForMonth(joinNumber);
            uint total = GetAllClickForMonth();
            uint result = 0;
            if (total != 0)
                result = (month * 100 / total);

            if (debug) CrestronConsole.PrintLine("Total click for input {0} for month {1}\n", joinNumber, month);
            if (debug) CrestronConsole.PrintLine("Total click for input {0} for month {1}%\n", joinNumber, result);

            return result;
        }

        public uint GetForYear(uint joinNumber)
        {
            uint year = GetNumberOfClickForYear(joinNumber);
            uint total = GetAllClickForYear();
            uint result = 0;
            if (total != 0)
                result = (year * 100 / total);

            if (debug) CrestronConsole.PrintLine("Total click for input {0} for year {1}\n", joinNumber, year);
            if (debug) CrestronConsole.PrintLine("Total click for input {0} for year {1}%\n", joinNumber, result);

            return result;
        }

        public uint GetStatisticForTime(uint joinNumber, string time)
        {
            var today2355 = DateTime.Today.AddHours(23).AddMinutes(55);
            var lastYear = today2355.AddYears(-1);

            var jts = repoLog.GetByTime(joinNumber, lastYear, today2355);
            var jtsTime = jts.Where(jt => jt.Time == time);

            uint result = 0;
            uint total = 0;
            uint totalByTime = 0;

            foreach (var jt in jts)
            {
                total = total + jt.Quantity;
            }

            if (debug) CrestronConsole.PrintLine("Total click for input {0} total {1}%\n", joinNumber, total);

            foreach (var jtTime in jtsTime)
            {
                totalByTime = totalByTime + jtTime.Quantity;
            }

            if (debug) CrestronConsole.PrintLine("Total click for input {0} for {1} time {2}%\n", joinNumber, time, totalByTime);

            if (total != 0)
                result = totalByTime * 100 / total;

            return result;
        }

        private void PostToLog(uint joinNumber, string time)
        {
            try
            {
                uint quantaty = 0;

                switch (time)
                {
                    case "morning":
                        quantaty = (uint)GetForMorning(joinNumber).ToArray().Length;
                        if (debug) CrestronConsole.PrintLine("total for input {0} today morning: {1}\n", joinNumber, quantaty);
                        break;
                    case "day":
                        quantaty = (uint)GetForDay(joinNumber).ToArray().Length;
                        if (debug) CrestronConsole.PrintLine("total for input {0} today day: {1}\n", joinNumber, quantaty);
                        break;
                    case "evening":
                        quantaty = (uint)GetForEvening(joinNumber).ToArray().Length;
                        if (debug) CrestronConsole.PrintLine("total for input {0} today evening: {1}\n", joinNumber, quantaty);
                        break;
                    case "night":
                        quantaty = (uint)GetForNight(joinNumber).ToArray().Length;
                        if (debug) CrestronConsole.PrintLine("total for input {0} today night: {1}\n", joinNumber, quantaty);
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