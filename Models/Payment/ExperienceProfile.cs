﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models.Payment
{
    public class ExperienceProfile
    {
        public string id { get; set; }

        public string name { get; set; }

        public bool temporary { get; set; }

        public InputFields input_fields { get; set; }

    }

    public class InputFields
    {
        public int no_shipping { get; set; }
    }
}
