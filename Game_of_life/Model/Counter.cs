using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Game_of_life.Model
{
    class Counter : INotifyPropertyChanged
    {
        private int _count;

        public int Count
        {
            get { return _count; }
            set 
                {
                    _count = value;
                OnPropertyChanged(nameof(Count));    
                }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
