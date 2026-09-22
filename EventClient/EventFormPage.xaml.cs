using EventClient.Models;
using EventClient.Service;
using System.Collections.ObjectModel;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace EventClient;

public partial class EventFormPage : ContentPage
{
    public Event Event { get; }
    public ObservableCollection<Attendee> Attendees { get; }

    public EventFormPage(Event ev)
    {
        Event = ev;
        Attendees = new ObservableCollection<Attendee>(ev.Attendees);
        Title = ev.Id == 0 ? "New Event" : "Edit Event";
        InitializeComponent();
        BindingContext = this; // XAML binds to Event.Title, Attendees ...
        Photo.Source = Base64ToImageConverter.ToImageSource(ev.ImageUrl);
    }

    async void OnChooseImageClicked(object? sender, EventArgs e)
    {
        try
        {
            var file = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Choose a picture",
                FileTypes = FilePickerFileType.Images
            });

            if (file is null)
                return;

            using var stream = await file.OpenReadAsync();
            using var memory = new MemoryStream();
            await stream.CopyToAsync(memory);

            Event.ImageUrl = Convert.ToBase64String(memory.ToArray());
            Photo.Source = Base64ToImageConverter.ToImageSource(Event.ImageUrl);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    void OnAddAttendeeClicked(object? sender, EventArgs e)
    {
        Attendees.Add(new Attendee()); // a new empty row to fill in
    }

    void OnRemoveAttendeeClicked(object? sender, EventArgs e)
    {
        if (sender is Button { BindingContext: Attendee attendee })
            Attendees.Remove(attendee);
    }

    async void OnSaveClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Event.Title))
        {
            await DisplayAlertAsync("Required", "Please enter a title.", "OK");
            return;
        }

        // keep only the attendee rows that were actually filled in
        Event.Attendees = Attendees
            .Where(a => !string.IsNullOrWhiteSpace(a.Name)
                     || !string.IsNullOrWhiteSpace(a.Email))
            .ToList();

        try
        {
            await Api.SaveEventAsync(Event);
            await Navigation.PopAsync(); // back to the list, which reloads itself
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
}