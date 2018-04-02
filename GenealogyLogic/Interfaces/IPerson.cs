namespace GenealogyLogic.Interfaces
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public interface IPerson : IPersonDetails, IPersonDetailsExtended
    {
        List<IPerson> Partners { get; }

        List<IPerson> Parents { get; }

        List<IPerson> Children { get; }
    }
}
