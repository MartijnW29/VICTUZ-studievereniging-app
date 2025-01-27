using VICTUZ_studievereniging_app.Classes;
using VICTUZ_studievereniging_app.Services;

namespace VICTUZ_studievereniging_app.MainPages;

public partial class CreateEventPage : ContentPage
{
    private readonly FirebaseHelper _firebaseHelper = new();

    public CreateEventPage()
    {
        InitializeComponent();
    }

    private async void OnAddEventClicked(object sender, EventArgs e)
    {
        var newEvent = new Event
        {
            Name = NameEntry.Text,
            Description = DescriptionEditor.Text,
            StartDateTime = StartDatePicker.Date,
            EndDateTime = EndDatePicker.Date,
            RoomId = LocationEntry.Text // Je kunt hier een echte locatie-id invullen
        };

        // Voeg het event toe aan de Firebase database
        await _firebaseHelper.AddItem(newEvent, "events");

        await DisplayAlert("Succes", "Event succesvol toegevoegd!", "OK");
        await Navigation.PopAsync(); // Ga terug naar de vorige pagina
    }
}
