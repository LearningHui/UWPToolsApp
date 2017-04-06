using LuaScript;
using MainProject.Test;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TLSLib;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace MainProject
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LuaManager luaManager = LuaManager.RootLuaManager;
            //luaManager.LoadLuaScript("window:alert(\"hello lua\")");
            //luaManager.LoadLuaScript("http: postSyn(nil, \"ebank / get_page\", \"name = UnitTest / FunLib / HTTP / http_postAsyn.xml\", nil, { name = \"Robin\"});");            
        }
        private void Navagate_Click(object sender, RoutedEventArgs e)
        {
            string str = "MainProject.Test." + (sender as ContentControl).Content.ToString();
            Type type = Type.GetType(str);
            this.Frame.Navigate(type);          
        }
    }
}
