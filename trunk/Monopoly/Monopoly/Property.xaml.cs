using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;

namespace Monopoly
{
    /// <summary>
    /// Interaction logic for Property.xaml
    /// </summary>
    public partial class Property : UserControl
    {
        #region Dependency Properties
        /*public static DependencyProperty PropertyColorProperty = DependencyProperty.Register("PropertyColor", typeof(Brush), typeof(Property));
        public Brush PropertyColor
        {
            get { return (Brush)GetValue(PropertyColorProperty); }
            set { SetValue(PropertyColorProperty, value); }
        }
        public static DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(String), typeof(Property));
        public String PropertyName
        {
            get { return (String)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }
        public static DependencyProperty PropertyCostProperty = DependencyProperty.Register("PropertyCost", typeof(int), typeof(Property));
        public int PropertyCost
        {
            get { return (int)GetValue(PropertyCostProperty); }
            set { SetValue(PropertyCostProperty, value); }
        }*/
        #endregion

        public static string CONTENT_NAME = "Content";
        public static string SPOTS_NAME = "Spots";

        public Grid Spots;
        public TextBlock OwnedBy;

        public enum Side
        {
            TOP = 0,
            RIGHT = 1,
            BOTTOM = 2,
            LEFT = 3,
            UNKNOWN = -1
        }

        public PropertyListing PropertyListing
        {
            get{ return property; }
        }
        private PropertyListing property;

        public Property(int loc, PropertyListing property)
        {
            this.property = property;
            InitializeComponent();
            switch (loc)
            {
                case (int)Side.TOP:
                    TopLayout();
                    break;
                case (int)Side.RIGHT:
                    RightLayout();
                    break;
                case (int)Side.LEFT:
                    LeftLayout();
                    break;
                case (int)Side.BOTTOM:
                    BottomLayout();
                    break;
                default :
                    break;
            }
            myGrid.Children.Add(mGrid);
        }

        Grid mGrid;

        private Border getGroupBorder()
        {
            Border b = null;

            if (!property.IsSpecial)
            {
                b = new Border();
                b.BorderBrush = Brushes.Black;
                b.BorderThickness = new Thickness(1);
                b.Background = property.ColorGroup;
            }

            return b;
        }

        private Border getInfoBorder()
        {
            Border b = new Border();
            b.BorderBrush = Brushes.Black;
            b.BorderThickness = new Thickness(1);
            b.Background = Brushes.Transparent;

            Grid content = new Grid();
            content.Name = CONTENT_NAME;
            Spots = new Grid();
            Spots.Name = SPOTS_NAME;
            RowDefinition rd = new RowDefinition();
            rd.Height = new GridLength(25, GridUnitType.Star);
            Spots.RowDefinitions.Add(rd);
            rd = new RowDefinition();
            rd.Height = new GridLength(25, GridUnitType.Star);
            Spots.RowDefinitions.Add(rd);
            rd = new RowDefinition();
            rd.Height = new GridLength(25, GridUnitType.Star);
            Spots.RowDefinitions.Add(rd);
            rd = new RowDefinition();
            rd.Height = new GridLength(25, GridUnitType.Star);
            Spots.RowDefinitions.Add(rd);
            ColumnDefinition cd = new ColumnDefinition();
            cd.Width = new GridLength(25, GridUnitType.Star);
            Spots.ColumnDefinitions.Add(cd);
            cd = new ColumnDefinition();
            cd.Width = new GridLength(25, GridUnitType.Star);
            Spots.ColumnDefinitions.Add(cd);
            cd = new ColumnDefinition();
            cd.Width = new GridLength(25, GridUnitType.Star);
            Spots.ColumnDefinitions.Add(cd);
            cd = new ColumnDefinition();
            cd.Width = new GridLength(25, GridUnitType.Star);
            Spots.ColumnDefinitions.Add(cd);


            TextBlock tb = new TextBlock();
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            tb.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.FontSize = 9;
            tb.TextAlignment = TextAlignment.Center;
            tb.Text = property.Name;
            content.Children.Add(tb);

            if (property.Cost != 0)
            {

                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                sp.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                sp.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                tb = new TextBlock();
                tb.FontSize = 11;
                tb.Text = "$";
                sp.Children.Add(tb);

                tb = new TextBlock();
                tb.FontSize = 11;
                tb.Text = property.Cost + " ";
                sp.Children.Add(tb);
                content.Children.Add(sp); 
                
                OwnedBy = new TextBlock();
                tb.FontSize = 11;
                tb.Text = "(Not Owned)";
                sp.Children.Add(OwnedBy);
                content.Children.Add(sp);
            }
            else if(!property.IsCorner)
                tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            if (property.IsCorner)
            {
                rd = new RowDefinition();
                rd.Height = new GridLength(20, GridUnitType.Star);
                content.RowDefinitions.Add(rd);
                rd = new RowDefinition();
                rd.Height = new GridLength(60, GridUnitType.Star);
                content.RowDefinitions.Add(rd);
                rd = new RowDefinition();
                rd.Height = new GridLength(20, GridUnitType.Star);
                content.RowDefinitions.Add(rd);

                Viewbox vb = new Viewbox();
                Grid.SetRow(vb, 1);
                Image i = new Image(); 
                i.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\" + property.ImageLocation, UriKind.RelativeOrAbsolute)); ;
                vb.Child = i;
                content.Children.Add(vb);
            }
            Grid.SetColumnSpan(Spots, 100);
            Grid.SetRowSpan(Spots, 100);
            content.Children.Add(Spots);
            b.Child = content;

            return b;
        }
        
