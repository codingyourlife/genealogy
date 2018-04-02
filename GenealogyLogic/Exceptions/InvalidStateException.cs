namespace GenealogyLogic.Exceptions
{
    using System;

    public class InvalidStateException : Exception
    {
        public InvalidStateException(string message, Exception ex = null) : base(message, ex)
        {
        }
    }
}
