using EvernoteClone.Model;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteClone.ViewModel.Helpers
{
    public class DatabaseHelper
    {
        // Inserts an object into the database i.e. Note, Notebook.
        public async static Task<bool> Insert<T>(T item) where T : HasIdInterface
        {
            // Serialize the object into JSON.
            string jsonBody = JsonConvert.SerializeObject(item);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // Create new HTTP client.
            using (var client = new HttpClient())
            {
                // Send a post request with the content to insert into the correct table.
                var result = await client.PostAsync($"{Keys.dbPath}{item.GetType().Name.ToLower()}.json", content);

                // If successfully inserted, parse the newly created object's ID and update the object's ID field.
                if (result.IsSuccessStatusCode)
                {
                    var jsonResult = await result.Content.ReadAsStringAsync();
                    var deserializedJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);
                    foreach ( var o in deserializedJson)
                    {
                        item.Id = o.Value;
                    }
                    await Update(item);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Updates an object in the database.
        public async static Task<bool> Update<T>(T item) where T : HasIdInterface
        {
            // Serialize the object into JSON.
            string jsonBody = JsonConvert.SerializeObject(item);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // Create new HTTP client.
            using (var client = new HttpClient())
            {
                // Send a patch request with the content to update the object in the correct table.
                var result = await client.PatchAsync($"{Keys.dbPath}{item.GetType().Name.ToLower()}/{item.Id}.json", content);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Deletes an item in the database.
        public async static Task<bool> Delete<T>(T item) where T : HasIdInterface
        {
            using (var client = new HttpClient())
            {
                var result = await client.DeleteAsync($"{Keys.dbPath}{item.GetType().Name.ToLower()}/{item.Id}.json");

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Gets the list of items from a table in the database.
        public async static Task<List<T>> Read<T>() where T : HasIdInterface
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync($"{Keys.dbPath}{typeof(T).Name.ToLower()}.json");
                List<T> list = new List<T>();

                if (result.IsSuccessStatusCode)
                {
                    var jsonResult = await result.Content.ReadAsStringAsync();

                    var objects = JsonConvert.DeserializeObject<Dictionary<string, T>>(jsonResult);

                    if (objects != null)
                    {
                        foreach (var o in objects)
                        {
                            o.Value.Id = o.Key;
                            list.Add(o.Value);
                        }
                    }
                }

                return list;
            }
        }
    }
}
