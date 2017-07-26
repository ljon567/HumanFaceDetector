using FaceDetector.DataModels;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FaceDetector
{
    public partial class AzureTable : ContentPage
    {

        MobileServiceClient client = AzureManager.AzureManagerInstance.AzureClient;

        public AzureTable()
        {
            InitializeComponent();

        }

        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            List<stats> statsInformation = await AzureManager.AzureManagerInstance.GetStatsInformation();

            StatsList.ItemsSource = statsInformation;
        }

    }
}
