class InsufficientAmountException : ApplicationException
{
    public InsufficientAmountException(string message = "Insufficient Amount of Balance.")
        : base(message) {}
}
