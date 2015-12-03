using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using worklogService.CommonMethod;
using worklogService.DBoperate;
using worklogService.Models;
using worklogService.Models.CommunicationClass;
using System.Collections;

namespace worklogService.Controllers
{
    public class SuiBiController : ApiController
    {

        public class OwnSuiBi
        {
            int suibiid;
            public int SuiBiId
            {
                get { return suibiid; }
                set { suibiid = value; }

            }

            string contents;
            public string Contents
            {
                get { return contents; }
                set { contents = value; }
            }

            string writetime;
            public string Writetime
            {
                get { return writetime; }
                set { writetime = value; }
            }
            string wktuserid;
            private int SuiBi;

            public string Wktuserid
            {
                get { return wktuserid; }
                set { wktuserid = value; }
            }
        }
        public class OwnSuiBiAll
        {
            List<OwnSuiBi> list;
            private List<OwnSuiBi> suibilist;

            public List<OwnSuiBi> List1
            {
                get { return suibilist; }
                set { suibilist = value; }
            }
        }


        public HttpResponseMessage Get()
        //获得个人随笔
        {

            string userid = "699";
            string sbtime = "0";
            string res = "";
            BaseService baseservice = new BaseService();
            string sqlstr = "";
            if (sbtime == "0")
            {
                sqlstr = "with cte as " +
                           "( " +
                           " select row_number()over(order by WtiteTime()), * from SuiBiView where WkTUserId =  " + userid.ToString() + " and STATE = 0 and WriteTime > " + sbtime.ToString() +
                           ") " +
                           " select * from cte where row between " + "1" + " and " + "10";
            }
            else
            {
                sqlstr = "with cte as " +
                           "( " +
                           " select row_number()over(order by WtiteTime()), * from SuiBiView where WkTUserId =  " + userid.ToString() + " and STATE = 0 and WriteTime < " + sbtime.ToString() +
                           ") " +
                           " select * from cte where row between " + "1" + " and " + "10";
            }
            // sqlstr = "from SuiBi where State=" + (int)IEntity.stateEnum.Normal + "and  WkTUserId=" + userid + " and WriteTime>" + sbtime + "order by WriteTime desc ";
            IList nbhsuibi = baseservice.ExecuteSQL(sqlstr);
            List<SuiBiinfo> suibilist = new System.Collections.Generic.List<SuiBiinfo>();
            if (nbhsuibi != null && nbhsuibi.Count > 0)
            {
                SuiBiinfo st = new SuiBiinfo();

                foreach (SuiBi o in nbhsuibi)
                {
                    WkTUser user = new WkTUser();
                    st.Suibiid = o.Id.ToString();
                    st.Suibitime = new DateTime(o.WriteTime).ToString("yyyy年MM月dd日 HH:mm");
                    st.Suibicontent = o.Contents;
                    //       st.Suibicontentbefor = Htmlsuibibefor(o.Contents);
                    st.Personid = userid.ToString();
                    //        user = (WkTUser)baseservice.loadEntity(user, Convert.ToInt64(st.Personid));
                    //        st.Personname = user.KuName;
                    suibilist.Add(st);

                }
                //for (int i = 0; i < 9; i++)
                //{
                //    SuiBiinfo st = new SuiBiinfo();
                //    st.Suibitime = new DateTime(((Models.SuiBi)nbhsuibi[i]).WriteTime).ToString("yyyy年MM月dd日 HH:mm");
                //    st.Suibicontentbefor = Htmlsuibibefor(((Models.SuiBi)nbhsuibi[i]).Contents);
                //    st.Suibicontent = ((SuiBi)nbhsuibi[i]).Contents;
                //    st.Suibiid = ((Models.SuiBi)nbhsuibi[i]).Id.ToString();

                //    //List <com > own=new System.Collections.Generic.List<com>() ;

                //}
                SuiBiAll all = new SuiBiAll();
                all.List = suibilist;
                res = "成功";
                string data = JsonTools.ObjectToJson(all);
                var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
                };
                return result;
            }
            else if (nbhsuibi != null && nbhsuibi.Count > 0 && nbhsuibi.Count < 10)
            {
                for (int i = 0; i < nbhsuibi.Count; i++)
                {
                    SuiBiinfo st = new SuiBiinfo();
                    st.Suibitime = new DateTime(((Models.SuiBi)nbhsuibi[i]).WriteTime).ToString("yyyy年MM月dd日 HH:mm");
                    // st.Suibicontentbefor = Htmlsuibibefor(((Models.SuiBi)nbhsuibi[i]).Contents);
                    st.Suibiid = ((Models.SuiBi)nbhsuibi[i]).Id.ToString();
                    suibilist.Add(st);

                }
                SuiBiAll all = new SuiBiAll();
                all.List = suibilist;
                res = "成功";
                string data = JsonTools.ObjectToJson(all);
                var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
                };
                return result;
            }
            else
            {
                res = "没有内容";
                string data = "";
                var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
                };
                return result;
            }

            /* string res = "";
             BaseService baseservice = new BaseService();
             IList suibitext = baseservice.loadEntityList("from SuiBi where wktuserid= " + userid.ToString() + "");
             List<SuiBiinfo > ownsuibi=new System.Collections.Generic.List<SuiBiinfo>();
             if (ownsuibi != null && ownsuibi.Count > 0)
             {
                 for (int i = 0; i < ownsuibi.Count; i++)
                 {
 
                 }
             }*/
        }



        public class SuiBiinfo
        {
            string suibiid;

            public string Suibiid
            {
                get { return suibiid; }
                set { suibiid = value; }
            }
            string suibicontent;

            public string Suibicontent
            {
                get { return suibicontent; }
                set { suibicontent = value; }
            }

            string personname;

            public string Personname
            {
                get { return personname; }
                set { personname = value; }
            }
            string personid;
            public string Personid
            {
                get { return personid; }
                set { personid = value; }
            }
            string persondeptname;
            public string Persondeptname
            {
                get { return persondeptname; }
                set { persondeptname = value; }
            }
            string suibitime;
            public string Suibitime
            {
                get { return suibitime; }
                set { suibitime = value; }
            }

        }
        public class SuiBiAll
        {
            List<SuiBiinfo> list;

            public List<SuiBiinfo> List
            {
                get { return list; }
                set { list = value; }
            }
        }
        public HttpResponseMessage GetAllSuiBi([FromUri]string userid, [FromUri]string sbtime)
        {
            string sqlstr = null;
            if (sbtime == "0")
            {
                sqlstr = "with cte as " +
                            "( " +
                            " select row=row_number()over(order by WriteTime()), * from SuiBi where  WriteTime > " + sbtime.ToString() +
                            ") " +
                            " select * from cte where row between " + "1" + " and " + "10";
            }
            else
            {
                sqlstr = "with cte as " +
                            "( " +
                            " select row=row_number()over(order by WriteTime()), * from SuiBi where  WriteTime < " + sbtime.ToString() +
                            ") " +
                            " select * from cte where row between " + "1" + " and " + "10";



            }
            string res = "";
            BaseService baseeriver = new BaseService();
            IList nbhsuibiallsql = baseeriver.ExecuteSQL(sqlstr);

            List<SuiBiinfo> info = new List<SuiBiinfo>();
            if (nbhsuibiallsql != null && nbhsuibiallsql.Count > 0)
            {
                for (int i = 0; i < nbhsuibiallsql.Count; i++)
                {
                    object[] sb = (object[])nbhsuibiallsql[i];
                    SuiBiinfo sub = new SuiBiinfo();

                    sub.Suibiid = sb[1].ToString();
                    sub.Personid = sb[4].ToString();
                    //sub.Personname = sb[4].ToString();
                    sub.Suibicontent = sb[2].ToString();
                    // sub.Persondeptname = o.;
                    sub.Suibitime = new DateTime(Convert.ToInt64(sb[3])).ToString("yyyy年MM月dd日 HH:mm");
                    //sub.Persondeptname = o.WkTUserId.Kdid.KdName.ToString().Trim();
                    WkTUser user = new WkTUser();
                    user = (WkTUser)baseeriver.loadEntity(user, Convert.ToInt64(sb[4]));
                    sub.Personname = user.KuName;
                    sub.Persondeptname = user.Kdid.KdName.ToString().Trim();
                    info.Add(sub);
                }


                //if (nbhsuibiallsql != null && nbhsuibiallsql.Count > 0)
                //{
                //    List<SuiBiinfo> info = new List<SuiBiinfo>();
                //    foreach(SuiBi o in nbhsuibiallsql)
                //    { 
                //        SuiBiinfo sub = new SuiBiinfo();
                //        sub.Suibiid = o.Id.ToString();
                //        sub.Personid = o.WkTUserId.Id.ToString();
                //        sub.Personname  = o.WkTUserId.KuName.ToString();
                //        sub.Suibicontent = o.Contents.ToString();
                //       // sub.Persondeptname = o.;
                //        sub.Suibitime = new DateTime(o.TimeStamp).ToString("yyyy年MM月dd日 HH:mm");
                //        sub.Persondeptname = o.WkTUserId.Kdid.KdName.ToString().Trim();
                //        info.Add(sub);

                //    }

                //for (int x = 0; x < nbhsuibiallsql.Count; x++)
                //{
                //    SuiBiinfo sub = new SuiBiinfo();
                //    sub.Personid = ((SuiBiinfo)nbhsuibiallsql[x]).Personid;
                //    sub.Personname = ((SuiBiinfo)nbhsuibiallsql[x]).Personname;
                //    sub.Suibicontent = ((SuiBiinfo)nbhsuibiallsql[x]).Suibicontent;
                //    sub.Persondeptname = ((SuiBiinfo)nbhsuibiallsql[x]).Persondeptname;
                //    sub.Suibitime = ((SuiBiinfo)nbhsuibiallsql[x]).Suibitime;
                //}
                SuiBiAll all = new SuiBiAll();
                all.List = info;
                res = "成功";
                string data = JsonTools.ObjectToJson(all);
                var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
                };
                return result;
            }
            else
            {
                res = "没有内容";
                string data = "";
                var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + "\"data\":" + data + "}";
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
                };
                return result;
            }

        }
    
    }
}
