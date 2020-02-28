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
        return ret;
    }
}
