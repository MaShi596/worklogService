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
        // GET: /SuiBi/
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
        public static string Htmlsuibibefor(string html)
        //随笔即视 
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
            string a = "";
            if (contentText.Length > 100)
            {
                a = contentText.Substring(0, 100) + "....";
                return a;
            }
            else
            {
                return contentText;
            }
        }

        /*public class SuiBiText
        {
            List<SuiBiText> list;
            public List<SuiBiText> List;
        {
                get { return list; }
                set { list = value; }
        }*/

        public HttpResponseMessage GetOwnSuibi(string userid, string logtick)
        //获得个人随笔
        {
            string res = "";
            BaseService baseservice = new BaseService();
            string sqlstr = "";
            if (logtick == "0")
            //上滑与下滑
            {
                sqlstr = "from SuiBi where State=" + (int)IEntity.stateEnum.Normal + "and  WkTUserId=" + userid + " and WriteTime>" + logtick + "order by WriteTime desc ";
            }
           
            IList nbhsuibi = baseservice.loadEntityList(sqlstr);
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
        /*public HttpRequestMessage GetAllSuiBi(int userid, int staffid)
    {
        string res="";
        BaseService baseeriver =new BaseService ();
        IList nbhsuibiall=baseeriver.ExecuteSQL("");
        List <>
        if(baseeriver !=null && nbhsuibiall .Count >0)
        {
            for (int i = 0; i < nbhsuibiall.Count; i++)
            {
                object[] sb = (object[])nbhsuibiall[i];
                
            }
        }
    }*/
    }
}
