using Prism;
using Prism.Ioc;
using ExpensesManagment.Prism.ViewModels;
using ExpensesManagment.Prism.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ExpensesManagment.Common.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ExpensesManagment.Prism
{
    public partial class App
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
        }
    }
}
