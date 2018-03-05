using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
          
            addRowsColumns();
            addBorders();
            addPieces();
        }
        private void addRowsColumns()
        {
            for (int i = 0; i < 9; i++)
            {
                grdGame.ColumnDefinitions.Add(new ColumnDefinition());
                grdGame.RowDefinitions.Add(new RowDefinition());
            }
           
        }
        //Adapted from https://github.com/dcMobileAppsDev/ClassContainers/blob/master/ClassContainers/MainPage.xaml.cs
        private void addBorders()
        {
            int iR, iC;
            Border brdr;
            //iR set to 1 to center the board in front of the background
            for (iR = 1; iR < 8; iR++)
            {//iC set to 1 to center board
                for (iC = 1; iC < 8; iC++)
                {
                    brdr = new Border();
                    //name for getting the position of the peices on the board.
                    brdr.Name = "square_" + iR.ToString() + "_" + iC.ToString();
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
                    if ((iR<3&&iC <3)||(iR <3  && iC > 5)|| (iR >5 && iC < 3) || (iR >5 && iC > 5))
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
        int _Rows = 8;
        
        private void addPieces()
        {
            Ellipse myEl;
            int iR, iC;
            // use R&C to name the objects
            for (iR = 1; iR < _Rows; iR++)
            {
                for (iC = 1; iC < _Rows; iC++)
                {//center square no elipse set for the opening move
                    if (!((iR < 3 && iC < 3) 
                        || (iR < 3 && iC > 5) 
                        || (iR > 5 && iC < 3) 
                        || (iR > 5 && iC > 5)
                        ||(iR==4&&iC==4)))
                    {
                        myEl = new Ellipse();
                        myEl.Name = "el_" + iR + "_" + iC;
                        myEl.Tag = "peices";
                        myEl.Fill = new SolidColorBrush(Colors.Silver);
                        myEl.Height = 40;
                        myEl.Width = 40;
                        myEl.SetValue(Grid.RowProperty, iR);
                        myEl.SetValue(Grid.ColumnProperty, iC);

                        myEl.Tapped += myEl_Tapped;
                        grdGame.Children.Add(myEl);
                        

                    }
                   
                }
                
            }
            
        }
        Ellipse moveMe;
        Border possible1;
         
        private void myEl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
            int toR1,toR2,toC1,toC2 = 0;
            Ellipse current = (Ellipse)sender;
            Debug.WriteLine(current.Name);
            moveMe = current;
            //current.Fill = new SolidColorBrush(Colors.Blue);
            // move cats up, mouse down.
            toR1 = (int)current.GetValue(Grid.RowProperty);
            toR2 = (int)current.GetValue(Grid.RowProperty) -1;

            // find the squares below this and to the left/rigth
            // toR++;
            // Debug.WriteLine(toR);


            toC1 = (int)current.GetValue(Grid.ColumnProperty)+1;
            toC2 = (int)current.GetValue(Grid.ColumnProperty) - 1;
            //Print out rows and coloums one above and below, one to left and right.
            Debug.WriteLine("Move row 1 : "+toR1+" Move row 2: "+toR2+" MOve col 1: "+toC1+" MOve col2: "+toC2);

          possible1 = new Border();
            possible1.SetValue(Grid.RowProperty, toR1);
            possible1.SetValue(Grid.ColumnProperty, toC1);
            possible1.Background = new SolidColorBrush(Colors.Gold);
            
            grdGame.Children.Add(possible1);
        }

       

        //@todo add tapped event to peices and move them no logic for now

    }

}
