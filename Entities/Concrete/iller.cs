﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class iller : IEntity
    {
        [Key]
        public int ilNo { get; set; }
        public string iladi { get; set; }
        public string bolgeNo { get; set; }
    }
}
