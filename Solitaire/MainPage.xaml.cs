using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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

//Adapted from https://github.com/dcMobileAppsDev/ClassContainers/blob/master/ClassContainers/MainPage.xaml.cs
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





        #region add rows and coulmns
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
        #endregion
        //_oob used for setting the boundries of the boards corners. 
        Border _brdr, _oob;

        #region add borders
        private void addBorders()
        {

            int iR, iC;

            //iR set to 1 to center the board in front of the background
            for (iR = 0; iR < 7; iR++)
            {//iC set to 1 to center board
                for (iC = 0; iC < 7; iC++)
                {
                    _brdr = new Border();
                    _oob = new Border();
                    //name for getting the position of the peices on the board.
                    _brdr.Name = iR.ToString() + "_" + iC.ToString();

                    //set default colour of border to balck
                    _brdr.Background = new SolidColorBrush(Colors.Black);

                    // if modulus of iR + iC is 0, then make the square white
                    if ((iR + iC) % 2 == 0)
                    {
                        _brdr.Background = new SolidColorBrush(Colors.White);

                    }
                    //remove the squares that are not needed for the game
                    //colour these squares the same colour as the _grid background
                    //@todo need to make these references in the _grid non playable.

                    _brdr.SetValue(Grid.RowProperty, iR);
                    _brdr.SetValue(Grid.ColumnProperty, iC);
                    _brdr.HorizontalAlignment = HorizontalAlignment.Center;
                    _brdr.VerticalAlignment = VerticalAlignment.Center;
                    //@todo set height and width of squares not hard coded.
                    _brdr.Height = 100;
                    _brdr.Width = 100;
                    //add squares to the board.

                    grdGame.Children.Add(_brdr);
                    if ((iR < 2 && iC < 2) || (iR < 2 && iC > 4) || (iR > 4 && iC < 2) || (iR > 4 && iC > 4))
                    {
                        _oob.Background = new SolidColorBrush(Colors.BurlyWood);
                        _oob.SetValue(Grid.RowProperty, iR);
                        _oob.SetValue(Grid.ColumnProperty, iC);
                        _oob.HorizontalAlignment = HorizontalAlignment.Center;
                        _oob.VerticalAlignment = VerticalAlignment.Center;
                        //@todo set height and width of squares not hard coded.
                        _oob.Height = 100;
                        _oob.Width = 100;
                        //add squares to the board.

                        grdGame.Children.Add(_oob);
                    }

                }

            }

        }
        #endregion


        //pieces for the board
        Ellipse _myEl;
        //2d array to  hold peices and Borders
        UIElement[,] _grid;
        #region add pieces
        //method to hold the initial board position of the pieces and borders.
        private void addPieces()
        {
            _myEl = new Ellipse();

            _grid = new UIElement[7, 7] {

            {_oob,_oob,_myEl,_myEl,_myEl,_oob,_oob},
            {_oob,_oob,_myEl,_myEl,_myEl,_oob,_oob},
            {_myEl,_myEl,_myEl,_myEl,_myEl,_myEl,_myEl},
            {_myEl,_myEl,_myEl,_brdr,_myEl,_myEl,_myEl},
            {_myEl,_myEl,_myEl,_myEl,_myEl,_myEl,_myEl},
            {_oob,_oob,_myEl,_myEl,_myEl,_oob,_oob},
            {_oob,_oob,_myEl,_myEl,_myEl,_oob,_oob},
            };

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (!_grid[i, j].Equals(_brdr) && !_grid[i, j].Equals(_oob))
                    {
                        _myEl = new Ellipse
                        {
                            Fill = new SolidColorBrush(Colors.Silver),
                            Name = "current",
                            Tag = "peices",
                            Height = 40,
                            Width = 40
                        };
                        _grid[i, j] = _myEl;

                        _grid[i, j].SetValue(Grid.RowProperty, i);
                        _grid[i, j].SetValue(Grid.ColumnProperty, j);



                        grdPieces.Children.Add(_myEl);
                        _myEl.Tapped += _myEl_Tapped;
                    }
                }
            }


        }
        #endregion
        Ellipse _moveMe;
        //Possible moves for the user to choose
        Border _possible1, _possible2, _possible3, _possible4;


        //Different possible moves to check for
        int _twoSquaresDown, _twoSquaresLeft, _twoSquaresUp, _twoSquaresRight, _curRow, _curCol,
            _oneSquareDown, _oneSquareUp, _oneSquarRight, _oneSquarLeft;
       
        
        //tapped event handler
        #region tapped event
        private void _myEl_Tapped(object sender, TappedRoutedEventArgs e)
        {


            grdGame.Children.Remove(_possible1);
            grdGame.Children.Remove(_possible2);
            grdGame.Children.Remove(_possible3);
            grdGame.Children.Remove(_possible4);

            Ellipse current = (Ellipse)sender;
            //_moveMe used in the border tapped event handler to update the positon of the ellipse
            _moveMe = current;
            //set all values below for updating the position of the ellipse in the game _grid
            //and for updting the peices array
            _curRow = (int)current.GetValue(Grid.RowProperty);
            _curCol = (int)current.GetValue(Grid.ColumnProperty);
            _oneSquareDown = (int)current.GetValue(Grid.RowProperty) + 1;
            _oneSquareUp = (int)current.GetValue(Grid.RowProperty) - 1;
            _oneSquarRight = (int)current.GetValue(Grid.ColumnProperty) + 1;
            _oneSquarLeft = (int)current.GetValue(Grid.ColumnProperty) - 1;
            _twoSquaresDown = (int)current.GetValue(Grid.RowProperty) + 2;

            _twoSquaresUp = (int)current.GetValue(Grid.RowProperty) - 2;
            _twoSquaresLeft = (int)current.GetValue(Grid.ColumnProperty) - 2;

            _twoSquaresRight = (int)current.GetValue(Grid.ColumnProperty) + 2;
            //@todo set condition for the four possible moves
            //set boudries for edge of board.


            //name for getting the position of the peices on the board.

            if (_twoSquaresDown < 7)
            {
                if (_grid[_twoSquaresDown, _curCol].Equals(_brdr) && !_grid[_oneSquareDown, _curCol].Equals(_brdr))
                { ////_grid = new UIElement[9, 9];
                    
                    _possible1 = new Border();

                    _possible1.Background = new SolidColorBrush(Colors.Red);

                    _possible1.SetValue(Grid.RowProperty, _twoSquaresDown);
                    _possible1.SetValue(Grid.ColumnProperty, _curCol);
                    _possible1.HorizontalAlignment = HorizontalAlignment.Center;
                    _possible1.VerticalAlignment = VerticalAlignment.Center;
                    //@todo set height and width of squares not hard coded.
                    _possible1.Height = 100;
                    _possible1.Width = 100;
                    _possible1.Name = "_possible1";

                    grdGame.Children.Add(_possible1);

                    _possible1.Tapped += _brdr_Tapped;



                }
                


            }



            //Condition for Boundry at top of board
            if (_twoSquaresUp >= 0)
            {
                // Condition for checking if the tapped ellipse has an ellipse above
                // it and the following square above is empty in the array
                if (_grid[_twoSquaresUp, _curCol].Equals(_brdr) && !_grid[_oneSquareUp, _curCol].Equals(_brdr))
                {
                    
                    _possible2 = new Border();
                    //set the border colour to red as an option to move to
                    _possible2.Background = new SolidColorBrush(Colors.Red);
                    // set the coordinates
                    _possible2.SetValue(Grid.RowProperty, _twoSquaresUp);
                    _possible2.SetValue(Grid.ColumnProperty, _curCol);
                    _possible2.HorizontalAlignment = HorizontalAlignment.Center;
                    _possible2.VerticalAlignment = VerticalAlignment.Center;
                    //@todo set height and width of squares not hard coded.
                    _possible2.Height = 100;
                    _possible2.Width = 100;
                    //name for removing the ellipse if the square is clicked
                    _possible2.Name = "_possible2";
                    //add the red  border to the _grid
                    grdGame.Children.Add(_possible2);

                    _possible2.Tapped += _brdr_Tapped;

                }
            }
            if (_twoSquaresLeft >= 0)
            {
                // Condition for checking if the tapped ellipse has an ellipse above
                // it and the following square above is empty in the array
                if (_grid[_curRow, _twoSquaresLeft].Equals(_brdr) && !_grid[_curRow, _oneSquarLeft].Equals(_brdr))
                {

                    _possible3 = new Border();
                    //set the border colour to red as an option to move to
                    _possible3.Background = new SolidColorBrush(Colors.Red);
                    // set the coordinates
                    _possible3.SetValue(Grid.RowProperty, _curRow);
                    _possible3.SetValue(Grid.ColumnProperty, _twoSquaresLeft);
                    _possible3.HorizontalAlignment = HorizontalAlignment.Center;
                    _possible3.VerticalAlignment = VerticalAlignment.Center;
                    //@todo set height and width of squares not hard coded.
                    _possible3.Height = 100;
                    _possible3.Width = 100;
                    //name for removing the ellipse if the square is clicked
                    _possible3.Name = "_possible3";
                    //add the red  border to the _grid
                    grdGame.Children.Add(_possible3);

                    _possible3.Tapped += _brdr_Tapped;

                }
            }
            if (_twoSquaresRight < 7)
            {
                // Condition for checking if the tapped ellipse has an ellipse above
                // it and the following square above is empty in the array
                if (_grid[_curRow, _twoSquaresRight].Equals(_brdr) && !_grid[_curRow, _oneSquarRight].Equals(_brdr))
                {

                    _possible4 = new Border();
                    //set the border colour to red as an option to move to
                    _possible4.Background = new SolidColorBrush(Colors.Red);
                    // set the coordinates
                    _possible4.SetValue(Grid.RowProperty, _curRow);
                    _possible4.SetValue(Grid.ColumnProperty, _twoSquaresRight);
                    _possible4.HorizontalAlignment = HorizontalAlignment.Center;
                    _possible4.VerticalAlignment = VerticalAlignment.Center;
                    //@todo set height and width of squares not hard coded.
                    _possible4.Height = 100;
                    _possible4.Width = 100;
                    //name for removing the ellipse if the square is clicked
                    _possible4.Name = "_possible4";
                    //add the red  border to the _grid
                    grdGame.Children.Add(_possible4);

                    _possible4.Tapped += _brdr_Tapped;

                }
            }


        }

        #endregion
        int _initScore = 0;

        #region border tapped event
        private void _brdr_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Border current = (Border)sender;


            //move the ellipse to the tapped border
            _moveMe.SetValue(Grid.RowProperty, current.GetValue(Grid.RowProperty));
            _moveMe.SetValue(Grid.ColumnProperty, current.GetValue(Grid.ColumnProperty));

            //GEt the name of the border that is tapped
            if (current.Name == "_possible1")
            {   //Clear the _grid of pieces
                grdPieces.Children.Clear();
                //set Ellipse to be removed from the array
                _myEl = (Ellipse)_grid[_oneSquareDown, _curCol];
                //remove the ellipse from the _grid
                grdPieces.Children.Remove(_myEl);
                //remove both highlighted squares
                grdGame.Children.Remove(_possible1);
                grdGame.Children.Remove(_possible2);
                grdGame.Children.Remove(_possible3);
                grdGame.Children.Remove(_possible4);
                //remove tapped handler
                _possible1.Tapped -= _brdr_Tapped;
                //update the array
                _grid[_oneSquareDown, _curCol] = _brdr;
                _grid[_twoSquaresDown, _curCol] = _myEl;
                _grid[_curRow, _curCol] = _brdr;
            }

            if (current.Name == "_possible2")

            {//Clear the _grid of pieces
                grdPieces.Children.Clear();
                //set Ellipse to be removed from the array
                _myEl = (Ellipse)_grid[_oneSquareUp, _curCol];
                //remove the ellipse from the _grid
                grdPieces.Children.Remove(_myEl);
                //remove both highlighted squares
                grdGame.Children.Remove(_possible1);
                grdGame.Children.Remove(_possible2);
                grdGame.Children.Remove(_possible3);
                grdGame.Children.Remove(_possible4);
                //remove tapped handler
                _possible2.Tapped -= _brdr_Tapped;
                //update the array
                _grid[_oneSquareUp, _curCol] = _brdr;
                _grid[_twoSquaresUp, _curCol] = _myEl;
                _grid[_curRow, _curCol] = _brdr;

            }
            if (current.Name == "_possible3")
            {//Clear the _grid of pieces
                grdPieces.Children.Clear();
                //set Ellipse to be removed from the array
                _myEl = (Ellipse)_grid[_curRow, _oneSquarLeft];
                //remove the ellipse from the _grid
                grdPieces.Children.Remove(_myEl);
                //remove both highlighted squares
                grdGame.Children.Remove(_possible1);
                grdGame.Children.Remove(_possible2);
                grdGame.Children.Remove(_possible3);
                grdGame.Children.Remove(_possible4);

                //remove tapped handler
                _possible3.Tapped -= _brdr_Tapped;
                //update the array
                _grid[_curRow, _oneSquarLeft] = _brdr;
                _grid[_curRow, _twoSquaresLeft] = _myEl;
                _grid[_curRow, _curCol] = _brdr;
            }
            if (current.Name == "_possible4")
            {//Clear the _grid of pieces
                grdPieces.Children.Clear();
                //set Ellipse to be removed from the array
                _myEl = (Ellipse)_grid[_curRow, _oneSquarRight];
                //remove the ellipse from the _grid
                grdPieces.Children.Remove(_myEl);
                //remove both highlighted squares
                grdGame.Children.Remove(_possible1);
                grdGame.Children.Remove(_possible2);
                grdGame.Children.Remove(_possible3);
                grdGame.Children.Remove(_possible4);

                //remove tapped handler
                _possible4.Tapped -= _brdr_Tapped;
                //update the array
                _grid[_curRow, _oneSquarRight] = _brdr;
                _grid[_curRow, _twoSquaresRight] = _myEl;
                _grid[_curRow, _curCol] = _brdr;
            }


            //Re-populate the _grid with the updated array

            var rowCount = _grid.GetLength(0);
            var colCount = _grid.GetLength(1);
            for (int row = 0; row < rowCount; row++)
            {

                for (int col = 0; col < colCount; col++)
                {
                    if (!_grid[row, col].Equals(_brdr) && !_grid[row, col].Equals(_oob))
                    {
                        _myEl = new Ellipse
                        {
                            Fill = new SolidColorBrush(Colors.Silver),
                            Name = row + "_" + col,
                            Tag = "peices",
                            Height = 40,
                            Width = 40
                        };
                        _grid[row, col] = _myEl;

                        _grid[row, col].SetValue(Grid.RowProperty, row);
                        _grid[row, col].SetValue(Grid.ColumnProperty, col);



                        grdPieces.Children.Add(_myEl);
                        _myEl.Tapped += _myEl_Tapped;
                    }

                }
            }
            //update the score after each piece is taken
            updateScore(_initScore += 10);
            checkGame();
        }
        #endregion
        //button control to clear the game and reset the pieces and score
        #region clear game and update score
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GameOverButton.Visibility = Visibility.Collapsed;
            grdPieces.Children.Clear();
            addPieces();
            updateScore(0);
            _initScore = 0;
        }
        //print out the score to the text block.
        private void updateScore(int inScore)
        {
            score.Text = "Score: " + inScore.ToString();
        }
        #endregion
        bool _up = false, _down = false, _left = false, _right = false;
        int _valid = 0;
        

        private void checkGame()
        {
            
            int rowLength = _grid.GetLength(0);
            int colLength = _grid.GetLength(1);
            _valid -= _valid;
            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {

                    if (i+2 < 7)
                    {
                        if (_grid[i+2, j].Equals(_brdr) && !_grid[i+1, j].Equals(_brdr) && !_grid[i , j].Equals(_brdr) && !_grid[i, j].Equals(_oob))
                        { ////_grid = new UIElement[9, 9];


                            _down = true;
                            
                        }
                        else
                        {
                           _down = false;
                        }



                    }



                    //Condition for Boundry at top of board
                    if (i-2 >= 0)
                    {
                        // Condition for checking if the tapped ellipse has an ellipse above
                        // it and the following square above is empty in the array
                        if (_grid[i-2, j].Equals(_brdr) && !_grid[i-1, j].Equals(_brdr) && !_grid[i, j].Equals(_brdr) && !_grid[i, j].Equals(_oob))
                        {
                            _up = true;


                        }
                        else
                        {
                            _up = false;
                        }
                    }
                    if (j-2 >= 0)
                    {
                        // Condition for checking if the tapped ellipse has an ellipse above
                        // it and the following square above is empty in the array
                        if (_grid[i, j - 2].Equals(_brdr) && !_grid[i, j - 1].Equals(_brdr) && !_grid[i, j].Equals(_brdr) && !_grid[i, j].Equals(_oob))
                        {

                            _left = true;

                        }
                        else
                        {

                            _left = false;
                        }
                        
                    }
                    if (j+2 < 7)
                    {
                        // Condition for checking if the tapped ellipse has an ellipse above
                        // it and the following square above is empty in the array
                        if (_grid[i, j+2].Equals(_brdr) && !_grid[i, j+1].Equals(_brdr) && !_grid[i, j].Equals(_brdr) && !_grid[i, j].Equals(_oob))
                        {

                            _right = true;
                        }
                        else
                        {
                            _right = false;
                        }
                       
                        
                       
                    }
                    
                    if(_right==true||_left==true||_up==true||_down==true)
                    {
                        _valid++;
                        
                    }
                   
                }
               
            }
            if (_valid==0)
            {
                GameOverButton.Visibility = Visibility.Visible;
               
                Debug.WriteLine("Games Over");
            }
        }
    }



}
