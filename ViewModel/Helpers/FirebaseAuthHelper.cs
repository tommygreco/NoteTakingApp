﻿using EvernoteClone.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace EvernoteClone.ViewModel.Helpers
{
    public class FirebaseAuthHelper
    {
        public static async Task<bool> Register(User user)
        {
            using (HttpClient client = new HttpClient())
            {
                // Create a new body for registration.
                var body = new
                {
                    email = user.Email,
                    password = user.Password,
                    returnSecureToken = true
                };

                // Serialize and send the post request.
                string jsonBody = JsonConvert.SerializeObject(body);
                var data = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={Keys.firebaseKey}", data);
                
                // Set the user ID in the application if registered properly. Show error otherwise.
                if (response.IsSuccessStatusCode)
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<FirebaseResult>(resultJson);
                    App.UserId = result.localId;
                    return true;
                }
                else
                {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<FirebaseError>(errorJson);
                    MessageBox.Show(error.error.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        public static async Task<bool> Login(User user)
        {
            using (HttpClient client = new HttpClient())
            {
                // Create a new body for login.
                var body = new
                {
                    email = user.Email,
                    password = user.Password,
                    returnSecureToken = true
                };

                // Serialize and send the post request.
                string jsonBody = JsonConvert.SerializeObject(body);
                var data = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={Keys.firebaseKey}", data);

                // Set the user ID in the application if registered properly. Show error otherwise.
                if (response.IsSuccessStatusCode)
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<FirebaseResult>(resultJson);
                    App.UserId = result.localId;
                    return true;
                }
                else
                {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeObject<FirebaseError>(errorJson);
                    MessageBox.Show(error.error.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }
    }
}
