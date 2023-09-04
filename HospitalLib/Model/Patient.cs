﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalLib.Model
{
    public class Patient
    {
        [JsonProperty(PropertyName = "Username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }
    }
}
