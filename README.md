# MA2-Project
## Peg Solitaire UWP App Usig c# and xaml.


This program was built using [UWP](https://docs.microsoft.com/en-us/windows/uwp/get-started/universal-application-platform-guide) With Visual Studio 2017.

Author: [Kevin Gleeson](https://github.com/kevgleeson78)

Third year student at:[GMIT](http://gmit.ie) Galway

## Application outline:
Peg Solitaire is a game where there are 31 peices at the start of the game.
To finish the game you need to take one piece and jump over a neighbouring piece that has has an empty square next to it.
The game finishes when there are no neighbouring pieces left to jump over.
THe object of winning the game is to have only one peice left and in the center square of the board.
## MainPage.xaml
The main page of the application only holds Three grids and a text block used as place holders to hold the elements sreated on the c# side of the application.
```XAML
<Page
    x:Class="Solitaire.MainPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:Solitaire"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    mc:Ignorable="d">

    <Grid x:Name="grdRoot"  Background="Cornsilk">


        <Grid x:Name="grdGame"  MaxHeight="600" MaxWidth="600" Grid.Column="1"/>
        <Grid x:Name="grdPieces" MaxHeight="600" MaxWidth="600" Grid.Column="1" />
        <Button Content="Restart Game" HorizontalAlignment="Left" Margin="10,10,0,0" VerticalAlignment="Top" Grid.Column="0" Click="Button_Click"/>
        <TextBlock HorizontalAlignment="Left" Margin="10,57,0,0" Text="Score: 0" TextWrapping="Wrap" VerticalAlignment="Top" Width="109" Height="40" x:Name="score"/>
    </Grid>
</Page>

```

* grdRoot is the main container and background.
* grdGame is used to set up the board.
* grdPieces is used to hold the peices of the game.
* The button is used to restart the game.
* The tex block is used to hold the user score as they take a new peice.


## Setting the board up:
### Firstly rows and columns need to be added for each grid.
```C#
 for (int i = 0; i < 7; i++)
            {

                grdPieces.ColumnDefinitions.Add(new ColumnDefinition());
                grdPieces.RowDefinitions.Add(new RowDefinition());

                grdGame.ColumnDefinitions.Add(new ColumnDefinition());
                grdGame.RowDefinitions.Add(new RowDefinition());
            }
```
### Adding the squares to the game board.
The game area is defined by asigning alternate Borders of Black and white as the games squares.
```C#
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
```
### The game boundries on the Board.
Are set by the Border variable _oob to a different colour based on the four corners of the grid.

```C#
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

```


## Setting the peices and board up:
 A 2d array is used to arepresent the game board with pieces at the start of the game.
 The grid grdPieces is then overlayed on the grdGAme grid holding borders.
```C#
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
```
## Choosing a peice to move:
When a piece is tapped it crestes an event handler.
```C#
 _myEl.Tapped += _myEl_Tapped;
```

The current ellipse tapped can then be accessed inside the tapped event handler method with.
```C#
Ellipse current = (Ellipse)sender;
```
This variable can then be used to get the current position of the ellipse on the gridRow and gridColumn.
These coordinates map exactly to the 2d array so can be used to manipulate the array and grid.

## Getting the values of all needed coordinates.
Global variables are created for getting the positon 1 and 2 squares to the left, right, up and down from the current tapped ellipse.

```C#
int _twoSquaresDown, _twoSquaresLeft, _twoSquaresUp, _twoSquaresRight, _curRow, _curCol,
            _oneSquareDown, _oneSquareUp, _oneSquarRight, _oneSquarLeft;
```

These are then set insde the tapped event handler method.

```C#
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
```
## Setting up boundaries of the board

To prevent an out of bounds exception for each of the grids a condition needs to be set to check if the current positon + 1, current positon + 2 in all directions left, right, up and down are within the excepted bounds of the grid.
```C#
//set boudries for edge of board.

            if (_twoSquaresDown < 7)
            {
```

```C#
//Condition for Boundry at top of board
            if (_twoSquaresUp >= 0)
            {
```
## Highlighting possible moves:

Firstly Global Border Variables are set to hold the four possible highlighted borders that may be used should the conditions arise.

When an ellipse has been tapped and the above conditions are met the array is checked for ellipses and borders from the current position.

In the below example we are checking in a downward direction.

The condition is checking if the 2d array has an ellipse direclty below and a border below that again.

If this has met these two conditions the border is coloured red.
Then added to the grdGame grid.

IF the border is tapped it triggers the event handler _brdr_Tapped.

```C#
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
```
## Keping current tapped peice only:
TO avoid highlighted borders being created after each tapped ellipse the borders have to be removed from the grid after a new tapped event has occured.
This is place at the yop of the tapped event handler method so it is executed for each tapped ellipse.

```C#
            grdGame.Children.Remove(_possible1);
            grdGame.Children.Remove(_possible2);
            grdGame.Children.Remove(_possible3);
            grdGame.Children.Remove(_possible4);
```


## Taking a peice:
Once  a highlighte Border has been tapped we get the current position of the border.

```C#
 private void _brdr_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Border current = (Border)sender;
```
Then set the global _moveMe ellipse to the value of the tapped border.

```C#
 //move the ellipse to the tapped border
            _moveMe.SetValue(Grid.RowProperty, current.GetValue(Grid.RowProperty));
            _moveMe.SetValue(Grid.ColumnProperty, current.GetValue(Grid.ColumnProperty));
```

## Clearing the board after peice has been taken:
* all peices need to be removed from the grid.
* the jumped ellipse needs to removed from the grid.
* all highlighted borders nedd to be removed.
* the tapped handler needs to be removed.
* the array needs to be updated.
```C#
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
```

Then the array is repopulated
```C#
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
```

## Displaying the score:
A textblock is used on the mainPAge.xaml to hold the user score on the game screen.
```xaml
 <TextBlock HorizontalAlignment="Left" Margin="10,57,0,0" Text="Score: 0" TextWrapping="Wrap" VerticalAlignment="Top" Width="109" Height="40" x:Name="score"/>
```
After each jump the user makes the score increments by ten.

The below global variable is used to hold the user score.

```C#
int _initScore = 0;
````
A method updateScore is used at the end of the _brdr_tapped event handler to increment by ten once a highlighted border has been tapped (A peice has been taken)

```C#
 //update the score after each piece is taken
            updateScore(_initScore += 10);
```
This method then binds the score to the textblock after each peice has been taken.

```xaml
 private void updateScore(int inScore)
        {

            score.Text = "Score: " + inScore.ToString();



        }
```


## Resetting the game:
The game is reset via a button on the main page.
```xaml
<Button Content="Restart Game" HorizontalAlignment="Left" Margin="10,10,0,0" VerticalAlignment="Top" Grid.Column="0" Click="Button_Click"/>
```
* When the button is clicked it removes all peices from the grid.
* Then adds the initial 2d array of peices back to the grid.
* Then resets the score to 0.
```C#
 private void Button_Click(object sender, RoutedEventArgs e)
        {
            grdPieces.Children.Clear();
            addPieces();
            updateScore(0);
            _initScore = 0;
        }
`` 
