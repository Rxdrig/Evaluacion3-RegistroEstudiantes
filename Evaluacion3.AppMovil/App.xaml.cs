using Evaluacion3.AppMovil.Vistas;

namespace Evaluacion3.AppMovil
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage (new ListEstudiantes());
        }
    }
}
