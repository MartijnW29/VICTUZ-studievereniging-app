using VICTUZ_studievereniging_app.Classes;
using VICTUZ_studievereniging_app.Services;
using VICTUZ_studievereniging_app.SQLite;

namespace VICTUZ_studievereniging_app
{
    public partial class App : Application
    {
        public static User? CurrentUser { get; set; }
        SQLiteDatabase localDatabase;
        public static LoggedInUser? LoggedInUser { get; set; }
        public App()
        {
            InitializeComponent();

            localDatabase = new SQLiteDatabase();
            SetCurrentUserAsync();

            if (CurrentUser == null) // dit werkt nog niet is voor later
            {
                MainPage = new LoginPage();
                //MainPage = new NavigationPage(new LoginPage());

            }
            else
            {
                MainPage = new MainBar();
                //MainPage = new NavigationPage(new MainBar());
            }
        }

        private async Task SetCurrentUserAsync()
        {
            LoggedInUser = await localDatabase.GetPreviouslyLoggedInUserAsync();

            if (LoggedInUser != null)
            {
                var firebaseHelper = new FirebaseHelper();
                CurrentUser = await firebaseHelper.GetUserById(LoggedInUser.Id);
                Application.Current.MainPage = new MainBar();
            }
        }

    }
}
