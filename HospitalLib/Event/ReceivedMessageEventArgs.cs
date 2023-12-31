﻿using HospitalLib.Record;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalLib.Event
{
    public class ReceivedMessageEventArgs : EventArgs
    {
        public object Record  { get; private set; }
        public TypeRecord TypeRecord { get; private set; }
        public string IdClient { get; private set; }
        public ReceivedMessageEventArgs(object record, TypeRecord typeRecord, string idClient) 
        {
            Record = record;
            TypeRecord = typeRecord;
            IdClient = idClient;
        }
    }
}
