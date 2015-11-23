using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace worklogService.Models.CommunicationClass
{
    public class Role
    {
        string roleOrder;

        public string RoleOrder
        {
            get { return roleOrder; }
            set { roleOrder = value; }
        }
        string roleName;

        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }
    }
}