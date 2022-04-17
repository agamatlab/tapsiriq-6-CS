

using Bogus;
partial class Program
{
    public static void MySetColor(ConsoleColor foreground, ConsoleColor background)
    {
        Console.ForegroundColor = foreground;
        Console.BackgroundColor = background;
    }

    public static bool manageChoice(ref ushort choice, ushort answerCount)
    {
        // Uİ'da dəyərlərin keyboardla idarəsini təmin edir.

        ConsoleKey kb = Console.ReadKey().Key;

        switch (kb)
        {
            case ConsoleKey.W:
            case ConsoleKey.A:
            case ConsoleKey.UpArrow:
            case ConsoleKey.LeftArrow:
                if (choice == 0) choice = answerCount;
                choice--;
                return true;
            case ConsoleKey.S:
            case ConsoleKey.D:
            case ConsoleKey.DownArrow:
            case ConsoleKey.RightArrow:
                choice++;
                choice %= answerCount;
                return true;
            case ConsoleKey.Enter:
                return false;
                break;
            default:
                return true;
        }
    }

    public static int GetChoice(string question, string[] answers)
    {
        // KEYBOARD - CONTROLLED Menu ilə MOD Seçimi

        ushort answerCount = Convert.ToUInt16(answers.Length);
        bool notFound = true;
        ushort choice = 0;

        while (notFound)
        {
            Console.Clear();
            Console.WriteLine(question);
            for (ushort i = 0; i < answerCount; i++)
            {
                char prefix = ' ';
                if (i == choice) { MySetColor(ConsoleColor.DarkGreen, ConsoleColor.Gray); prefix = '◙'; }
                Console.WriteLine($" {prefix} << {answers[i]} >>");
                Console.ResetColor();
            }
            notFound = manageChoice(ref choice, answerCount);
        }

        return choice;
    }

    public static CreditCard FakeCardGenerator()
    {
        CreditCard card = new Faker<CreditCard>()
            .RuleFor(card => card.PAN, bogus => bogus.Random.String(16, '0', '9'))
            .RuleFor(card => card.PIN, bogus => bogus.Random.String(4, '0', '9'))
            .RuleFor(card => card.CVC, bogus => bogus.Random.Short(100, 999))
            .RuleFor(card => card.CVC, bogus => bogus.Random.Short(100, 999))
            .RuleFor(card => card.ExpireDate, bogus => $"{bogus.Random.Int(1, 12)}/{DateTime.Now.Year + bogus.Random.Int(1, 10)}")
            .RuleFor(card => card.Balance, bogus => bogus.Random.Decimal(0,1500));

        return card;
    }

    public static Client FakeClientGenerator()
    {
        Client client = new Faker<Client>()
            .RuleFor(client => client.Name, bogus => bogus.Name.FirstName())
            .RuleFor(client => client.Surname, bogus => bogus.Name.LastName());
        client.Card = FakeCardGenerator();
        return client;
    }

    public static void Start(ref Client[] clients)
    {
        Console.Clear();
        Client client = null;

        while (true)
        {
            Console.Clear();

            foreach (var c in clients)
                Console.WriteLine(c);

            Console.Write("Enter PIN Code: ");
            string inPIN = Console.ReadLine() ?? String.Empty;
            if (inPIN.Length != 4) continue;

            for (int i = 0; i < clients.Length; i++)
                if (clients[i].Card.PIN == inPIN) 
                { 
                    client = clients[i]; 
                    break; 
                }
            if (client != null) break;
        }

        Console.Clear();
        client.AddMessage("User Entered System.");
        sbyte choice = Convert.ToSByte(GetChoice("Welcome! Want you want to do?"
            , new string[] { "Balance", "Get Cash", "Show LOG", "Card To Card"}) + 1);
        Console.WriteLine('\n');

        try
        {
            if (choice == 1)
            {

                if (client.Card == null)
                {
                    client.AddMessage("Tried To Access Balance.");
                    throw new ArgumentNullException("Can not Access Client's Card.");
                }
                else
                {
                    Console.WriteLine($"Balance: {client.Card.Balance}");
                    client.AddMessage("Balance Shown.");
                }

            }
            else if (choice == 2)
            {
                sbyte moneyChoice = Convert.ToSByte(GetChoice("Enter Amount of Money:", 
                    new string[] { "10₼","20₼","50₼","100₼","Other"}) + 1);

                int moneyAmount = default;
                switch (moneyChoice)
                {
                    case 1:
                        moneyAmount = 10;
                        break;
                    case 2:
                        moneyAmount = 20;
                        break;
                    case 3:
                        moneyAmount = 50;
                        break;
                    case 4:
                        moneyAmount = 100;
                        break;
                    case 5:
                        do
                        {
                            Console.Write("Enter Money Amount: ");
                            int.TryParse(Console.ReadLine(), out moneyAmount);
                            Console.Clear();
                        } while (moneyAmount <= 0);
                        break;
                }

                if (client.Card.Balance < moneyAmount)
                {
                    client.AddMessage("Tried to Withdraw More Money Than The Balance.");
                    throw new InsufficientAmountException();
                }
                else
                {
                    Console.WriteLine("Successfully Withdrawn.");
                    client.AddMessage($"Withdrew {moneyAmount}₼ Money.");
                    client.Card.Balance -= moneyAmount;
                }
            }
            else if (choice == 3)
                if (client.Card == null)
                    throw new ArgumentNullException("Can not Access Client's Card.");
                else
                    {
                        foreach (var log in client.Log)
                            Console.WriteLine(log);
                        Console.ReadKey();
                    }
            else if(choice == 4)
            {
                if (client.Card == null) throw new ArgumentNullException("Can not Access Client's Card.");

                CreditCard targetCard = null;

                while (true)
                {
                    string inPIN = string.Empty;
                    Console.Write("Enter PIN: ");
                    inPIN = Console.ReadLine();
                    foreach (var user in clients)
                        if (user.Card.PIN == inPIN && 
                            !CreditCard.ReferenceEquals(user.Card, client.Card))
                            {
                                targetCard = user.Card;
                                break;
                            }
                    if (targetCard != null) break;
                    Console.Clear();
                }

                decimal moneyAmount = default;
                do
                {
                    Console.Write("Enter Money Amount: ");
                    moneyAmount = Convert.ToDecimal(Console.ReadLine());
                    Console.Clear();
                } while (moneyAmount <= 0);

                if (client.Card.Balance < moneyAmount) throw new InsufficientAmountException();

                client.Card.Balance -= moneyAmount;
                targetCard.Balance += moneyAmount;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            client.AddMessage($"Encountered Exception: {ex.Message}");
        }
        finally
        {
            Console.Write("Press any key to continue");
            Console.ReadKey();
        }
    }

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.InputEncoding = System.Text.Encoding.Unicode;
        
        Client[] clients =
        {
            FakeClientGenerator(),
            FakeClientGenerator(),
            FakeClientGenerator(),
            FakeClientGenerator(),
            FakeClientGenerator()
        };

        while (true)
            Start(ref clients);
    }
}