        private void TopLayout()
        {
            mGrid = new Grid();
            RowDefinition rd = new RowDefinition();
            rd.Height = new GridLength(80, GridUnitType.Star);
            mGrid.RowDefinitions.Add(rd);

            rd = new RowDefinition();
            rd.Height = new GridLength(20, GridUnitType.Star);
            mGrid.RowDefinitions.Add(rd);

            Border b = getGroupBorder();
            if (b != null)
            {
                Grid.SetRow(b, 1);
                mGrid.Children.Add(b);
            }
            else
            {
                mGrid.RowDefinitions.RemoveAt(1);
                mGrid.RowDefinitions.RemoveAt(0);
            }
            b = getInfoBorder();
            Grid.SetRow(b, 0);
            mGrid.Children.Add(b);
        }

        private void BottomLayout()
        {
            mGrid = new Grid();
            RowDefinition rd = new RowDefinition();
            rd.Height = new GridLength(20, GridUnitType.Star);
            mGrid.RowDefinitions.Add(rd);

            rd = new RowDefinition();
            rd.Height = new GridLength(80, GridUnitType.Star);
            mGrid.RowDefinitions.Add(rd);

            Border b = getGroupBorder();
            if (b != null)
            {
                Grid.SetRow(b, 0);
                mGrid.Children.Add(b);
            }
            else
            {
                mGrid.RowDefinitions.RemoveAt(1);
                mGrid.RowDefinitions.RemoveAt(0);
            }

            b = getInfoBorder();
            Grid.SetRow(b, 1);
            mGrid.Children.Add(b);
        }

        private void RightLayout()
        {
            mGrid = new Grid();
            ColumnDefinition cd = new ColumnDefinition();
            cd.Width = new GridLength(20, GridUnitType.Star);
            mGrid.ColumnDefinitions.Add(cd);

            cd = new ColumnDefinition();
            cd.Width = new GridLength(80, GridUnitType.Star);
            mGrid.ColumnDefinitions.Add(cd);

            Border b = getGroupBorder();
            if (b != null)
            {
                Grid.SetColumn(b, 0);
                mGrid.Children.Add(b);
            }
            else
            {
                mGrid.ColumnDefinitions.RemoveAt(1);
                mGrid.ColumnDefinitions.RemoveAt(0);
            }

            b = getInfoBorder();
            Grid.SetColumn(b, 1);
            mGrid.Children.Add(b);                        
        }

        private void LeftLayout()
        {
            mGrid = new Grid();
            ColumnDefinition cd = new ColumnDefinition();
            cd.Width = new GridLength(80, GridUnitType.Star);
            mGrid.ColumnDefinitions.Add(cd);

            cd = new ColumnDefinition();
            cd.Width = new GridLength(20, GridUnitType.Star);
            mGrid.ColumnDefinitions.Add(cd);

            Border b = getGroupBorder();
            if (b != null)
            {
                Grid.SetColumn(b, 1);
                mGrid.Children.Add(b);
            }
            else
            {
                mGrid.ColumnDefinitions.RemoveAt(1);
                mGrid.ColumnDefinitions.RemoveAt(0);
            }

            b = getInfoBorder();
            Grid.SetColumn(b, 0);
            mGrid.Children.Add(b);  
        }

        private void CornerLayout()
        {
            mGrid = new Grid();
            RowDefinition rd = new RowDefinition();
            rd.Height = new GridLength(20, GridUnitType.Star);
            mGrid.RowDefinitions.Add(rd);
            rd = new RowDefinition();
            rd.Height = new GridLength(80, GridUnitType.Star);
            mGrid.RowDefinitions.Add(rd);

            Border b = getGroupBorder();
            if (b != null)
            {
                Grid.SetRow(b, 0);
                mGrid.Children.Add(b);
            }
            else
            {
                mGrid.RowDefinitions.RemoveAt(1);
                mGrid.RowDefinitions.RemoveAt(0);
            }

            b = getInfoBorder();
            Grid.SetRow(b, 1);
            mGrid.Children.Add(b);
        }
    }
}
