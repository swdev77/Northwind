using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NorthwindFluent
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void NavView_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            switch (args.InvokedItem.ToString())
            {
                case "Categories":
                    ContentFrame.Navigate(typeof(CategoriesPage));
                    break;
                default:
                    ContentFrame.Navigate(typeof(NotImplementedPage));
                    break;
            }
        }

        private void NavView_OnLoaded(object sender, RoutedEventArgs e)
        {
            NavView.MenuItems.Add(new NavigationViewItem
            {
                Content = "Categories",
                Icon = new SymbolIcon(Symbol.BrowsePhotos),
                Tag = "categories"
            });
            NavView.MenuItems.Add(new NavigationViewItem
            {
                Content = "Products",
                Icon = new SymbolIcon(Symbol.AllApps),
                Tag = "products"
            });
            NavView.MenuItems.Add(new NavigationViewItem
            {
                Content = "Suppliers",
                Icon = new SymbolIcon(Symbol.Contact2),
                Tag = "suppliers"
            });
            NavView.MenuItems.Add(new NavigationViewItemSeparator());
            NavView.MenuItems.Add(new NavigationViewItem
            {
                Content = "Customers",
                Icon = new SymbolIcon(Symbol.People),
                Tag = "customers"
            });

            NavView.MenuItems.Add(new NavigationViewItem
            {
                Content = "Orders",
                Icon = new SymbolIcon(Symbol.PhoneBook),
                Tag = "orders"
            });
            NavView.MenuItems.Add(new NavigationViewItem
            {
                Content = "Shippers",
                Icon = new SymbolIcon(Symbol.PostUpdate),
                Tag = "shippers"
            });
        }

        private async void RefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
            var notImplementedDialog = new ContentDialog
            {
                Title = "Not Implemented",
                Content = "The Refresh functionality has not yet been implemented.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result =
                await notImplementedDialog.ShowAsync();
        }
    }
}
