﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class SuperCategory:IEntity
    {
        [Key]
        public int SuperCategoryId { get; set; }
        public string SuperCategoryName { get; set; }
        public bool Status { get; set; }
    }
}
