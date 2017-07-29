using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using FaceDetector.Model;
using Xamarin.Forms;
using FaceDetector.DataModels;

namespace FaceDetector
{
    public partial class CustomVision : ContentPage
    {
        public CustomVision()
        {
            InitializeComponent();
        }

        private async void loadCamera(object sender, EventArgs e) 
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                return file.GetStream();
            });

            await MakePredictionRequest(file);
        }

        //Convert image file into format that can be utilized by the A.I
        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        async Task MakePredictionRequest(MediaFile file)
        {
            Contract.Ensures(Contract.Result<Task>() != null);

            var client = new HttpClient();

            //The following key and url taken from customvision.ai
            client.DefaultRequestHeaders.Add("Prediction-Key", "2470ae283d364813a0e2854a5d09e524");

            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/01bcff2d-5c19-4177-9a46-f4a137226ce7/image?iterationId=6fc16337-2cc3-4fd0-b4de-b6fa682c693f";

            HttpResponseMessage response;

            byte[] byteData = GetImageAsByteArray(file);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    //Put contents of responseString into table
                    EvaluationModel responseModel = JsonConvert.DeserializeObject<EvaluationModel>(responseString);

                    double max = responseModel.Predictions.Max(m => m.Probability); //Probability of humanity value

                    stats model = new stats()
                    {
                        Probability = (float)max
                    };

                    //Update Azure database/table with new entry
                    await AzureManager.AzureManagerInstance.PostStatsInformation(model); 

                    //Final result based on whether face has at least fifty percent chance of being human
                    TagLabel.Text = (max >= 0.5) ? "Humanity confirmed" : "Humanity questionable";

                }

                file.Dispose();
            }
        }
    }
}
