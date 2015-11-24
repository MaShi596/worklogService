using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace worklogService.Models
{
    public class WorkDay : IEntity
    {
        private long workDateTime;

        public virtual long WorkDateTime
        {
            get { return workDateTime; }
            set { workDateTime = value; }
        }
        private Holiday holidayId;

        public virtual Holiday HolidayId
        {
            get { return holidayId; }
            set { holidayId = value; }
        }
    }
}