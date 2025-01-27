using Firebase.Database;
using Firebase.Database.Query;
using VICTUZ_studievereniging_app.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VICTUZ_studievereniging_app.Services
{
    public class FirebaseHelper
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseHelper()
        {
            // Vervang door jouw Firebase Database URL
            _firebaseClient = new FirebaseClient("https://victuz-64299-default-rtdb.europe-west1.firebasedatabase.app/");
        }

        public async Task AddEvent(Event newEvent)
        {
            // Voeg het evenement toe aan de Firebase-database
            var result = await _firebaseClient
                .Child("events") // Dit is de "node" waar evenementen worden opgeslagen
                .PostAsync(newEvent);

            // Krijg de unieke key (ID) van het evenement
            string eventId = result.Key;
            newEvent.Id = eventId; // Wijs de gegenereerde sleutel toe aan het event-object

            // Werk het evenement bij met de nieuwe ID
            await _firebaseClient
                .Child("events")
                .Child(eventId) // Gebruik de gegenereerde sleutel hier
                .PutAsync(newEvent);
        }



        public async Task UpdateSpecificUser(string userId, User updatedUser)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID mag niet leeg zijn.");
            }

            if (updatedUser == null)
            {
                throw new ArgumentNullException(nameof(updatedUser), "Updated user mag niet null zijn.");
            }

            await _firebaseClient
                .Child("users")
                .Child(userId)
                .PutAsync(updatedUser);
        }


        public async Task<User> GetUserById(string userId)
        {
            var users = await _firebaseClient
                .Child("users")
                .OnceAsync<User>();

            var user = users.FirstOrDefault(u => u.Object.Id == userId)?.Object;
            return user;
        }


        public async Task<string> CheckUserExistence(string email, string Username, string password)
        {
            var users = await _firebaseClient
                .Child("users")
                .OnceAsync<User>();

            foreach (var u in users)
            {
                if (u.Object.Email == email && u.Object.Password == password)
                {
                    return u.Object.Id;
                }
            }
            return null;
        }


        // Voeg een nieuwe gebruiker toe aan de database
        public async Task MakeAccount(User user)
        {
            // Voeg de gebruiker toe aan de Firebase-database
            var result = await _firebaseClient
                .Child("users")
                .PostAsync(user);

            // Haal de unieke Firebase-sleutel op en sla die op als Id
            string userId = result.Key;
            user.Id = userId; // Wijs de gegenereerde sleutel toe aan het user-object

            // Update de gebruiker in Firebase met de nieuwe Id
            await _firebaseClient
                .Child("users")
                .Child(userId) // Gebruik de gegenereerde sleutel hier
                .PutAsync(user);
        }




        // Voeg data toe
        public async Task AddItem<T>(T item, string node)
        {
            await _firebaseClient
                .Child(node)
                .PostAsync(item);
        }

        // Haal data op
        public async Task<List<T>> GetItems<T>(string node)
        {
            var items = await _firebaseClient
                .Child(node)
                .OnceAsync<T>();

            return items.Select(x => x.Object).ToList();
        }

        // Update data
        public async Task UpdateItem<T>(string node, string key, T item)
        {
            await _firebaseClient
                .Child(node)
                .Child(key)
                .PutAsync(item);
        }

        // Verwijder data
        public async Task DeleteItem(string node, string key)
        {
            await _firebaseClient
                .Child(node)
                .Child(key)
                .DeleteAsync();
        }
    }
}
