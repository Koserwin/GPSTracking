﻿namespace MauiApp2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("RouteMap", typeof(RouteMap));
        }
    }
}
