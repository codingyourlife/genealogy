namespace GenealogyLogic
{
    using ApiModule;
    using GenealogyLogic.Interfaces;

    public static class PersonExtension
    {
        public static void AutoDefineGender(this IUIPerson person)
        {
            person.IsFemale = GenderizeApi.FirstNameIsFemale(person.FirstName);
        }
    }
}
