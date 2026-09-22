using EventClient.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace EventClient.Service
{
    public static class Api
    {
        // The Android emulator reaches your PC through 10.0.2.2;
        // Windows / iOS / Mac use localhost.
        static readonly HttpClient http = new()
        {
            BaseAddress = new Uri(DeviceInfo.Platform == DevicePlatform.Android
                ? "http://10.0.2.2:5191"
                : "http://localhost:5191")
        };

        public static async Task<List<Event>> GetEventsAsync()
            => await http.GetFromJsonAsync<List<Event>>("api/events") ?? new List<Event>();

        // Id == 0 means "new event" -> POST, otherwise -> PUT
        public static async Task SaveEventAsync(Event ev)
        {
            var response = ev.Id == 0
                ? await http.PostAsJsonAsync("api/events", ev)
                : await http.PutAsJsonAsync($"api/events/{ev.Id}", ev);

            response.EnsureSuccessStatusCode();
        }

        public static async Task DeleteEventAsync(int id)
        {
            var response = await http.DeleteAsync($"api/events/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
