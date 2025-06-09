using Domino.Enumerations;

namespace Domino.Interfaces;

public interface IDisplay
{
    string? Id { get; set; }
    ICard PromptCard(List<ICard> playableCards);
    Side PromptSide();
    void Clear();
    void DirectMessage(string message);
    void AddMessage(string message);
    void ClearMessage();
    string GetMessage();
    void ShowGameInfo(int round, string playerName);
    void ShowMessage();
    void ShowBoard(IBoard board);
    void ShowHand(string name, List<ICard> hand, List<ICard> playableCards);
    void Wait();
    void LoadDot(int count);
    void ShowScore(Dictionary<IPlayer, int> scores);
    void MainMenu();
    int PromptMenu();
    void ConfigMenu();
    void ShowHint(List<ICard> cards, List<ICard> playableCards);
    void PrintHeader(string title);
}