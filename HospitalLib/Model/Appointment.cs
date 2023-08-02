using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalLib.Model
{
    public class Appointment
    {
        public int Id { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public AppointmentTime AppointmentTime { get; set; }
    }
}
