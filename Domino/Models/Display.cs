using System.Text;
using Domino.Enums;
using Domino.Interfaces;
namespace Domino.Models;

public class Display : IDisplay
{
    public string? Id { get; set; }
                    = "Basic Console Display";
    private StringBuilder _message = new();
    private ConsoleColor _originalForeground
          = Console.ForegroundColor;
    private ConsoleColor _originalBackground
          = Console.BackgroundColor;

    private string[][] _pattern = new string[][]
    {
    new string[] { "         ", "         ", "         " },  // 0
    new string[] { "         ", "    ●    ", "         " },  // 1
    new string[] { " ●       ", "         ", "       ● " },  // 2
    new string[] { " ●       ", "    ●    ", "       ● " },  // 3
    new string[] { " ●     ● ", "         ", " ●     ● " },  // 4
    new string[] { " ●     ● ", "    ●    ", " ●     ● " },  // 5
    new string[] { " ●  ●  ● ", "         ", " ●  ●  ● " }   // 6
    };

    private string[] art = new string[]
    {
        " _____     ______     __    __     __     __   __     ______    ",
        "/\\  __-.  /\\  __ \\   /\\ \"-./  \\   /\\ \\   /\\ \"-.\\ \\   /\\  __ \\   ",
        "\\ \\ \\/\\ \\ \\ \\ \\/\\ \\  \\ \\ \\-./\\ \\  \\ \\ \\  \\ \\ \\-.  \\  \\ \\ \\/\\ \\  ",
        " \\ \\____-  \\ \\_____\\  \\ \\_\\ \\ \\_\\  \\ \\_\\  \\ \\_\\\"\\_\\  \\ \\_____\\ ",
        "  \\/____/   \\/_____/   \\/_/  \\/_/   \\/_/   \\/_/ \\/_/   \\/_____/ "
    };


    public Display()
    {
        Clear();
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }

    public ICard PromptCardId(List<ICard> playableCards)
    {
        while (true)
        {
            Console.WriteLine(
                "Choose a card to play");
            Console.Write("Enter ID, e.g., 50 for [5|0]): ");

            if (int.TryParse((string?)ReadString(), out int cardId)
            && playableCards.Any(
                c => c.Id == cardId))
                return playableCards.First(
                    c => c.Id == cardId);

            PrintError("Invalid input. Try again.");
        }
    }

    public ICard PromptCard(List<ICard> playableCards)
    {
        while (true)
        {
            Console.WriteLine(
                "Choose a card to play:");
            Console.Write($"Enter index (1 to {playableCards.Count}): ");

            if (int.TryParse(ReadString(), out int idx)
                && idx >= 0
                && idx < playableCards.Count + 1)
                return playableCards[idx - 1];

            PrintError("Invalid selection. Please choose a valid index.");
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

            PrintError("Invalid input. Please enter LEFT or RIGHT.");
        }
    }

    public void Clear()
    {
        Console.Clear();
    }

    public string ReadString()
    {
        return Console.ReadLine()?
                      .ToString()
                      .Trim()
            ?? string.Empty;
    }

    public void DirectMessage(string message)
    {
        Console.Write(message);
    }

    public void AddMessage(string message)
    {
        _message.AppendLine(message);
    }

    public void ClearMessage()
    {
        _message.Clear();
    }

    public string GetMessage()
    {
        return _message.ToString();
    }

    public void ShowMessage()
    {
        if (_message.Length != 0)
        {
            PrintGlow(GetMessage(),
                ConsoleColor.Cyan);
            Console.WriteLine(
                $"{DrawLine(7)}\n");
        }
        ClearMessage();
    }

    public void ShowHint(List<ICard> cards, List<ICard> playableCards)
    {
        int idx = 1;
        Console.Write("█");
        foreach (var card in cards)
        {
            if (playableCards.Contains(card))
            {
                SetColor(
                    ConsoleColor.Cyan,
                    ConsoleColor.White);
                Console.Write(
                    $"    {idx++}    ");
                ResetColor();
            }
            else Console.Write("         ");

            if (card != cards.Last())
                Console.Write(" ");
            else
            {
                Console.Write("█");
                PrintGlow(" play",
                    ConsoleColor.Yellow);
                PrintGlow("able\n\n",
                    ConsoleColor.Cyan);
            }
        }
    }

    private void SetColor(ConsoleColor bg, ConsoleColor fg)
    {
        Console.BackgroundColor = bg;
        Console.ForegroundColor = fg;
    }

    private void ResetColor()
    {
        Console.BackgroundColor = _originalBackground;
        Console.ForegroundColor = _originalForeground;
    }

    private string DrawLine(int length)
    {
        return "█"
             + new string('━', (length * 9) + (length - 1))
             + "█";
    }

    private void DrawTitle(string title, int contentLength)
    {
        Console.ForegroundColor =
            ConsoleColor.Cyan;

        int totalWidth = (contentLength * 9) +
                         (contentLength - 1) + 2;
        int titleLength = title.Length;
        int padding = Math.Max(0, (totalWidth - titleLength) / 2);

        Console.WriteLine(new string(' ', padding) + title);
        ResetColor();

    }

