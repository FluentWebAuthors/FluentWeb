using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FluentWeb2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        CoreApplicationViewTitleBar coreTitleBar =
                CoreApplication.GetCurrentView().TitleBar;
        public MainPage()
        {
            this.InitializeComponent();
            //DataAcces dataAcces = new DataAcces();
            //dataAcces.CreateSettingsFile();

            // Hide default title bar.
            
            coreTitleBar.ExtendViewIntoTitleBar = true;

            // Set caption buttons background to transparent.
            ApplicationViewTitleBar titleBar =
                ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;

            // Set XAML element as a drag region.
            Window.Current.SetTitleBar(AppTitleBar);

            // Register a handler for when the size of the overlaid caption control changes.
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

            // Register a handler for when the title bar visibility changes.
            // For example, when the title bar is invoked in full screen mode.
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;

            // Register a handler for when the window activation changes.
            Window.Current.CoreWindow.Activated += CoreWindow_Activated;
            
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            // Get the size of the caption controls and set padding.
            LeftPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayLeftInset);
            RightPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayRightInset);
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (sender.IsVisible)
            {
                AppTitleBar.Visibility = Visibility.Visible;
            }
            else
            {
                AppTitleBar.Visibility = Visibility.Collapsed;
            }
        }

        private void CoreWindow_Activated(CoreWindow sender, WindowActivatedEventArgs args)
        {
            UISettings settings = new UISettings();
            if (args.WindowActivationState == CoreWindowActivationState.Deactivated)
            {
                AppTitleTextBlock.Foreground =
                   new SolidColorBrush(settings.UIElementColor(UIElementType.GrayText));
            }
            else
            {
                AppTitleTextBlock.Foreground =
                   new SolidColorBrush(settings.UIElementColor(UIElementType.WindowText));
            }
        }

        

        int pageCount = -2;
        int redoPageCount = 0;
        int rPageCount = 0;
        string searchUri = "https://google.com/search?q=";
        string homeUri = "https://google.com";
        byte b = 0;

        private void CanGoCheck()
        {
            if (webview.CanGoBack) { Bback.IsEnabled = true; } else { Bback.IsEnabled = false; }
            if (webview.CanGoForward) { Bfoward.IsEnabled = true; } else { Bfoward.IsEnabled = false; }
        }





        private void Command(string str)
        {
            MenuFlyout testFlyout = new MenuFlyout();
            MenuFlyoutItem t = new MenuFlyoutItem();
            t.Text = str;

            if (str == "//beta settings")
            {
                //this.Frame.Navigate(typeof(SettingsPage));
            }
            else if (str == "//dev pageConut")
            {
                Tsearch.Text = "pageCount = " + pageCount.ToString() + "/" + pageCount++.ToString();
            }
            else if (str == "//dev forceBack")
            {
                webview.GoBack();
            }
            else if (str == "//dev rPageCount++")
            {
                rPageCount++;
            }
            else if (str == "//dev forceFoward")
            {
                webview.GoForward();
            }
            else if (str == "//dev showMenuFlyout.atMoreButton")
            {
                testFlyout.ShowAt(Bmore);
            }
            else if (str == "//dev MenuFlyout.AddItem")
            {
                testFlyout.Items.Add(t);
                testFlyout.Items.Add(t);
                
            }
        }


        private void Search(string str)
        {
            if (str.Contains("/")
            )
            {
                Command(str);
            }

            else
            {

                if (str.Contains("."))
                {
                    if (str.Contains("://"))
                    {
                        webview.Source = new Uri("" + str);
                    }
                    else
                    {
                        try
                        {
                            webview.Source = new Uri("https://" + str);
                        } catch { webview.Source = new Uri(searchUri + str); }
                        
                    }

                }
                else
                {
                    webview.Source = new Uri(searchUri + str);
                }


                Bback.IsEnabled = true;
            }

            Cats.Add(str);

        }
        private void Brefresh_Click(object sender, RoutedEventArgs e)
        {
            webview.Reload();
        }

        private void Bback_Click(object sender, RoutedEventArgs e)
        {
            if (webview.CanGoBack) { webview.GoBack(); }
            else { }
            pageCount -= 2;
            rPageCount += 1;
            if (pageCount == 0) { Bback.IsEnabled = false; }

        }

        private void Bfoward_Click(object sender, RoutedEventArgs e)
        {
            if (webview.CanGoForward) { webview.GoForward(); }
            Bback.IsEnabled = true;
            rPageCount--;
        }

        private void Bhome_Click(object sender, RoutedEventArgs e)
        {
            webview.Source = new Uri(homeUri);
        }

        private void Bmore_Click_1(object sender, RoutedEventArgs e)
        {




        }

        private void debugFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            Tsearch.Text = "pageCount = " + pageCount + " / ";
        }

        private void webview_Loading(FrameworkElement sender, object args)
        {
            progressBar.Visibility = Visibility.Visible;
            progressBar.IsEnabled = true;
        }

        private void webview_LoadCompleted(object sender, NavigationEventArgs e)
        {


            progressBar.Visibility = Visibility.Collapsed;

        }

        private void Tserach_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Search(Tsearch.Text);
            }
        }

        private void TabView_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                (sender as TabView).TabItems.Add(CreateNewTab(i));
            }
        }

        private void TabView_AddButtonClick(TabView sender, object args)
        {
            sender.TabItems.Add(CreateNewTab(sender.TabItems.Count));
        }

        private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);

        }

        private TabViewItem CreateNewTab(int index)
        {
            TabViewItem newItem = new TabViewItem();

            newItem.Header = $"Document {index}";
            newItem.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document };

            // The content of the tab is often a frame that contains a page, though it could be any UIElement.
            Frame frame = new Frame();

            switch (index % 3)
            {
                case 0:
                    //frame.Navigate(typeof(SamplePage1));
                    break;
                case 1:
                    //frame.Navigate(typeof(SamplePage2));
                    break;
                case 2:
                    //frame.Navigate(typeof(SamplePage3));
                    break;
            }

            newItem.Content = frame;

            return newItem;
        }

        private List<string> Cats = new List<string>()
        {
            "google.com ",
            "youtube.com ",
            "gmail.com ",
            "microsoft.com ",
            "amazon.com ",
            "outlook.com ",
            "yahoo.com ",
            "dailymotion.com ",
            "start.com ",
            "apple.com ",
            "samsung.com ",
            "github.com ",
            "facebook.com ",
            "mica.com ",
            "twitter.com ",
            "reddit.com ",
            "tiktok.com ",
            "snapchat.com ",
            "instagram.com ",
            "duckduckgo.com ",
            "netflix.com ",



        };

        bool historyIsTrue = false;

        private void Tsearch_TextChanged_1(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var suitableItems = new List<string>();
            var splitText = sender.Text.ToLower().Split(" ");
            foreach (var cat in Cats)
            {
                var found = splitText.All((key) =>
                {
                    return cat.ToLower().Contains(key);
                });
                if (found)
                {
                    suitableItems.Add(cat);
                }
                if (historyIsTrue == true)
                {
                    string o = " ";
                    suitableItems.Add(cat);
                }
            }

            sender.ItemsSource = suitableItems;
        }

        private void ShowHistory(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var suitableItems = new List<string>();
            var splitText = sender.Text.ToLower().Split(" ");
            foreach (var cat in Cats)
            {
                var found = splitText.All((key) =>
                {
                    return cat.ToLower().Contains(key);
                });

                string o = " ";
                suitableItems.Add(cat);

            }

            sender.ItemsSource = suitableItems;
        }
        private void Tsearch_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {

        }

        private void googleAsDefault_Click(object sender, RoutedEventArgs e)
        {
            homeUri = "https://www.google.com/";
            searchUri = "https://www.google.com/search?q=";
            bingAsDefault.IsChecked = false;
            yahooAsDefault.IsChecked = false;
            youdotcomAsDefault.IsChecked = false;
        }

        private void bingAsDefault_Click(object sender, RoutedEventArgs e)
        {
            homeUri = "https://www.start.com/";
            searchUri = "https://www.bing.com/search?q=";
            googleAsDefault.IsChecked = false;
            yahooAsDefault.IsChecked = false;
            youdotcomAsDefault.IsChecked = false;
        }

        private void yahooAsDefault_Click(object sender, RoutedEventArgs e)
        {
            homeUri = "https://www.yahoo.com/";
            searchUri = "https://www.search.yahoo.com/search?q=";
            googleAsDefault.IsChecked = false;
            bingAsDefault.IsChecked = false;
            youdotcomAsDefault.IsChecked = false;
        }

        private void youdotcomAsDefault_Click(object sender, RoutedEventArgs e)
        {
            homeUri = "https://www.you.com/";
            searchUri = "https://you.com/search?q=";
            googleAsDefault.IsChecked = false;
            bingAsDefault.IsChecked = false;
            yahooAsDefault.IsChecked = false;
        }

        private void forceUndo_Click(object sender, RoutedEventArgs e)
        {
            pageCount -= 2;
            redoPageCount++;
            webview.GoBack();
        }

        private void forceRedo_Click(object sender, RoutedEventArgs e)
        {
            Bback.IsEnabled = true;
            redoPageCount--;
            webview.GoForward();
        }




        private void Tsearch_SuggestionChosen_1(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Tsearch.Text = args.SelectedItem.ToString();
            Search(args.SelectedItem.ToString());
        }

        private void Tsearch_QuerySubmitted_1(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            Search(Tsearch.Text);
        }
        AutoSuggestBox sendere;
        AutoSuggestBoxTextChangedEventArgs yo;
        private void Bhistory_Click(object sender, RoutedEventArgs e)
        {

            

        }



        private void webview_NavigationStarting_1(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            pageCount++;

            CanGoCheck();

            pageCountRedo.Text = "Redo integer = " + rPageCount;
            pageCountUndo.Text = "Undo integer = " + pageCount;

            if (webview.CanGoBack) { forceUndo.IsEnabled = true; } else { forceUndo.IsEnabled = false; }
            if (webview.CanGoForward) { forceRedo.IsEnabled = true; } else { forceRedo.IsEnabled = false; }

            if (b == 1) { progressBar.Visibility = Visibility.Visible; }
        }

        private void webview_NavigationCompleted_1(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            CanGoCheck();
            progressBar.Visibility = Visibility.Collapsed;
            progressBar.IsEnabled = false;
            Tsearch.Text = webview.Source.ToString();
            b = 1;
            //AppTitle.Text = webview.Source.LocalPath;
        }

        private void Tsearch_GotFocus(object sender, RoutedEventArgs e)
        {
            
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.SelectAll();
            }
        }

        private void Bfavorite_Click(object sender, RoutedEventArgs e)
        {
            
            NavBar.Height = new GridLength(80);
        }
    }
}
