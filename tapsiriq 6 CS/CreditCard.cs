class CreditCard
{
    public CreditCard() { }

    public CreditCard(string pan, string pin, short cvc, string expireDate, decimal balance)
    {
        PAN = pan;
        PIN = pin;
        CVC = cvc;
        ExpireDate = expireDate;
        Balance = balance;
    }

    public string PAN { get;  set; }
    public string PIN { get; set; }
    public short CVC { get;  set; }
    public string ExpireDate { get; set; }
    public decimal Balance { get; set; }
}
