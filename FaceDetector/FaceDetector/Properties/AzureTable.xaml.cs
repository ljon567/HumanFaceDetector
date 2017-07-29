using FaceDetector.DataModels;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FaceDetector
{
    public partial class AzureTable : ContentPage
    {

        public AzureTable()
        {
            InitializeComponent();

        }

        //Updates history (a list) on page with latest entries from Azure database/table
        async void ClickUpdate(object sender, System.EventArgs e)
        {
            List<stats> statsInformation = await AzureManager.AzureManagerInstance.GetStatsInformation(); 

            StatsList.ItemsSource = statsInformation;
        }

        //Clear entries from database and updates current tab to show deletions
        async void ClickDelete(object sender, System.EventArgs e)
        {        
            List<stats> statsToDelete = await AzureManager.AzureManagerInstance.GetStatsInformation();

            foreach (var prob in statsToDelete)
            {
                await AzureManager.AzureManagerInstance.DeleteStatsInformation(prob);
            }

            List<stats> statsInformation = await AzureManager.AzureManagerInstance.GetStatsInformation();

            StatsList.ItemsSource = statsInformation;
        }

    }
}
