﻿#pragma checksum "..\..\..\..\Pages\FixedAssetCardPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "F7184557AC4C55AF997F72E2C8613B778F1ACFB5"
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


namespace FAM_App.Pages {
    
    
    /// <summary>
    /// FixedAssetCardPage
    /// </summary>
    public partial class FixedAssetCardPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 35 "..\..\..\..\Pages\FixedAssetCardPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border BorderPanel;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\..\Pages\FixedAssetCardPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox InventoryNumber_TxtBox;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\Pages\FixedAssetCardPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ShowHistory_Button;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\Pages\FixedAssetCardPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ExportToPDF_Button;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\..\Pages\FixedAssetCardPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border BorderPage;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\..\Pages\FixedAssetCardPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid FixedAssetGrid;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\..\Pages\FixedAssetCardPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid HistoryDataGrid;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/FAM App;component/pages/fixedassetcardpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\FixedAssetCardPage.xaml"
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
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.BorderPanel = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.InventoryNumber_TxtBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.ShowHistory_Button = ((System.Windows.Controls.Button)(target));
            
            #line 51 "..\..\..\..\Pages\FixedAssetCardPage.xaml"
            this.ShowHistory_Button.Click += new System.Windows.RoutedEventHandler(this.ShowHistory_Button_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ExportToPDF_Button = ((System.Windows.Controls.Button)(target));
            
            #line 52 "..\..\..\..\Pages\FixedAssetCardPage.xaml"
            this.ExportToPDF_Button.Click += new System.Windows.RoutedEventHandler(this.ExportToPDF_Button_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.BorderPage = ((System.Windows.Controls.Border)(target));
            return;
            case 6:
            this.FixedAssetGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 7:
            this.HistoryDataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

