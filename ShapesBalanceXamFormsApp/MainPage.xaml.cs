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

        public MainPage()
        {
            InitializeComponent();


            IEnumerable<Frontend.FSharp.Wallet> amounts = Frontend.FSharp.PieChart.getWallet;
            double amount = 0;

            foreach (Frontend.FSharp.Wallet bal in amounts)
            {
                amount += bal.CryptoValue;
            }

            Grid grid = new Grid
            {
                RowDefinitions = { new RowDefinition() },
                ColumnDefinitions = { new ColumnDefinition() },

            };




            Label approx = new Label
            {
                Text = "~",
                FontSize = 25,
                HorizontalOptions = LayoutOptions.Center,
                TranslationY = 160,
            TranslationX = -40
            };
            var beautifiedAmount = Frontend.FSharp.PieChart.beautifyAmount(amounts);

            Label balance = new Label
            {
                Text = beautifiedAmount,
                FontSize = 25,
                HorizontalOptions = LayoutOptions.Center,
                TranslationY = 160
            };

            
            Label currency = new Label
            {
                Text = "U.S.D.",
                FontAttributes = FontAttributes.Bold,
                FontSize = 24,
                HorizontalOptions = LayoutOptions.Center,
                TranslationY = 160,
                TranslationX = 65
            };

            Label total_tag = new Label
            {
                Text = "Amount Balance",
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Center,
                TranslationY = 200
            };

            grid.Children.Add(approx);
            grid.Children.Add(currency);
            grid.Children.Add(balance);
            grid.Children.Add(total_tag);



            Frontend.FSharp.PieChart.makePies(grid, amounts);
            
            Content = grid;



        }



  








    }
}


