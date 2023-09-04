using HospitalLib.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalLib.Event
{
    public class NotificationEventArgs : EventArgs
    {
        public string ClientId { get; set; }
        public ResultOperation ResultOperation { get; set; }

        public NotificationEventArgs(string clientId, ResultOperation resultOperation)
        {
            ClientId = clientId;
            ResultOperation = resultOperation;
        }
    }
}
