namespace GenealogyLogic.Components
{
    using GalaSoft.MvvmLight.Ioc;

    public class GenealogyPersistor
    {
        private string filename;
        private string backupFile;

        private PersistorCore persistorCore = new PersistorCore();

        public GenealogyPersistor(string filename)
        {
            this.filename = filename;
            this.backupFile = filename + ".bak";
        }

        public void Persist(GenealogyMaster genealogyMaster)
        {
            persistorCore.Persist(this.filename, this.backupFile, genealogyMaster.GetAll());
        }

        public GenealogyMaster Restore()
        {
            var genealogyMaster = SimpleIoc.Default.GetInstance<GenealogyMaster>();

            var persons = persistorCore.Restore(this.filename);

            if(persons == null)
            {
                return genealogyMaster;
            }

            genealogyMaster.Load(persons);

            return genealogyMaster;
        }
    }
}
