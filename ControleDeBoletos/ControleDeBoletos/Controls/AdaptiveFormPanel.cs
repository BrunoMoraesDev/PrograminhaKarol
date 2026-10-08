using System;
using System.Windows;
using System.Windows.Controls;

namespace ControleDeBoletos.Controls
{
    // Keeps fields readable instead of scaling the entire interface down.
    public class AdaptiveFormPanel : Panel
    {
        public static readonly DependencyProperty MinColumnWidthProperty = DependencyProperty.Register(
            nameof(MinColumnWidth), typeof(double), typeof(AdaptiveFormPanel),
            new FrameworkPropertyMetadata(320d, FrameworkPropertyMetadataOptions.AffectsMeasure),
            value => (double)value > 0 && double.IsFinite((double)value));

        public static readonly DependencyProperty MaxColumnsProperty = DependencyProperty.Register(
            nameof(MaxColumns), typeof(int), typeof(AdaptiveFormPanel),
            new FrameworkPropertyMetadata(2, FrameworkPropertyMetadataOptions.AffectsMeasure),
            value => (int)value > 0);

        public double MinColumnWidth
        {
            get => (double)GetValue(MinColumnWidthProperty);
            set => SetValue(MinColumnWidthProperty, value);
        }

        public int MaxColumns
        {
            get => (int)GetValue(MaxColumnsProperty);
            set => SetValue(MaxColumnsProperty, value);
        }

        private int ColumnCount(double width) => double.IsInfinity(width)
            ? MaxColumns : Math.Max(1, Math.Min(MaxColumns, (int)(width / MinColumnWidth)));

        protected override Size MeasureOverride(Size availableSize)
        {
            int columns = ColumnCount(availableSize.Width);
            double width = double.IsInfinity(availableSize.Width)
                ? MinColumnWidth : availableSize.Width / columns;
            double height = 0;
            double rowHeight = 0;
            int index = 0;
            foreach (UIElement child in InternalChildren)
            {
                if (child.Visibility == Visibility.Collapsed) continue;
                child.Measure(new Size(width, double.PositiveInfinity));
                rowHeight = Math.Max(rowHeight, child.DesiredSize.Height);
                if (++index % columns == 0)
                {
                    height += rowHeight;
                    rowHeight = 0;
                }
            }
            return new Size(width * columns, height + rowHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int columns = ColumnCount(finalSize.Width);
            double width = finalSize.Width / columns;
            double top = 0;
            int index = 0;
            while (index < InternalChildren.Count)
            {
                int start = index;
                int count = 0;
                double height = 0;
                while (index < InternalChildren.Count && count < columns)
                {
                    UIElement child = InternalChildren[index++];
                    if (child.Visibility == Visibility.Collapsed) continue;
                    height = Math.Max(height, child.DesiredSize.Height);
                    count++;
                }
                int column = 0;
                for (int i = start; i < index; i++)
                {
                    UIElement child = InternalChildren[i];
                    if (child.Visibility == Visibility.Collapsed) continue;
                    child.Arrange(new Rect(column++ * width, top, width, height));
                }
                top += height;
            }
            return finalSize;
        }
    }
}
