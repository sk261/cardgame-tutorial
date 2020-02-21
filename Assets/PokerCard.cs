public class PokerCard : Card
{
    public int Value { get; set; }
    public string Suit { get; set; }

    public PokerCard()
	{
        Value = 0;
        Suit = "";
	}
}
