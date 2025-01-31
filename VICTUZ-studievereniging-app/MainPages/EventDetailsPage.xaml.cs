using VICTUZ_studievereniging_app.Services;
using VICTUZ_studievereniging_app.Classes;
using System;



namespace VICTUZ_studievereniging_app.MainPages
{
    public partial class EventDetailsPage : ContentPage
    {
        private FirebaseHelper firebaseHelper;
        private Event currentEvent;

        public EventDetailsPage(Event selectedEvent)
        {
            InitializeComponent();
            firebaseHelper = new FirebaseHelper();
            currentEvent = selectedEvent;
            DisplayEventDetails();
        }

        // Methode om de evenementdetails weer te geven
        private void DisplayEventDetails()
        {
            if (currentEvent != null)
            {
                eventNameLabel.Text = currentEvent.Name ?? string.Empty;
                eventDescriptionLabel.Text = currentEvent.Description ?? string.Empty;
                eventDateLabel.Text = $"Datum: {currentEvent.StartDateTime:dd MMM yyyy}";
                eventTimeLabel.Text = $"Tijd: {currentEvent.StartDateTime:HH:mm} - {currentEvent.EndDateTime:HH:mm}";

                // Hosts weergeven
                var hostNames = string.Join(", ", currentEvent.Hosts?.Select(h => $"{h.Firstname} {h.Lastname}") ?? Enumerable.Empty<string>());
                eventHostsLabel.Text = $"Hosts: {hostNames}";

                // Aantal geregistreerde deelnemers weergeven
                registeredCountLabel.Text = $"Aantal Geregistreerde Deelnemers: {currentEvent.Registered?.Count ?? 0}";

                // Aantal aanwezige deelnemers weergeven
                attendedCountLabel.Text = $"Aantal Aanwezige Deelnemers: {currentEvent.Attended?.Count ?? 0}";

                // Namen van de geregistreerde deelnemers weergeven
                var registeredNames = string.Join(", ", currentEvent.Registered?.Select(r => $"{r.Firstname} {r.Lastname}") ?? Enumerable.Empty<string>());
                registeredNamesLabel.Text = $"Geregistreerde Deelnemers: {registeredNames}";
            }
        }

        // Wanneer de gebruiker op de "Bewerk Evenement" knop klikt
        private void OnEditEventClicked(object sender, EventArgs e)
        {
            // Verberg de niet-bewerkbare labels en toon de bewerkbare velden
            eventNameLabel.IsVisible = false;
            eventDescriptionLabel.IsVisible = false;
            eventDateLabel.IsVisible = false;
            eventTimeLabel.IsVisible = false;
            eventHostsLabel.IsVisible = false;

            eventNameEntry.IsVisible = true;
            eventDescriptionEditor.IsVisible = true;

            // Toon de opslaan knop
            saveEventButton.IsVisible = true;
            editEventButton.IsVisible = false;
        }

        // Wanneer de gebruiker op de "Opslaan" knop klikt
        private async void OnSaveEventClicked(object sender, EventArgs e)
        {
            // Werk de huidige gebeurtenis bij met de nieuwe waarden
            currentEvent.Name = eventNameEntry.Text;
            currentEvent.Description = eventDescriptionEditor.Text;

            // Werk het evenement bij in Firebase
            await firebaseHelper.UpdateItem("events", currentEvent.Id, currentEvent);

            // Toon een succesbericht
            await DisplayAlert("Succes", "Evenement bijgewerkt!", "OK");

            // Vernieuw de details en maak de velden weer verborgen
            DisplayEventDetails();

            // Toon de bewerkknop en verberg de opslaan knop
            saveEventButton.IsVisible = false;
            editEventButton.IsVisible = true;
        }



        private void cameraView_CamerasLoaded(object sender, EventArgs e)
        {
            if (cameraView.Cameras.Count > 0)
            {
                cameraView.Camera = cameraView.Cameras.First();
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await cameraView.StopCameraAsync();
                    await cameraView.StartCameraAsync();
                });
            }
        }



        private void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (args.Result != null)
                {
                    string scannedUserId = args.Result[0].Text; // QR-code ID ophalen

                    // Zoek de gebruiker in de database
                    var user = await firebaseHelper.GetUserById(scannedUserId);

                    if (user != null)
                    {
                        barcodeResult.Text = $"Gebruiker: {user.Firstname} {user.Lastname}";

                        // Controleren of gebruiker zich heeft geregistreerd voor het evenement
                        bool isRegistered = currentEvent.Registered?.Any(r => r.Id == scannedUserId) ?? false;

                        if (isRegistered)
                        {
                            registrationStatus.Text = "✅ Gebruiker is aangemeld voor dit evenement!";
                            registrationStatus.TextColor = Colors.Green;
                        }
                        else
                        {
                            registrationStatus.Text = "❌ Gebruiker is niet aangemeld voor dit evenement.";
                            registrationStatus.TextColor = Colors.Red;
                        }
                    }
                    else
                    {
                        barcodeResult.Text = "Gebruiker niet gevonden!";
                        registrationStatus.Text = string.Empty;
                    }
                }
            });
        }








    }
}
