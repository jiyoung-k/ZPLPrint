using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZPLPrint
{
    /// <summary>
    /// Interaction logic for PasswordControl.xaml
    /// </summary>
    public partial class PasswordControl : UserControl
    {
        public event EventHandler Succeeded;
        public PasswordControl()
        {
            InitializeComponent();
        }

        private void pass_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                if (pass.Password.Equals("neuronaware"))
                {
                    Succeeded?.Invoke(sender, e);
                }
                else
                {
                    MessageBox.Show("비밀번호가 일치하지 않습니다.");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (pass.Password.Equals("neuronaware"))
            {
                Succeeded?.Invoke(sender, e);
            }
            else
            {
                MessageBox.Show("비밀번호가 일치하지 않습니다.");
            }
        }
    }
}

