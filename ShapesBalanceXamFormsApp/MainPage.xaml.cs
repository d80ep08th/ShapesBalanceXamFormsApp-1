using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

using static ShapesBalanceXamFormsApp.MainPage;

namespace ShapesBalanceXamFormsApp
{





    public partial class MainPage : ContentPage
    {
        public IEnumerable<Frontend.FSharp.Wallet> GetWallet()
        {


            //yield return new Wallet(50, Brush.Blue);
            yield return new Frontend.FSharp.Wallet(750, Brush.Gray);
            yield return new Frontend.FSharp.Wallet(325, Brush.Brown);
            yield return new Frontend.FSharp.Wallet(425, Brush.Green);

        }
        public MainPage()
        {
  
            IEnumerable<Frontend.FSharp.Wallet> wallet = GetWallet();
            Content = Frontend.FSharp.PieChart.create(wallet);


        }



  








    }
}


