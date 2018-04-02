namespace GenealogyLogic.EventArguments
{
    using GenealogyLogic.Interfaces;
    using System;
    using System.Collections.Generic;

    public class GenealogyMasterEventArgs : EventArgs
    {
        public GenealogyMasterEventArgs(List<IUIPerson> persons)
        {
            this.Persons = persons;
        }

        public List<IUIPerson> Persons { get; }
    }
}
