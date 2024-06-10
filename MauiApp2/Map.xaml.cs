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
        // Nahrazení "API_KEY" vaším skuteèným Mapy.cz API klíèem
       

        // Vytvoøení URL pro získání interaktivní mapy
        var mapUrl = $"https://frame.mapy.cz/turisticka";

        // Vytvoøení WebView pro zobrazení mapy
        var webView = new WebView
        {
            Source = mapUrl,
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        // Pøidání WebView do obsahu stránky
        Content = new StackLayout
        {
            Children = { webView }
        };
    }
}