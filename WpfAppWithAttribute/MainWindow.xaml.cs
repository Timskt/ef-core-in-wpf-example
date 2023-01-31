using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace WpfAppWithAttribute
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainWindowViewModel = Ioc.Default.GetRequiredService<MainWindowViewModel>();
            AddressesCollectionViewSource = Resources["AddressesCollectionViewSource"] as CollectionViewSource;
        }

        public CollectionViewSource AddressesCollectionViewSource { get; set; }

        public MainWindowViewModel MainWindowViewModel { get; set; }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await MainWindowViewModel.InitializeAsync();
            MainWindowViewModel.PropertyChanged += MainWindowViewModelOnPropertyChanged;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.PropertyChanged -= MainWindowViewModelOnPropertyChanged;
        }

        private void MainWindowViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainWindowViewModel.Addresses))
                AddressesCollectionViewSource.View.Refresh();
        }
    }
}