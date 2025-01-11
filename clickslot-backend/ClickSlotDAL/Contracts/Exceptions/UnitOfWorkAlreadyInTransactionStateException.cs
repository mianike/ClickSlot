namespace ClickSlotDAL.Contracts.Exceptions
{
    [Serializable]
    internal class UnitOfWorkAlreadyInTransactionStateException : Exception
    {
        public UnitOfWorkAlreadyInTransactionStateException()
        {
        }

        public UnitOfWorkAlreadyInTransactionStateException(string? message) : base(message)
        {
        }

        public UnitOfWorkAlreadyInTransactionStateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}