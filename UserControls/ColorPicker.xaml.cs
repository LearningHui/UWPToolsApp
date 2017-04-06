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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UserControls
{
    public sealed partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            this.InitializeComponent();
        }
        
        #region ColorProperty
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(ColorPicker), new PropertyMetadata(Colors.Black, OnColorChanged));

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker colorPicker = (ColorPicker)d;

            Color oldColor = (Color)e.OldValue;
            Color newColor = (Color)e.NewValue;
            colorPicker.Red = newColor.R;
            colorPicker.Green = newColor.G;
            colorPicker.Blue = newColor.B;

            //colorPicker.previousColor = oldColor;
            //colorPicker.OnColorChanged(oldColor, newColor);
        }
        #endregion

        private static void OnColorRGBChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker colorPicker = (ColorPicker)d;
            Color color = colorPicker.Color;
            if (e.Property == RedProperty)
                color.R = (byte)(double)e.NewValue;
            else if (e.Property == GreenProperty)
                color.G = (byte)(double)e.NewValue;
            else if (e.Property == BlueProperty)
                color.B = (byte)(double)e.NewValue;

            colorPicker.Color = color;
        }

        #region RedProperty
        public double Red
        {
            get { return (byte)GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }
        public static readonly DependencyProperty RedProperty =
            DependencyProperty.Register("Red", typeof(double), typeof(ColorPicker), new PropertyMetadata(0, OnColorRGBChanged));

        #endregion

        #region GreenProperty
        public double Green
        {
            get { return (Byte)GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }
        public static readonly DependencyProperty GreenProperty =
            DependencyProperty.Register("Green", typeof(double), typeof(ColorPicker), new PropertyMetadata(0, OnColorRGBChanged));

        #endregion

        #region  BlueProperty
        public double Blue
        {
            get { return (double)GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }
        public static readonly DependencyProperty BlueProperty =
            DependencyProperty.Register("Blue", typeof(double), typeof(ColorPicker), new PropertyMetadata(0, OnColorRGBChanged));

        #endregion

        

    }
}
