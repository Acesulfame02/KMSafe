using Safe.Services;
using System.ComponentModel;

namespace Safe.Model
{
    public class ReportModel : INotifyPropertyChanged
    {
        private string _date = DateTime.Today.ToString("dd-MM-yyyy");
        private string _title = string.Empty;
        private string _description = string.Empty;
        private string _type = string.Empty;
        private string _severity = string.Empty;
        private string _message = string.Empty;
        private string _userId = string.Empty;
        private string _time = DateTime.Now.ToString("HH:mm:ss");
        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Date
        {
            get => _date;
            set
            {
                _date = value;
                RaisePropertyChanged(nameof(Date));
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                RaisePropertyChanged(nameof(Type));
            }
        }

        public string Severity
        {
            get => _severity;
            set
            {
                _severity = value;
                RaisePropertyChanged(nameof(Severity));
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged(nameof(Message));
            }
        }

        public string Time
        {
            get => _time;
            set
            {
                _time = value;
                RaisePropertyChanged(nameof(Time));
            }
        }

        public string UserId 
        {
            get => _userId;
            set
            {
                _userId = value;
                RaisePropertyChanged(nameof(UserId));
            }
        }
    }
}
