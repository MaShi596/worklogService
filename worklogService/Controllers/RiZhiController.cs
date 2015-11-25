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


using System.Text.RegularExpressions;






namespace worklogService.Controllers
{
    public class RiZhiController : ApiController
    {
        public class StaffandPerson
        {
            int headerId;
            public int HeaderId
            {
                get { return headerId; }
                set { headerId = value; }
            }

            string name;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            string timeText;
            public string TimeText
            {
                get { return timeText; }
                set { timeText = value; }
            }

            string contentext;
            public string Contentext
            {
                get { return contentext; }
                set { contentext = value; }
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

        
        public HttpResponseMessage GetRiZhi(int userid, int staffid)//别人分享给自己的日志
        {
            string res = "";
            BaseService baseservice = new BaseService();
            IList nbhstaff = baseservice.ExecuteSQL("with cte as " +
                            "( " +
                            " select row=row_number()over(order by getdate()), * from WktuserShareUserId where SharePresonid =  " + userid.ToString() + " and WktuserShareUserId.STATE = 0 and Id > " + staffid.ToString() +
                            ") " +
                            " select * from cte where row between " + "1" + " and " + "10");

            List<StaffandPerson> stafflist = new List<StaffandPerson>();

            if (nbhstaff != null && nbhstaff.Count > 0)
            {
                for (int i = 0; i < nbhstaff.Count; i++)
                {
                    object[] sf = (object[])nbhstaff[i];
                    StaffandPerson st = new StaffandPerson();
                    st.HeaderId = int.Parse(sf[3].ToString());//谁分享的
                    st.Name = sf[6].ToString();//分享人的姓名
                    st.Contentext = HtmlToReguFormat(sf[5].ToString());
                    //st.Contentext = sf[5].ToString();//日志全部内容
                    st.TimeText = new DateTime(Convert.ToInt64(sf[4].ToString())).ToString("yyyy年MM月dd日 HH:mm");
                    //st.Tag = sf[7]; //日志id
                    //st.ContentClicked += rizhi_ContentClicked;
                    //st.Parent = rz_flowLayoutPanel;
                    //rz_flowLayoutPanel.Tag = sf[1];//Id号;
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

        public static string HtmlToReguFormat(string html)
        {
            Regex r = new Regex("<[^<]*>");
            MatchCollection mc = r.Matches(html);
            String contentText = html.Replace("&nbsp;", " ");
            for (int j = 0; j < mc.Count; j++)
            {
                if (mc[j].Value.Contains("src"))
                {
                    contentText = contentText.Replace(mc[j].Value, "[图片]");
                }
                else
                {
                    contentText = contentText.Replace(mc[j].Value, " ");
                }
            }

            return contentText;
        }

        public class OwnStaff
        {
            long id;
            public long Id
            {
                get { return id; }
                set { id = value; }
            }

            private string time;

            public string Time
            {
                get { return time; }
                set { time = value; }
            }
            string contents;

            public string Contents
            {
                get { return contents; }
                set { contents = value; }
            }
        }

        public class OwnStaffAll
        {
            List<OwnStaff> list;

            public List<OwnStaff> List
            {
                get { return list; }
                set { list = value; }
            }
        }


        public HttpResponseMessage GetOwnRiZhi(int userid, int staffid)//获得自己的日志
        {
            string res = "";
            BaseService baseservice = new BaseService();
            IList nbhstaff = baseservice.loadEntityList("from StaffLog where State=" + (int)IEntity.stateEnum.Normal + "and Staff=" + userid.ToString() + "and Id>"+staffid.ToString()+"order by WriteTime desc");
            List<OwnStaff> ownlist = new List<OwnStaff>();
            if (nbhstaff != null && nbhstaff.Count > 0)
            {
                foreach (StaffLog sl in nbhstaff)
                {
                    OwnStaff st = new OwnStaff();
                    st.Id = sl.Id;
                    st.Time = new DateTime(Convert.ToInt64(sl.TimeStamp.ToString())).ToString("yyyy年MM月dd日 HH:mm");
                    st.Contents = HtmlToReguFormat(sl.Content.ToString());
                    ownlist.Add(st);
                }
                OwnStaffAll l = new OwnStaffAll();
                l.List = ownlist;
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

