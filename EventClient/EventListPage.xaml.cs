using EventClient.Models;
using EventClient.Service;

namespace EventClient;

public partial class EventListPage : ContentPage
{
    public EventListPage()
    {
        InitializeComponent();
    }

    // Runs the first time AND every time we come back from the form page,
    // so the list is always fresh.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAsync();
    }

    async Task LoadAsync()
    {
        try
        {
            EventsView.ItemsSource = await Api.GetEventsAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error",
                "Could not load events. Is the Server running?\n" + ex.Message, "OK");
        }
    }

    async void OnAddClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new EventFormPage(new Event()));
    }

    async void OnEditClicked(object? sender, EventArgs e)
    {
        // The button's BindingContext is the event of that row
        if (sender is Button { BindingContext: Event ev })
            await Navigation.PushAsync(new EventFormPage(ev));
    }

    async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (sender is not Button { BindingContext: Event ev })
            return;

        bool yes = await DisplayAlertAsync("Delete", $"Delete {ev.Title}?", "Yes", "No");
        if (!yes)
            return;

        try
        {
            await Api.DeleteEventAsync(ev.Id);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
}
