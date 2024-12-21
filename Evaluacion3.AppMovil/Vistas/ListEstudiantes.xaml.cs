namespace Evaluacion3.AppMovil.Vistas;

using Evaluacion3.Modelos.Modelos;
using Firebase.Database;
using Firebase.Database.Query;
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

    private async void CargarLista()
    {
        Lista.Clear();
        var estudiantes = await client.Child("Estudiantes").OnceAsync<Estudiante>();
        var estudiantesActivos = estudiantes.Where(e => e.Object.Estado == true).ToList();

        foreach (var estudiante in estudiantesActivos)
        {
            Lista.Add(new Estudiante
            {
                Id = estudiante.Key,
                PrimerNombre = estudiante.Object.PrimerNombre,
                SegundoNombre = estudiante.Object.SegundoNombre,
                PrimerApellido = estudiante.Object.PrimerApellido,
                SegundoApellido = estudiante.Object.SegundoApellido,
                CorreoElectronico = estudiante.Object.CorreoElectronico,
                Edad = estudiante.Object.Edad,
                Estado = estudiante.Object.Estado,
                Curso = estudiante.Object.Curso
            });
        }
        #region CodigoAntiguo
        //client.Child("Estudiantes").AsObservable<Estudiante>().Subscribe((estudiante) =>
        //{
        //    if (estudiante != null)
        //    {
        //        Lista.Add(estudiante.Object);
        //    }
        //});
        #endregion
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


    private async void EditarButton_Clicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;
        var estudiante = boton?.CommandParameter as Estudiante;

        if (estudiante != null && !string.IsNullOrEmpty(estudiante.Id))
        {
            await Navigation.PushAsync(new EditarEstudiante(estudiante.Id));
        }
        else
        {
            await DisplayAlert("Error", "No se puede obtener la información del estudiante", "OK");
        }
    }


    private async void DeshabilitarButton_Clicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;
        var estudiante = boton?.CommandParameter as Estudiante;

        if (estudiante is null)
        {
            await DisplayAlert("Error", "No se puede obtener la informacion del estudiante", "OK");
            return;
        }

        bool confirmacion = await DisplayAlert("Confirmación", $"Está seguro de deshabilitar al estudiante {estudiante.NombreCompleto}?", "Si", "No");

        if (confirmacion)
        {
            try
            {
                estudiante.Estado = false;
                await client.Child("Estudiantes").Child(estudiante.Id).PutAsync(estudiante);
                await DisplayAlert("Éxito", $"El estudiante {estudiante.NombreCompleto} ha sido deshabilitado con éxito!", "OK");
                CargarLista();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
