using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firebase.Database.Query;
using System.Threading.Tasks;


namespace PM2Examen3P.Models
{
    public class Singleton
    {
        private static Singleton instance = null;
        private readonly FirebaseClient firebaseClient;

        private Singleton()
        {
            firebaseClient = new FirebaseClient("https://pm2examen3p-default-rtdb.firebaseio.com/");
        }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }

                return instance;
            }
        }

        public async Task CreateData(Notes data)
        {
            await firebaseClient
                .Child("Notas")
                .PostAsync(data);
        }

        public async Task<List<Notes>> ReadData()
        {
            var peopleList = await firebaseClient
                .Child("Notas")
                .OnceAsync<Notes>();

            return peopleList.Select(item => {
                var people = item.Object;
                people.id_nota = item.Key; // Asigna el ID de Firebase al objeto people
                return people;
            }).ToList();
        }

        public async Task UpdateData(string key, Notes data)
        {
            await firebaseClient
                .Child("Notas")
                .Child(key)
                .PutAsync(data);
        }

        public async Task DeleteData(string key)
        {
            await firebaseClient
                .Child("Notas")
                .Child(key)
                .DeleteAsync();
        }
    }
}

