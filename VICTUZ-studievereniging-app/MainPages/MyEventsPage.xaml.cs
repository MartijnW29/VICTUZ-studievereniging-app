using VICTUZ_studievereniging_app.Services;
using VICTUZ_studievereniging_app.Classes;
using System.Linq;
using Camera.MAUI;

namespace VICTUZ_studievereniging_app.MainPages
{
    public partial class MyEventsPage : ContentPage
    {
        private FirebaseHelper firebaseHelper;

        public MyEventsPage()
        {
            InitializeComponent();
            firebaseHelper = new FirebaseHelper();
            LoadHostedEvents(); // Laad de evenementen bij het starten van de pagina
        }

        private async void LoadHostedEvents()
        {
            try
            {
                // Haal de evenementen op uit Firebase
                var allEvents = await firebaseHelper.GetItems<Event>("events");

                // Laat alleen toekomstige evenementen zien
                var hostedEvents = allEvents
                    .Where(e => e.Hosts != null && e.Hosts.Any(h => h.Id == App.CurrentUser?.Id)
                                && e.StartDateTime > DateTime.Now) // comment deze om oude events te zien
                                
                    .ToList();

                // Zet de evenementen als de bron van de ListView
                eventsListView.ItemsSource = hostedEvents;

            }
            catch (Exception ex)
            {
                // Foutmelding voor als er iets mis gaat bij het ophalen van de evenementen
                System.Diagnostics.Debug.WriteLine($"Fout bij het laden van evenementen: {ex.Message}");
            }
        }

        // Methode om naar de EventDetailsPage te navigeren
        private async void OnEventTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                Event selectedEvent = e.Item as Event;

                // Navigeer naar EventDetailsPage en geef het geselecteerde evenement door
                await Navigation.PushAsync(new EventDetailsPage(selectedEvent));
            }
        }

        // Methode om het formulier zichtbaar te maken
        private void OnCreateEventFormClicked(object sender, EventArgs e)
        {
            // Toggle de zichtbaarheid van het formulier
            eventForm.IsVisible = !eventForm.IsVisible;
        }

        // Wanneer de gebruiker op de knop "Maak Event Aan" klikt
        private async void OnCreateEventClicked(object sender, EventArgs e)
        {
            // Verkrijg de waarden van de velden
            string eventName = eventNameEntry.Text;
            string eventDescription = eventDescriptionEntry.Text;
            DateTime eventDate = eventStartDate.Date;
            TimeSpan startTime = eventStartTime.Time;
            TimeSpan endTime = eventEndTime.Time;

            // Combineer de datum en tijd
            DateTime startDateTime = eventDate.Add(startTime);
            DateTime endDateTime = eventDate.Add(endTime);

            if (string.IsNullOrEmpty(eventName) || string.IsNullOrEmpty(eventDescription))
            {
                await DisplayAlert("Fout", "Vul alle velden in", "OK");
            }
            else
            {
                // Maak het evenement object
                Event newEvent = new Event
                {
                    Name = eventName,
                    Description = eventDescription,
                    StartDateTime = startDateTime,
                    EndDateTime = endDateTime,
                    Hosts = new List<User> { App.CurrentUser } // Voeg de host (de huidige gebruiker) toe aan het evenement
                };

                // Sla het evenement op in de database
                await firebaseHelper.AddEvent(newEvent);

                // Toon een succesbericht
                await DisplayAlert("Succes", $"Evenement '{eventName}' is aangemaakt!", "OK");

                // Reset de velden
                ResetFields();

                // Maak het formulier weer verborgen na het aanmaken van het evenement
                eventForm.IsVisible = false;

                // Laad de evenementen opnieuw
                Application.Current.MainPage = new MainBar(1);
            }
        }

        private void ResetFields()
        {
            // Zet alle velden terug naar hun oorspronkelijke waarde (leeg of standaard)
            eventNameEntry.Text = string.Empty;
            eventDescriptionEntry.Text = string.Empty;
            eventStartDate.Date = DateTime.Now; // Zet naar de huidige datum
            eventStartTime.Time = TimeSpan.Zero; // Zet naar middernacht
            eventEndTime.Time = TimeSpan.Zero; // Zet naar middernacht
        }
    }
}
