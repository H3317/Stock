using System;
using System.Windows.Input;
using Stock.Models;

namespace Stock.ReportView
{
    public class ReportViewModel : CustomDialogExampleContent
    {
        private string _firstName;
        private string _lastName;
        private readonly ICommand _closeCommand;
        public ReportViewModel(Action<CustomDialogExampleContent> closeHandler)
            : base(closeHandler)
        {
            _closeCommand = new SimpleCommand
            {
                ExecuteDelegate = o => closeHandler(this)
            };
            
        }
           
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }
       
    }
}
