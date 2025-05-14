using System;
using static System.Console;

public interface ICard {
    public string Rank { get; }
    public string Suit { get; }
    public string ToString();
}

public interface IHand {
    List<ICard> Cards { get; }
    string RandomizeRank();
    string RandomizeSuit();
    void ShowHand();
}


public class Card : ICard {
    public string Rank { get; private set; }
    public string Suit { get; private set; }
    public Card(string rank, string suit) {
        Rank = rank;
        Suit = suit;
    }
    public override string ToString() => $"{Rank} of {Suit}";
}

public class Hand : IHand {
    public List<ICard> Cards { get; private set; }
    List<ICard> IHand.Cards => Cards;

    public Hand() {
        Cards = DrawCards();
    }
    public string RandomizeRank () {
        Random rng = new Random();
        int randomRank = rng.Next(14);
        if (randomRank == 0) {
            return "A";
        } if (randomRank == 11) {
            return "J";
        } if (randomRank == 12) {
            return "Q";
        } if (randomRank == 13) {
            return "K";
        }
        return randomRank.ToString();
    }
    public string RandomizeSuit () {
        Random rng = new Random();
        int randomSuit = rng.Next(4);
        switch (randomSuit) {
            case 0:
                return "Spade";
            case 1:
                return "Love";
            case 2:
                return "Club";
            case 3:
                return "Diamond";
        } return "0";
    }
    public List<ICard> DrawCards() {
        List<ICard> cards = new List<ICard>();
        for (int i = 0; i < 5; i++) {
            cards.Add(new Card(RandomizeRank(), RandomizeSuit()));
        }
        return cards;
    }
    public void ShowHand() {
        foreach (var card in Cards) {
            WriteLine(card);
        }
    }
}

class Program {
    static void Main() {
        Hand MyHand = new Hand();
        MyHand.ShowHand();
    }
}