﻿namespace Nicebike.Views;

public partial class TechnicianHome : ContentPage
{
    int technicianNumber;

    public TechnicianHome(int technicianNumber)
    {
        this.technicianNumber = technicianNumber;
        InitializeComponent();
    }
    private async void NavigateToStock(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StockManagement());
    }

    private async void NavigateToMakeBike(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.MakeBike(technicianNumber));
    }

    private async void NavigateToBuild(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Build(technicianNumber));
    }
}