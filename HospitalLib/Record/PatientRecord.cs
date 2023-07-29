using HospitalLib.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalLib.Record
{
    public class PatientRecord : AbstractRecord<Patient>
    {
        public PatientRecord(Patient patient) : base(TypeRecord.ClientLogin, patient) { }
    }
}
