namespace Domino.Interfaces;

public interface ICard
{
    public int Id { get; set; }
    public int RightFaceValue { get; set; }
    public int LeftFaceValue { get; set; }
    public bool IsDouble();
    public int GetValue();
}