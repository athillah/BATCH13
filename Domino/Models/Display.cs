using System.Text;
using Domino.Enumerations;
using Domino.Interfaces;
namespace Domino.Models;

public class Display : IDisplay
{
    public string? Id { get; set; } =
                    "Basic Console Display";
    private StringBuilder _message = new();
    private readonly ConsoleColor _originalForeground
                   = Console.ForegroundColor;
    private readonly ConsoleColor _originalBackground
                   = Console.BackgroundColor;

    private readonly string[][] _pattern = new string[][]{
        new string[] { "   ", "   ", "   " },
        new string[] { "   ", " ● ", "   " },
        new string[] { "●  ", "   ", "  ●" },
        new string[] { "●  ", " ● ", "  ●" },
        new string[] { "● ●", "   ", "● ●" },
        new string[] { "● ●", " ● ", "● ●" },
        new string[] { "● ●", "● ●", "● ●" },
    };

    public Display()
    {
        ClearConsole();
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }

    public ICard PromptCard(List<ICard> playableCards)
    {
        while (true)
        {
            Console.WriteLine(
                "Choose a card to play");
            Console.Write("Enter ID, e.g., 50 for [5|0]): ");

            string? input = ReadString();
            if (int.TryParse(input, out int cardId)
            && playableCards.Any(c => c.Id == cardId))
                return playableCards.First(c => c.Id == cardId);

            printError("Invalid input. Try again.");
        }
    }

    public Side PromptSide()
    {
        while (true)
        {
            Console.WriteLine(
                "Choose side");
            Console.Write("Left or Right: ");

            string? input = ReadString().ToUpper();
            if (Enum.TryParse(typeof(Side), input, out var side))
                return (Side)side;

            printError("Invalid input. Please enter LEFT or RIGHT.");
        }
    }

    public void ClearConsole()
        => Console.Clear();

    public string ReadString()
        => Console.ReadLine()?
        .ToString().Trim()
        ?? string.Empty;

    public void DirectMessage(string message) => Console.Write(message);
    public void AddMessage(string message) => _message.AppendLine(message);
    public void ClearMessage() => _message.Clear();
    public string GetMessage() => _message.ToString();
    public void ShowMessage()
    {
        if (_message.Length != 0)
        {
            printGlow(
                GetMessage(),
                ConsoleColor.Cyan);
            Console.WriteLine(
                $"{drawLine(7)}\n");
        }
        ClearMessage();
    }

    public void ShowHint()
    {
        Console.Write(" ");
        printGlow("playable\n",
                    ConsoleColor.Yellow);
        Console.Write(" ");
        printGlow("unplayable",
                    ConsoleColor.Red);
        Console.WriteLine("\n");
    }

    private void setColor(ConsoleColor bg, ConsoleColor fg)
    {
        Console.BackgroundColor = bg;
        Console.ForegroundColor = fg;
    }

    private void resetColor()
    {
        Console.BackgroundColor = _originalBackground;
        Console.ForegroundColor = _originalForeground;
    }

    private string drawLine(int length)
        => "+"
        + new string('-'
        , (length * 5)
        + (length - 1))
        + "+";

    private void drawTitle(string title, int contentLength)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        int totalWidth = (contentLength * 5) +
                         (contentLength - 1) + 2;
        int titleLength = title.Length;
        int padding = Math.Max(0, (totalWidth - titleLength) / 2);

        Console.WriteLine(new string(' ', padding) + title);
        resetColor();

    }

    private void printCard(int left, int right, bool playable)
    {
        if (playable) setColor(
            ConsoleColor.Yellow,
            ConsoleColor.Red);
        else setColor(
            ConsoleColor.Red,
            ConsoleColor.Yellow);

        Console.Write($" {left}|{right} ");
        resetColor();
    }

    // private string _getDominoTile(int left, int right, bool isHorizontal = true)
    // {
    //     int baseCodePoint = isHorizontal ? 0x1F031 : 0x1F063;
    //     int codePoint = baseCodePoint + (7 * left) + right;
    //     return char.ConvertFromUtf32(codePoint);
    // }

    // private string encoder

    public void PrintHeader(string title)
        => printGlow(
                $"=== {title} ===\n",
                ConsoleColor.Cyan);

    private void printError(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"!!{message}");
        resetColor();
    }

    private void printGlow(string message, ConsoleColor glow)
    {
        Console.ForegroundColor = glow;
        Console.Write(message);
        resetColor();
    }

    public void LoadDot(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Console.Write("."); Thread.Sleep(66);
        }
        Console.WriteLine();
    }

    public void ShowBoard(IBoard board)
    {
        drawTitle(
            $"Board", board.PlayedCards.Count);

        Console.Write($"{drawLine(board.PlayedCards.Count)}\n ");
        foreach (var card in board.PlayedCards)
        {
            printCard(card.LeftFaceValue, card.RightFaceValue, true);
            if (card != board.PlayedCards.Last()) Console.Write(" ");
        }
        Console.WriteLine($"\n{drawLine(board.PlayedCards.Count)}");
    }

    public void ShowHand(string name, List<ICard> hand, List<ICard> playableCards)
    {
        drawTitle(
            $"{name}'s hand", hand.Count);

        Console.WriteLine(drawLine(Math.Max(1, hand.Count)));
        if (hand.Count == 0)
        {
            Console.WriteLine(drawLine(1)); return;
        }
        Console.Write(" ");
        foreach (var card in hand)
        {
            printCard(card.LeftFaceValue, card.RightFaceValue,
                      playableCards.Contains(card));
            if (card != hand.Last()) Console.Write(" ");
        }
        Console.WriteLine($"\n{drawLine(hand.Count)}");
    }

    public void Wait()
    {
        Console.Write(
            "\nPress any key to continue...");
        Console.ReadKey();
    }

    public void ShowScore(Dictionary<IPlayer, int> scores)
    {
        Console.WriteLine(
            "\nScore Summary");

        int pos = 1;
        var ordered = scores.OrderBy(pair => pair.Value);

        Console.WriteLine(drawLine(7));
        Console.WriteLine("   Player\tScore");
        Console.WriteLine(drawLine(7));

        foreach (var pair in ordered)
        {
            Console.Write($"{pos++}. {pair.Key.Name}\t\t{pair.Value}   ");
            if (pos == 2)
            {
                setColor(ConsoleColor.Yellow, ConsoleColor.Red);
                Console.Write("  Winner!");
                resetColor();
            }
            Console.WriteLine();
        }
    }

    public void ShowGameInfo(int round, string playerName)
        => AddMessage(
                $"= Round {round} - {playerName}'s turn.");

    public int PromptMenu()
    {
        while (true)
        {
            string? input = ReadString();
            if (int.TryParse(input, out int result) && result > 0)
                return result;

            printError(
                "Please enter a valid number.");
        }
    }

    public void MainMenu()
    {
        ClearConsole();
        Console.Write("===== ");
        setColor(ConsoleColor.Yellow, ConsoleColor.Red);
        Console.Write(
            "[ DOM|INO ]");
        resetColor();
        Console.WriteLine(" =====\n");
        Console.WriteLine(
            " 1. Start Game\n 2. Settings\n 3. Exit");
    }

    public void ConfigMenu()
    {
        ClearConsole();
        PrintHeader(
            "Settings");
        Console.Write(
            "\n 1. Set Max Players\n 2. Set Max Hand Size\n 3. Back\n\nInsert an option: ");
    }
}