namespace GenealogyLogic.Components
{
    using GenealogyLogic.Enums;
    using GenealogyLogic.Interfaces;
    using Shields.GraphViz.Components;
    using Shields.GraphViz.Interfaces;
    using Shields.GraphViz.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GenealogyVisualizer
    {
        private string graphvizbin;
        private object locker = new object();

        public GenealogyVisualizer()
        {
            graphvizbin = Path.Combine(Environment.CurrentDirectory, @"_libs/Graphviz2.38");

            if (!Directory.Exists(graphvizbin))
            {
                throw new IOException("graphviz is not installed");
            }
        }

        public async Task WriteFile(IEnumerable<IUIPerson> persons, string filename, RendererFormats renderFormat, bool transparent = true)
        {
            persons = persons.Where(x => !x.IsHidden).ToList();
            persons = persons.OrderByDescending(x => x.Parents.Count);

            List<IWriteTo> statements = new List<IWriteTo>();

            foreach (var person in persons)
            {
                var personNodeDict = new Dictionary<Id, Id> { { "label", person.Label }, { "shape", "box" }, { "color", person.IsFemale ? "pink" : "blue" }, { "fillcolor", "white" }, { "style", "filled" } }.ToImmutableDictionary();
                var personNode = new NodeStatement(person.Id.ToString(), personNodeDict);

                var partners = person.Partners.Where(x => !x.IsHidden);
                partners = partners.OrderByDescending(x => x.Parents.Count);
                foreach (var partner in partners)
                {
                    this.AddPartner(ref statements, person, personNode, partner);
                }

                if (IsSingleParent(person, partners))
                {
                    this.AddChildren(ref statements, person, personNode, person.Children);
                }
            }

            var statementsImmutable = statements.ToImmutableList<IWriteTo>();
            var graph = new Graph(GraphKinds.Directed, "genealogy", statementsImmutable);
            graph = graph.Add(AttributeStatement.Graph.Set("rankdir", "TB"));
            graph = graph.Add(AttributeStatement.Graph.Set("nodesep", "0.5"));
            graph = graph.Add(AttributeStatement.Graph.Set("splines", "false"));

            if(transparent)
            {
                graph = graph.Add(AttributeStatement.Graph.Set("bgcolor", "transparent"));
            }

            await this.Render(graph, filename, renderFormat);
        }

        private bool IsSingleParent(IUIPerson person, IEnumerable<IUIPerson> partners)
        {
            return !partners.Any() && person.Children.Any();
        }

        private void AddPartner(ref List<IWriteTo> statements, IUIPerson person, NodeStatement personNode, IUIPerson partner)
        {
            if(partner.IsHidden)
            {
                return;
            }

            var edgeDictPoint = new Dictionary<Id, Id> { { "shape", "point" }, { "label", "" } }.ToImmutableDictionary();
            var point = new NodeStatement("partners" + (partner.Id + "z" + person.Id), edgeDictPoint);

            var partnerNodeDict = new Dictionary<Id, Id> { { "label", partner.Label }, { "shape", "box" }, { "color", partner.IsFemale ? "pink" : "blue" }, { "fillcolor", "white" }, { "style", "filled" } }.ToImmutableDictionary();
            var partnerNode = new NodeStatement(partner.Id.ToString(), partnerNodeDict);

            var edgeDictLinePersonToPoint = new Dictionary<Id, Id> { { "dir", "none" } }.ToImmutableDictionary();
            var linePersonToPoint = new EdgeStatement(person.Id.ToString(), point.Id.ToString(), edgeDictLinePersonToPoint);

            var linePartnerToPointDict = new Dictionary<Id, Id> { { "dir", "none" } }.ToImmutableDictionary();
            var linePartnerToPoint = new EdgeStatement(partner.Id.ToString(), point.Id.ToString(), linePartnerToPointDict);

            var parentSubgraphExists = statements.Where(x => x is SubgraphStatement).Select(y => ((SubgraphStatement)y).Name).Any(z => z.Value == "partners" + (partner.Id + "z" + person.Id) || z.Value == "partners" + (person.Id + "z" + partner.Id));
            if (!parentSubgraphExists)
            {
                var parentSubgraphStatements = new List<IWriteTo>();

                parentSubgraphStatements.Add(point);

                parentSubgraphStatements.Add(partnerNode);

                parentSubgraphStatements.Add(personNode);

                parentSubgraphStatements.Add(linePersonToPoint);

                parentSubgraphStatements.Add(linePartnerToPoint);

                var parentSubgraphStatement = new SubgraphStatement(point.Id.ToString(), parentSubgraphStatements.ToImmutableList());
                statements.Add(parentSubgraphStatement);

                var childrenSubgraphExists = statements.Where(x => x is SubgraphStatement).Select(y => ((SubgraphStatement)y).Name).Any(z => z.Value == "children" + (partner.Id + "z" + person.Id) || z.Value == "children" + (person.Id + "z" + partner.Id));
                if (!childrenSubgraphExists)
                {
                    var childrenOfCouple = person.Children.Where(x => x.Parents.Contains(partner) && x.Parents.Contains(person)).Where(x => !x.IsHidden);
                    childrenOfCouple = childrenOfCouple.OrderByDescending(x => x.GetAllDescendants().Count);
                    this.AddChildren(ref statements, person, partner, point, childrenOfCouple);
                }
            }
        }

        private void AddChildren(ref List<IWriteTo> statements, IUIPerson person, NodeStatement personNode, IEnumerable<IUIPerson> childrenOfSingleParent)
        {
            childrenOfSingleParent = childrenOfSingleParent.Where(x => !x.IsHidden);
            var parentSubgraphStatements = new List<IWriteTo>();

            parentSubgraphStatements.Add(personNode);

            var parentSubgraphStatement = new SubgraphStatement(personNode.Id.ToString(), parentSubgraphStatements.ToImmutableList());
            statements.Add(parentSubgraphStatement);

            var childrenSubgraphStatements = new List<IWriteTo>();

            foreach (var child in childrenOfSingleParent)
            {
                var childNodeDict = new Dictionary<Id, Id> { { "label", child.Label }, { "shape", "box" }, { "color", child.IsFemale ? "pink" : "blue" }, { "fillcolor", "white" }, { "style", "filled" } }.ToImmutableDictionary();
                var childNode = new NodeStatement(child.Id.ToString(), childNodeDict);

                var childToSingleParentDict = new Dictionary<Id, Id> { { "dir", "none" } }.ToImmutableDictionary();
                var childToSingleParentStatement = new EdgeStatement(personNode.Id.Value, child.Id.ToString(), childToSingleParentDict);

                childrenSubgraphStatements.Add(childNode);

                statements.Add(childToSingleParentStatement);
            }

            var childrenSubgraphStatement = new SubgraphStatement("children" + (person.Id + "z" + "SINGLE"), childrenSubgraphStatements.ToImmutableList());
            statements.Add(childrenSubgraphStatement);
        }

        private void AddChildren(ref List<IWriteTo> statements, IUIPerson person, IUIPerson partner, NodeStatement point, IEnumerable<IUIPerson> childrenOfCouple)
        {
            childrenOfCouple = childrenOfCouple.Where(x => !x.IsHidden);
            var childrenSubgraphStatements = new List<IWriteTo>();

            foreach (var child in childrenOfCouple)
            {
                var childNodeDict = new Dictionary<Id, Id> { { "label", child.Label }, { "shape", "box" }, { "color", child.IsFemale ? "pink" : "blue" }, { "fillcolor", "white" }, { "style", "filled" } }.ToImmutableDictionary();
                var childNode = new NodeStatement(child.Id.ToString(), childNodeDict);

                var childToDotDict = new Dictionary<Id, Id> { { "dir", "none" } }.ToImmutableDictionary();
                var childToDotStatement = new EdgeStatement(point.Id.Value, child.Id.ToString(), childToDotDict);

                childrenSubgraphStatements.Add(childNode);

                statements.Add(childToDotStatement);
            }

            var childrenSubgraphStatement = new SubgraphStatement("children" + (person.Id + "z" + partner.Id), childrenSubgraphStatements.ToImmutableList());
            statements.Add(childrenSubgraphStatement);
        }

        private async Task Render(Graph graph, string filenameParameter, RendererFormats renderFormat)
        {
            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(filenameParameter);
            var filename = string.Format("{0}.{1}", filenameWithoutExtension, renderFormat);

            await Task.Run(() => {
                lock (this.locker)
                {
                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }

                    using (var renderer = new Renderer(graphvizbin))
                    {
                        using (var file = File.Create(filename))
                        {
                            renderer.RunAsync(graph, file, Shields.GraphViz.Services.RendererLayouts.Dot, (Shields.GraphViz.Services.RendererFormats)renderFormat, CancellationToken.None).Wait();
                        }
                    }
                }
            });
        }
    }
}
