namespace GenealogyLogicTests.Components
{
    using GenealogyLogic.Components;
    using GenealogyLogic.Enums;
    using GenealogyLogic.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    [TestClass]
    public class GenealogyVisualizerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            this.GenealogyVisualizerModel = new GenealogyVisualizer();
            this.GenealogyMaster = new GenealogyMaster();
        }

        public GenealogyVisualizer GenealogyVisualizerModel { get; private set; }
        public GenealogyMaster GenealogyMaster { get; private set; }

        [TestMethod]
        public async Task FatherAndSon()
        {
            var filename = "homerAndBart.png";

            var parent = new UIPerson("Homer", "Simpson");
            parent.DateOfBirth = new DateTime(1990, 12, 10);
            parent.DateOfDeath = new DateTime(1990, 12, 12);
            this.GenealogyMaster.AddPerson(parent);
            this.GenealogyMaster.ChildRelationmanager.AddChild(parent, new UIPerson("Bart", "Simpson"));

            await this.GenealogyVisualizerModel.WriteFile(this.GenealogyMaster.GetAll(), filename, RendererFormats.Png, false);

            Assert.IsTrue(File.Exists(filename));
            Process.Start(filename);
        }

        [TestMethod]
        public async Task FatherMotherAndSon()
        {
            var filename = "homerMargeAndBart.png";
            var renderFormat = RendererFormats.Png;

            var father = new UIPerson("Homer", "Simpson");
            var mother = new UIPerson("Marge", "Simpson") { IsFemale = true };
            var child = new UIPerson("Bart", "Simpson");

            this.GenealogyMaster.AddPerson(father);
            this.GenealogyMaster.AddPerson(mother);
            this.GenealogyMaster.ChildRelationmanager.AddChild(father, child);
            this.GenealogyMaster.ChildRelationmanager.AddChild(mother, child);

            var persons = this.GenealogyMaster.GetAll();
            await this.GenealogyVisualizerModel.WriteFile(persons, filename, renderFormat, false);

            Assert.IsTrue(File.Exists(Path.GetFileNameWithoutExtension(filename) + "." + renderFormat));
            Process.Start(Path.GetFileNameWithoutExtension(filename) + "." + renderFormat);
        }

        [TestMethod]
        public async Task SvgWorks()
        {
            var filename = "homerMargeAndBart.svg";
            var renderFormat = RendererFormats.Svg;

            var father = new UIPerson("Homer", "Simpson");
            var mother = new UIPerson("Marge", "Simpson") { IsFemale = true };
            var child = new UIPerson("Bart", "Simpson");

            this.GenealogyMaster.AddPerson(father);
            this.GenealogyMaster.AddPerson(mother);
            this.GenealogyMaster.ChildRelationmanager.AddChild(father, child);
            this.GenealogyMaster.ChildRelationmanager.AddChild(mother, child);

            var persons = this.GenealogyMaster.GetAll();
            await this.GenealogyVisualizerModel.WriteFile(persons, filename, renderFormat, false);

            Assert.IsTrue(File.Exists(Path.GetFileNameWithoutExtension(filename) + "." + renderFormat));
            Process.Start(Path.GetFileNameWithoutExtension(filename) + "." + renderFormat);
        }
        
        [TestMethod]
        public async Task FatherMotherAndThreeChildren()
        {
            var filename = "homerMargeBartLisaMaggie.png";
            var renderFormat = RendererFormats.Png;
            
            var father = new UIPerson("Homer", "Simpson");
            var mother = new UIPerson("Marge", "Simpson") { IsFemale = true };
            var child1 = new UIPerson("Bart", "Simpson");
            var child2 = new UIPerson("Lisa", "Simpson") { IsFemale = true };
            var child3 = new UIPerson("Maggie", "Simpson") { IsFemale = true };
            this.GenealogyMaster.AddPerson(father);
            this.GenealogyMaster.AddPerson(mother);

            this.GenealogyMaster.ChildRelationmanager.AddChild(father, child1);
            this.GenealogyMaster.ChildRelationmanager.AddChild(mother, child1);

            this.GenealogyMaster.ChildRelationmanager.AddChild(father, child2);
            this.GenealogyMaster.ChildRelationmanager.AddChild(mother, child2);

            this.GenealogyMaster.ChildRelationmanager.AddChild(father, child3);
            this.GenealogyMaster.ChildRelationmanager.AddChild(mother, child3);

            var persons = this.GenealogyMaster.GetAll();
            await this.GenealogyVisualizerModel.WriteFile(persons, filename, renderFormat, false);

            Assert.IsTrue(File.Exists(Path.GetFileNameWithoutExtension(filename) + "." + renderFormat));
            Process.Start(Path.GetFileNameWithoutExtension(filename) + "." + renderFormat);
        }

        [TestMethod]
        public async Task GranpaGranmaFatherMotherSon()
        {
            var filename = "granpaGranmaHomerMargeAndBart.png";
            var renderFormat = RendererFormats.Png;

            var granpa = new UIPerson("Abraham", "Simpson");
            var granma = new UIPerson("Mona", "Simpson");
            var father = new UIPerson("Homer", "Simpson");
            var mother = new UIPerson("Marge", "Simpson");
            var child1 = new UIPerson("Bart", "Simpson");

            this.GenealogyMaster.AddPerson(mother);
            this.GenealogyMaster.AddPerson(father);

            this.GenealogyMaster.ChildRelationmanager.AddChild(mother, child1);
            this.GenealogyMaster.ChildRelationmanager.AddChild(father, child1);

            this.GenealogyMaster.ParentRelationManager.AddParent(father, granpa);
            this.GenealogyMaster.ParentRelationManager.AddParent(father, granma);

            var persons = this.GenealogyMaster.GetAll();
            await this.GenealogyVisualizerModel.WriteFile(persons, filename, renderFormat, false);

            Assert.IsTrue(File.Exists(Path.GetFileNameWithoutExtension(filename) + "." + renderFormat));
            Process.Start(Path.GetFileNameWithoutExtension(filename) + "." + renderFormat);
        }

        [TestMethod]
        public async Task FamilyWith3Kids_WithBastard()
        {
            var filename = "familyWithBastard.png";
            var renderFormat = RendererFormats.Png;

            var father = new UIPerson("Homer", "Simpson");
            var mother = new UIPerson("Marge", "Simpson");
            var child1 = new UIPerson("Bart", "Simpson");
            var child2 = new UIPerson("Lisa", "Simpson");
            var child3 = new UIPerson("Maggie", "Simpson");

            var bitch = new UIPerson("Bitch", "Samson");
            var bastard = new UIPerson("Bastard", "Samson");

            this.GenealogyMaster.AddPerson(father);
            this.GenealogyMaster.AddPerson(mother);
            this.GenealogyMaster.AddPerson(child1);
            this.GenealogyMaster.AddPerson(child2);
            this.GenealogyMaster.AddPerson(child3);
            this.GenealogyMaster.AddPerson(bitch);
            this.GenealogyMaster.AddPerson(bastard);

            this.GenealogyMaster.ChildRelationmanager.AddChild(father, child1);
            this.GenealogyMaster.ChildRelationmanager.AddChild(mother, child1);
            this.GenealogyMaster.ChildRelationmanager.AddChild(father, child2);
            this.GenealogyMaster.ChildRelationmanager.AddChild(mother, child2);
            this.GenealogyMaster.ChildRelationmanager.AddChild(father, child3);
            this.GenealogyMaster.ChildRelationmanager.AddChild(mother, child3);

            this.GenealogyMaster.ChildRelationmanager.AddChild(father, bastard);
            this.GenealogyMaster.ChildRelationmanager.AddChild(bitch, bastard);

            var persons = this.GenealogyMaster.GetAll();
            await this.GenealogyVisualizerModel.WriteFile(persons, filename, renderFormat, false);

            Assert.IsTrue(File.Exists(Path.GetFileNameWithoutExtension(filename) + "." + renderFormat));
            Process.Start(Path.GetFileNameWithoutExtension(filename) + "." + renderFormat);
        }

        [TestMethod]
        public async Task FamilyWith3Kids_WithBastard_ThenDisproven_NotFromFather()
        {
            var filename = "familyWithBastard_disproven.png";
            var renderFormat = RendererFormats.Png;

            var father = new UIPerson("Homer", "Simpson");
            var mother = new UIPerson("Marge", "Simpson");
            var child1 = new UIPerson("Bart", "Simpson");
            var child2 = new UIPerson("Lisa", "Simpson");
            var child3 = new UIPerson("Maggie", "Simpson");

            var bitch = new UIPerson("Bitch", "Samson");
            var bastard = new UIPerson("Bastard", "Samson");

            this.GenealogyMaster.AddPerson(father);
            this.GenealogyMaster.AddPerson(mother);
            this.GenealogyMaster.AddPerson(child1);
            this.GenealogyMaster.AddPerson(child2);
            this.GenealogyMaster.AddPerson(child3);
            this.GenealogyMaster.AddPerson(bitch);
            this.GenealogyMaster.AddPerson(bastard);

            this.GenealogyMaster.ChildRelationmanager.AddChild(father, child1);
            this.GenealogyMaster.ChildRelationmanager.AddChild(mother, child1);
            this.GenealogyMaster.ChildRelationmanager.AddChild(father, child2);
            this.GenealogyMaster.ChildRelationmanager.AddChild(mother, child2);
            this.GenealogyMaster.ChildRelationmanager.AddChild(father, child3);
            this.GenealogyMaster.ChildRelationmanager.AddChild(mother, child3);

            this.GenealogyMaster.ChildRelationmanager.AddChild(father, bastard);
            this.GenealogyMaster.ChildRelationmanager.AddChild(bitch, bastard);

            this.GenealogyMaster.ChildRelationmanager.RemoveChild(father, bastard); //not his bastard

            var persons = this.GenealogyMaster.GetAll();
            await this.GenealogyVisualizerModel.WriteFile(persons, filename, renderFormat, false);

            Assert.IsTrue(File.Exists(Path.GetFileNameWithoutExtension(filename) + "." + renderFormat));
            Process.Start(Path.GetFileNameWithoutExtension(filename) + "." + renderFormat);
        }
    }
}
