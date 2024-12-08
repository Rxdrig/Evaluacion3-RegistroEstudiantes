using Evaluacion3.Modelos.Modelos;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Logging;

namespace Evaluacion3.AppMovil
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            Registrar();
            return builder.Build();
           
        }

        public static void Registrar() 
        {
            FirebaseClient client = new FirebaseClient("https://evaluacion3-49d99-default-rtdb.firebaseio.com/");

            var cursos = client.Child("Curso").OnceAsync<Curso>();

            if (cursos.Result.Count == 0) 
            {
                client.Child("Curso").PostAsync(new Curso { Nombre = "1ero Básico" });
                client.Child("Curso").PostAsync(new Curso { Nombre = "2do Básico" });
                client.Child("Curso").PostAsync(new Curso { Nombre = "3ero Básico" });
                client.Child("Curso").PostAsync(new Curso { Nombre = "4to Básico" });
                client.Child("Curso").PostAsync(new Curso { Nombre = "5to Básico" });
                client.Child("Curso").PostAsync(new Curso { Nombre = "6to Básico" });
                client.Child("Curso").PostAsync(new Curso { Nombre = "7mo Básico" });
                client.Child("Curso").PostAsync(new Curso { Nombre = "8vo Básico" });
                client.Child("Curso").PostAsync(new Curso { Nombre = "1ero Medio" });
                client.Child("Curso").PostAsync(new Curso { Nombre = "2do Medio" });
                client.Child("Curso").PostAsync(new Curso { Nombre = "3ero Medio" });
                client.Child("Curso").PostAsync(new Curso { Nombre = "4to Medio" });
            }
        }
    }
}
