namespace GenealogyLogic.Interfaces
{
    using GenealogyLogic.Models;

    public interface IPersonConvert
    {
        SerializablePerson GetSerializeable();
    }
}
