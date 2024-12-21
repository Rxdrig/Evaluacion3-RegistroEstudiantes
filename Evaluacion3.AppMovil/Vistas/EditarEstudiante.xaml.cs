using Evaluacion3.Modelos.Modelos;
using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.ObjectModel;

namespace Evaluacion3.AppMovil.Vistas;

public partial class EditarEstudiante : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://evaluacion3-49d99-default-rtdb.firebaseio.com/");
    public List<Curso> Cursos { get; set; }
    public ObservableCollection<string> ListaCursos { get; set; } = new ObservableCollection<string>();
    private Estudiante estudianteActual = new Estudiante();
    private string estudianteId;


    public EditarEstudiante(string idEstudiante)
    {
        InitializeComponent();
        BindingContext = this;
        estudianteId = idEstudiante;
        CargarListaCursos();
        CargarEstudiante(estudianteId);

    }

    private async void CargarListaCursos()
    {
        try
        {
            var cursos = await client.Child("Curso").OnceAsync<Curso>();
            ListaCursos.Clear();
            foreach (var curso in cursos)
            {
                ListaCursos.Add(curso.Object.Nombre);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error:" + ex.Message, "OK");
        }
    }


    private async void CargarEstudiante(string idEstudiantes)
    {
        var estudiante = await client.Child("Estudiantes").Child(idEstudiantes).OnceSingleAsync<Estudiante>();

        if (estudiante != null)
        {
            EditprimerNombreEntry.Text = estudiante.PrimerNombre;
            EditsegundoNombreEntry.Text = estudiante.SegundoNombre;
            EditprimerApellidoEntry.Text = estudiante.PrimerApellido;
            EditsegundoApellidoEntry.Text = estudiante.SegundoApellido;
            EditcorreoEntry.Text = estudiante.CorreoElectronico;
            EditEdadEntry.Text = estudiante.Edad.ToString();
            EditcursoPicker.SelectedItem = estudiante.Curso?.Nombre;
        }
    }

    private async void ActualizarButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(EditprimerNombreEntry.Text) ||
                string.IsNullOrWhiteSpace(EditsegundoNombreEntry.Text) ||
                string.IsNullOrWhiteSpace(EditprimerApellidoEntry.Text) ||
                string.IsNullOrWhiteSpace(EditsegundoApellidoEntry.Text) ||
                string.IsNullOrWhiteSpace(EditcorreoEntry.Text) ||
                string.IsNullOrWhiteSpace(EditEdadEntry.Text) ||
                EditcursoPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Todos los campos son obligatiorios", "OK");
                return;
            }

            if (!EditcorreoEntry.Text.Contains("@"))
            {
                await DisplayAlert("Error", "El correo no es valido", "OK");
                return;
            }

            if (!int.TryParse(EditEdadEntry.Text, out var edad))
            {
                await DisplayAlert("Error", "La edad ingresada no es valida", "OK");
                return;
            }

            if (edad <= 6)
            {
                await DisplayAlert("Error", "La edad debe mayor a 6 años", "OK");
                return;
            }

            estudianteActual.Id = estudianteId;
            estudianteActual.PrimerNombre = EditprimerNombreEntry.Text.Trim();
            estudianteActual.SegundoNombre = EditsegundoNombreEntry.Text.Trim();
            estudianteActual.PrimerApellido = EditprimerApellidoEntry.Text.Trim();
            estudianteActual.SegundoApellido = EditsegundoApellidoEntry.Text.Trim();
            estudianteActual.CorreoElectronico = EditcorreoEntry.Text.Trim();
            estudianteActual.Edad = edad;
            estudianteActual.Curso = new Curso { Nombre = EditcursoPicker.SelectedItem.ToString() };
            estudianteActual.Estado = editEstadoSwitch.IsToggled;

            await client.Child("Estudiantes").Child(estudianteActual.Id).PutAsync(estudianteActual);
            await DisplayAlert("Éxito", "El estudiante se ha modificado de manera correcta", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception) 
        {
            throw;
        }
    }
}