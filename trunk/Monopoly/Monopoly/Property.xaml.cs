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

namespace Monopoly
{
    /// <summary>
    /// Interaction logic for Property.xaml
    /// </summary>
    public partial class Property : UserControl
    {
        #region Dependency Properties
        public static DependencyProperty PropertyColorProperty = DependencyProperty.Register("PropertyColor", typeof(Brush), typeof(Property));
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
        }
        #endregion

        public enum Location
        {
            TOP = 0,
            RIGHT = 1,
            BOTTOM = 2,
            LEFT = 3
        }

        public Property(int loc)
        {
            InitializeComponent();
            switch (loc)
            {
                case (int)Location.TOP:
                    Top.Visibility = Visibility.Visible;
                    break;
                case (int)Location.RIGHT:
                    Right.Visibility = Visibility.Visible;
                    break;
                case (int)Location.LEFT:
                    Left.Visibility = Visibility.Visible;
                    break;
                case (int)Location.BOTTOM:
                    Bottom.Visibility = Visibility.Visible;
                    break;
                default :
                    break;
            }
        }
    }
}
