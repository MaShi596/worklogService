using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace worklogService.Models.CommunicationClass
{
    public class Dept
    {
        string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        string deptName;

        public string DeptName
        {
            get { return deptName; }
            set { deptName = value; }
        }
        
    }
}