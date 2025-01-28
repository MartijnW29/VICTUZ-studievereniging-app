using VICTUZ_studievereniging_app;
using VICTUZ_studievereniging_app.Services;
using VICTUZ_studievereniging_app.Classes;
using VICTUZ_studievereniging_app.SQLite; // nodig voor telefoon database


namespace VICTUZ_studievereniging_app.MainPages
{
    public partial class PlanningPage : ContentPage
    {
        private FirebaseHelper firebaseHelper;
        public static User CurrentUser => App.CurrentUser;

        public PlanningPage()
        {
            InitializeComponent();
            firebaseHelper = new FirebaseHelper();
            LoadUpcomingEvents();
        }

        private bool IsUserHost(Event eventToCheck)
        {
            // Check of de Hosts-lijst null is en of de huidige gebruiker al in de lijst staat
            return eventToCheck.Hosts != null && eventToCheck.Hosts.Any(h => h.Id == CurrentUser?.Id);
        }


        private async void LoadUpcomingEvents()
        {
            // Haal evenementen op die nog moeten plaatsvinden
            var allEvents = await firebaseHelper.GetItems<Event>("events");
            var upcomingEvents = allEvents.Where(e => e.StartDateTime > DateTime.Now).ToList();

            // Voeg een extra eigenschap toe voor de zichtbaarheid van de inschrijfknop
            foreach (var ev in upcomingEvents)
            {
                ev.IsUserNotHost = !IsUserHost(ev); // Zet de knop zichtbaarheid afhankelijk van of de gebruiker al host is
            }

            // Zet de lijst in de ListView
            eventsListView.ItemsSource = upcomingEvents;
        }


        // Event handler voor de inschrijfknop
        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var eventToRegister = (Event)button.BindingContext;

            if (eventToRegister.Registered == null)
                eventToRegister.Registered = new List<User>();

            // Controleer of de gebruiker al ingeschreven is
            if (eventToRegister.Registered.Any(user => user.Id == CurrentUser?.Id))
            {
                await DisplayAlert("Melding", $"Je bent al ingeschreven voor '{eventToRegister.Name}'.", "OK");
                return; // Stop de functie hier als de gebruiker al ingeschreven is
            }

            // Voeg de gebruiker toe aan de geregistreerde lijst
            eventToRegister.Registered.Add(CurrentUser);

            // Werk de database bij
            await firebaseHelper.UpdateItem("events", eventToRegister.Id, eventToRegister);

            // Laat een bevestiging zien
            await DisplayAlert("Succes", $"Je bent ingeschreven voor '{eventToRegister.Name}'!", "OK");

            // Herlaad de evenementen om de wijzigingen weer te geven
            LoadUpcomingEvents();
        }

    }
}
