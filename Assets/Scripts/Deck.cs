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
        List<LinkedList<Card>> piles;
        piles = new List<LinkedList<Card>>();
        for (int n = 2 + (rand.Next(5)); n > 0; n--)
            piles.Add(new LinkedList<Card>());
        while (Cards.Count > 0)
        {
            int pile = rand.Next(piles.Count);
            bool top = (1 == rand.Next(0,2));
            Card c;
            if (top) c = drawFromTop();
            else c = drawFromBottom();
            if (pile % 2 == 0) piles[pile].AddLast(c);
            else piles[pile].AddFirst(c);
        }
        while (piles.Count > 0)
        {
            int pile = rand.Next(piles.Count);
            while (piles[pile].Count > 0)
            {
                Cards.AddFirst(piles[pile].First.Value);
                piles[pile].RemoveFirst();
            }
            piles.RemoveAt(pile);
        }
        
    }
}
