namespace Domino.Interfaces;

public class Card : ICard
{
    public int Id { get; set; }
    public int RightFaceValue { get; set; }
    public int LeftFaceValue { get; set; }
    public Card(int id, int rightFaceValue, int leftFaceValue)
    {
        Id = id;
        RightFaceValue = rightFaceValue;
        LeftFaceValue = leftFaceValue;
    }
    public bool IsDouble()
    {
        return RightFaceValue == LeftFaceValue;
    }
    public int GetValue()
    {
        return 10;
    }
}