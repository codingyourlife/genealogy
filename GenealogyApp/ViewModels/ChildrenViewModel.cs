namespace GenealogyApp.ViewModels
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Ioc;
    using GenealogyLogic.Interfaces;
    using GenealogyLogic.Models;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class ChildrenViewModel : ViewModelBase
    {
        public ChildrenViewModel()
        {
            this.MainViewModel = SimpleIoc.Default.GetInstance<MainViewModel>();
        }

        public MainViewModel MainViewModel { get; private set; }

        public ObservableCollection<IUIPerson> Persons { get { return this.MainViewModel.Persons; } }

        public ObservableCollection<IUIPerson> ChildrenOfSelected { get { return this.MainViewModel.ChildrenOfSelected; } }

        internal void SelectedChildrenUpdatedViaCodeBehind(List<IUIPerson> selectedPersons, List<IUIPerson> unselectedPersons)
        {
            foreach (IUIPerson child in selectedPersons)
            {
                this.MainViewModel.AddChild(this.MainViewModel.SelectedPerson, child);
            }

            foreach (IUIPerson nonChild in unselectedPersons)
            {
                this.MainViewModel.RemoveChild(this.MainViewModel.SelectedPerson, nonChild);
            }
        }
    }
}
