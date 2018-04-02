namespace GenealogyLogic.Exceptions
{
    using System;

    public class OutOfMiniGuidsException : Exception
    {
        public OutOfMiniGuidsException(string message) : base(message)
        {
        }
    }
}
