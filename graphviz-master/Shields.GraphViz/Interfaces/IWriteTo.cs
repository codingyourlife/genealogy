namespace Shields.GraphViz.Interfaces
{
    using Shields.GraphViz.Models;
    using System.IO;

    public interface IWriteTo
    {
        void WriteTo(StreamWriter writer, GraphKinds graphKind);
    }
}
