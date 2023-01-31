using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using App.BLL.DTO;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace WpfAppWithAbstractClass
{
    public class MainWindowViewModel : BLLViewModel
    {
        private ObservableCollection<Address> _addresses;
        private Address _selectedAddress;

        public MainWindowViewModel(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
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
            await ScopeAsync(async () =>
            {
                Addresses = new ObservableCollection<Address>(await BLL.Addresses.AllAsync());
            });
        }

        public async Task UpdateAddressLine1()
        {
            var index = Addresses.IndexOf(SelectedAddress);
            await ScopeAsync(async () =>
            {
                SelectedAddress = BLL.Addresses.Update(SelectedAddress);
                await BLL.SaveChangesAsync();
            });
            Addresses[index] = SelectedAddress;
        }
    }
}