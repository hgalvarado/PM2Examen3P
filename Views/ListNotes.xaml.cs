namespace PM2Examen3P.Views;
using Firebase.Database;
using Microsoft.Maui.Controls;
using PM2Examen3P.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using static Android.Provider.ContactsContract.CommonDataKinds;

public partial class ListNotes : ContentPage
{
    public ObservableCollection<Notes> list_Notes { get; set; }
    

    public ListNotes()
    {
		InitializeComponent();

        list_Notes = new ObservableCollection<Notes>();
        list.ItemsSource = list_Notes;
        LoadData();

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadData();
    }
    public async void SetListNotes(ObservableCollection<Notes> notas)
    {
        var notasOrdenadas = notas.OrderByDescending(n => n.fecha);
        list_Notes.Clear();
        foreach (var note in notasOrdenadas)
        {
            list_Notes.Add(note);
        }
    }
    private async Task LoadData()
    {
        try
        {
            var firebaseInstance = Singleton.Instance;

            var notes = await firebaseInstance.ReadData();

            SetListNotes(new ObservableCollection<Notes>(notes));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar datos: {ex.Message}", "OK");
        }
    }

    private async void list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        var selecnotas = (Notes)e.SelectedItem;

        var action = await DisplayActionSheet($"Opciones para {selecnotas.id_nota}", "Cancelar", null, "Editar", "Eliminar");

        switch (action)
        {
            case "Editar":
                var update = new UpdateNotes();
                
                update.BindingContext = selecnotas;
                update.SetNotes(selecnotas);
                await Navigation.PushAsync(update);
                break;
            case "Eliminar":
                var firebaseInstance = Singleton.Instance;
                try
                {

                    Console.WriteLine("error: " + selecnotas.id_nota);
                    await firebaseInstance.DeleteData(selecnotas.id_nota.ToString());
                    await LoadData();
                    await DisplayAlert("Éxito", "Nota eliminada correctamente.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Error al eliminar la Nota: {ex.Message}", "OK");
                }
                break;
        }

        list.SelectedItem = null;

    }
}