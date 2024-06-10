using Newtonsoft.Json;
using System;

namespace MauiApp2;

public partial class RouteMap : ContentPage
{
	Route route;
    int zoom = 11;
	public RouteMap()
	{
		InitializeComponent();

		route = LoadRoute();
        nazev.Text = route.Name;
        cas.Text = route.Time.ToString()+"h";
        datum.Text = route.Date.ToString();
        Show();
    }

    private async void Show()
    {
        string api = "FhAdtf3kvx4ITWbASrIcdiE5d40q2tVTa8KTT9TzM7c";
        int width = 400;
        int height = 470;
        string url = $"https://api.mapy.cz/v1/static/map?lon={route.CenterLongitude}&lat={route.CenterLatitude}&zoom={zoom}&width={width}&height={height}&mapset=outdoor&markers=color:blue;size:small;{route.Waypoints}&markers=color:red;size:normal;label:S;{route.Startpoint}&markers=color:red;size:normal;label:F;{route.Stoppoint}&apikey={api}";
        staticMap.Source = new UriImageSource
        {
            Uri = new Uri(url),
            CachingEnabled = false // Nastavte na true, pokud chcete povolit ukládání do mezipamìti
        };

    }

    List<Route> routes;
    private async void delete_Clicked(object sender, EventArgs e)
    {
        routes = LoadRoutes();
        routes.Remove(route);
        SaveList();
        await Shell.Current.GoToAsync("..");
    }

    private void button_plus_Clicked(object sender, EventArgs e)
    {
        zoom++;
        Show();
    }

    private void button_minus_Clicked(object sender, EventArgs e)
    {
        zoom--;
        Show();
    }


    private void SaveList()
    {
        // Uložení seznamu do bezpeèného úložištì
        string serializedRoutes = JsonConvert.SerializeObject(routes);
        SecureStorage.SetAsync("MyRoutes", serializedRoutes);

    }
    private Route LoadRoute()
    {
        // Naètení seznamu knih z bezpeèného úložištì,
        string serializedRoutes = SecureStorage.GetAsync("SelectedRoute").Result;
        return JsonConvert.DeserializeObject<Route>(serializedRoutes);
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

