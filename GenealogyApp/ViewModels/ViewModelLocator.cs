namespace GenealogyApp
{
    using CommonServiceLocator;
    using GalaSoft.MvvmLight.Ioc;
    using GenealogyApp.ViewModels;
    using GenealogyLogic.Components;

    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<PersonsViewModel>();
            SimpleIoc.Default.Register<ParentsViewModel>();
            SimpleIoc.Default.Register<ChildrenViewModel>();
            SimpleIoc.Default.Register<PersonDetailsViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();

            SimpleIoc.Default.Register<GenealogyMaster>();
        }

        public PersonsViewModel PersonsViewModel { get { return SimpleIoc.Default.GetInstance<PersonsViewModel>(); } }
        public ParentsViewModel ParentsViewModel { get { return SimpleIoc.Default.GetInstance<ParentsViewModel>(); } }
        public ChildrenViewModel ChildrenViewModel { get { return SimpleIoc.Default.GetInstance<ChildrenViewModel>(); } }
        public PersonDetailsViewModel PersonDetailsViewModel { get { return SimpleIoc.Default.GetInstance<PersonDetailsViewModel>(); } }
        public MainViewModel MainViewModel { get { return SimpleIoc.Default.GetInstance<MainViewModel>(); } }
    }
}
