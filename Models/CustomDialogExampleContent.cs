using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Metro.Annotations;

namespace Stock.Models
{
    public class CustomDialogExampleContent : INotifyPropertyChanged
    {
        private readonly ICommand _closeCommand;

        public CustomDialogExampleContent(Action<CustomDialogExampleContent> closeHandler)
        {
            _closeCommand = new SimpleCommand
            {
                ExecuteDelegate = o => closeHandler(this)
            };
        }

        public ICommand CloseCommand
        {
            get { return _closeCommand; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
