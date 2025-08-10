using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Transference.Models
{
    public class LoginModel : INotifyPropertyChanged
    {
        private string userName;
        private string numDocumnent;

        public string UserName
        {
            get => userName; set
            {
                userName = value;
                OnPropertyChanged();
            }
        }
        public string NumDocumnent
        {
            get => numDocumnent; set
            {
                numDocumnent = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
