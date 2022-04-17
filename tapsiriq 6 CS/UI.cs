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
}