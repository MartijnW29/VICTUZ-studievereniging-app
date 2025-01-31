using VICTUZ_studievereniging_app.Services;
using VICTUZ_studievereniging_app.Classes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace VICTUZ_studievereniging_app.MainPages
{
    public partial class EventDetailsPage : ContentPage
    {
        private FirebaseHelper firebaseHelper;
        private Event currentEvent;
        private string ScannedId;

        public EventDetailsPage(Event selectedEvent)
        {
            InitializeComponent();
            firebaseHelper = new FirebaseHelper();
            currentEvent = selectedEvent;
            DisplayEventDetails();
        }

        private void DisplayEventDetails()
        {
            if (currentEvent != null)
            {
                eventNameLabel.Text = currentEvent.Name ?? string.Empty;
                eventDescriptionLabel.Text = currentEvent.Description ?? string.Empty;
                eventDateLabel.Text = $"Datum: {currentEvent.StartDateTime:dd MMM yyyy}";
                eventTimeLabel.Text = $"Tijd: {currentEvent.StartDateTime:HH:mm} - {currentEvent.EndDateTime:HH:mm}";
                var hostNames = string.Join(", ", currentEvent.Hosts?.Select(h => $"{h.Firstname} {h.Lastname}") ?? Enumerable.Empty<string>());
                eventHostsLabel.Text = $"Hosts: {hostNames}";
                registeredCountLabel.Text = $"Aantal Geregistreerde Deelnemers: {currentEvent.Registered?.Count ?? 0}";
                attendedCountLabel.Text = $"Aantal Aanwezige Deelnemers: {currentEvent.Attended?.Count ?? 0}";
                var registeredNames = string.Join(", ", currentEvent.Registered?.Select(r => $"{r.Firstname} {r.Lastname}") ?? Enumerable.Empty<string>());
                registeredNamesLabel.Text = $"Geregistreerde Deelnemers: {registeredNames}";
            }
        }


        private async void cameraView_CamerasLoaded(object sender, EventArgs e)
        {
            if (cameraView.Cameras.Count > 0)
            {
                cameraView.Camera = cameraView.Cameras.First();
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await cameraView.StopCameraAsync();
                    await cameraView.StartCameraAsync();
                });
            }
        }


        private async void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
        {
            if (args.Result != null)
            {
                ScannedId = args.Result[0].Text;
                var user = await firebaseHelper.GetUserById(ScannedId);

                if (user == null)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        barcodeResult.Text = "Gebruiker niet gevonden!";
                        registrationStatus.Text = string.Empty;
                    });
                    return;
                }

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    barcodeResult.Text = $"Gebruiker: {user.Firstname} {user.Lastname}";
                });

                bool isRegistered = currentEvent.Registered?.Any(r => r.Id == ScannedId) ?? false;
                bool isAttended = currentEvent.Attended?.Any(a => a.Id == ScannedId) ?? false;

                if (isAttended)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        registrationStatus.Text = "Gebruiker is al Toegelaten tot dit evenement";
                        registrationStatus.TextColor = Colors.Orange;
                        btnBinnenLaten.IsVisible = false;
                        btnAlsnogToelaten.IsVisible = false;
                    });
                }
                else if (isRegistered)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        registrationStatus.Text = "✅ Gebruiker is aangemeld voor dit evenement!";
                        registrationStatus.TextColor = Colors.Green;
                        btnBinnenLaten.IsVisible = true;
                        btnAlsnogToelaten.IsVisible = false;
                    });
                }
                else
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        registrationStatus.Text = "❌ Gebruiker is niet aangemeld voor dit evenement.";
                        registrationStatus.TextColor = Colors.Red;
                        btnBinnenLaten.IsVisible = false;
                        btnAlsnogToelaten.IsVisible = true;
                    });
                }
            }
        }


        private async void OnBinnenLatenClicked(object sender, EventArgs e)
        {
            await VoegGebruikerToeAanAanwezigen();
            btnBinnenLaten.IsVisible = false;
        }

        private async void OnAlsnogToelatenClicked(object sender, EventArgs e)
        {
            await VoegGebruikerToeAanAanwezigen();
            btnAlsnogToelaten.IsVisible = false;
        }

        private async Task VoegGebruikerToeAanAanwezigen()
        {
            
            var user = await firebaseHelper.GetUserById(ScannedId);

            if (user == null || currentEvent.Attended?.Any(a => a.Id == ScannedId) == true) return;

            currentEvent.Attended.Add(user);
            await firebaseHelper.UpdateItem("events", currentEvent.Id, currentEvent);
            await DisplayAlert("Succes", "Gebruiker toegevoegd aan aanwezigen!", "OK");
            DisplayEventDetails();
        }
    }
}
