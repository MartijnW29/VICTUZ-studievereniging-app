using VICTUZ_studievereniging_app.Classes;
using SQLite;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace VICTUZ_studievereniging_app.SQLite
{
    public class SQLiteDatabase
    {
        private SQLiteAsyncConnection Database;

        public SQLiteDatabase()
        {
        }

        async Task Init()
        {
            if (Database != null)
                return;

            Database = new SQLiteAsyncConnection(SQLiteConstants.DatabasePath, SQLiteConstants.Flags);
            var result = await Database.CreateTableAsync<LoggedInUser>();
        }

        public async Task<LoggedInUser> GetPreviouslyLoggedInUserAsync()
        {
            await Init();
            return await Database.Table<LoggedInUser>().FirstOrDefaultAsync();
        }

        public async Task<int> SaveUserAsync(LoggedInUser user)
        {
            try
            {
                await Init();

                // Kijk of er al een gebruiker bestaat met dezelfde email, zo ja, update
                var existingUser = await Database.Table<LoggedInUser>().FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    user.Id = existingUser.Id;  // Gebruik dezelfde Id als de bestaande gebruiker
                    return await Database.UpdateAsync(user);  // Update de bestaande gebruiker
                }
                else
                {
                    return await Database.InsertAsync(user);  // Voeg de gebruiker toe als die nog niet bestaat
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return 0;
            }
        }

        // Verwijder gebruiker op basis van string Id
        public async Task<int> DeleteUserAsync(string id)
        {
            await Init();
            return await Database.Table<LoggedInUser>()
                .Where(user => user.Id == id)
                .DeleteAsync();
        }

        // Verwijder de ingelogde gebruiker op basis van de string Id
        public async Task DeleteLoggedInUserAsync()
        {
            await Init();

            var loggedInUser = await Database.Table<LoggedInUser>().FirstOrDefaultAsync();
            if (loggedInUser != null)
            {
                await Database.DeleteAsync(loggedInUser);
            }
        }
    }
}
