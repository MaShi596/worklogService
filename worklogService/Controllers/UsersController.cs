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
    public class UsersController : ApiController
    {
        // GET api/users
        public IEnumerable<string> Get()
        {


            BaseService baseservice = new BaseService();

            IList i = baseservice.loadEntityList("select u from WkTUser u ");


            string[] b = new string[84];
            int num = 0;
            foreach (WkTUser o in i)
            {
                b[num] = o.KuName;

                num++;
            }
            return b;
            //return new string[] { "value1", "value2" };
        }

        public class DeptsandPerson
        {
            Dept dept;

            public Dept Dept
            {
                get { return dept; }
                set { dept = value; }
            }
            
            List<PersonInfo> persons;

            public List<PersonInfo> Persons
            {
                get { return persons; }
                set { persons = value; }
            }
            
        }

        public class DeptsandPersonlist
        {
            List<DeptsandPerson> list;

            public List<DeptsandPerson> List
            {
                get { return list; }
                set { list = value; }
            }

        
        }

        public HttpResponseMessage GetAlldeptAndusers()
        {
             BaseService baseservice = new BaseService();
             IList nhbdepts = baseservice.loadEntityList("select u from WkTDept u");
             IList nhbpersons = baseservice.loadEntityList("select u from WkTUser u ");
             List<DeptsandPerson> dplist = new List<DeptsandPerson>();
             
             foreach (WkTDept o in nhbdepts)
             {
                 DeptsandPerson dp = new DeptsandPerson();
                 
                 Dept d = new Dept();

                 d.Id = o.Id.ToString();
                 d.DeptName = o.KdName.Trim();
                 dp.Dept = d;
                 List<PersonInfo> pers = new List<PersonInfo>();

                 foreach (WkTUser n in nhbpersons)
                 {
                     if (n.Kdid.Id == o.Id)
                     {
                         PersonInfo per = new PersonInfo();
                         per.Id = n.Id.ToString();
                         per.PersonName = n.KuName;
                         pers.Add(per);
                     
                     }
                     
                 }
                 dp.Persons = pers;

                 dplist.Add(dp);
                 
             }
             DeptsandPersonlist l = new DeptsandPersonlist();

             l.List = dplist;

           

             
             
             string res = "成功";
             string data = JsonTools.ObjectToJson(l);
             
            var jsonStr = "{\"Message\":" + "\"" + res + "\"" + "," + " \"data\":" + data + "}";
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
            };
            return result;


        }

        
        

        public class namepwd
        {

            public string  Name {get; set;}
            public string Pwd { get; set;}

        }


        public HttpResponseMessage toLoginMessage([FromBody]namepwd nn)
        {


            string res;
            string data = "1";


            string name = nn.Name; string orpwd = nn.Pwd; 
            //string name = "mashi"; string orpwd = "186754";

            BaseService baseservice = new BaseService();
            IList pwd = baseservice.ExecuteSQL("select right(sys.fn_VarBinToHexStr(hashbytes('MD5', '" + orpwd.Trim() + "')),32)"); // 通过数据库加密
            if (pwd == null || pwd.Count <= 0)
            {
                res =  "登录异常！";
            }
            object[] pwdArray = (object[])pwd[0];
            //因为是共用表 选择是工作小秘书相关的角色
            IList userList = baseservice.loadEntityList("select u from WkTUser u right join u.UserRole role where role.KrDESC='工作小秘书角色' and u.KuLid='" + name.Trim() + "' and u.KuPassWD='" + pwdArray[0] + "'");
            if (userList == null || userList.Count <= 0)
            {
                res =  "用户名或密码错误！";
            }
            else if (userList.Count > 1)
            {
                res =  "用户异常，请联系管理员！";
            }
            else
            {

                Role role = new Role();
                WkTUser u = (WkTUser)userList[0];
                foreach (WkTRole r in u.UserRole)
                {
                    if (r.KrDESC.Trim().Equals("工作小秘书角色"))//是本系统的用户角色
                    {
                        role.RoleOrder = r.KrOrder.ToString();
                        role.RoleName = r.KrName;
                    }
                }
               

                Dept d = new Dept();
                d.Id = u.Kdid.Id.ToString();
                d.DeptName = u.Kdid.KdName.Trim();

                PersonInfo per = new PersonInfo();
                per.Id = u.Id.ToString();
                per.PersonName = u.KuName;
                per.PersonPhone = u.KuPhone;
                per.PersonDept = d;
                per.PersonRole = role;
                per.PersonAccount = u.KuLid;

                
                data =  JsonTools.ObjectToJson(per);
                
                res =  "登录成功";
            }


            var jsonStr = "{\"Message\":" + "\""+res+"\"" + ","+" \"data\":"+ data+"}";
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "text/json")
            };
            return result;
        }

        // GET api/users/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/users
        public void Post([FromBody]string value)
        {
        }

        // PUT api/users/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/users/5
        public void Delete(int id)
        {


        }
    }
}
