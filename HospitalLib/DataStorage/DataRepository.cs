using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalLib.DataStorage
{
    public class DataRepository
    {
        private HospitalContext _context { get; set; }
        public DataRepository() 
        {
            _context = new HospitalContext();
        }


    }
}
