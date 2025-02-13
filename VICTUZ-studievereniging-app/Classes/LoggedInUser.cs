﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VICTUZ_studievereniging_app.Classes
{
    public class LoggedInUser
    {
        [PrimaryKey]
        public string? Id { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

    }
}
