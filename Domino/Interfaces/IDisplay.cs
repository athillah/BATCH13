using Domino.Enumerations;

namespace Domino.Interfaces;

public interface IDisplay
{
    string? Id { get; set; }
    ICard PromptCard(List<ICard> playableCards);
    Side PromptSide();
    void ClearConsole();
    void DirectMessage(string message);
    void AddMessage(string message);
    void ClearMessage();
    string GetMessage();
    void ShowMessage();
    void ShowBoard(IBoard board);
    void ShowHand(string name, List<ICard> hand, List<ICard> playableCards);
    void Wait();
    void LoadDot(int count);
}