﻿namespace Shields.GraphViz.Components
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Shields.GraphViz.Services;
    using Shields.GraphViz.Models;

    public class Renderer : IRenderer
    {
        private readonly string graphvizBin;

        public Renderer(string graphvizBin)
        {
            if (graphvizBin == null)
            {
                throw new ArgumentNullException("graphvizBin");
            }

            this.graphvizBin = graphvizBin;
        }

        public async Task RunAsync(Graph graph, Stream outputStream, RendererLayouts layout, RendererFormats format, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var fileName = Path.Combine(graphvizBin, GetRendererLayoutExecutable(layout));
            var arguments = GetRendererFormatArgument(format);
            var startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            var tcs = new TaskCompletionSource<object>();
            using (var process = Process.Start(startInfo))
            {
                cancellationToken.ThrowIfCancellationRequested();
                cancellationToken.Register(() =>
                {
                    tcs.TrySetCanceled();
                    process.CloseMainWindow();
                });

                using (var sw = new StreamWriter("rendererOutput.txt", false))
                {
                    using (var standardInput = process.StandardInput)
                    {
                        graph.WriteTo(standardInput);
                        graph.WriteTo(sw);
                    }
                }
                
                using (var standardOutput = process.StandardOutput)
                {
                    await Task.WhenAny(tcs.Task, standardOutput.BaseStream.CopyToAsync(outputStream, 4096, cancellationToken));
                }
            }
            cancellationToken.ThrowIfCancellationRequested();
        }

        private static string Escape(string s)
        {
            const string quote = "\"";
            const string backslash = "\\";
            return quote + s.Replace(quote, backslash + quote) + quote;
        }

        private static string GetRendererLayoutExecutable(RendererLayouts layout)
        {
            switch (layout)
            {
                case RendererLayouts.Dot:
                    return "dot.exe";
                default:
                    throw new ArgumentOutOfRangeException("layout");
            }
        }

        private static string GetRendererFormatArgument(RendererFormats format)
        {
            switch (format)
            {
                case RendererFormats.Png:
                    return "-Tpng";
                case RendererFormats.Svg:
                    return "-Tsvg";
                default:
                    throw new ArgumentOutOfRangeException("format");
            }
        }

        public void Dispose()
        {
        }
    }
}
