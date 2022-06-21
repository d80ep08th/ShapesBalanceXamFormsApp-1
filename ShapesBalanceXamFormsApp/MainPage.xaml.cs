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

        public IEnumerable<Frontend.FSharp.Wallet> GetAmount()
        {


            //yield return new Wallet(50, Brush.Blue);
            yield return new Frontend.FSharp.Wallet(750, Brush.Gray);
            yield return new Frontend.FSharp.Wallet(325, Brush.Brown);
            yield return new Frontend.FSharp.Wallet(425, Brush.Green);

        }

        public MainPage()
        {
            InitializeComponent();


            IEnumerable<Frontend.FSharp.Wallet> amounts = GetAmount();
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



            makePies(grid, amounts);
            
            Content = grid;



        }



        public void makePies(Grid grid, IEnumerable<Frontend.FSharp.Wallet> amounts)
        {
            IEnumerable<Frontend.FSharp.Percentage> Pies = new List<Frontend.FSharp.Percentage>();
          
            int n = amounts.Count();
            
            double lengthOfArc;
            double arcAngle = 0;
            double total = 0;
            
            //manipulate the gap between the arcs
            double gap;




            foreach (Frontend.FSharp.Wallet p in amounts)
                total += p.CryptoValue;

            if (n == 1)
            {


                lengthOfArc = 360;
                gap = 0;

                Path path = new Path { Stroke = amounts.ElementAt(0).Stroke };
                PathGeometry geometry = new PathGeometry();
                PathFigureCollection pathFigures = new PathFigureCollection();
                PathFigure pathFigure = new PathFigure();
                PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
                ArcSegment arcSegment = new ArcSegment();

                Frontend.FSharp.PieChart.renderArc(path, pathFigure, arcSegment, arcAngle + gap, lengthOfArc - gap * 2);


                path.Data = geometry;
                geometry.Figures = pathFigures;
                pathFigures.Add(pathFigure);
                pathFigure.Segments.Add(arcSegment);

                grid.Children.Add(path);

            }
            else
            {

                Pies = Frontend.FSharp.PieChart.Normalize(amounts);

                for (int i = 0; i < n; i++)
                {
                    lengthOfArc = (Pies.ElementAt(i).Percent) * 360 / 100;
                   
                    Path path = new Path { Stroke = Pies.ElementAt(i).Stroke };
                    PathGeometry geometry = new PathGeometry();
                    PathFigureCollection pathFigures = new PathFigureCollection();
                    PathFigure pathFigure = new PathFigure();
                    PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
                    ArcSegment arcSegment = new ArcSegment();

                    gap = 2.5;
                    //lengthOfArc = setArcSizeAndArcAngle(lengthOfArc, gap, arcAngle, path, pathFigure, arcSegment);
                    arcAngle = Frontend.FSharp.PieChart.setArcAngle(lengthOfArc, gap, arcAngle, path, pathFigure, arcSegment);






                    path.Data = geometry;
                    geometry.Figures = pathFigures;
                    pathFigures.Add(pathFigure);
                    pathFigure.Segments.Add(arcSegment);

                    grid.Children.Add(path);







                }
            }





        }








    }
}


