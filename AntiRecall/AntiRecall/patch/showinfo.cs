using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AntiRecall.patch
{
    public class showinfo : ICommand
    {

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            info info = new info();
            info.ShowDialog();
        }

        public event EventHandler CanExecuteChanged;
    }
}
