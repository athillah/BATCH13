namespace Domino.Interfaces;

public interface ICard
{
    int Id { get; set; }
    int RightFaceValue { get; set; }
    int LeftFaceValue { get; set; }
    bool IsDouble();
    IEnumerable<int> GetValue();
}