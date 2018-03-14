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
            addPieces();
          
            addRowsColumns();

            
        }
        private void addRowsColumns()
        {
            for (int i = 0; i < 9; i++)
            {
               
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
            for (iR = 1; iR < 8; iR++)
            {//iC set to 1 to center board
                for (iC = 1; iC < 8; iC++)
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
                    if ((iR < 3 && iC < 3) || (iR < 3 && iC > 5) || (iR > 5 && iC < 3) || (iR > 5 && iC > 5))
                    {
                        brdr.Background = new SolidColorBrush(Colors.BurlyWood);
                    }
                    brdr.SetValue(Windows.UI.Xaml.Controls.Grid.RowProperty, iR);
                    brdr.SetValue(Windows.UI.Xaml.Controls.Grid.ColumnProperty, iC);
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


        private UIElement[,] InitialPeices()
        {
            // loop rows 
            return new UIElement[9, 9] {
            {null,null,null,null,null,null,null,null,null},
            {null,null,null,myEl,myEl,myEl,null,null,null},
            {null,null,null,myEl,myEl,myEl,null,null,null},
            {null,myEl,myEl,myEl,myEl,myEl,myEl,myEl,null},
            {null,myEl,myEl,myEl,null,myEl,myEl,myEl,null},
            {null,myEl,myEl,myEl,myEl,myEl,myEl,myEl,null},
            {null,null,null,myEl,myEl,myEl,null,null,null},
            {null,null,null,myEl,myEl,myEl,null,null,null},
            {null,null,null,null,null,null,null,null,null},
        };
        }



        Ellipse myEl;
        UIElement[,] grid;
        private void addPieces()
        {

            myEl = new Ellipse();

         grid = InitialPeices();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (grid[i, j] != null)
                    {
                        myEl = new Ellipse();
                        myEl.Fill = new SolidColorBrush(Colors.Silver);
                        myEl.Name = i + "_" + j;
                        myEl.Tag = "peices";
                        myEl.Height = 40;
                        myEl.Width = 40;
                        grid[i, j] = myEl;

                        grid[i, j].SetValue(Windows.UI.Xaml.Controls.Grid.RowProperty, i);
                        grid[i, j].SetValue(Windows.UI.Xaml.Controls.Grid.ColumnProperty, j);

                        grdGame.Children.Add(grid[i, j]);
                        grid[i, j].Tapped += myEl_Tapped;

                    }


                }
            }

        }





        private void myEl_Tapped(object sender, TappedRoutedEventArgs e)
        {

            grid = InitialPeices();
            Ellipse moveMe;
            
            int toR1, toC1, brdr1;
            Ellipse current = (Ellipse)sender;
           


            moveMe = current;
           

            toR1 = (int)moveMe.GetValue(Grid.RowProperty);
            Debug.WriteLine(toR1);

            toC1 = (int)moveMe.GetValue(Grid.ColumnProperty);

            brdr1 = (int)brdr.GetValue(Grid.RowProperty);
            if(grid[toC1, toR1+1]!=null&& grid[toC1, toR1+2] == null)
            {
                
                UIElement[,] grid = new UIElement[9, 9];
                grid[toC1, toR1 + 1] = null;
                moveMe.SetValue(Grid.RowProperty, toR1+2);
                moveMe.SetValue(Grid.ColumnProperty, toC1);
                

                

               for (int i = 0; i < 9; i++)
               {
                   for (int j = 0; j < 9; j++)
                   {
                       if (grid[i, j] != null)
                       {
                           myEl = new Ellipse();
                           myEl.Fill = new SolidColorBrush(Colors.Silver);
                           myEl.Name = i + "_" + j;
                            myEl.Tag = "peices";
                            myEl.Height = 40;
                            myEl.Width = 40;
                            grid[i, j] = myEl;

                            grid[i, j].SetValue(Grid.RowProperty, i);
                            grid[i, j].SetValue(Grid.ColumnProperty, j);

                            grdGame.Children.Add(grid[i, j]);


                        }


                    }
                }
            }
            
           
            // 



        }
        }
    //@todo add tapped event to peices and move them no logic for now
}
