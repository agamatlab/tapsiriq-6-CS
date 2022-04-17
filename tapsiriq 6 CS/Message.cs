class Message
{
    public Message(string thrownMessage, DateTime time)
    {
        ThrownMessage = thrownMessage;
        Time = time;
    }

    public string ThrownMessage { get; set; }
    public DateTime Time { get; set; }

    public override string ToString() => $"{ThrownMessage} => {Time.ToString()}";
}
