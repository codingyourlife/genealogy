namespace Shields.GraphViz.Models
{
    using Shields.GraphViz.Interfaces;
    using System.Collections.Immutable;
    using System.IO;

    public class SubgraphStatement : IWriteTo
    {
        public SubgraphStatement(Id name, ImmutableList<IWriteTo> statements)
        {
            this.Name = name.Value;
            this.Statements = statements;
        }

        public Id Name { get; }
        public ImmutableList<IWriteTo> Statements { get; }

        public void WriteTo(StreamWriter writer, GraphKinds graphKind)
        {
            writer.Write(string.Format("subgraph {0}", Name.Value));
            writer.WriteLine("{");

            writer.WriteLine("rank = 1");

            foreach (var statement in this.Statements)
            {
                statement.WriteTo(writer, graphKind);
                writer.WriteLine();
            }

            writer.Write("}");
        }
    }
}
