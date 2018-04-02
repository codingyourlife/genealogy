﻿namespace GenealogyApp
{
    using SharpVectors.Converters;
    using System;
    using System.Windows;

    public class SvgViewboxAttachedProperties : DependencyObject
    {
        public static string GetSource(DependencyObject obj)
        {
            return (string)obj.GetValue(SourceProperty);
        }

        public static void SetSource(DependencyObject obj, string value)
        {
            obj.SetValue(SourceProperty, value);
        }

        private static void OnSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var svgControl = obj as SvgViewbox;
            if (svgControl != null)
            {
                var path = (string)e.NewValue;
                svgControl.Source = string.IsNullOrWhiteSpace(path) ? default(Uri) : new Uri(path);
            }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached("Source", typeof(string), typeof(SvgViewboxAttachedProperties), new PropertyMetadata(null, OnSourceChanged));
    }
}
