using Plugin.Media;
using PM2Examen3P.Models;
using static Android.Provider.ContactsContract.CommonDataKinds;
using Microsoft.Maui.Controls;
using System.IO;

namespace PM2Examen3P.Views;

public partial class UpdateNotes : ContentPage
{
    private Notes Notas;
    bool tomarfoto=false;
    Plugin.Media.Abstractions.MediaFile photo_camera = null;
    public UpdateNotes()
	{
		InitializeComponent();
	}
    public byte[] image_to_array_byte()
    {
        if (photo_camera != null)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                Stream stream = photo_camera.GetStream();
                stream.CopyTo(memory);
                byte[] data = memory.ToArray();
                return data;
            }
        }

        return null;
    }

    public void SetNotes(Notes notas)
    {
        Notas = notas;
        byte[] imageBytes = notas.photo_record;
        ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));

        photo.Source = imageSource;
        txtDescripcionUpd.Text = notas.descripcion;
        fechaPicker.Date = notas.fecha;


    }

    private async void btnTomarFoto_Clicked(object sender, EventArgs e)
    {
        photo_camera = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
        {
            Directory = "MiAlbum",
            Name = "Foto.jpg",
            SaveToAlbum = true
        });

        if (photo_camera != null)
        {
            photo.Source = ImageSource.FromStream(() => {
                return photo_camera.GetStream();
            });
        }
        tomarfoto = true;
    }

    private async void btnActualizar_Clicked(object sender, EventArgs e)
    {
        if (Notas != null)
        {
            string descripAntigua = Notas.descripcion;
            DateTime fechaAntigua = Notas.fecha;
            byte[] antiguaphoto = Notas.photo_record;

            Notas.descripcion = txtDescripcionUpd.Text;
            Notas.fecha = fechaPicker.Date;
            if(tomarfoto)
            {
                Notas.photo_record = image_to_array_byte();
            }

            try
            {
                var firebaseInstance = Singleton.Instance;

                await firebaseInstance.UpdateData(Notas.id_nota.ToString(), Notas);

                await DisplayAlert("Éxito", "Notas actualizados correctamente.", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Notas.descripcion = descripAntigua;
                Notas.fecha = fechaAntigua;
                Notas.photo_record = antiguaphoto;

                await DisplayAlert("Error", $"Error al actualizar las Notas: {ex.Message}", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "No se ha seleccionado ninguna Nota para actualizar.", "OK");
        }
    }

    private async void btnLista_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ListNotes());
    }
}