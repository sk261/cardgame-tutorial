using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerDeck : Deck<PokerCard>
{
    public Material cardBack;
    // Start is called before the first frame update
    public PokerDeck()
    {
        foreach (string suit in new[] { "Hearts", "Diamonds", "Spades", "Clubs"})
            for (int value = 1; value <= 13; value++)
                AddToTop(new PokerCard(value, suit));
    }

    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
