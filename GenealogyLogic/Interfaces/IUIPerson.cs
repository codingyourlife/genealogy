namespace GenealogyLogic.Interfaces
{
    using System.Collections.Generic;

    public interface IUIPerson : IPerson, IPersonUIFeatures, IPersonConvert
    {
        List<IUIPerson> Partners { get; }

        List<IUIPerson> Parents { get; }

        List<IUIPerson> Children { get; }

        List<IUIPerson> GetAllDescendants();

        void HidePerson();

        void HidePersonAndDescendants();

        void ShowPerson();

        void ShowPersonAndDescendants();

        List<IUIPerson> GetChildrenConceivedWithPerson(IUIPerson parentOfPersonToRemoveParent);
    }
}
