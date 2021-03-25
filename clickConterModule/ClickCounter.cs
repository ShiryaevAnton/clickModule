using System;
using System.Text;
using Crestron.SimplSharp;     // For Basic SIMPL# Classes

using clickConterModule.Entity;
using clickConterModule.Repository;
using clickConterModule.Service;


namespace clickConterModule
{
    public class ClickCounter
    {

        /// <summary>
        /// SIMPL+ can only execute the default constructor. If you have variables that require initialization, please
        /// use an Initialize method
        /// </summary>
        /// 

        public ClickCounter()
        {

        }

        IJoinTrackerRepo joinRepo;
        IJoinTrackerLogRepo joinRepoLog;
        JoinTrackerService service;

        public void Init(uint numberOfInputs)
        {
            //joinRepo = new JoinTrackerRepo();
            joinRepo = new JoinTrackerInMemoryRepo();
            joinRepoLog = new JoinTrackerLogRepo();
            service = new JoinTrackerService(joinRepo, joinRepoLog, numberOfInputs);
        }

        public void SaveOneClick(uint joinNumber)
        {
            try
            {
                service.PostToTempLog(joinNumber);
            }
            catch (Exception e)
            {
                CrestronConsole.Print(e.ToString());
            }
        }

        public void PostAndClear() 
        {
            try
            {
                service.PostAndClear();
            }
            catch (Exception e)
            {
                CrestronConsole.Print(e.ToString());
            } 
        }

        public uint GetForWeek(uint joinNumber)
        {
            return service.GetForWeek(joinNumber);
        }

        public uint GetForMonth(uint joinNumber)
        {
            return service.GetForMonth(joinNumber);
        }

        public uint GetForYear(uint joinNumber)
        {
            return service.GetForYear(joinNumber);
        }

        public uint GetStatisticMorningTime(uint joinNumber)
        {
            return service.GetStatisticForTime(joinNumber, "morning");
        }

        public uint GetStatisticDayTime(uint joinNumber)
        {
            return service.GetStatisticForTime(joinNumber, "day");
        }

        public uint GetStatisticEveningTime(uint joinNumber)
        {
            return service.GetStatisticForTime(joinNumber, "evening");
        }

        public uint GetStatisticNightTime(uint joinNumber)
        {
            return service.GetStatisticForTime(joinNumber, "night");
        }
    }
}
