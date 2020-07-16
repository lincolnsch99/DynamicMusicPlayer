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

namespace DynamicMusicPlayerWPF.KeyboardUI
{
    /// <summary>
    /// Interaction logic for IndivKey.xaml
    /// </summary>
    public partial class IndivKey : UserControl
    {
        private IndivKeyModel model;
        public IndivKeyModel Model { get => model; }

        public IndivKey()
        {
            InitializeComponent();
            model = new IndivKeyModel(this);
        }

        private void selectButton_Click(object sender, RoutedEventArgs e)
        {
            model.Selected();
        }
    }
}
