namespace GenealogyApp.ViewModels
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Ioc;
    using GenealogyLogic.Interfaces;
    using GenealogyLogic.Models;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class PersonsViewModel : ViewModelBase
    {
        public PersonsViewModel()
        {
            this.AddPersonCommand = new RelayCommand(AddPersonMethod);
            this.RemovePersonCommand = new RelayCommand<IUIPerson>(RemovePersonMethod);
            this.MoveOneUpCommand = new RelayCommand<IUIPerson>(MoveOneUpMethod);

            this.MainViewModel = SimpleIoc.Default.GetInstance<MainViewModel>();
        }

        private void MoveOneUpMethod(IUIPerson personToMoveUp)
        {
            this.MainViewModel.MoveOneUp(personToMoveUp);
        }

        public MainViewModel MainViewModel { get; private set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICommand AddPersonCommand { get; set; }
        public ICommand RemovePersonCommand { get; set; }
        public ICommand MoveOneUpCommand { get; set; }

        public ObservableCollection<IUIPerson> Persons { get { return this.MainViewModel.Persons; } }

        private void AddPersonMethod()
        {
            var coolGuy = new UIPerson(this.FirstName, this.LastName);
            this.Persons.Add(coolGuy);

            this.FirstName = string.Empty;
        }

        private void RemovePersonMethod(IUIPerson personToRemove)
        {
            this.Persons.Remove(personToRemove);
        }
    }
}
