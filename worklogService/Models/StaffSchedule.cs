using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace worklogService.Models
{
    public class StaffSchedule:IEntity
    {
        private string content;

        public virtual string Content
        {
            get { return content; }
            set { content = value; }
        }
        private string subject;

        public virtual string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        private int ifRemind;

        public virtual int IfRemind
        {
            get { return ifRemind; }
            set { ifRemind = value; }
        }
        private WkTUser staff;

        public virtual WkTUser Staff
        {
            get { return staff; }
            set { staff = value; }
        }


        private WkTUser arrangeMan;

        public virtual WkTUser ArrangeMan
        {
            get { return arrangeMan; }
            set { arrangeMan = value; }
        }


        private long scheduleTime;
        //日程时间
        public virtual long ScheduleTime
        {
            get { return scheduleTime; }
            set { scheduleTime = value; }
        }
        private long remindTime;

        //提醒时间
        public virtual long RemindTime
        {
            get { return remindTime; }
            set { remindTime = value; }
        }

        private int doState;

        public virtual int DoState
        {
            get { return doState; }
            set { doState = value; }
        }

        private IList<WkTUser> staffScheduleStaffs;

        public virtual IList<WkTUser> StaffScheduleStaffs
        {
            get { return staffScheduleStaffs; }
            set { staffScheduleStaffs = value; }
        }

        #region 枚举变量

        public enum IfRemindEnum
        {
            Renmind = 1,
            NotRemind = 0
        }

        #endregion
    }
}