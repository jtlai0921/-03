﻿#pragma checksum "C:\Users\joshhu\Desktop\win10\第12章\Example_5\MyApp\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9DCFF165FCE4CB480E366FA5C30A51B0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyApp
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.cmb = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    #line 12 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.cmb).SelectionChanged += this.OnCmbSelectionChanged;
                    #line default
                }
                break;
            case 2:
                {
                    this.txt1 = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 18 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.txt1).LostFocus += this.OnText1Lostfocus;
                    #line default
                }
                break;
            case 3:
                {
                    this.txt2 = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 19 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.txt2).LostFocus += this.OnText2Lostfocus;
                    #line default
                }
                break;
            case 4:
                {
                    this.tgswitch = (global::Windows.UI.Xaml.Controls.ToggleSwitch)(target);
                    #line 20 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ToggleSwitch)this.tgswitch).Toggled += this.OnToggled;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

