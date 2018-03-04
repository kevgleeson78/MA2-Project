using System;
using System.Collections.Generic;
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
        private void addPieces()
        {
            Ellipse ellps;
            int i;

           

            
            for (i = 1; i < 8; i++)
            {
                
                    ellps = new Ellipse();
                    ellps.Name = "piece" + i.ToString();
                    ellps.Tag = "piece";
                    ellps.Height = 20;
                    ellps.Width = 20;
                    ellps.Fill = new SolidColorBrush(Colors.Violet);
             
                    ellps.SetValue(Grid.RowProperty, i); // at the top
                    ellps.SetValue(Grid.ColumnProperty, i);
                    
                    grdGame.Children.Add(ellps);
                
            }

        }

    }
  
}
