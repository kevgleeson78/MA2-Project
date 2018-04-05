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

       


        Ellipse myEl;
        UIElement[,] grid;
        private void addPieces()
        {
            myEl = new Ellipse();

            grid = 
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
        //Possible moves for the user to choose
        Border possible1, possible2,possible3,possible4;
        //Different possible moves to check for
        int twoSquaresDown, twoSquaresLeft,twoSquaresUp,twoSquaresRight,curRow,curCol,
            oneSquareDown,oneSquareUp,oneSquareRight,oneSquareLeft;
        //tapped event handler
        private void myEl_Tapped(object sender, TappedRoutedEventArgs e)
        {
             


           
            Ellipse current = (Ellipse)sender;
            //moveME used in the border tapped event handler to update the positon of the ellipse
            moveMe = current;
            //set all values below for updating the position of the ellipse in the game grid
            //and for updting the peices array
            curRow = (int)current.GetValue(Grid.RowProperty);
            curCol = (int)current.GetValue(Grid.ColumnProperty);
            oneSquareDown = (int)current.GetValue(Grid.RowProperty) + 1;
            oneSquareUp = (int)current.GetValue(Grid.RowProperty) - 1;
            oneSquareRight = (int)current.GetValue(Grid.ColumnProperty) + 1;
            oneSquareLeft = (int)current.GetValue(Grid.ColumnProperty) - 1;
            twoSquaresDown = (int)current.GetValue(Grid.RowProperty)+2;
           
            twoSquaresUp = (int)current.GetValue(Grid.RowProperty) -2;
            twoSquaresLeft = (int)current.GetValue(Grid.ColumnProperty) - 2;

            twoSquaresRight = (int)current.GetValue(Grid.ColumnProperty) + 2;
            //@todo set condition for the four possible moves
            //set boudries for edge of board.

            
            //name for getting the position of the peices on the board.

            if (twoSquaresDown < 7)
            {
                if (grid[twoSquaresDown, curCol].Equals(brdr) && !grid[oneSquareDown, curCol].Equals(brdr))
                { ////grid = new UIElement[9, 9];
                    possible1 = new Border();
                    possible1.Background = new SolidColorBrush(Colors.Red);

                    possible1.SetValue(Grid.RowProperty, twoSquaresDown);
                    possible1.SetValue(Grid.ColumnProperty, curCol);
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
            
          
           
            //Condition for Boundry at top of board
            if (twoSquaresUp > 0)
            {
                // Condition for checking if the tapped ellipse has an ellipse above
                // it and the following square above is empty in the array
                if (grid[twoSquaresUp, curCol].Equals(brdr) && !grid[oneSquareUp, curCol].Equals(brdr))
                { 

                    possible2 = new Border();
                    //set the border colour to red as an option to move to
                    possible2.Background = new SolidColorBrush(Colors.Red);
                    // set the coordinates
                    possible2.SetValue(Grid.RowProperty, twoSquaresUp);
                    possible2.SetValue(Grid.ColumnProperty, curCol);
                    possible2.HorizontalAlignment = HorizontalAlignment.Center;
                    possible2.VerticalAlignment = VerticalAlignment.Center;
                    //@todo set height and width of squares not hard coded.
                    possible2.Height = 100;
                    possible2.Width = 100;
                    //name for removing the ellipse if the square is clicked
                    possible2.Name = "possible2";
                    //add the red  border to the grid
                    grdGame.Children.Add(possible2);

                    possible2.Tapped += Brdr_Tapped;
                    
                }
            }
            if (twoSquaresLeft > 0)
            {
                // Condition for checking if the tapped ellipse has an ellipse above
                // it and the following square above is empty in the array
                if (grid[curRow, twoSquaresLeft].Equals(brdr) && !grid[curRow, oneSquareLeft].Equals(brdr))
                {

                    possible3 = new Border();
                    //set the border colour to red as an option to move to
                    possible3.Background = new SolidColorBrush(Colors.Red);
                    // set the coordinates
                    possible3.SetValue(Grid.RowProperty, curRow);
                    possible3.SetValue(Grid.ColumnProperty, twoSquaresLeft);
                    possible3.HorizontalAlignment = HorizontalAlignment.Center;
                    possible3.VerticalAlignment = VerticalAlignment.Center;
                    //@todo set height and width of squares not hard coded.
                    possible3.Height = 100;
                    possible3.Width = 100;
                    //name for removing the ellipse if the square is clicked
                    possible3.Name = "possible3";
                    //add the red  border to the grid
                    grdGame.Children.Add(possible3);

                    possible3.Tapped += Brdr_Tapped;

                }
            }
            if (twoSquaresRight < 7)
            {
                // Condition for checking if the tapped ellipse has an ellipse above
                // it and the following square above is empty in the array
                if (grid[curRow, twoSquaresRight].Equals(brdr) && !grid[curRow, oneSquareRight].Equals(brdr))
                {

                    possible4 = new Border();
                    //set the border colour to red as an option to move to
                    possible4.Background = new SolidColorBrush(Colors.Red);
                    // set the coordinates
                    possible4.SetValue(Grid.RowProperty, curRow);
                    possible4.SetValue(Grid.ColumnProperty, twoSquaresRight);
                    possible4.HorizontalAlignment = HorizontalAlignment.Center;
                    possible4.VerticalAlignment = VerticalAlignment.Center;
                    //@todo set height and width of squares not hard coded.
                    possible4.Height = 100;
                    possible4.Width = 100;
                    //name for removing the ellipse if the square is clicked
                    possible4.Name = "possible4";
                    //add the red  border to the grid
                    grdGame.Children.Add(possible4);

                    possible4.Tapped += Brdr_Tapped;

                }
            }


        }
       
      

        private void Brdr_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Border current = (Border)sender;

           
            //move the ellipse toe the tapped border
            moveMe.SetValue(Grid.RowProperty, current.GetValue(Grid.RowProperty));
            moveMe.SetValue(Grid.ColumnProperty,current.GetValue(Grid.ColumnProperty));
            
            //GEt the name of the border that is tapped
            if (current.Name == "possible1")
            {
                grdPieces.Children.Clear();
                //set Ellipse to be removed from the array
                myEl = (Ellipse)grid[oneSquareDown, curCol];
                //remove the ellipse from the grid
                grdPieces.Children.Remove(myEl);
                //remove both highlighted squares
                grdGame.Children.Remove(possible1);
                grdGame.Children.Remove(possible2);
                grdGame.Children.Remove(possible3);
                //remove tapped handler
                possible1.Tapped -= Brdr_Tapped;
                //update the array
                grid[oneSquareDown, curCol] = brdr;
                grid[twoSquaresDown, curCol] = myEl;
                grid[curRow, curCol] = brdr;
            }
            
            if (current.Name=="possible2")
            // else It's possible2 border tapped
            {
                grdPieces.Children.Clear();
                //set Ellipse to be removed from the array
                myEl = (Ellipse)grid[oneSquareUp, curCol];
                //remove the ellipse from the grid
                grdPieces.Children.Remove(myEl);
                //remove both highlighted squares
                grdGame.Children.Remove(possible1);
                grdGame.Children.Remove(possible2);
                grdGame.Children.Remove(possible3);
                grdGame.Children.Remove(possible4);
                //remove tapped handler
                possible2.Tapped -= Brdr_Tapped;
                //update the array
                grid[oneSquareUp, curCol] = brdr;
                grid[twoSquaresUp, curCol] = myEl;
                grid[curRow, curCol] = brdr;

            }
            if(current.Name=="possible3")
            {
                grdPieces.Children.Clear();
                //set Ellipse to be removed from the array
                myEl = (Ellipse)grid[curRow, oneSquareLeft];
                //remove the ellipse from the grid
                grdPieces.Children.Remove(myEl);
                //remove both highlighted squares
                grdGame.Children.Remove(possible1);
                grdGame.Children.Remove(possible2);
                grdGame.Children.Remove(possible3);
                grdGame.Children.Remove(possible4);

                //remove tapped handler
                possible3.Tapped -= Brdr_Tapped;
                //update the array
                grid[curRow, oneSquareLeft] = brdr;
                grid[curRow, twoSquaresLeft] = myEl;
                grid[curRow, curCol] = brdr;
            }
            if (current.Name == "possible4")
            {
                grdPieces.Children.Clear();
                //set Ellipse to be removed from the array
                myEl = (Ellipse)grid[curRow, oneSquareRight];
                //remove the ellipse from the grid
                grdPieces.Children.Remove(myEl);
                //remove both highlighted squares
                grdGame.Children.Remove(possible1);
                grdGame.Children.Remove(possible2);
                grdGame.Children.Remove(possible3);
                grdGame.Children.Remove(possible4);

                //remove tapped handler
                possible4.Tapped -= Brdr_Tapped;
                //update the array
                grid[curRow, oneSquareRight] = brdr;
                grid[curRow, twoSquaresRight] = myEl;
                grid[curRow, curCol] = brdr;
            }


            //For testing the array update printed out to console

            var rowCount = grid.GetLength(0);
            var colCount = grid.GetLength(1);
            for (int row = 0; row < rowCount; row++)
            {

                for (int col = 0; col < colCount; col++)
                {
                    if (!grid[row, col].Equals(brdr))
                    {
                        myEl = new Ellipse
                        {
                            Fill = new SolidColorBrush(Colors.Silver),
                            Name = row + "_" + col,
                            Tag = "peices",
                            Height = 50,
                            Width = 50
                        };
                        grid[row, col] = myEl;

                        grid[row, col].SetValue(Grid.RowProperty, row);
                        grid[row, col].SetValue(Grid.ColumnProperty, col);



                        grdPieces.Children.Add(myEl);
                        myEl.Tapped += myEl_Tapped;
                    }
                   
                }
            }
            for (int row = 0; row < rowCount; row++)
            {

                for (int col = 0; col < colCount; col++)
                
                    
                    Debug.Write(String.Format("{0}\t", grid[row, col]));
                    Debug.WriteLine("");
                
            }
            Debug.WriteLine(grdPieces.Children.Count);
        }
    }
    

    //@todo add tapped event to peices and move them no logic for now
}
