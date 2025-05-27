namespace Domino.Interfaces;

public class Player : IPlayer
{
    public string Name { get; set; }
    public int Score { get; set; }
    public int HandValue { get; set; }
    public Player(string name, ICard card)
    {
        Name = name;
    }

    public int GetHandValue()
    {
        return HandValue;
    }
}