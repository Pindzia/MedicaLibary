﻿#pragma checksum "..\..\AddVisitPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "773622F8B5FD8BD9033FC3BC19F7896D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace MedicaLibary {
    
    
    /// <summary>
    /// AddVisit
    /// </summary>
    public partial class AddVisit : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\AddVisitPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid addgrid;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\AddVisitPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ID;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\AddVisitPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Imię;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\AddVisitPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Nazwisko;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\AddVisitPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Pesel;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\AddVisitPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox DataWizyty;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\AddVisitPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Komentarz;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MedicaLibary;component/addvisitpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\AddVisitPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.addgrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.ID = ((System.Windows.Controls.TextBox)(target));
            
            #line 35 "..\..\AddVisitPage.xaml"
            this.ID.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.onInput);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Imię = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.Nazwisko = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.Pesel = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.DataWizyty = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.Komentarz = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            
            #line 43 "..\..\AddVisitPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.saveToXML);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
