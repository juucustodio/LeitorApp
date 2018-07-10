using Plugin.TextToSpeech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Leitor
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Detalhe : ContentPage
	{
        String Texto = "";
		public Detalhe (string _texto)
		{
			InitializeComponent ();
            Texto = _texto;
            lblText.Text = Texto;

            BindingContext = this;
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            IsBusy = true;
            await CrossTextToSpeech.Current.Speak(Texto);
            IsBusy = false;
        }

        private async void Ouvir(object sender, EventArgs e)
        {
            IsBusy = true;
            await CrossTextToSpeech.Current.Speak(Texto);
            IsBusy = false;
        }
    }
}