using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalLib.Model
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public TypeDoctor Type { get; set; }

    }
}
