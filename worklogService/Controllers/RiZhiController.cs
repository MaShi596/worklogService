using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using worklogService.CommonMethod;
using worklogService.DBoperate;
using worklogService.Models;
using worklogService.Models.CommunicationClass;


namespace worklogService.Controllers
{
    public class RiZhiController : ApiController
    {
        public class StaffandPerson
        {
           // StaffLog staff;
            //public StaffLog Staff
            //{
            //    get { return staff; }
            //    set { staff = value; }
            //}
            string con;
            public string Con
            {
                get { return con; }
                set { con = value; }
            }

            WkTUser per;
            public WkTUser Per
            {
                get { return per; }
                set { per = value; }
            }

        }

        public class StaffAll
        {
            List<StaffandPerson> list;
            public List<StaffandPerson> List
            {
                get { return list; }
                set { list = value; }
            }

        }

        //int userid, int staffid
        // userid.ToString()
        // staffid.ToString()
        public HttpResponseMessage GetRiZhi()
        {
            string res = "";
            BaseService baseservice = new BaseService();
            IList nbhstaff = baseservice.ExecuteSQL("with cte as " +
                            "( " +
                            " select row=row_number()over(order by getdate()), * from WktuserShareUserId where SharePresonid =  " +758+ " and WktuserShareUserId.STATE = 0 and Id > " +2000+
                            ") " +
                            " select * from cte where row between " + "1" + " and " + "10");

            List<StaffandPerson> stafflist = new List<StaffandPerson>();


            if (nbhstaff != null && nbhstaff.Count > 0)
            {
                foreach (StaffLog o in nbhstaff)
                {
                    StaffandPerson st = new StaffandPerson();
                    
                    st.Con = o.Content;
                    st.Per = o.Staff;
                    stafflist.Add(st);
                   
                }

                StaffAll l = new StaffAll();
                l.List = stafflist;

                res = "成功";
                string data = JsonTools.ObjectToJson(l);

                var jsonStr = "{\"Mseeage\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
                };
                return result;


            }

            return null;

        }

        public IEnumerable<long> Get()
        {


            BaseService baseservice = new BaseService();

            IList i = baseservice.loadEntityList("select u from Comments u ");


            long[] b = new long[2000];
            int num = 0;
            foreach (Comments o in i)
            {
                b[num] = o.Id;

                num++;
            }
            return b;
            //return new string[] { "value1", "value2" };
        }
    
    
    
    }
}

