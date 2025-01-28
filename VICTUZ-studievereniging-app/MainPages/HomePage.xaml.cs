using VICTUZ_studievereniging_app.Classes;
using VICTUZ_studievereniging_app.Services;

namespace VICTUZ_studievereniging_app.MainPages;

public partial class HomePage : ContentPage
{
    private FirebaseHelper firebaseHelper;
    private Event nextEvent;

    public HomePage()
    {
        InitializeComponent();
        firebaseHelper = new FirebaseHelper();
        LoadNextEvent();
    }

    private async void LoadNextEvent()
    {
        // Haal alle evenementen op
        var allEvents = await firebaseHelper.GetItems<Event>("events");

        // Filter de aankomende evenementen en sorteer op startdatum
        nextEvent = allEvents
            .Where(e => e.StartDateTime > DateTime.Now)
            .OrderBy(e => e.StartDateTime)
            .FirstOrDefault();

        if (nextEvent != null)
        {
            // Vul de labels met evenementgegevens
            eventTitleLabel.Text = nextEvent.Name;
            eventDateLabel.Text = $"Op: {nextEvent.StartDateTime:dd MMM yyyy HH:mm}";
            eventDescriptionLabel.Text = nextEvent.Description;

            // Controleer of de inschrijfknop zichtbaar moet zijn
            var isUserHost = nextEvent.Hosts?.Any(h => h.Id == App.CurrentUser.Id) ?? false;
            var isUserRegistered = nextEvent.Registered?.Any(r => r.Id == App.CurrentUser.Id) ?? false;

            // Toon de knop alleen als de gebruiker niet de host is en niet is ingeschreven
            registerButton.IsVisible = !isUserHost && !isUserRegistered;
        }
        else
        {
            // Geen aankomende evenementen gevonden
            eventTitleLabel.Text = "Geen aankomende evenementen";
            eventDateLabel.Text = "";
            eventDescriptionLabel.Text = "";
            registerButton.IsVisible = false;
        }
    }

    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        if (nextEvent == null)
            return;

        if (nextEvent.Registered == null)
            nextEvent.Registered = new List<User>();

        // Voeg de gebruiker toe aan de lijst van ingeschreven gebruikers
        nextEvent.Registered.Add(App.CurrentUser);

        // Update het evenement in de database
        await firebaseHelper.UpdateItem("events", nextEvent.Id, nextEvent);

        // Toon een bevestiging
        await DisplayAlert("Succes", $"Je bent ingeschreven voor '{nextEvent.Name}'!", "OK");

        // Werk de knop bij
        registerButton.IsVisible = false;
    }
}
