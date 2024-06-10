using Newtonsoft.Json;

namespace MauiApp2;

public partial class Record : ContentPage
{
    private List<Route> seznam { get; set; }
    private string waypoints = "";
    private bool ride = false;
    private bool ridestop = false;
    private int TimeToCheckWaypontsInSeconds = 10;

    public Record()
    {
        InitializeComponent();
        Pause_BTN.IsEnabled = false;
        Stop_BTN.IsEnabled = false;
        seznam = LoadRoutes();

    }
    DateOnly date;
    TimeOnly time;

    private async void Start_Clicked(object sender, EventArgs e)
    {
        waypoints = "";
        time = new TimeOnly();
        date = DateOnly.FromDateTime(DateTime.Now);
        

        Status.Text = "Record starting";
        entry_name.IsEnabled = false;
        Save_BTN.IsEnabled = false;
        Start_BTN.IsEnabled = false;
        ride = true;
        await GetLocation();
        Status.Text = "Record started";
        Pause_BTN.IsEnabled = true;
        Stop_BTN.IsEnabled = true;
        CheckRoute();
        CheckTime();

    }

    private async void Pause_Clicked(object sender, EventArgs e)
    {
        if (Pause_BTN.Text == "Pause")
        {
            Pause_BTN.Text = "Resume";
            Status.Text = "Record paused";
            Pause_BTN.IsEnabled = false;
            Stop_BTN.IsEnabled = false;
            ride = false;
            await GetLocation();
            Pause_BTN.IsEnabled = true;
            Stop_BTN.IsEnabled = true;

        }
        else
        {
            Pause_BTN.Text = "Pause";
            Status.Text = "Record started";
            Pause_BTN.IsEnabled = false;
            Stop_BTN.IsEnabled = false;
            ride = true;
            await GetLocation();
            Pause_BTN.IsEnabled = true;
            Stop_BTN.IsEnabled = true;
            CheckRoute();
            CheckTime();


        }
        //Pause_BTN.Text == "Pause" ? Pause_BTN.Text = "Resume" : Pause_BTN.Text = "Pause";


    }

    private async void Stop_Clicked(object sender, EventArgs e)
    {
        Status.Text = "Record stopped";
        Pause_BTN.IsEnabled = false;
        Stop_BTN.IsEnabled = false;
        ride = false;
        ridestop = true;
        await GetLocation();
        entry_name.IsEnabled = true;
        Save_BTN.IsEnabled = true;
        ridestop = false;

    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        if (entry_name.Text != null)
        {
            entry_name.IsEnabled = false;
            Save_BTN.IsEnabled = false;
            Start_BTN.IsEnabled = true;
            seznam.Add(new Route(entry_name.Text, waypoints, time, date));
            entry_name.Text = "";
            SaveList();
            TimeL.Text = "0:0:0";
        }
        else
        await DisplayAlert("alert", "select a name", "ok");
    }

    private async Task CheckTime()
    {
        while (ride)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            time = time.Add(TimeSpan.FromSeconds(1));
            TimeL.Text = $"{time.Hour.ToString()}:{time.Minute.ToString()}:{time.Second.ToString()}";
        }
    }
    private async Task CheckRoute()
    {
        while (ride) 
        {
            await Task.Delay(TimeSpan.FromSeconds(TimeToCheckWaypontsInSeconds));
            if (ride)
                GetLocation();
        }
    }

    private CancellationTokenSource _cancelTokenSource;
    private async Task GetLocation()
    {
        try
        {

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();

            Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            string lon = "";
            string lat = "";
            if (location != null)
            {
                //await DisplayAlert("s", $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}", "ok");
                lon = $"{location.Longitude}";
                lat = $"{location.Latitude}";
                lon = lon.Replace(',', '.');
                lat = lat.Replace(',', '.');
            }
            if (!ridestop)
                waypoints += lon + "," + lat + ";";
            else
                waypoints += lon + "," + lat;

        }
        // Catch one of the following exceptions:
        //   FeatureNotSupportedException
        //   FeatureNotEnabledException
        //   PermissionException
        catch (Exception ex)
        {
            // Unable to get location
        }
        
    }

    private void SaveList()
    {
        // Uložení seznamu do bezpeèného úložištì
        string serializedRoutes = JsonConvert.SerializeObject(seznam);
        SecureStorage.SetAsync("MyRoutes", serializedRoutes);
        
    }


    private List<Route> LoadRoutes()
    {
        // Naètení seznamu knih z bezpeèného úložištì, pokud existuje
        if (ListExists())
        {
            string serializedRoutes = SecureStorage.GetAsync("MyRoutes").Result;
            return JsonConvert.DeserializeObject<List<Route>>(serializedRoutes);
        }

        // Pokud seznam neexistuje, vytvoøíme nový
        return new List<Route>();
    }

    private bool ListExists()
    {
        try
        {
            string storedRoutes = SecureStorage.GetAsync("MyRoutes").Result;
            return !string.IsNullOrEmpty(storedRoutes);
        }
        catch (Exception)
        {
            // Zpracujte výjimku, pokud chcete
            return false;
        }
    }
}