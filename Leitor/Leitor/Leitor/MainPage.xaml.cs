using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.TextToSpeech;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Leitor
{
	public partial class MainPage : ContentPage
	{
        private readonly IVisionServiceClient _visionServiceClient;
        private string url = "";
        string resultado = "";
        public MainPage()
        {
            InitializeComponent();

            _visionServiceClient = new VisionServiceClient("KEY", "URL");

            BindingContext = this;
        }

        private async Task TakePicture(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
            {
                await DisplayAlert("Ops", "Nenhuma câmera detectada.", "OK");

                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = false,
                    PhotoSize = PhotoSize.Medium
                });

            
            if (file == null)
                return;

            IsBusy = true;
            Elementos.IsVisible = false;
            await CreateHandwriting(file.AlbumPath);

            await GetHandwriting(url);

            IsBusy = false;
            Elementos.IsVisible = true;

        }


        public async Task CreateHandwriting(string image)
        {
            // Call the Vision API.
            try
            {
                using (Stream imageFileStream = File.OpenRead(image))
                {
                    var result = await _visionServiceClient.CreateHandwritingRecognitionOperationAsync(imageFileStream);

                    url = result.Url;

                }
            }
            // Catch and display Vision API errors.
            catch (ClientException c)
            {
                await DisplayAlert("Error", c.Message, "OK");
            }
            // Catch and display all other errors.
            catch (Exception e)
            {
                await DisplayAlert("Error", "Ops... Algo deu errado na análise da imagem. :( ", "OK");
            }
        }


        public async Task GetHandwriting(string text)
        {
            HandwritingRecognitionOperation operation = new HandwritingRecognitionOperation();
            operation.Url = text;
            resultado = "";

            // Call the Vision API.
            try
            {
                var result = await _visionServiceClient.GetHandwritingRecognitionOperationResultAsync(operation);


                foreach (var line in result.RecognitionResult.Lines)
                {
                    foreach (var word in line.Words)
                    {
                        resultado += word.Text + " ";
                    }
                }

                if (!String.IsNullOrWhiteSpace(resultado))
                    await Navigation.PushAsync(new Detalhe(resultado));
                else
                    await DisplayAlert("", "Nenhum texto encontrado :(", "OK");
            }
            // Catch and display Vision API errors.
            catch (ClientException c)
            {
                await DisplayAlert("Error", c.Message, "ok");
            }
            // Catch and display all other errors.
            catch (Exception e)
            {
                await DisplayAlert("Error", "Ops... Algo deu errado na análise da imagem. :( ", "OK");
            }
        }

        private void InfoClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InfoPage());
        }
    }
}
