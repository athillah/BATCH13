using Domino.Interfaces;
namespace Domino.Models;

public class Card : ICard
{
    public int Id { get; set; }
    public int RightFaceValue { get; set; }
    public int LeftFaceValue { get; set; }
    public Card(int id, int leftFaceValue, int rightFaceValue)
    {
        Id = id;
        RightFaceValue = rightFaceValue;
        LeftFaceValue = leftFaceValue;
    }
    public bool IsDouble() => RightFaceValue == LeftFaceValue;
    public IEnumerable<int> GetValue() => new[] { LeftFaceValue, RightFaceValue };
}