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

namespace BlindTestGroupe44.Main
{
    /// <summary>
    /// Logique d'interaction pour PopUp.xaml
    /// </summary>
    public partial class PopUp : Window
    {

        
        public PopUp()
        {
            InitializeComponent();
        }


        public void setMessage(String message)
        {
            this.labelTitre.Content = "Message :";
            this.labelMessage.Content = message;
        }
        public void setErreur(String erreur)
        {
            this.labelTitre.Content = "Erreur :";
            this.labelMessage.Content = erreur;
        }
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
