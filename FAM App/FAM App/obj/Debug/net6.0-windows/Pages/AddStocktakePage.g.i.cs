// Updated by XamlIntelliSenseFileGenerator 27.01.2023 21:36:12
#pragma checksum "..\..\..\..\Pages\AddStocktakePage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0AD0832DB64194F9529DB6D45D6670E43FE31A37"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

using FAM_App.Pages;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace FAM_App.Pages
{


    /// <summary>
    /// AddStocktakePage
    /// </summary>
    public partial class AddStocktakePage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector
    {

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.1.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/FAM App;V1.0.0.0;component/pages/addstocktakepage.xaml", System.UriKind.Relative);

#line 1 "..\..\..\..\Pages\AddStocktakePage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);

#line default
#line hidden
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            this._contentLoaded = true;
        }

        internal System.Windows.Controls.Border BorderPanel;
        internal System.Windows.Controls.Border BorderPage;
        internal System.Windows.Controls.TextBox City_TextBox;
        internal System.Windows.Controls.Border PostCodeAdress;
        internal System.Windows.Controls.TextBox PostCode_TextBox1;
        internal System.Windows.Controls.TextBox PostCode_TextBox2;
        internal System.Windows.Controls.TextBox Street_TextBox;
        internal System.Windows.Controls.TextBox BuildingNumber_TextBox;
        internal System.Windows.Controls.TextBox ApartmentNumber_TextBox;
        internal System.Windows.Controls.TextBox RoomNumber_TextBox;
        internal System.Windows.Controls.TextBox Name_TextBox;
        internal System.Windows.Controls.TextBox AdditionalInformation_TextBox;
        internal System.Windows.Controls.Button AddAdress_Button;
    }
}
