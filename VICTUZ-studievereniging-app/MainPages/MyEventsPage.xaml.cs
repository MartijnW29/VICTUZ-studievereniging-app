using VICTUZ_studievereniging_app.Services;
using VICTUZ_studievereniging_app.Classes;

namespace VICTUZ_studievereniging_app.MainPages
{

    public partial class MyEventsPage : ContentPage
    {
        private FirebaseHelper firebaseHelper;

        public MyEventsPage()
        {
            InitializeComponent();
            firebaseHelper = new FirebaseHelper();
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
                    EndDateTime = endDateTime
                };

                // Sla het evenement op in de database
                await firebaseHelper.AddItem(newEvent, "events");

                // Toon een succesbericht
                await DisplayAlert("Succes", $"Evenement '{eventName}' is aangemaakt!", "OK");

                // Reset de velden
                ResetFields();

                // Maak het formulier weer verborgen na het aanmaken van het evenement
                eventForm.IsVisible = false;
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