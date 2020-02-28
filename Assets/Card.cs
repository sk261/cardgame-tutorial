public class Card
{
    public string Name { get; set; }
    public bool isFaceUp { get; private set; }

    public Card(string name = "")
    {
        isFaceUp = true;
        Name = name;
    }

    public void Flip()
    {
        isFaceUp = !isFaceUp;
    }


}
