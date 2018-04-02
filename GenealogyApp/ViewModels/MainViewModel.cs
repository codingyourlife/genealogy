namespace GenealogyApp.ViewModels
{
    using GenealogyLogic.Models;
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using GenealogyLogic.Components;
    using GalaSoft.MvvmLight;
    using System.Threading.Tasks;
    using System.Threading;
    using GenealogyLogic.Interfaces;
    using GenealogyLogic;
    using GenealogyLogic.Enums;
    using Svg;
    using System.Windows.Media.Imaging;
    using System.IO;
    using System.Windows.Input;
    using System.Windows;
    using GenealogyLogic.Exceptions;

    public class MainViewModel : ViewModelBase
    {
        private SemaphoreSlim updateImageSemaphore = new SemaphoreSlim(1);

        public MainViewModel()
        {
            _genealogyMaster = this._persistor.Restore();
            this._genealogyMaster.GenealogyChanged += _genealogyMaster_GenealogyChanged;

            this.Persons = new ObservableCollection<IUIPerson>(_genealogyMaster.GetAll());
            this.Persons.CollectionChanged += Persons_CollectionChanged;

            var bgFile = new Uri("Tree-And-Leaves-Silhouette.png", UriKind.Relative);
            if(File.Exists(bgFile.ToString()))
            {
                this.BackgroundImage = new BitmapImage(bgFile);
            }

            this.UpdateImage();
        }

        private void _genealogyMaster_GenealogyChanged(object sender, EventArgs e)
        {
            this.UpdateImage();
        }

        private bool delayedUpdateImageInProgress = false;

        internal async void UpdateImage()
        {
            if(this.delayedUpdateImageInProgress)
            {
                return;
            }

            await updateImageSemaphore.WaitAsync();

            this.delayedUpdateImageInProgress = true;

            await Task.Delay(300);

            this.delayedUpdateImageInProgress = false;

            try
            {
                var renderFormat = RendererFormats.Svg;

                string filenameWithoutExtension = "genealogy";
                string filename = string.Format("{0}.{1}", filenameWithoutExtension, renderFormat);
                var all = this._genealogyMaster.GetAll();

                await this._genealogyVisualizer.WriteFile(all, filenameWithoutExtension, renderFormat);
                
                if(File.Exists(filename))
                {
                    var svgDoc = SvgDocument.Open(filename);
                    this.GenealogyImage = null; //reset to tell wpf to update
                    this.GenealogyImage = svgDoc.BaseUri;
                }
            }
            finally
            {
                updateImageSemaphore.Release();
            }
        }

        private GenealogyMaster _genealogyMaster;
        private GenealogyVisualizer _genealogyVisualizer = new GenealogyVisualizer();

        private IUIPerson _selectedPerson = new UIPerson("VN", "NN");
        public IUIPerson SelectedPerson
        {
            get
            {
                return _selectedPerson;
            }
            set
            {
                IUIPerson personToBe;
                if (value != null)
                {
                    personToBe = value;
                }
                else
                {
                    personToBe = new UIPerson("VN", "NN");
                }

                this._selectedPerson = personToBe;

                this.UpdateSelectedParents(personToBe);
                this.UpdateSelectedChildren(personToBe);

                this.UpdateImage();
            }
        }

        private void UpdateSelectedParents(IUIPerson value)
        {
            this.ParentsOfSelected.Clear();

            if(value == null)
            {
                return;
            }

            foreach (var parent in value.Parents)
            {
                this.ParentsOfSelected.Add(parent);
            }

            this.UpdateImage();
        }

        private void UpdateSelectedChildren(IUIPerson value)
        {
            this.ChildrenOfSelected.Clear();

            if (value == null)
            {
                return;
            }

            foreach (var child in value.Children)
            {
                this.ChildrenOfSelected.Add(child);
            }

            this.UpdateImage();
        }

        internal void AddChild(IUIPerson selectedPerson, IUIPerson child)
        {
            try
            {
                this._genealogyMaster.ChildRelationmanager.AddChild(selectedPerson, child);
                this.UpdateSelectedChildren(selectedPerson);
            }
            catch (InvalidStateException ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.UpdateImage();
        }

        internal void RemoveChild(IUIPerson selectedPerson, IUIPerson nonChild)
        {
            this._genealogyMaster.ChildRelationmanager.RemoveChild(selectedPerson, nonChild);
            this.UpdateSelectedChildren(selectedPerson);

            this.UpdateImage();
        }

        internal void AddParent(IUIPerson selectedPerson, IUIPerson parent)
        {
            try
            {
                this._genealogyMaster.ParentRelationManager.AddParent(selectedPerson, parent);
                this.UpdateSelectedParents(selectedPerson);
            }
            catch (InvalidStateException ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.UpdateImage();
        }

        internal void RemoveParent(IUIPerson selectedPerson, IUIPerson nonParent)
        {
            this._genealogyMaster.ParentRelationManager.RemoveParent(selectedPerson, nonParent);
            this.UpdateSelectedParents(selectedPerson);

            this.UpdateImage();
        }

        public void MoveOneUp(IUIPerson personToMoveUp)
        {
            var indexOfPerson = this.Persons.IndexOf(personToMoveUp);

            if (indexOfPerson > 0)
            {
                this.Persons.Swap(indexOfPerson, indexOfPerson - 1);
                this._genealogyMaster.Swap(indexOfPerson, indexOfPerson - 1);

                this.UpdateImage();
            }
        }

        private void Persons_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IUIPerson item in e.NewItems)
                {
                    this._genealogyMaster.AddPersonAsync(item, autoDefineGender: true);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (IUIPerson item in e.OldItems)
                {
                    this._genealogyMaster.RemovePerson(item);
                }
            }

            this.UpdateImage();
        }

        public ObservableCollection<IUIPerson> Persons { get; } = new ObservableCollection<IUIPerson>();
        public ObservableCollection<IUIPerson> ParentsOfSelected { get; } = new ObservableCollection<IUIPerson>();
        public ObservableCollection<IUIPerson> ChildrenOfSelected { get; } = new ObservableCollection<IUIPerson>();

        public ICommand ExportCommand { get; set; }

        public Uri GenealogyImage { get; set; }

        public BitmapImage BackgroundImage { get; set; }

        private GenealogyPersistor _persistor = new GenealogyPersistor("genealogy.json");

        public void Persist()
        {
            this._persistor.Persist(this._genealogyMaster);
        }
    }
}
