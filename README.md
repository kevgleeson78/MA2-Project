# MA2-Project
## Peg Solitaire UWP App Usig c# and xaml.


This program was built using [UWP](https://docs.microsoft.com/en-us/windows/uwp/get-started/universal-application-platform-guide) With Visual Studio 2017.

Author: [Kevin Gleeson](https://github.com/kevgleeson78)

Third year student at:[GMIT](http://gmit.ie) Galway

## Application outline:
Peg Solitaire is a game where there are 31 peices at the start of the game.
To finish the game you need to take one piece and jump over a neighbouring piece that has has an empty square next to it.
The game finishes when there are no neighbouring pieces left to jump over.
THe object of winning the game is to have only one peice left and in the center square of the bord.
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
## Setting the board up:



## Setting the peices up:


## Choosing a peice to move:


## Highlighting possible moves:



## Keping current tapped peice only:




## Taking a peice:

## Clearing the board after peice has been taken:


## Displaying the score:


## Resetting the game:
