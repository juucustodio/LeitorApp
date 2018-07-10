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
	public partial class InfoPage : ContentPage
	{
		public InfoPage ()
		{
			InitializeComponent ();
		}

        private void Facebook(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://facebook.com/juucustodio"));
        }

        private void Twitter(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://twitter.com/juucustodio"));
        }

        private void GitHub(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://github.com/juucustodio"));
        }
    }
}