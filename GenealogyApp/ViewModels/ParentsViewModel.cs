namespace GenealogyApp.ViewModels
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Ioc;
    using GenealogyLogic.Interfaces;
    using GenealogyLogic.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Markup;

    public class ParentsViewModel : ViewModelBase
    {
        public ParentsViewModel()
        {
            this.MainViewModel = SimpleIoc.Default.GetInstance<MainViewModel>();
        }

        public MainViewModel MainViewModel { get; private set; }

        public ObservableCollection<IUIPerson> Persons { get { return this.MainViewModel.Persons; } }

        public ObservableCollection<IUIPerson> ParentsOfSelected { get { return this.MainViewModel.ParentsOfSelected; } }

        internal void SelectedParentsUpdatedViaCodeBehind(List<IUIPerson> selectedPersons, List<IUIPerson> unselectedPersons)
        {
            foreach (var parent in selectedPersons)
            {
                this.MainViewModel.AddParent(this.MainViewModel.SelectedPerson, parent);
            }

            foreach (var nonParent in unselectedPersons)
            {
                this.MainViewModel.RemoveParent(this.MainViewModel.SelectedPerson, nonParent);
            }
        }
    }
}
