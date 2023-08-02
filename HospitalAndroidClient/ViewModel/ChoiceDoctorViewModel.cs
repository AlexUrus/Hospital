using HospitalLib;
using HospitalLib.Model;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace HospitalAndroidClient.ViewModel
{
    public class ChoiceDoctorViewModel : BaseViewModel
    {
        
        private ObservableCollection<Doctor> _doctorsList;
        public ObservableCollection<Doctor> DoctorsList
        {
            get => _doctorsList;
            set => this.RaiseAndSetIfChanged(ref _doctorsList, value);
        }

        public ChoiceDoctorViewModel()
        {
            // Replace this with the logic to fetch the list of doctors from your data source.
            // For demonstration purposes, we're creating a sample list here.
            DoctorsList = new ObservableCollection<Doctor>
            {
                new Doctor { Name = "Dr. John Doe",  Type = TypeDoctor.Therapist },
                new Doctor { Name = "Dr. Jane Smith", Type = TypeDoctor.Gastroenterologist },
                new Doctor { Name = "Dr. Mike Johnson", Type = TypeDoctor.Otolaryngologist },
                // Add more doctors as needed
            };
        }
    }
}
