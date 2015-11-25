using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace worklogService.Models
{
    public class Comments:IEntity
    {
        private String content;

        public virtual String Content
        {
            get { return content; }
            set { content = value; }
        }
        private String commentPersonName;

        public virtual String CommentPersonName
        {
            get { return commentPersonName; }
            set { commentPersonName = value; }
        }
    }
}