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
using System.Windows.Navigation;

namespace ActiviaAPP
{
    public partial class MainWindow : Window

    {

        //Konstruktør for MainWindow klassen, der derved fra start automatisk kører login siden
        public MainWindow()
        {
            InitializeComponent();

            // Laver startsiden om til login
            MainFrame.Navigate(new Login());

        }
    }
}
