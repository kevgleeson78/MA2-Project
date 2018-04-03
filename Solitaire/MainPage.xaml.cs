using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Solitaire
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {

            this.InitializeComponent();
            addBorders();
            addRowsColumns();
            addPieces();

        }
        private void addRowsColumns()
        {
           
            
            for (int i = 0; i < 7; i++)
            {

                grdPieces.ColumnDefinitions.Add(new ColumnDefinition());
                grdPieces.RowDefinitions.Add(new RowDefinition());

                grdGame.ColumnDefinitions.Add(new ColumnDefinition());
                grdGame.RowDefinitions.Add(new RowDefinition());
            }

        }
        Border brdr;
        //Adapted from https://github.com/dcMobileAppsDev/ClassContainers/blob/master/ClassContainers/MainPage.xaml.cs
        private void addBorders()
        {

            int iR, iC;

            //iR set to 1 to center the board in front of the background
            for (iR = 0; iR < 7; iR++)
            {//iC set to 1 to center board
                for (iC = 0; iC < 7; iC++)
                {
                    brdr = new Border();
                    //name for getting the position of the peices on the board.
                    brdr.Name = iR.ToString() + "_" + iC.ToString();

                    //set default colour of border to balck
                    brdr.Background = new SolidColorBrush(Colors.Black);

                    // if modulus of iR + iC is 0, then make the square white
                    if ((iR + iC) % 2 == 0)
                    {
                        brdr.Background = new SolidColorBrush(Colors.White);

                    }
                    //remove the squares that are not needed for the game
                    //colour these squares the same colour as the grid background
                    //@todo need to make these references in the grid non playable.
                    if ((iR < 2&& iC < 2) || (iR < 2 && iC > 4) || (iR > 4 && iC < 2) || (iR > 4 && iC > 4))
                    {
                        brdr.Background = new SolidColorBrush(Colors.BurlyWood);
                    }
                    brdr.SetValue(Grid.RowProperty, iR);
                    brdr.SetValue(Grid.ColumnProperty, iC);
                    brdr.HorizontalAlignment = HorizontalAlignment.Center;
                    brdr.VerticalAlignment = VerticalAlignment.Center;
                    //@todo set height and width of squares not hard coded.
                    brdr.Height = 100;
                    brdr.Width = 100;
                    //add squares to the board.

                    grdGame.Children.Add(brdr);


                }

            }

        }

        UIElement[,] InitialPeices() =>
            // loop rows 
            new UIElement[7, 7] {

            {brdr,brdr,myEl,myEl,myEl,brdr,brdr},
            {brdr,brdr,myEl,myEl,myEl,brdr,brdr},
            {myEl,myEl,myEl,myEl,myEl,myEl,myEl},
            {myEl,myEl,myEl,brdr,myEl,myEl,myEl},
            {myEl,myEl,myEl,myEl,myEl,myEl,myEl},
            {brdr,brdr,myEl,brdr,myEl,brdr,brdr},
            {brdr,brdr,myEl,myEl,myEl,brdr,brdr},
            };


        Ellipse myEl;
        UIElement[,] grid;
        private void addPieces()
        {
            myEl = new Ellipse();

            grid = InitialPeices();

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (!grid[i, j].Equals(brdr))
                    {
                        myEl = new Ellipse
                        {
                            Fill = new SolidColorBrush(Colors.Silver),
                            Name = i + "_" + j,
                            Tag = "peices",
                            Height = 50,
                            Width = 50
                        };
                        grid[i, j] = myEl;

                        grid[i, j].SetValue(Grid.RowProperty, i);
                        grid[i, j].SetValue(Grid.ColumnProperty, j);



                        grdPieces.Children.Add(myEl);
                        myEl.Tapped += myEl_Tapped;
                    }
                }
            }
            
            
        }
        Ellipse moveMe;
        Border possible1, possible2, possible3, possible4;
        int toR1, toC1;
        private void myEl_Tapped(object sender, TappedRoutedEventArgs e)
        {
             


           
            Ellipse current = (Ellipse)sender;
            moveMe = current;

            toR1 = (int)moveMe.GetValue(Grid.RowProperty);
            Debug.WriteLine(current.Name);
            toC1 = (int)moveMe.GetValue(Grid.ColumnProperty);
            String removeMe = (toR1 + 1) + "_" + toC1;
            //@todo set condition for the four possible moves
            //set boudries for edge of board.

            possible1 = new Border();
            //name for getting the position of the peices on the board.

            if (toR1 + 2 < 7)
            {
                if (grid[toR1 + 2, toC1].Equals(brdr) && !grid[toR1 + 1, toC1].Equals(brdr))
                { ////grid = new UIElement[9, 9];

                    possible1.Background = new SolidColorBrush(Colors.Red);

                    possible1.SetValue(Grid.RowProperty, toR1 + 2);
                    possible1.SetValue(Grid.ColumnProperty, toC1);
                    possible1.HorizontalAlignment = HorizontalAlignment.Center;
                    possible1.VerticalAlignment = VerticalAlignment.Center;
                    //@todo set height and width of squares not hard coded.
                    possible1.Height = 100;
                    possible1.Width = 100;
                    possible1.Name = "possible1";

                    grdGame.Children.Add(possible1);

                    possible1.Tapped += Brdr_Tapped;



                }
            }

            possible2 = new Border();
            //name for getting the position of the peices on the board.

            if (toR1 - 2 > 0)
            {
                if (grid[toR1 - 2, toC1].Equals(brdr) && !grid[toR1 - 1, toC1].Equals(brdr))
                { ////grid = new UIElement[9, 9];

                    possible2.Background = new SolidColorBrush(Colors.Red);

                    possible2.SetValue(Grid.RowProperty, toR1 - 2);
                    possible2.SetValue(Grid.ColumnProperty, toC1);
                    possible2.HorizontalAlignment = HorizontalAlignment.Center;
                    possible2.VerticalAlignment = VerticalAlignment.Center;
                    //@todo set height and width of squares not hard coded.
                    possible2.Height = 100;
                    possible2.Width = 100;
                    possible2.Name = "possible1";

                    grdGame.Children.Add(possible2);

                    possible2.Tapped += Brdr_Tapped;



                }
            }



        }
       
      

        private void Brdr_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Border current = (Border)sender;

            toR1 = (int)moveMe.GetValue(Grid.RowProperty);
            toC1 = (int)moveMe.GetValue(Grid.ColumnProperty);

            moveMe.SetValue(Grid.RowProperty, current.GetValue(Grid.RowProperty));
            moveMe.SetValue(Grid.ColumnProperty,current.GetValue(Grid.ColumnProperty));
            moveMe.Name = toR1 + 2 + "_" + toC1;
           
            myEl = (Ellipse)grid[toR1 + 1, toC1];

            grdPieces.Children.Remove(myEl);

            grdGame.Children.Remove(possible1);


            possible1.Tapped -= Brdr_Tapped;
            grid[toR1 + 1, toC1] = brdr;
            grid[toR1 + 2, toC1] = myEl;
            grid[toR1, toC1] = brdr;
            
            //possible1.Background = new SolidColorBrush(Colors.White);

            Debug.WriteLine(grdPieces.Children.Count);
            var rowCount = grid.GetLength(0);
            var colCount = grid.GetLength(1);
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                    Debug.Write(String.Format("{0}\t", grid[row, col]));
                Debug.WriteLine("");
            }
        }
    }
    

    //@todo add tapped event to peices and move them no logic for now
}
