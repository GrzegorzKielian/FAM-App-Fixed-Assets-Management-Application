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

namespace FAM_App.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy EditFixedAssetWindow.xaml
    /// </summary>
    public partial class EditFixedAssetWindow : Window
    {
        public EditFixedAssetWindow(int fixedAsset_ID)
        {
            InitializeComponent();
            txtBox.Text = fixedAsset_ID.ToString();
        }
    }
}
