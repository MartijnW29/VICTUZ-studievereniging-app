using VICTUZ_studievereniging_app;
using VICTUZ_studievereniging_app.Services;
using VICTUZ_studievereniging_app.SQLite; // nodig voor telefoon database



namespace VICTUZ_studievereniging_app
{
    public partial class LoginPage : ContentPage
    {
        private readonly FirebaseHelper _firebaseHelper;
        SQLiteDatabase localdatabase;

        public LoginPage()
        {
            InitializeComponent();
            localdatabase = new SQLiteDatabase();
            _firebaseHelper = new FirebaseHelper();  // Firebase helper voor database-interactie
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            bool isEmailEmpty = string.IsNullOrEmpty(EmailEntry.Text);
            bool isPasswordEmpty = string.IsNullOrEmpty(PasswordEntry.Text);

            if (isEmailEmpty)
            {
                EmailEntry.Placeholder = "U bent dit invulveld vergeten!\nVul je email in!";
            }
            else if (isPasswordEmpty)
            {
                PasswordEntry.Placeholder = "U bent dit invulveld vergeten!\nVul je wachtwoord in!";
            }
            else
            {
                string email = EmailEntry.Text;
                string password = PasswordEntry.Text;

                var userId = await _firebaseHelper.CheckUserExistence(email, password);

                if (userId != null)
                {
                    var user = await _firebaseHelper.GetUserById(userId);

                    if (user != null)
                    {
                        // Sla de ingelogde gebruiker op in de applicatie
                        App.CurrentUser = user;

                        // Vul de velden in op de UI
                        FirstnameEntry.Text = user.Firstname;
                        LastnameEntry.Text = user.Lastname;
                    }

                    Application.Current.MainPage = new MainBar();
                }
                else
                {
                    await DisplayAlert("Fout", "Ongeldige e-mail of wachtwoord.", "OK");
                }
            }
        }


        private async void OnCreateAccountButtonClicked(object sender, EventArgs e)
        {
            bool isFirstnameEmpty = string.IsNullOrEmpty(FirstnameEntry.Text);
            bool isLastnameEmpty = string.IsNullOrEmpty(LastnameEntry.Text);
            bool isEmailEmpty = string.IsNullOrEmpty(EmailEntry.Text);
            bool isPasswordEmpty = string.IsNullOrEmpty(PasswordEntry.Text);

            // Controleer of de invoervelden leeg zijn
            if (isFirstnameEmpty)
            {
                FirstnameEntry.Placeholder = "U bent dit invulveld vergeten!";
            }
            else if (isLastnameEmpty)
            {
                LastnameEntry.Placeholder = "U bent dit invulveld vergeten!";
            }
            else if (isEmailEmpty)
            {
                EmailEntry.Placeholder = "U bent dit invulveld vergeten!";
            }
            else if (isPasswordEmpty)
            {
                PasswordEntry.Placeholder = "U bent dit invulveld vergeten!";
            }
            else
            {
                // Verkrijg gegevens uit invoervelden
                string firstname = FirstnameEntry.Text;
                string lastname = LastnameEntry.Text;
                string email = EmailEntry.Text;
                string password = PasswordEntry.Text;

                // Maak een nieuw User-object
                var newUser = new Classes.User
                {
                    Firstname = firstname,
                    Lastname = lastname,
                    Email = email,
                    Password = password
                };

                // Voeg de nieuwe gebruiker toe aan de database
                await _firebaseHelper.MakeAccount(newUser);

                // Bevestiging geven en terug naar de loginpagina
                await DisplayAlert("Account aangemaakt", "Je account is succesvol aangemaakt!", "OK");
                Application.Current.MainPage = new LoginPage();
            }
        }


    }
}
