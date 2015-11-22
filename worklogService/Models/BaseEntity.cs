using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace worklogService.Models
{
    public class BaseEntity
    {

        private long id;
        public virtual long Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}