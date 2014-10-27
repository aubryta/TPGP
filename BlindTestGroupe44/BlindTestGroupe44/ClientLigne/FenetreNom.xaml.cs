using BlindTestGroupe44.Main;
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

namespace BlindTestGroupe44.ClientLigne
{
    /// <summary>
    /// Logique d'interaction pour FenetreNom.xaml
    /// </summary>
    public partial class FenetreNom : Window
    {

        private String name = null;
        public FenetreNom()
        {
            InitializeComponent();
           
        }

        public void pseudoExistant()
        {
            PopUp pu = new PopUp();
            pu.setErreur("Ce pseudo existe déjà");
            pu.ShowDialog();

        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            name = champPseudo.Text;
            if(name.Length >= 10 || name.Length <=0)
            {
                PopUp pu = new PopUp();
                pu.setErreur("Le pseudo ne fait pas la bonne taille");
                pu.ShowDialog();
            }
            else
            {
                this.Close();
            }
        }

        public String getName()
        {
            return name;
        }

        private void champPseudo_GotFocus(object sender, RoutedEventArgs e)
        {
            champPseudo.Text = "";
        }

        private void keyEnterDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                name = champPseudo.Text;
                if (name.Length >= 10 || name.Length <= 0)
                {
                    PopUp pu = new PopUp();
                    pu.setErreur("Le pseudo ne fait pas la bonne taille");
                    pu.ShowDialog();
                }
                else
                {
                    this.Close();
                }
            }
        }


    }
}
