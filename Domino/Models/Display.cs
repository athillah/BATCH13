using System.Text;
using Domino.Enumerations;

namespace Domino.Interfaces;

public class Display : IDisplay
{
    public string? Id { get; set; }
    private StringBuilder _message { get; set; }
    private readonly ConsoleColor _originalForeground;
    private readonly ConsoleColor _originalBackground;
    public Display()
    {
        ClearConsole();
        Id = "Basic Console Display";
        _message = new StringBuilder();
        _originalBackground = Console.BackgroundColor;
        _originalForeground = Console.ForegroundColor;
    }
    public ICard PromptCard(List<ICard> playableCards)
    {
        while (true)
        {
            Console.Write("Play a card (enter ID, e.g., 50 for [5|0]): ");
            string? input = ReadString();

            if (int.TryParse(input, out int cardId) && playableCards.Any(c => c.Id == cardId))
            {
                return playableCards.First(c => c.Id == cardId);
            }

            Console.WriteLine("Invalid input. Try again.");
        }
    }

    public Side PromptSide()
    {
        while (true)
        {
            Console.Write("Choose side to place (LEFT/RIGHT): ");
            string? input = ReadString().ToUpper();

            if (Enum.TryParse(typeof(Side), input, out var side)) return (Side)side;

            Console.WriteLine("Invalid input. Please enter LEFT or RIGHT.");
        }
    }
    public string ReadString()
        => Console.ReadLine().ToString().Trim() ?? string.Empty;
    public void DirectMessage(string message)
        => Console.Write(message);
    public void ClearConsole()
        => Console.Clear();
    public void AddMessage(string message)
        => _message.Append(message);
    public void ClearMessage()
        => _message.Clear();
    public void ShowMessage()
    {
        if (_message.Length != 0)
        {
            Console.WriteLine(GetMessage());
            Console.WriteLine($"{Line(_message.Length / 5)}");
        }
    }
    public string GetMessage()
        => _message.ToString();
    private void setDominoColor()
    {
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.ForegroundColor = ConsoleColor.Red;
    }
    private void setReversedDominoColor()
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.ForegroundColor = ConsoleColor.Yellow;
    }
    private void resetConsoleColor()
    {
        Console.BackgroundColor = _originalBackground;
        Console.ForegroundColor = _originalForeground;
    }
    private string Line(int length)
    {
        StringBuilder line = new StringBuilder();
        line.Append("+");
        line.Append('-', (5 * length) + (length - 1));
        line.Append("+");
        return line.ToString();
    }
    private void Space(int length)
    {
        for (int i = 0; i < length; i++) Console.Write(" ");
    }
    public void LoadDot(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Console.Write(".");
            Thread.Sleep(66);
        }
        Console.WriteLine();
    }

    public void ShowBoard(IBoard board)
    {
        Console.WriteLine($"= Board");
        Console.WriteLine($"{Line(board.PlayedCards.Count)}");
        Space(1);
        foreach (ICard card in board.PlayedCards)
        {
            setDominoColor();
            Console.Write($" {card.LeftFaceValue}|{card.RightFaceValue} ");
            resetConsoleColor();
            if (card != board.PlayedCards.Last()) Space(1);
        }
        Console.WriteLine($"\n{Line(board.PlayedCards.Count)}");
    }
    public void ShowHand(string name, List<ICard> hand, List<ICard> playableCards)
    {
        Console.WriteLine($"= {name}'s hand");
        Console.WriteLine($"{Line(hand.Count)}");
        Space(1);
        foreach (ICard card in hand)
        {
            if (playableCards.Contains(card))
                setDominoColor();
            else
                setReversedDominoColor();
            Console.Write($" {card.LeftFaceValue}|{card.RightFaceValue} ");
            resetConsoleColor();
            if (card != hand.Last()) Space(1);
        }
        Console.WriteLine($"\n{Line(hand.Count)}");
    }
    public void Wait()
    {
        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }
    public void ShowScore(Dictionary<IPlayer, int> scores)
    {
        int pos = 1;
        scores = scores.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        Console.WriteLine("Score Summary");
        Console.WriteLine("   Player\tScore");
        Console.WriteLine($"{Line(3*5)}");
        foreach (var player in scores)
        {
            Console.Write($"{pos++}. {player.Key.Name}\t{player.Value}   ");
            if (pos == 2)
            {
                setDominoColor();
                Console.Write("  Winner!");
            }
            resetConsoleColor();
            Space(1);
        }
    }
    public void ShowGameInfo(int round, string playerName)
        => AddMessage($"= Round {round} - {playerName}'s turn");
    public int PromptMenu()
    {
        while (true)
        {
            string? input = ReadString();
            if (int.TryParse(input, out int result) && result > 0)
            {
                return result;
            }
            Console.WriteLine("Please enter a valid number.");
        }
    }
    public void MainMenu()
    {
        ClearConsole();
        Console.Write("===== ");
        setDominoColor();
        Console.Write("[ DOM|INO ]");
        resetConsoleColor();
        Console.WriteLine(" =====\n");
        Console.WriteLine(" 1. Start Game\n 2. Settings\n 3. Exit");
    }
    public void ConfigMenu()
    {
        ClearConsole();
        Console.WriteLine("===== Settings =====\n");
        Console.Write(" 1. Set Max Players\n 2. Set Max Hand Size\n 3. Back\n\nInsert an option: ");
    }
}