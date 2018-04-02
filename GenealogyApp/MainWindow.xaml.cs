namespace GenealogyApp
{
    using GalaSoft.MvvmLight.Ioc;
    using GenealogyApp.ViewModels;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Linq;
    using GenealogyLogic.Interfaces;
    using GenealogyApp.Extensions;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.MainViewModel = SimpleIoc.Default.GetInstance<MainViewModel>();

            this.ParentsViewModel = SimpleIoc.Default.GetInstance<ParentsViewModel>();
            this.ParentsViewModel.ParentsOfSelected.CollectionChanged += ParentsOfSelected_CollectionChanged;

            this.ChildrenViewModel = SimpleIoc.Default.GetInstance<ChildrenViewModel>();
            this.ChildrenViewModel.ChildrenOfSelected.CollectionChanged += ChildrenOfSelected_CollectionChanged;
        }

        public MainViewModel MainViewModel { get; private set; }
        public ParentsViewModel ParentsViewModel { get; private set; }
        public ChildrenViewModel ChildrenViewModel { get; private set; }

        #region ParentsLst
        private bool listParentsUpdating = false;

        private object parentsAtomic = new object();

        private void ParentsOfSelected_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            lock (this.parentsAtomic)
            {
                this.listParentsUpdating = true;

                var toSelect = this.ParentsViewModel.ParentsOfSelected;

                LstParents.SelectedItems.Clear();
                foreach (IUIPerson item in LstParents.Items)
                {
                    if(toSelect.Contains(item))
                    {
                        LstParents.SelectedItems.Add(item);

                    }
                }

                this.listParentsUpdating = false;
            }
        }

        private void LstParents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listParentsUpdating)
            {
                return;
            }

            lock(this.parentsAtomic)
            {
                this.listParentsUpdating = true;

                var selectedItems = LstParents.SelectedItems;

                var selectedPersons = new List<IUIPerson>();
                foreach (IUIPerson item in selectedItems)
                {
                    if(selectedItems.Contains(item))
                    {
                        selectedPersons.Add(item);
                    }
                }

                var unselectedPersons = new List<IUIPerson>();
                foreach (IUIPerson item in LstParents.Items)
                {
                    if(!selectedItems.Contains(item))
                    {
                        unselectedPersons.Add(item);
                    }
                }

                this.ParentsViewModel.SelectedParentsUpdatedViaCodeBehind(selectedPersons, unselectedPersons);

                this.listParentsUpdating = false;
            }
        }
        #endregion

        #region ChildrenList
        private bool listChildrenUpdating = false;

        private object childrenAtomic = new object();

        private void ChildrenOfSelected_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            lock (this.childrenAtomic)
            {
                this.listChildrenUpdating = true;

                var toSelect = this.ChildrenViewModel.ChildrenOfSelected;

                LstChildren.SelectedItems.Clear();
                foreach (var item in LstChildren.Items)
                {
                    if (toSelect.Contains(item))
                    {
                        LstChildren.SelectedItems.Add(item);

                    }
                }

                this.listChildrenUpdating = false;
            }
        }

        private void LstChildren_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listChildrenUpdating)
            {
                return;
            }

            lock (this.childrenAtomic)
            {
                this.listChildrenUpdating = true;

                var selectedItems = LstChildren.SelectedItems;

                var selectedPersons = new List<IUIPerson>();
                foreach (IUIPerson item in selectedItems)
                {
                    if (selectedItems.Contains(item))
                    {
                        selectedPersons.Add(item);
                    }
                }

                var unselectedPersons = new List<IUIPerson>();
                foreach (IUIPerson item in LstChildren.Items)
                {
                    if (!selectedItems.Contains(item))
                    {
                        unselectedPersons.Add(item);
                    }
                }

                this.ChildrenViewModel.SelectedChildrenUpdatedViaCodeBehind(selectedPersons, unselectedPersons);

                this.listParentsUpdating = false;
            }
        }
        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                this.MainViewModel.Persist();
            }
            catch (System.Exception ex)
            {

                MessageBox.Show(string.Format("Failed to save {0}", ex));
                e.Cancel = true;
            }
        }

        private void LstPersons_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedPersonInList = this.LstPersons.SelectedItem as IUIPerson;

            if(selectedPersonInList != null)
            {
                var personDetailsWindow = new PersonDetails(selectedPersonInList);
                personDetailsWindow.ShowDialog();

                this.MainViewModel.UpdateImage();
            }
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Export();
                Process.Start("export.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Export()
        {
            GrdGenealogyImage.RenderToBitmap(5, "export.png", System.Windows.Media.Brushes.White);
        }
    }
}
