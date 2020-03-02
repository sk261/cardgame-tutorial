using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milestone3Handler : MonoBehaviour
{
    PokerDeck deck;
    // Start is called before the first frame update
    void Start()
    {
        deck = new PokerDeck();
    }

    public void drawTop()
    {
        Debug.Log(deck.drawFromTop());
    }

    public void drawBottom()
    {
        Debug.Log(deck.drawFromBottom());
    }

    public void shuffle()
    {
        printTopN(3);
        deck.Shuffle();
        Debug.Log("Deck Shuffled.");
        printTopN(3);
    }

    public void printTopN(int N)
    {
        string ret = ("Top card " + N.ToString() + " cards: ");
        List<PokerCard> cards = new List<PokerCard>();
        for (int i = 0; i < N; i++)
            cards.Add(deck.drawFromTop());
        ret += string.Join(", ", cards);
        for (int i = N-1; i >= 0; i--)
            deck.AddToTop(cards[i]);
        Debug.Log(ret);
    }

    public void addDeck()
    {
        Debug.Log("Current size: " + deck.size.ToString());
        deck.Merge(new PokerDeck());
        Debug.Log("Deck added, new size: " + deck.size.ToString());
    }

}
