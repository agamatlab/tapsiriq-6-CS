class Client
{
    public Client() {}
    public Client(string name, string surname, CreditCard card)
    {
        Name = name;
        Surname = surname;
        Card = card;
    }

    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public CreditCard Card { get; set; } = null;
    public Message[] Log { get; set; } = null;

    public override string ToString() => $"{Name} / {Surname} / {Card.PAN} / {Card.PIN} / {Card.ExpireDate}";

    public void AddMessage(string message)
    {
        Message messageObject = new Message(message, DateTime.Now);
        if (Log == null) Log = new Message[] { messageObject };
        else Log = Log.Append<Message>(messageObject).ToArray();
    }
}