    private void PrintCard(int left, int right, bool playable)
    {
        ChangeColor(playable);
        Console.Write(
            $" {left}|{right} ");
        ResetColor();
    }

    private string GetUTF(int left, int right, bool isHorizontal = true)
    {
        int baseCodePoint = isHorizontal ? 0x1F031 : 0x1F063;
        int codePoint = baseCodePoint + (7 * left) + right;

        return char.ConvertFromUtf32(codePoint);
    }

    private void PrintTileHorizontal(List<ICard> cards)
    {
        Console.WriteLine(
            "▞" + new string('▀', cards.Count * 19 + cards.Count - 1) + "▚");

        for (int line = 0; line < 3; line++)
        {
            Console.Write("▌");
            foreach (var card in cards)
            {
                SetColor(
                    ConsoleColor.Yellow,
                    ConsoleColor.Red);
                Console.Write(
                    $"{_pattern[card.LeftFaceValue][line]}┃{_pattern[card.RightFaceValue][line]}");
                ResetColor();

                if (card != cards.Last())
                    Console.Write(" ");
                else Console.WriteLine("▐");
            }
        }

        Console.WriteLine(
            "▚" + new string('▄', cards.Count * 19 + cards.Count - 1) + "▞");
    }

    private void ChangeColor(bool playable)
    {
        if (playable)
            SetColor(ConsoleColor.Yellow,
                     ConsoleColor.Red);
        else SetColor(ConsoleColor.Red,
                      ConsoleColor.Yellow);
    }

    private void PrintTileVertical(List<ICard> cards, List<ICard> playableCards)
    {
        if (cards.Count != 0)
            for (int line = 0; line < 7; line++)
            {
                Console.Write("▌");

                foreach (var card in cards)
                {
                    ChangeColor(playableCards.Contains(card));

                    if (line < 3)
                        Console.Write(
                            $"{_pattern[card.LeftFaceValue][line]}");

                    if (line == 3)
                        Console.Write(
                            $"━━━━━━━━━");

                    if (card.RightFaceValue >= 0 &&
                        card.RightFaceValue < _pattern.Length &&
                        (line - 4) >= 0 &&
                        (line - 4) < _pattern[card.RightFaceValue].Length)
                        Console.Write(
                            $"{_pattern[card.RightFaceValue][line - 4]}");

                    ResetColor();

                    if (card == cards.Last())
                        Console.WriteLine("▐");
                    else Console.Write(" ");
                }
            }
    }

    public void PrintHeader(string title)
    {
        PrintGlow(
            $"=== {title} ===\n", ConsoleColor.Cyan);
    }

    private void PrintError(string message)
    {
        Console.ForegroundColor =
            ConsoleColor.DarkRed;
        Console.WriteLine($"!!{message}");

        ResetColor();
    }

    private void PrintGlow(string message, ConsoleColor glow)
    {
        Console.ForegroundColor = glow;
        Console.Write(message);

        ResetColor();
    }

    public void LoadDot(int count)
    {
        for (int i = 0; i < count; i++)
            Console.Write("."); Thread.Sleep(66);

        Console.WriteLine();
    }

    public void ShowBoard(IBoard board)
    {
        DrawTitle(
            $"Board", board.PlayedCards.Count * 2);

        PrintTileHorizontal(board.PlayedCards);
        Console.WriteLine();
    }

    public void ShowHand(string name, List<ICard> hand, List<ICard> playableCards)
    {
        Console.WriteLine();
        DrawTitle(
            $"{name}'s hand", hand.Count);

        Console.WriteLine(
            DrawLine(Math.Max(1, hand.Count)));
        PrintTileVertical(
            hand, playableCards);
        Console.WriteLine(
            DrawLine(Math.Max(1, hand.Count)));
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
        var ordered = scores.OrderBy(
            pair => pair.Value);

        Console.WriteLine(DrawLine(7));
        Console.WriteLine(
            "   Player\tScore");
        Console.WriteLine(DrawLine(7));

        foreach (var pair in ordered)
        {
            Console.Write(
                $"{pos++}. {pair.Key.Name}\t\t{pair.Value}   ");

            if (pos == 2)
            {
                SetColor(ConsoleColor.Yellow, ConsoleColor.Red);
                Console.Write("  Winner!");
                ResetColor();
            }

            Console.WriteLine();
        }
    }

    public void ShowGameInfo(int round, string playerName)
    {
        AddMessage(
            $"█ Round {round} - {playerName}'s turn.");
    }

    public int PromptMenu()
    {
        while (true)
        {
            if (int.TryParse((string?)ReadString(), out int result) && result > 0)
                return result;

            PrintError(
                "Please enter a valid number.");
        }
    }

    public void MainMenu()
    {
        Clear();
        Console.Write("===== ");
        SetColor(
            ConsoleColor.Yellow,
            ConsoleColor.Red);
        Console.Write(
            "[ DOM|INO ]");

        ResetColor();
        Console.WriteLine(" =====\n");
        Console.WriteLine(
            " 1. Start Game\n 2. Settings\n 3. Exit");
    }

    public void ConfigMenu()
    {
        Clear();
        PrintHeader(
            "Settings");

        Console.Write(
            "\n 1. Set Max Players\n 2. Set Max Hand Size\n 3. Back\n\nInsert an option: ");
    }

}