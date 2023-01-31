using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using App.BLL.DTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WpfAppWithDbContextFactory
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly ExampleAppBLLFactory _exampleAppBllFactory;
        private ObservableCollection<Address> _addresses;
        private Address _selectedAddress;

        public MainWindowViewModel(ExampleAppBLLFactory exampleAppBllFactory)
        {
            _exampleAppBllFactory = exampleAppBllFactory;
            UpdateAddressLine1Command = new AsyncRelayCommand(UpdateAddressLine1);
        }

        public ObservableCollection<Address> Addresses
        {
            get => _addresses;
            set => SetProperty(ref _addresses, value);
        }

        public Address SelectedAddress
        {
            get => _selectedAddress;
            set
            {
                if (value != _selectedAddress)
                {
                    SetProperty(ref _selectedAddress, value);
                    OnPropertyChanged(nameof(SelectedAddressAddressLine1));
                }
            }
        }

        public string SelectedAddressAddressLine1
        {
            get => SelectedAddress?.AddressLine1 ?? string.Empty;
            set
            {
                if (value != _selectedAddress.AddressLine1)
                {
                    SetProperty(_selectedAddress.AddressLine1, value, _selectedAddress,
                        (address, s) => address.AddressLine1 = s);
                    OnPropertyChanged(nameof(Addresses));
                }
            }
        }


        public ICommand UpdateAddressLine1Command { get; }

        public async Task InitializeAsync()
        {
            using (var bll = _exampleAppBllFactory.Create())
            {
                Addresses = new ObservableCollection<Address>(await bll.Addresses.AllAsync());
            }
        }

        public async Task UpdateAddressLine1()
        {
            var index = Addresses.IndexOf(SelectedAddress);
            using (var bll = _exampleAppBllFactory.Create())
            {
                SelectedAddress = bll.Addresses.Update(SelectedAddress);
                await bll.SaveChangesAsync();
            }

            Addresses[index] = SelectedAddress;
        }
    }
}