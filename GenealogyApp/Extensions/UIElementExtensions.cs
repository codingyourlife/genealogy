namespace GenealogyApp.Extensions
{
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public static class UIElementExtensions
    {
        public static BitmapSource RenderToBitmap(this UIElement element, double scale, string filename, Brush background)
        {
            var renderWidth = (int)(element.RenderSize.Width * scale);
            var renderHeight = (int)(element.RenderSize.Height * scale);

            var renderTarget = new RenderTargetBitmap(renderWidth, renderHeight, 96, 96, PixelFormats.Default);
            var sourceBrush = new VisualBrush(element);

            var drawingVisual = new DrawingVisual();
            var drawingContext = drawingVisual.RenderOpen();

            var rect = new Rect(0, 0, element.RenderSize.Width, element.RenderSize.Height);

            using (drawingContext)
            {
                drawingContext.PushTransform(new ScaleTransform(scale, scale));
                drawingContext.DrawRectangle(background, null, rect); // here
                drawingContext.DrawRectangle(sourceBrush, null, rect);
            }

            renderTarget.Render(drawingVisual);

            SaveToFile(renderTarget, filename);

            return renderTarget;
        }

        private static void SaveToFile(RenderTargetBitmap renderTarget, string filename)
        {
            using (var stream = new FileStream("export.png", FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTarget));

                encoder.Save(stream);
            }
        }
    }
}
