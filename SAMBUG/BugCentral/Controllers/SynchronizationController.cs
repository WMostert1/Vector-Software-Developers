using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Results;
using BugBusiness.Interface.BugSecurity;
using BugBusiness.Interface.BugSecurity.DTO;
using BugBusiness.Interface.BugSecurity.Exceptions;
using Newtonsoft.Json.Linq;
using DataAccess.Interface;

using System.Web;

using System.Text;
using System.Web.Mvc;
using BugCentral.Controllers.DTO;
using BugCentral.Controllers.Exceptions;
using System.Reflection;
using System.IO;

namespace BugCentral.Controllers
{
    [System.Web.Http.RoutePrefix("Synchronization")]
    public class SynchronizationController : Controller
    {
        private IDbSynchronization dbSynchronization { get; set; }
        private string json;
    
        public SynchronizationController(IDbSynchronization _dbSynchronization ) //, String _json)
        {
            dbSynchronization = _dbSynchronization;
            
           // json = _json;
        }

        public void ReadInJson(String location)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), location);
            json = System.IO.File.ReadAllText(path);
            //json = System.IO.File.ReadAllText(location);
        }

        public SynResult sync()
        {
            

            JObject jsonFile;
            JObject actualData;
            Boolean success = true;
            try
            {
                jsonFile = JObject.Parse(json);
                actualData = JObject.Parse(jsonFile["syncData"].ToString());
            }
            catch(Exception){
                success = false;
                throw new ParsingException();
            }

            //The exceptions for functions below qill be thrown by dbSynchronizaion
            try{
                foreach (var scoutStop in actualData["ScoutStops"])
                {
                    Int64 scoutStopID = Convert.ToInt64(scoutStop["ScoutStopID"].ToString());
                    Int64 userID = Convert.ToInt64(scoutStop["UserID"].ToString());
                    Int64 blockID = Convert.ToInt64(scoutStop["BlockID"].ToString());
                    int numberOfTrees = Convert.ToInt16(scoutStop["NumberOfTrees"].ToString());
                    float latitude = float.Parse(scoutStop["Latitude"].ToString().Replace(".", ","));
                    float longitude = float.Parse(scoutStop["Longitude"].ToString().Replace(".", ","));
                    DateTime date = Convert.ToDateTime(scoutStop["Date"].ToString());
                    int lastModifiedID = Convert.ToInt16(scoutStop["LastModifiedID"].ToString());
                    DateTime tmStamp = Convert.ToDateTime(scoutStop["TMStamp"].ToString());

                    dbSynchronization.PersistScoutStops(scoutStopID, userID, blockID, numberOfTrees, latitude, longitude, date, lastModifiedID, tmStamp);
                }

                foreach (var scoutBugs in actualData["ScoutBugs"])
                {
                    Int64 scoutBugID = Convert.ToInt64(scoutBugs["ScoutBugID"].ToString());
                    Int64 scoutStopID = Convert.ToInt64(scoutBugs["ScoutStopID"].ToString());
                    Int64 speciesID = Convert.ToInt64(scoutBugs["SpeciesID"].ToString());
                    int numberOfBugs = Convert.ToInt16(scoutBugs["NumberOfBugs"].ToString());
                    byte[] fieldPicture = Encoding.ASCII.GetBytes(scoutBugs["FieldPicture"].ToString());
                    String comments = scoutBugs["Comments"].ToString();
                    int lastModifiedID = Convert.ToInt16(scoutBugs["LastModifiedID"].ToString());
                    DateTime tmStamp = Convert.ToDateTime(scoutBugs["TMStamp"].ToString());

                    dbSynchronization.PersistScoutBugs(scoutBugID, scoutStopID, speciesID, numberOfBugs, fieldPicture, comments, lastModifiedID, tmStamp);
                }
            }
            catch(Exception){
                success = false;   
            }

            SynResult synR = new SynResult();
            synR.json = json;
            synR.Passed = success;

            return synR;

        }
    }
}