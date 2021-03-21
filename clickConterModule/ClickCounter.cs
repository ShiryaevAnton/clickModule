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

        JoinTrackerRepo joinRepo;
        JoinTrackerLogRepo joinRepoLog;
        JoinTrackerService service;

        public void Init(uint numberOfInputs)
        {
            joinRepo = new JoinTrackerRepo();
            joinRepoLog = new JoinTrackerLogRepo();
            service = new JoinTrackerService(joinRepo, joinRepoLog);
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

        public void CleanUp(uint numberOfInputs) 
        {
            try
            {
                for (uint i = 1; i < numberOfInputs + 1; i++)
                {
                    service.PostToLog(i, "morning");
                    service.PostToLog(i, "day");
                    service.PostToLog(i, "evening");
                    service.PostToLog(i, "night");
                }

                service.ClearTempLog();
                service.ClearLog();

            }
            catch (Exception e)
            {
                CrestronConsole.Print(e.ToString());
            }
 
        }

        public ClickCounter()
        {
        }
    }
}
