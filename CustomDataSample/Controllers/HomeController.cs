using Proxomo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomDataSample.Models;

namespace CustomDataSample.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }


        /**
         * working sample of geocode, use debug mode to view the data
         */
        public String GeoCodeSample()
        {
            //your address
            string address = "2900 West Plano Parkway, plano, texas";
            //your ip
            string ip = "76.184.188.152";

            //get ProxomoAPI obj
            ProxomoApi api = Prox.api;

            //actual geo code
            GeoCode gc = api.GeoCodebyAddress(address);
            //reverse geocode
            Location l = api.ReverseGeocode(gc.Latitude.ToString(), gc.Longitude.ToString());

            GeoIP gip = api.GeoCodeByIPAddress(ip);

            string result = "";
            result += "GeoCode :  _  Address: " + gc.Address + ", City: " + gc.City + ", Country: " + gc.CountryName + ", long: " + gc.Longitude + ", lat: " + gc.Latitude;
            return result;
        }

        public string ReverseGeo()
        {
            string address = "2900 West Plano Parkway, plano, texas";
            //your ip
            string ip = "76.184.188.152";

            //get ProxomoAPI obj
            ProxomoApi api = Prox.api;
            string result = "";
            //actual geo code
            GeoCode gc = api.GeoCodebyAddress(address);
            //reverse geocode
            Location l = api.ReverseGeocode(gc.Latitude.ToString(), gc.Longitude.ToString());

            GeoIP gip = api.GeoCodeByIPAddress(ip);
            result += "Reverse GeoCode from long: " + gc.Longitude + ", and lat: " + gc.Latitude + " : _  Address: " + l.Address1 + ", City: " + l.City + ", Country: " + l.CountryName;
            return result;
        }
        /**
         * link /Home/AppDataSample
         * 
         */
        public string AppDataSample()
        {
            //create a new ProxomoAPI object to handle the CRUD
            ProxomoApi api = Prox.api;

            AppData ad = new AppData();
            ad.Key = "MyFirstAppKey";
            ad.ObjectType = "MyApp";
            ad.Value = "MyAppValue";
            //add an app data
            string result_id = api.AppDataAdd(ad);
            //get app data value 
            AppData temp = api.AppDataGet(result_id);

            string result = "AppId: " + temp.ID + "; AppKey: " + temp.Key;
            return result;
        }
        public string AppDataGet()
        {
            //create a new ProxomoAPI object to handle the CRUD
            ProxomoApi api = Prox.api;

            AppData ad = new AppData();
            ad.Key = "MyFirstAppKey";
            ad.ObjectType = "MyApp";
            ad.Value = "MyAppValue";
            //add an app data
            string result_id = api.AppDataAdd(ad);
            //get app data value 
            AppData temp = api.AppDataGet(result_id);
            //get all the app data you have added

            temp = api.AppDataGet(result_id);

            string result = "AppId: " + temp.ID + "; AppKey: " + temp.Key;
            //delete an app data
            return result;
        }
        public string AppDataUpdate()
        {
            //create a new ProxomoAPI object to handle the CRUD
            ProxomoApi api = Prox.api;

            AppData ad = new AppData();
            ad.Key = "MyFirstAppKey";
            ad.ObjectType = "MyApp";
            ad.Value = "MyAppValue";
            //add an app data
            string result_id = api.AppDataAdd(ad);
            ad.ID = result_id;
            ad.Value = "new updated value";
            string result = "";
            api.AppDataUpdate(ad);
            AppData ad1 = api.AppDataGet(result_id);
            result += "new value: " + ad1.Value;

            return result;
        }
        public string AppDataDelete()
        {
            //create a new ProxomoAPI object to handle the CRUD
            ProxomoApi api = Prox.api;

            AppData ad = new AppData();
            ad.Key = "MyFirstAppKey";
            ad.ObjectType = "MyApp";
            ad.Value = "MyAppValue";
            //add an app data
            string result_id = api.AppDataAdd(ad);
            string result = "";
            //delete an app data
            api.AppDataDelete(result_id);

            result += "<br/>After Delete: -> ";
            try
            {
                AppData temp = api.AppDataGet(result_id);
            }
            catch
            {
                result += "app data with the id " + result_id + " was not found => delete success";
            }

            return result;
        }
        /*
         * link /Home/EventsSample 
         */
        public string EventsSample()
        {
            //create a new ProxomoAPI object to handle the CRUD
            ProxomoApi api = Prox.api;

            string result = "";
            Event e1 = new Event();

            string personid = CreatePerson1(api).ID;

            e1.Address1 = "address 1";
            e1.Address2 = "address 2";
            e1.City = "Plano";
            e1.CountryName = "US";
            e1.Description = "some description";
            e1.EventName = "MyEvent";
            e1.EventType = "MyType";
            e1.MaxParticipants = 30;
            e1.StartTime = DateTime.Now.AddDays(7);
            e1.EndTime = DateTime.Now.AddDays(10);
            e1.Latitude = 37.416488999999999;
            e1.Longitude = -122.01371;
            e1.Status = 0;
            e1.MinParticipants = 0;
            e1.Privacy = EventPrivacy.Secret;
            //event needs a person id
            e1.PersonID = personid;

            //add an event, retrieve it and print out
            string id = api.EventAdd(e1);
            Event e = api.EventGet(id);
            result = "Event: " + e.ID + "| Address: " + e.Address1 + " | City: " + e.City + " <br/> ";

            return result;
        }
        public string EventUpdate()
        {
            //proxomo api
            ProxomoApi api = Prox.api;
            //the event we use to update
            string eventId = "werFW57F1ZtmORox";
            Event e = api.EventGet(eventId);
            string result = "";
            result += "Try updating event description to \"new description\"<br/>";
            e.Description = "new description";
            api.EventUpdate(e);
            e = api.EventGet(eventId);
            result += "Updated description: " + e.Description;
            return result;
        }
        public string EventCommentAdd()
        {
            //proxomo api
            string eventId = "werFW57F1ZtmORox";
            string result = "";
            ProxomoApi api = Prox.api;
            Event e = api.EventGet(eventId);
            EventComment ec = new EventComment();
            ec.Comment = "this is an exciting event";
            ec.EventID = e.ID;
            ec.PersonName = "some person";
            ec.PersonID = "";
            api.EventCommentAdd(e.ID, ec);
            var cmt = api.EventCommentsGet(e.ID);
            result += "There are " + cmt.Count + " comments for this event <br/>";
            return result;
        }
        public string EventGet()
        {
            //proxomo api
            string eventId = "werFW57F1ZtmORox";
            ProxomoApi api = Prox.api;
            Event e = api.EventGet(eventId);
            string result = "";
            result = "Event: " + e.ID + "| Address: " + e.Address1 + " | City: " + e.City + " <br/> ";

            return result;
        }

        public string EventParticipantInvite()
        {
            string eventId = "werFW57F1ZtmORox";
            ProxomoApi api = Prox.api;
            Person p = CreatePerson1(api);
            p.EmailAlerts = true; //emailAlert and emailVerified must be set true to notifications
            p.EmailVerified = true;
            string personid = CreatePerson1(api).ID;
            string result = "";
            api.EventParticipantInvite(eventId, p.ID);
            //update the person email alert to send notification
            UpdatePerson1(p, api);
            NotificationSample(api, p);

            api.EventRSVP(eventId, EventParticipantStatus.Attending, p.ID);
            //get all events of a person
            List<Event> le = api.EventsGetByPerson(personid, DateTime.Now, DateTime.Now.AddDays(30));
            result += "Person: " + personid + " has " + le.Count + " events";
            return result;
        }
        /**
         * some advance stuff
         */
        public string AdvancedStuff()
        {
            //create a new ProxomoAPI object to handle the CRUD
            ProxomoApi api = Prox.api;
            string result = "";

            result += "Adding first person: Success</br>";
            PersonLogin pl = new PersonLogin();
            try
            {
                pl = api.SecurityPersonCreate("zxc", "zxc", "admin");
            }
            catch { }

            Person p = null;
            if (pl.PersonID == null)
            {
                UserToken ut = api.SecurityPersonAuthenticate("zxc", "zxc");
                p = api.PersonGet(ut.PersonID);
            }
            else
                p = api.PersonGet(pl.PersonID);

            p = api.PersonGet(p.ID);

            Person p2 = CreatePerson2(api);
            UpdatePerson1(p, api);

            //friend sample code
            FriendSample(api, p, p2);

            NotificationSample(api, p);

            //check friendship
            var t1 = api.FriendsGet(p.ID);
            result += "Person " + p.ID + " has " + t1.Count + " friend(s)";
            return result;
        }
        public string PersonAdd()
        {
            ProxomoApi api = Prox.api;
            PersonLogin pl = new PersonLogin();
            try
            {
                pl = api.SecurityPersonCreate("qwe", "qwe", "admin");
            }
            catch { }

            Person p = null;
            if (pl.PersonID == null)
            {
                UserToken ut = api.SecurityPersonAuthenticate("qwe", "qwe");
                p = api.PersonGet(ut.PersonID);
            }
            else
                p = api.PersonGet(pl.PersonID);
            return p.UserName;
        }
        public string PersonUpdate()
        {
            ProxomoApi api = Prox.api;
            //create a person
            Person p = CreatePerson1(api);
            //update the name
            p.FullName = "test name";
            api.PersonUpdate(p);
            //get the person and print name
            p = api.PersonGet(p.ID);

            return p.FullName;
        }
        public string PersonDelete()
        {
            ProxomoApi api = Prox.api;
            //create a person
            Person p = CreatePerson1(api);
            api.SecurityPersonDelete(p.ID);
            try
            {
                api.PersonGet(p.ID);
            }
            catch
            {
                return "Person with id " + p.ID + " not found => delete success";
            }
            return "error";
        }
        /**
         * Sample code for Friend
         */
        private void FriendSample(ProxomoApi api, Person p, Person p2)
        {
            //invite friend
            api.FriendInvite(p.ID, p2.ID);
            //respond to friend request
            api.FriendshipRespond(FriendResponse.Accept, p.ID, p2.ID);

        }

        /**
         * Sample code for notification
         */
        private void NotificationSample(ProxomoApi api, Person p)
        {
            Notification n = new Notification();
            n.PersonID = p.ID;
            n.EMailMessage = "your friend request has been accepted";
            n.EMailSubject = "Friend req status";
            n.SendMethod = NotificationSendMethod.EMail;
            n.NotificationType = NotificationType.EventInvite;
            n.EMailMessage = "email mess";
            n.EMailSubject = "email subject";

            api.NotificationSend(n);
        }



        /**
         * Create the 2nd person
         */
        private Person CreatePerson2(ProxomoApi api)
        {
            PersonLogin pl = new PersonLogin();
            try
            {
                pl = api.SecurityPersonCreate("abc", "abc", "admin");
            }
            catch { }

            Person p = null;
            if (pl.PersonID == null)
            {
                UserToken ut = api.SecurityPersonAuthenticate("abc", "abc");
                p = api.PersonGet(ut.PersonID);
            }
            else
                p = api.PersonGet(pl.PersonID);
            return p;
        }

        /**
         * Create the first person
         */
        private Person CreatePerson1(ProxomoApi api)
        {
            PersonLogin pl = new PersonLogin();
            try
            {
                pl = api.SecurityPersonCreate("qwe", "qwe", "admin");
            }
            catch { }

            Person p = null;
            if (pl.PersonID == null)
            {
                UserToken ut = api.SecurityPersonAuthenticate("qwe", "qwe");
                p = api.PersonGet(ut.PersonID);
            }
            else
                p = api.PersonGet(pl.PersonID);
            return p;
        }

        /**
         * Update person attributes
         */
        private void UpdatePerson1(Person p, ProxomoApi api)
        {
            // the followings are required for notification
            p.EmailAddress = "person1@email.com";
            p.EmailAlerts = true;
            p.EmailVerified = true;
            p.EmailVerificationStatus = VerificationStatus.Complete;
            p.EmailVerificationCode = "123456";
            p.MobileVerified = true;

            api.PersonUpdate(p);
        }

        /**
         * use the link /Home/GenerateData to view the working sample 
         */
        public string GenerateData()
        {
            ProxomoApi api = Prox.api;
            string s = TestDataInserted(api);
            return s;
        }

        private void GenerateData(ProxomoApi api)
        {
            //MyCustomData represent the schema of the data you want to add
            MyCustomData cd = new MyCustomData();

            //some sample data
            for (int i = 0; i < 5; i++)
            {
                cd.TableName = "MyCustomTable";
                cd.Data = "data " + i;
                cd.ID = "";
                cd.Date = DateTime.Today.AddDays(i);

                //call api.CustomDataAdd to add data
                api.CustomDataAdd<MyCustomData>(cd);
            }
        }
        private String TestDataInserted(ProxomoApi api)
        {
            ContinuationTokens ct = new ContinuationTokens(string.Empty, string.Empty);

            //get back all the data in a specified table
            List<MyCustomData> lcd = api.CustomDataSearch<MyCustomData>("MyCustomTable", string.Empty, 20, ref ct);
            String s = "";
            //if there is no such table, create and insert data then get the data back to display
            if (lcd == null || lcd.Count == 0)
            {
                GenerateData(api);

                lcd = api.CustomDataSearch<MyCustomData>("MyCustomTable", string.Empty, 20, ref ct);
            }
            //print the data to web page
            foreach (var item in lcd)
            {
                s += item.Data + " | ";
            }
            return s;
        }

        public string CustomDataUpdate()
        {
            ProxomoApi api = Prox.api;
            //create a custom data (CD)
            MyCustomData mcd = new MyCustomData();
            mcd.Data = "test data";
            mcd.TableName = "TestTable";
            mcd.Date = DateTime.Now;
            //add the data and get the id
            string id = api.CustomDataAdd(mcd);
            //get the CD obj
            MyCustomData mcd1 = api.CustomDataGet<MyCustomData>("TestTable", id);
            mcd1.Data = "new data"; //change some value
            api.CustomDataUpdate<MyCustomData>(mcd1); //update the data
            //retreive data and print out
            MyCustomData mcd2 = api.CustomDataGet<MyCustomData>("TestTable", id);
            string s = "After update: " + mcd2.Data;
            return s;
        }

        public string CustomDataDelete()
        {
            ProxomoApi api = Prox.api;
            //create a custom data (CD)
            MyCustomData mcd = new MyCustomData();
            mcd.Data = "test data";
            mcd.TableName = "TestTable";
            mcd.Date = DateTime.Now;
            //add the data and get the id
            string id = api.CustomDataAdd(mcd);
            //delete the data
            api.CustomDataDelete("TestTable", id);
            //try get data again
            string s = "";
            try
            {
                MyCustomData mcd2 = api.CustomDataGet<MyCustomData>("TestTable", id);
            }
            catch
            {
                s = "custom data with id " + id + " not found => delete success";
            }
            return s;
        }
    }
}
