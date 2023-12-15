using Microsoft.Maui.Controls;
using Plugin.Maui.Audio;
using Firebase.Auth;
using Firebase.Auth.Providers;
using System;
using Firebase.Storage;
using Plugin.Media;
using System.Globalization;
using PM2Examen3P.Models;

namespace PM2Examen3P
{
    public partial class MainPage : ContentPage
    {
        Plugin.Media.Abstractions.MediaFile photo_camera = null;

        public MainPage()
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
                ImgFoto.Source = ImageSource.FromStream(() => {
                    return photo_camera.GetStream();
                });
            }
        }

        private void btnGrabarAudio_Clicked(object sender, EventArgs e)
        {

        }

        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            string descripcion = txtDescripcion.Text;
            DateTime fechaini = datePicker.Date;
            byte[] image_to_array_bytes = image_to_array_byte();

            if (string.IsNullOrWhiteSpace(descripcion))
            {
                await DisplayAlert("Error", "Por favor, completa todos los campos.", "OK");
                return;
            }

            try
            {
                var firebaseInstance = Singleton.Instance;
                Notes nota = new Notes { descripcion = descripcion, fecha = fechaini, photo_record = image_to_array_bytes };

                await firebaseInstance.CreateData(nota);

                await DisplayAlert("Éxito", "Datos Guardados Correctamente.", "OK");

                txtDescripcion.Text = string.Empty;

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al guardar los datos: {ex.Message}", "OK");
            }
        }

        private async void btnListar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.ListNotes());
        }
    }
}