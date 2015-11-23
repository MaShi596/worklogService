﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace worklogService.Models.CommunicationClass
{
    public class PersonInfo
    {
        string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        string personAccount;
        /// <summary>
        /// 登录名
        /// </summary>
        
        public string PersonAccount
        {
            get { return personAccount; }
            set { personAccount = value; }
        }
        string personName;
        /// <summary>
        /// 姓名
        /// </summary>
        public string PersonName
        {
            get { return personName; }
            set { personName = value; }
        }
        Dept personDept;
        /// <summary>
        /// 部门信息
        /// </summary>
        public Dept PersonDept
        {
            get { return personDept; }
            set { personDept = value; }
        }
        Role personRole;

        /// <summary>
        /// 角色信息
        /// </summary>
        public Role PersonRole
        {
            get { return personRole; }
            set { personRole = value; }
        }

        string personPhone;

        public string PersonPhone
        {
            get { return personPhone; }
            set { personPhone = value; }
        }

    }
}