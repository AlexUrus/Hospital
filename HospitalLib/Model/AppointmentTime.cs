using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalLib.Model
{
    public class AppointmentTime
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

    }
}
