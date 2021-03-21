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
    public class JoinTrackerLogRepo : IJoinTrackerLogRepo
    {
        string pathToJson = "\\NVRAM\\ServerData\\jointracker.json";

        public JoinTrackerLogRepo()
        {

        }

        public void Create(JoinTrackerDaily joinTrackerDaily)
        {
            try
            {
                List<JoinTrackerDaily> joinTrackersFromDB = GetEntityFromJson();
                joinTrackersFromDB.Add(joinTrackerDaily);
                SaveJson(joinTrackersFromDB);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public void Delete(string uuid)
        {
            try
            {
                List<JoinTrackerDaily> joinTrackersFromDB = GetEntityFromJson();

                joinTrackersFromDB.RemoveAll(rl => rl.UUID == uuid);

                SaveJson(joinTrackersFromDB);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public List<JoinTrackerDaily> GetByTime(uint joinNumber, string day, string time)
        {
            try
            {
                List<JoinTrackerDaily> joinTrackers = GetEntityFromJson()
                                                .Where(l => l.JoinNumber == joinNumber)
                                                .Where(l => l.Day == day)
                                                .Where(l => l.Time == time)
                                                .ToList();


                return joinTrackers;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }


        public List<JoinTrackerDaily> GetByTime(DateTime from, DateTime to)
        {
            try
            {
                List<JoinTrackerDaily> joinTrackers = GetEntityFromJson()
                                                .Where(l => DateTime.Parse(l.Day) >= from)
                                                .Where(l => DateTime.Parse(l.Day) <= to)
                                                .ToList();


                return joinTrackers;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public List<JoinTrackerDaily> GetByTime(uint joinNumber, DateTime from, DateTime to)
        {
            try
            {
                List<JoinTrackerDaily> joinTrackers = GetEntityFromJson()
                                                .Where(l => l.JoinNumber == joinNumber)
                                                .Where(l => DateTime.Parse(l.Day) >= from)
                                                .Where(l => DateTime.Parse(l.Day) <= to)
                                                .ToList();


                return joinTrackers;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        private void SaveJson(List<JoinTrackerDaily> joinTrackersDaily)
        {

            string json = JsonConvert.SerializeObject(joinTrackersDaily);

            using (var writter = new StreamWriter(pathToJson))
            {
                writter.Write(json);
            }
        }

        private List<JoinTrackerDaily> GetEntityFromJson()
        {
            try
            {
                string json;

                using (var reader = new StreamReader(pathToJson))
                {
                    json = reader.ReadToEnd();
                }

                JArray jsonJoinTrackers = JArray.Parse(json);

                List<JoinTrackerDaily> joinTrackers = jsonJoinTrackers.Select(jt => new JoinTrackerDaily()
                {
                    UUID = (string)jt["UUID"],
                    JoinNumber = (uint)jt["JoinNumber"],
                    Day = (string)jt["Day"],
                    Time = (string)jt["Time"],
                    Quantity = (uint)jt["Quantity"]
                })
                .ToList();

                return joinTrackers;
            }
            catch (Exception e)
            {
                throw (e);
            }

        }

    }
}
   