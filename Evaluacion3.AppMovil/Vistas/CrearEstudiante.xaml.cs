using Evaluacion3.Modelos.Modelos;
using Firebase.Database;
using Firebase.Database.Query;

namespace Evaluacion3.AppMovil.Vistas;

public partial class CrearEstudiante : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://evaluacion3-49d99-default-rtdb.firebaseio.com/");

    public List<Curso> Curso { set; get; }

    public CrearEstudiante()
	{
		InitializeComponent();
        ListarCursos();
        BindingContext = this;
    }
    private void ListarCursos()
    {
        var curso = client.Child("Curso").OnceAsync<Curso>();
        Curso = curso.Result.Select(x => x.Object).ToList();
    }

    private async void guardarButton_Clicked(object sender, EventArgs e)
    {
        Curso curso = cursoPicker.SelectedItem as Curso;
        var estudiante = new Estudiante
        {
            PrimerNombre = primerNombreEntry.Text,
            SegundoNombre = segundoNombreEntry.Text,
            PrimerApellido = primerApellidoEntry.Text,
            SegundoApellido = segundoApellidoEntry.Text,
            CorreoElectronico = correoEntry.Text,
            Edad = int.Parse(edadEntry.Text),
            Curso = curso
        };

        try
        {
            await client.Child("Estudiantes").PostAsync(estudiante);
            await DisplayAlert("Exito", $"El Estudiante {estudiante.PrimerNombre} {estudiante.PrimerApellido} fue guardado correctamente", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}