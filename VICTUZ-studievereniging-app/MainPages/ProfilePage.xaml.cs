using VICTUZ_studievereniging_app.SQLite; // Nodig voor de database
using VICTUZ_studievereniging_app.Services;

namespace VICTUZ_studievereniging_app.MainPages
{
    public partial class ProfilePage : ContentPage
    {
        SQLiteDatabase localdatabase;

        public ProfilePage()
        {
            InitializeComponent();
            localdatabase = new SQLiteDatabase();
        }

        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Uitloggen", "Weet je zeker dat je wilt uitloggen?", "Ja", "Nee");
            try
            {

            
            if (confirm)
            {
                // Verwijder de huidige gebruiker uit de lokale database
                //await localdatabase.DeleteLoggedInUserAsync();
                await localdatabase.DeleteUserAsync(App.CurrentUser.Id);

                // Maak de CurrentUser in de app leeg
                App.CurrentUser = null;

                // Navigeer terug naar de inlogpagina
                Application.Current.MainPage = new LoginPage();
            }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fout", $"Er is iets misgegaan: {ex.Message}", "OK");
            }
            
        }
    }
}
