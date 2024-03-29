﻿using System;
using System.Collections.Generic;

namespace Cap4TaskList.Models
{
    public partial class UserTask
    {
        public int TaskId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Complete { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
