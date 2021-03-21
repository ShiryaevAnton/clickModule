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
    public class JoinTrackerRepo : IJoinTrackerRepo
    {

        string pathToJson = "\\NVRAM\\ServerData\\jointrackertemp.json";

        public JoinTrackerRepo()
        {

        }

        public void Create(JoinTracker joinTracker)
        {
            try
            {
                List<JoinTracker> joinTrackersFromDB = GetEntityFromJson();
                joinTrackersFromDB.Add(joinTracker);
                SaveJson(joinTrackersFromDB);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public void DeleteAll()
        {
            try
            {
                List<JoinTracker> joinTrackersFromDB = GetEntityFromJson();

                joinTrackersFromDB.RemoveAll(rl => rl.UUID != null);

                SaveJson(joinTrackersFromDB);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public List<JoinTracker> GetByTime(uint joinNumber, DateTime from, DateTime to)
        {
            try
            {
                List<JoinTracker> joinTrackers = GetEntityFromJson()
                                                .Where(l => l.JoinNumber == joinNumber)
                                                .Where(l => DateTime.Parse(l.TimeStamp) >= from)
                                                .Where(l => DateTime.Parse(l.TimeStamp) <= to)
                                                .ToList();


                return joinTrackers;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }


        private void SaveJson(List<JoinTracker> joinTrackers)
        {
            string json = JsonConvert.SerializeObject(joinTrackers);

            using (var writter = new StreamWriter(pathToJson))
            {
                writter.Write(json);
            }
        }

        private List<JoinTracker> GetEntityFromJson()
        {
            try
            {
                string json;

                using (var reader = new StreamReader(pathToJson))
                {
                    json = reader.ReadToEnd();
                }

                JArray jsonJoinTrackers = JArray.Parse(json);

                List<JoinTracker> joinTrackers = jsonJoinTrackers.Select(jt => new JoinTracker()
                {
                    UUID = (string)jt["UUID"],
                    JoinNumber = (uint)jt["JoinNumber"],
                    TimeStamp = (string)jt["TimeStamp"],
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