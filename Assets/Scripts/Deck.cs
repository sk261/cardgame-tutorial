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

    public void Add(Card card)
    {
        Cards.AddLast(card);
    }

    public Card drawFromTop()
    {
        return Remove();
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

    public void Merge(Deck<Card> deck)
    {
        LinkedList<Card> pull = deck.Cards;
        if (deck.Cards.Count > Cards.Count)
            pull = Cards;

    }

    public void AddToBottom(Card card)
    {
        Cards.AddFirst(card);
    }

    public Card Remove()
    {
        Card top = Cards.Last();
        Cards.RemoveLast();
        return top;
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
            if (pile % 2 == 0)
                piles[pile].AddLast(Remove());
            else
                piles[pile].AddFirst(Remove());
        }
        while (piles.Count > 0)
        {
            int pile = rand.Next(piles.Count);
            Cards.Union(piles[pile]);
            piles.RemoveAt(pile);
        }
        
    }
}
