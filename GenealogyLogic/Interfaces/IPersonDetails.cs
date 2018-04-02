namespace GenealogyLogic.Interfaces
{
    using System;

    public interface IPersonDetails
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        bool IsFemale { get; set; }
        string Biography { get; set; }
        DateTime? DateOfBirth { get; set; }
        DateTime? DateOfDeath { get; set; }
    }
}
