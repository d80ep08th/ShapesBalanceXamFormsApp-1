namespace Frontend.FSharp

open System
open System.Linq
open System.Text
open System.Globalization
open System.IO
open Xamarin.Forms
open Xamarin.Forms.Shapes
open System.Threading.Tasks
open System.ComponentModel
open Microsoft.FSharp.Collections

type Wallet = { CryptoValue: double ; Stroke: Brush }
type Percentage =  { mutable Percent: double;  Stroke: Brush } 


module PieChart =

    let computeCartesianCoordinate (angle:double) (radius:double) = 

        let centerX = 50.
        let centerY = 50.

        let angleRad = (Math.PI / 180.) * (angle - 90.)

        let x = radius *  Math.Cos(angleRad) + (radius + centerX)
        
        let y = radius *  Math.Sin(angleRad) + (radius + centerY)

        Point(x,y)

    let renderArc (pathRoot:Path) (pathFigure:PathFigure) (arc:ArcSegment) (startAngle:double) (endAngle:double) = 
        
        let radius = 150.
        let angle =  0.
        let largeArc = 
            if endAngle > 180. then
                true
            else
                false

        pathRoot.StrokeLineCap <- PenLineCap.Round
        pathRoot.StrokeThickness <- 12.
        
        arc.SweepDirection <- SweepDirection.Clockwise
        arc.Size <- Size(radius, radius)
        arc.RotationAngle <- angle
        arc.IsLargeArc <- largeArc

        pathFigure.StartPoint <- computeCartesianCoordinate startAngle  radius
        arc.Point <- computeCartesianCoordinate (startAngle + endAngle) radius
        ()

    let setArcAngle (lengthOfArc:double) (gap:double) (arcAngle:double) (path:Path) (pathFigure:PathFigure) (arcSegment:ArcSegment) =
    
        let lowestNaturalNumber = 1.
        let arcAngle =
            if lengthOfArc > lowestNaturalNumber then
                renderArc path pathFigure arcSegment (arcAngle + gap) (lengthOfArc - gap * 2.)
                arcAngle + lengthOfArc                            
            else
                renderArc path pathFigure arcSegment (arcAngle - gap) (lengthOfArc + gap * 2.)
                arcAngle + lengthOfArc
        arcAngle

    let Normalize (wallets: seq<Wallet>)  =
        
        let minimumShowablePercentage =  2.
        let visiblePercentageLimit = 1.
        let fullPie = 100.

        let total = wallets |> Seq.sumBy(fun x -> x.CryptoValue)
            //wallets |> Seq.sum(fun x -> x.CryptoValue)
            //wallets |> List.sumBy(fun x -> x.CryptoValue)
        
        let innerNormalize (wallet: Wallet) : Option<Percentage> =
            
            let percent = wallet.CryptoValue * fullPie / total
            
            if fullPie  >= percent && percent >= minimumShowablePercentage then        
      
                {Percentage.Percent = percent; Stroke = wallet.Stroke} |> Some
            
            elif minimumShowablePercentage > percent && percent >= visiblePercentageLimit then
            
                {Percentage.Percent = minimumShowablePercentage; Stroke = wallet.Stroke} |> Some
            
            else 
                None

        let pies =  
            wallets |> Seq.choose innerNormalize//List.choose innerNormalize


        

        let wholePie = 
            pies |> Seq.sumBy(fun x -> x.Percent)

        let sortedPies = 
            pies |> Seq.sortByDescending(fun x -> x.Percent)
            
        let result = 
            sortedPies |> Seq.mapi (fun index  x -> 

                if index = 0 && wholePie > fullPie then
                    {x with Percent =  x.Percent - (wholePie - fullPie)}
                elif index = (Seq.length(sortedPies)-1) && wholePie < fullPie then
                    {x with Percent = x.Percent + (fullPie - wholePie)}
                else
                    x
            )


        result

    let beautifyAmount (wallet:seq<Wallet>) =
        
        let total = 
            wallet |> Seq.sumBy(fun x -> x.CryptoValue)
        
        let value = total.ToString()
        let mutable balance = value
        let mutable digits = balance.Length

        let place = 4 
        


        for digit in 1..digits do
            if place*digit <= digits then
                
                balance <- balance.Insert(digits+1-place*digit,",")
                digits <- balance.Length

        balance



    let makePies (grid:Grid, wallet:seq<Wallet>) =
    
        let arcAngle:double = 0.
        let slices = Seq.length(wallet)
        let total = 
            wallet |> Seq.sumBy(fun x -> x.CryptoValue)

        
        let pies  = Normalize wallet
        
        for pie in pies do

            if slices = 1 then
                let lengthOfArc = 360.
                let gap = 0.
                

                    
                let path = Path(Stroke = pie.Stroke)
                let pathG = PathGeometry ()

                let pathFC = PathFigureCollection()
                let pathF =  PathFigure()
                let pathSC = PathSegmentCollection()
                let arcS = ArcSegment()
                
                renderArc path pathF arcS (arcAngle + gap) (lengthOfArc - gap * 2.)
                
                path.Data <- pathG
                pathG.Figures <- pathFC
                pathFC.Add(pathF)
                pathF.Segments.Add(arcS)



                grid.Children.Add(path)            
            else
                                                              
                let gap:double = 2.5
                let lengthOfArc = ( pie.Percent ) * 360. / 100.
                   

                let path = Path(Stroke = pie.Stroke)
                let pathG = PathGeometry ()

                let pathFC = PathFigureCollection()
                let pathF =  PathFigure()
                let pathSC = PathSegmentCollection()
                let arcS = ArcSegment()

                let arcAngle = setArcAngle lengthOfArc gap arcAngle path pathF arcS
                    
                path.Data <- pathG
                pathG.Figures <- pathFC
                pathFC.Add(pathF)
                pathF.Segments.Add(arcS)

                grid.Children.Add(path)




