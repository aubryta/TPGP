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
    /// Logique d'interaction pour FenetreStyle.xaml
    /// </summary>
    public partial class FenetreStyle : Window
    {
        private IEnumerable<System.Windows.Controls.RadioButton> listeRadioButtons = null;
        public FenetreStyle()
        {
            InitializeComponent();
        }

        public void setListeStyle(List<String> listeStyle)
        {
            int y = 15;
            //On crée un radiobouton par style
            foreach(String nomStyle in listeStyle)
            {
                System.Windows.Controls.RadioButton r = new System.Windows.Controls.RadioButton();
                r.Margin = new Thickness(20, y, 0, 0);
                r.Content = nomStyle;
                r.FontFamily = new System.Windows.Media.FontFamily("Arial Rounded MT Bold");
                this.gridButton.Children.Add(r);
                y = y + 20;
            }
            listeRadioButtons = this.gridButton.Children.OfType<System.Windows.Controls.RadioButton>();
        }

        //Retourne le bouton coché, null si aucun ne l'est
        public String getCoche()
        {
            for(int i = 0; i < listeRadioButtons.Count(); i++)
            {
                if(listeRadioButtons.ElementAt(i).IsChecked == true)
                {
                    return (String)listeRadioButtons.ElementAt(i).Content;
                }
            }
            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String res = getCoche();

            //On ne peux pas fermer la fenêtre tant que un style n'est pas coché
            if(res != null)
                this.Close();
        }
    }
}
