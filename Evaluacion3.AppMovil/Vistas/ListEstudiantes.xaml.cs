namespace Evaluacion3.AppMovil.Vistas;

using Evaluacion3.Modelos.Modelos;
using Firebase.Database;
using System.Collections.ObjectModel;

public partial class ListEstudiantes : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://evaluacion3-49d99-default-rtdb.firebaseio.com/");
    public ObservableCollection<Estudiante> Lista { get; set; } = new ObservableCollection<Estudiante>();

    public ListEstudiantes()
	{
		InitializeComponent();
        BindingContext = this;
        CargarLista();
    }

    private void CargarLista()
    {
        client.Child("Estudiantes").AsObservable<Estudiante>().Subscribe((estudiante) =>
        {
            if (estudiante != null)
            {
                Lista.Add(estudiante.Object);
            }
        });
    }

    private void filtroSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string filtro = filtroSearchBar.Text.ToLower();

        if (filtro.Length > 0) 
        {
            ListaCollection.ItemsSource = Lista.Where(x => x.NombreCompleto.ToLower().Contains(filtro));
        }
        else 
        {
            ListaCollection.ItemsSource = Lista;
        }
    }

    private async void NuevoEstudianteBoton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CrearEstudiante());
    }
}