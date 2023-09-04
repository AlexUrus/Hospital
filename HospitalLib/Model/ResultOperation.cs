using HospitalLib.Record;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalLib.Model
{
    public class ResultOperation : IRecord
    {
        public TypeRecord Type => TypeRecord.ResultOperation;

        public const int E_OK = 0;
        public const int E_INV_PARAM = 1;
        public int Error { get; }
        public string Message { get; }

        public ResultOperation(int error, string message)
        {
            Error = error;
            Message = message;
        }
    }
}
