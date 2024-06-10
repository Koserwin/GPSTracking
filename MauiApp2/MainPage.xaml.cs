using Newtonsoft.Json;

namespace MauiApp2
{
    public partial class MainPage : ContentPage
    {
        public List<Route> routes;

        public MainPage()
        {
            InitializeComponent();
            routes = LoadRoutes();
            MyList.ItemsSource = routes;

            Refresh();
        }
        

        private async Task Refresh()
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                routes = LoadRoutes();
                MyList.ItemsSource = null;
                MyList.ItemsSource = routes;
            }
        }

        private List<Route> LoadRoutes()
        {
            // Načtení seznamu knih z bezpečného úložiště, pokud existuje
            if (ListExists())
            {
                string serializedRoutes = SecureStorage.GetAsync("MyRoutes").Result;
                return JsonConvert.DeserializeObject<List<Route>>(serializedRoutes);
            }

            // Pokud seznam neexistuje, vytvoříme nový
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

        Route selected;
        private async void MyList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selected = (sender as ListView)?.SelectedItem as Route;
            if (selected != null)
            {
                SaveList();
                await Shell.Current.GoToAsync("RouteMap");
               
            }
            else
            {
                
            }
        }
        private void SaveList()
        {
            // Uložení seznamu do bezpečného úložiště
            string serializedRoutes = JsonConvert.SerializeObject(selected);
            SecureStorage.SetAsync("SelectedRoute", serializedRoutes);

        }
    }

}
