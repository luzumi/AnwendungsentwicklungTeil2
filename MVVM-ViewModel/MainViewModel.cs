using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MVVM_ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand AddUser { get; set; }

        private string filter;
        public string Filter
        {
            get { return filter; }
            set
            {
                if (filter != value)
                {
                    filter = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Filter)));
                }
            }
        }


        private ObservableCollection<UserViewModel> entries;
        public ObservableCollection<UserViewModel> EntryList
        {
            get => entries;
            set
            {
                if (entries != value)
                {
                    entries = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EntryList)));
                }
            }
        }

        
        public MainViewModel()
        {
            AddUser = new AddUserCommand() {Parent = this};
            EntryList = new ObservableCollection<UserViewModel>();
            EntryList.Add(new UserViewModel { Name = "Hans", Salary = 55000.0 });
            EntryList.Add(new UserViewModel { Name = "Peter", Salary = 58000.0 });
            EntryList.Add(new UserViewModel { Name = "Hildegard", Salary = 62000.0 });

            Filter = "A";
        }

    }
}
