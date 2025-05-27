namespace Domino.Interfaces;

public interface IPlayer
{
    public string Name { get; set; }
    public int Score { get; set; }
    public int HandValue { get; set; }

    public int GetHandValue();
}