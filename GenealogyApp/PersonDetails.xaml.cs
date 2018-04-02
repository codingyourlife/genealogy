namespace GenealogyApp
{
    using GalaSoft.MvvmLight.Ioc;
    using GenealogyApp.ViewModels;
    using GenealogyLogic.Interfaces;
    using System.Windows;

    /// <summary>
    /// Interaktionslogik für PersonDetails.xaml
    /// </summary>
    public partial class PersonDetails : Window
    {
        public PersonDetailsViewModel PersonDetailsViewModel = SimpleIoc.Default.GetInstance<PersonDetailsViewModel>();

        public PersonDetails(IUIPerson selectedPersonInList)
        {
            PersonDetailsViewModel.Person = selectedPersonInList;

            InitializeComponent();
        }
    }
}
