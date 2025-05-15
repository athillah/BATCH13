using System;
using static System.Console;

public interface ICard {
    public string Rank { get; }
    public string Suit { get; }
    public string ToString();
}

public interface IHand {
    void ShowHand();
}

public interface IDrawer {
    List<ICard> Cards { get; }
}

public interface IRandomizer {
    string Randomize();
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
    List<ICard> Cards = new List<ICard>();

    public Hand() {
        List<ICard> Cards = CardDrawer().DrawCards();
    }
    public void ShowHand() {
        foreach (var card in Cards) {
            WriteLine(card);
        }
    }
}

public class CardDrawer : IDrawer {
    List<ICard> Cards { get; }
    public static List<ICard> DrawCards() {
        List<ICard> cards = new List<ICard>();
        for (int i = 0; i < 5; i++) {
            cards.Add(new Card(RankRandomizer.Randomize(), SuitRandomizer.Randomize()));
        }
        return cards;
    }
}

public class RankRandomizer : IRandomizer {
    public static string Randomize () {
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
}

public class SuitRandomizer : IRandomizer {
    public static string Randomize () {
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
}

class Program {
    static void Main() {
        Hand MyHand = new Hand();
        MyHand.ShowHand();
    }
}