using UnityEngine;
using System.Collections;

public class PokerCard : Card
{
    public int Value { get; set; }
    public string Suit { get; set; }
    public Material Face { get; set; }

    

    public PokerCard(int value = 0, string suit = "")
	{
        Value = value;
        Suit = suit;
        Face = null;
	}

    public override string ToString()
    {
        string ret = " of " + Suit;
        if (Value >= 2 && Value <= 10)
            ret = Value.ToString() + ret;
        else
            ret = (new[] { "Ace", "Jack", "Queen", "King" })["+567".IndexOf((char)(Value+42))] + ret;
        // ^^ This is my favourite line of code in the whole thing.
        // It checks for the index of +567 by the ascii values of 43, 53, 54, and 55.
        // Value is going to be either 1, 11, 12, or 13.
        // It's my favourite thing of this whole project.
        return ret;
    }
}
