using VICTUZ_studievereniging_app.Classes;
using VICTUZ_studievereniging_app;
using VICTUZ_studievereniging_app.Services;

namespace VICTUZ_studievereniging_app.MainPages
{
    public partial class MyEventsPage : ContentPage
    {
        private readonly FirebaseHelper _firebaseHelper;

        public MyEventsPage()
        {
            InitializeComponent();
            _firebaseHelper = new FirebaseHelper(); // FirebaseHelper instantie
        }

        // Wanneer de gebruiker op de knop "Maak Event Aan" klikt
        private async void OnCreateEventClicked(object sender, EventArgs e)
        {
            // Vraag naar de naam en beschrijving van het evenement
            string eventName = await DisplayPromptAsync("Evenementnaam", "Voer de naam van het evenement in");
            string eventDescription = await DisplayPromptAsync("Beschrijving", "Voer een beschrijving in");

            // Vraag de start- en einddatum van het evenement
            string startDateTimeStr = await DisplayPromptAsync("Startdatum", "Voer de startdatum van het evenement in (bijv. 2025-01-30 14:00)");
            string endDateTimeStr = await DisplayPromptAsync("Einddatum", "Voer de einddatum van het evenement in (bijv. 2025-01-30 16:00)");

            // Vraag extra velden
            string joinLimitStr = await DisplayPromptAsync("Deelnemerslimiet", "Voer de deelnemerslimiet in");
            string roomId = await DisplayPromptAsync("Room ID", "Voer de zaal-ID in");

            // Controleer of alle velden zijn ingevuld
            if (string.IsNullOrEmpty(eventName) || string.IsNullOrEmpty(eventDescription) || string.IsNullOrEmpty(startDateTimeStr) || string.IsNullOrEmpty(endDateTimeStr) || string.IsNullOrEmpty(joinLimitStr) || string.IsNullOrEmpty(roomId))
            {
                await DisplayAlert("Fout", "Vul alle velden in", "OK");
                return;
            }

            // Probeer de datums om te zetten
            if (!DateTime.TryParse(startDateTimeStr, out DateTime startDateTime) || !DateTime.TryParse(endDateTimeStr, out DateTime endDateTime))
            {
                await DisplayAlert("Fout", "De opgegeven datums zijn ongeldig", "OK");
                return;
            }

            // Probeer de deelnemerslimiet om te zetten naar een integer
            if (!int.TryParse(joinLimitStr, out int joinLimit))
            {
                await DisplayAlert("Fout", "De deelnemerslimiet moet een getal zijn", "OK");
                return;
            }

            // Maak het evenement object
            var newEvent = new Event
            {
                Name = eventName,
                Description = eventDescription,
                StartDateTime = startDateTime,
                EndDateTime = endDateTime,
                JoinLimit = joinLimit,
                RoomId = roomId,
                Registered = new List<User>(), // Voeg een lege lijst toe voor geregistreerde gebruikers
                Attended = new List<User>(),   // Voeg een lege lijst toe voor aanwezige gebruikers
                Hosts = new List<User>(),      // Voeg een lege lijst toe voor hosts
                Categories = new List<Category>() // Voeg een lege lijst toe voor categorieën
            };

            // Voeg het evenement toe aan Firebase
            await _firebaseHelper.AddEvent(newEvent);

            // Toon een succesbericht
            await DisplayAlert("Succes", $"Evenement '{eventName}' is succesvol aangemaakt!", "OK");
        }
    }
}
