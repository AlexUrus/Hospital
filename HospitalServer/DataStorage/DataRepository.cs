using HospitalLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HospitalLib.DataStorage
{
    public class DataRepository
    {
        private HospitalContext _context { get; set; }
        private static readonly Lazy<DataRepository> _instance = new Lazy<DataRepository>(() => new DataRepository());
        public static DataRepository Instance => _instance.Value;
        private DataRepository()
        {
            _context = new HospitalContext();
        }

        public void SetPatient(Patient patient)
        {
            try
            {
                _context.Patients.Add(patient);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception();
            } 
        }

        public Patient GetPatient(string username)
        {
            try
            {
                Patient patient = _context.Patients.FirstOrDefault(p => p.Username == username);
                return patient;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool PatientExists(string username)
        {
            try
            {
                bool exists = _context.Patients.Any(p => p.Username == username);
                return exists;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking patient existence: {ex.Message}");
                throw;
            }
        }
    }
}
