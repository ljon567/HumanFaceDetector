using FaceDetector.DataModels;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaceDetector
{
    public class AzureManager
    {

        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<stats> statsTable; //Used for history display on AzureTable tab

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://ljon567.azurewebsites.net/");
            this.statsTable = this.client.GetTable<stats>(); //Get contents of database
        }

        public async Task<List<stats>> GetStatsInformation()
        {
            return await this.statsTable.ToListAsync();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }

        public async Task PostStatsInformation(stats statsInfo) 
        {
            await this.statsTable.InsertAsync(statsInfo);
        }

        //Clear all entries from database
        public async Task DeleteStatsInformation(stats statsInfo)
        {
            await this.statsTable.DeleteAsync(statsInfo);
        }

    }
}
