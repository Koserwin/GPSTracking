namespace MauiApp2;

public partial class Map : ContentPage
{
    public Map()
    {
        InitializeComponent();
        LoadMap();
    }

    private async void LoadMap()
    {
        // Nahrazen� "API_KEY" va��m skute�n�m Mapy.cz API kl��em
       

        // Vytvo�en� URL pro z�sk�n� interaktivn� mapy
        var mapUrl = $"https://frame.mapy.cz/turisticka";

        // Vytvo�en� WebView pro zobrazen� mapy
        var webView = new WebView
        {
            Source = mapUrl,
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        // P�id�n� WebView do obsahu str�nky
        Content = new StackLayout
        {
            Children = { webView }
        };
    }
}