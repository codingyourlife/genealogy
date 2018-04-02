namespace GenealogyApp.ViewModels
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Ioc;
    using GenealogyLogic.Interfaces;
    using System;
    using System.Windows;

    public class PersonDetailsViewModel : ViewModelBase
    {
        private bool isHidden;

        public MainViewModel MainViewModel { get { return SimpleIoc.Default.GetInstance<MainViewModel>(); } }

        public IUIPerson Person { get; set; }

        public bool IsHidden
        {
            get
            {
                return this.isHidden;
            }
            set
            {
                this.isHidden = value;

                var actionText = value ? "ausblenden" : "einblenden";

                var messageBoxResult = MessageBox.Show(string.Format("Auch Nachfahren {0}?", actionText), string.Format("Nachfahren {0}", actionText), MessageBoxButton.YesNo);

                if (value)
                {
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        this.Person.HidePersonAndDescendants();
                    }
                    else if (messageBoxResult == MessageBoxResult.No)
                    {
                        this.Person.HidePerson();
                    }
                }
                else
                {
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        this.Person.ShowPersonAndDescendants();
                    }
                    else if (messageBoxResult == MessageBoxResult.No)
                    {
                        this.Person.ShowPerson();
                    }
                }
            }
        }
    }
}
