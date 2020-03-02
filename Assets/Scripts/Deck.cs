using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Deck<Card>
{
    private LinkedList<Card> Cards { get; set; }
	public Deck()
	{
        Cards = new LinkedList<Card>();
	}

    public int size { get { return Cards.Count; }  }

    public void Add(Card card)
    {
        Cards.AddLast(card);
    }

    public Card drawFromTop()
    {
        Card top = Cards.Last();
        Cards.RemoveLast();
        return top;
    }

    public Card drawFromBottom()
    {
        Card bottom = Cards.First();
        Cards.RemoveFirst();
        return bottom;
    }

    public void AddToTop(Card card)
    {
        Cards.AddLast(card);
    }

    public void AddToBottom(Card card)
    {
        Cards.AddFirst(card);
    }

    public void Merge(Deck<Card> deck)
    {
        while (deck.size > 0)
            AddToBottom(deck.drawFromTop());
    }

    public void Shuffle()
    {
        Random rand = new Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        // Changed my shuffle method after demo because wanted something cleaner looking
        List<Card> pile = new List<Card>();
        while (Cards.Count > 0)
            pile.Add(drawFromTop());
        for (int i = 0; i < pile.Count; i++)
        {
            Card c = pile[i];
            int r = rand.Next(pile.Count);
            pile[i] = pile[r];
            pile[r] = c;
        }
    }
}
