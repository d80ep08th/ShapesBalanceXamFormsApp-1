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
    public partial class Normalization
    {
        public static IEnumerable<Percentage> Normalize(IEnumerable<Wallet> wallets)
        {


            double minimumShowablePercentage =  2;
            double visiblePercentageLimit = 1;
            double percent = 0;
            
            List<Percentage> Pies = new List<Percentage>();


            var total = wallets.Sum(x => x.CryptoValue);




            
            foreach(Wallet balance in wallets)
            {
                percent = balance.CryptoValue * 100 / total;



                
                if (100  >= percent && percent >= minimumShowablePercentage)
                {
                    //no normalization
                    Pies.Add(new Percentage(percent, balance.Stroke));
                }
                else if (minimumShowablePercentage > percent && percent >= visiblePercentageLimit)
                {
                    //normalize to 2%
                     
                    percent = minimumShowablePercentage;
                    Pies.Add(new Percentage(percent, balance.Stroke));
                }
                /*
                else if (visiblePercentageLimit > percent)
                {
                    //normalizes to 0%
                    percent = 0;
                    yield return new percentage(percent, balance.Stroke);
                }
                

                */
            }

            var wholePie = Pies.Sum(x => x.Percent);
            Pies = Pies.OrderByDescending(x => x.Percent).ToList();
            
            if(wholePie > 100)
            {
                Pies[0].Percent -= (wholePie - 100);

            }
            else if(wholePie < 100)
            {
                Pies[Pies.Count - 1].Percent += (100 - wholePie);
            }


            return Pies; 
            
        }
    }

    public partial class Beautification
    {


        public static string BeautifyAmount(string balance)
        {
            var digits = balance.Length;
            var position = 4;
            for (var i = 1; position*i <= digits; i++ )
            { 

                balance = balance.Insert(digits + 1 - position*i, ",");
                digits = balance.Length;
            }
            return balance;
        }



    }


    public partial class MainPage : ContentPage
    {



        public class Wallet
        {
            public Wallet(double value, Brush color)
            {
                CryptoValue = value;
                Stroke = color;
            }

            public double CryptoValue { get; private set; }
            public Brush Stroke { get; private set; }
        }

        public class Percentage
        {
            public Percentage(double value, Brush color)
            {

                if (Percent > 100 || 0 > Percent )
                {
                    throw new ArgumentException(" Value is not between 100 and 0");
                }

                Percent = value;
                Stroke = color;


            }

            public double Percent { get; set; }
            public Brush Stroke { get; private set; }
        }

        public IEnumerable<Wallet> GetAmount()
        {


            //yield return new Wallet(50, Brush.Blue);
            yield return new Wallet(750, Brush.Gray);
            yield return new Wallet(325, Brush.Brown);
            yield return new Wallet(425, Brush.Green);

        }

        public MainPage()
        {
            InitializeComponent();


            IEnumerable<Wallet> amounts = GetAmount();
            double amount = 0;

            foreach (Wallet bal in amounts)
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
            var beautifiedAmount = Beautification.BeautifyAmount(amount.ToString());

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



        public void makePies(Grid grid, IEnumerable<Wallet> amounts)
        {
            IEnumerable<Percentage> Pies = new List<Percentage>();
          
            int n = amounts.Count();
            
            double lengthOfArc;
            double arcAngle = 0;
            double total = 0;
            
            //manipulate the gap between the arcs
            double gap;




            foreach (Wallet p in amounts)
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

                renderArc(path, pathFigure, arcSegment, arcAngle + gap, lengthOfArc - gap * 2);


                path.Data = geometry;
                geometry.Figures = pathFigures;
                pathFigures.Add(pathFigure);
                pathFigure.Segments.Add(arcSegment);

                grid.Children.Add(path);

            }
            else
            {

                Pies = Normalization.Normalize(amounts);

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
                    arcAngle = setArcAngle(lengthOfArc, gap, arcAngle, path, pathFigure, arcSegment);






                    path.Data = geometry;
                    geometry.Figures = pathFigures;
                    pathFigures.Add(pathFigure);
                    pathFigure.Segments.Add(arcSegment);

                    grid.Children.Add(path);







                }
            }





        }

        public double setArcAngle(double lengthOfArc, double gap, double arcAngle, Path path, PathFigure pathFigure, ArcSegment arcSegment)
        {
            int lowestNaturalNumber = 1;

            if (lengthOfArc > lowestNaturalNumber)
            {
                renderArc(path, pathFigure, arcSegment, arcAngle + gap, lengthOfArc - gap * 2);
                arcAngle = arcAngle + lengthOfArc;

            }
            else
            {
                renderArc(path, pathFigure, arcSegment, arcAngle - gap, lengthOfArc + gap * 2);
                arcAngle = arcAngle + lengthOfArc;

            }

            return arcAngle;
        }
        private void renderArc(Path pathRoot, PathFigure pathFigure, ArcSegment ARC, double startAngle, double endAngle)
        {
            double Radius = 150;
            double angle = 0;
            bool largeArc = false;

            if (endAngle > 180)
                largeArc = true;

            pathRoot.StrokeLineCap = PenLineCap.Round;
            pathRoot.StrokeThickness = 12;
            ARC.SweepDirection = SweepDirection.Clockwise;
            ARC.Size = new Size(Radius, Radius);
            ARC.RotationAngle = angle;
            ARC.IsLargeArc = largeArc;
            //////////////////////////
            pathFigure.StartPoint = ComputeCartesianCoordinate(startAngle, Radius);
            ARC.Point = ComputeCartesianCoordinate(startAngle + endAngle, Radius);

            

        }

        private Point ComputeCartesianCoordinate(double angle, double radius)
        {
            //center (50,50)
            double cX = 50;
            double cY = 50;
            // convert to radians
            double angleRad = (Math.PI / 180.0) * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            x += radius + cX;
            y += radius + cY;

            return new Point(x, y);

        }







    }
}


