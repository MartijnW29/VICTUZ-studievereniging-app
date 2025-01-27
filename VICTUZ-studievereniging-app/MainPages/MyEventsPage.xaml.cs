namespace VICTUZ_studievereniging_app.MainPages
{
    public partial class MyEventsPage : ContentPage
    {
        public MyEventsPage()
        {
            InitializeComponent();
        }

        // Wanneer de gebruiker op de knop "Maak Event Aan" klikt
        private async void OnCreateEventClicked(object sender, EventArgs e)
        {
            // Toon een simpele popup voor de naam en beschrijving van het evenement
            string eventName = await DisplayPromptAsync("Evenementnaam", "Voer de naam van het evenement in");
            string eventDescription = await DisplayPromptAsync("Beschrijving", "Voer een beschrijving in");

            if (string.IsNullOrEmpty(eventName) || string.IsNullOrEmpty(eventDescription))
            {
                await DisplayAlert("Fout", "Vul alle velden in", "OK");
            }
            else
            {
                // Hier kun je het evenement opslaan in je database of in een lijst
                await DisplayAlert("Succes", $"Evenement '{eventName}' is aangemaakt!", "OK");
            }
        }
    }
}
