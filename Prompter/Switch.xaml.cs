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
using System.Windows.Shapes;

namespace Prompter
{
    /// <summary>
    /// Interaction logic for Switch.xaml
    /// </summary>
    public partial class Switch : Window
    {
        MainWindow MainForm = Application.Current.Windows[0] as MainWindow;

        public Switch()
        {
            InitializeComponent();

            lbPageIdx.Content = $"Slide# {Docs.PageIdx + 1}";


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Docs.PageIdx--;
            MainForm.UpdateSlide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Docs.PageIdx++;
            MainForm.UpdateSlide();
        }
    }
}
