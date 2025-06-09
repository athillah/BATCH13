namespace Domino.Models;

using Domino.Interfaces;

public class Player : IPlayer
{
    public string Name { get; set; }
    public int Score { get; set; }
    public Player(string name) => Name = name;
}