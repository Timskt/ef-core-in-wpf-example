using App.Contracts.BLL;

namespace WpfAppWithAttribute
{
    public interface IBLLViewModel
    {
        IAppBLL BLL { get; set; }
    }
}