using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackjack : MonoBehaviour
{

    public GameObject Chip1;
    public GameObject Chip5;

    public GravityWell Bid;
    public GravityWell Chips;
    public GravityWell Board;
    public GravityWell Dealer;

    public UnityEngine.UI.Text DealerText;
    public UnityEngine.UI.Text PlayerText;
    public UnityEngine.UI.Text BidText;
    public UnityEngine.UI.Text MoneyText;

    public UnityEngine.UI.Button ContinueButton;
    private UnityEngine.UI.Text ButtonText;

    public PlayerControls DeckController;


    // Blackjack:
    /*
     * 0 GAME 1: Create betting tokens
     * 1 Phase 1: Betting. Bid MUST BE greater than 0.
     * 2 Phase 2: Dealer deals self 1 FU, 1 FD to self
     * 3 Phase 3: Dealer deals 2 FU to you.
     * 4 Phase 4: Hit/Stand
     * 5 Game Check: If you bust, you lose. Your bet disappears.
     * 6 Phase 5: Dealer Hits until > 17.
     * 7 Game Check: If your score is higher than dealer or dealer busts, your bet doubles.
     * 8 Cards disappear. Deck shuffles.
     * 9 GAME 2: All money gone? YOU LOSE. Else, restart at Phase 1.
     * */

    private int phase = 0;
    private bool awaitingPhaseShift = false;

    private List<GameObject> Coins;
    private List<GameObject> Cards;
    private List<string> playerCards;
    private List<string> dealerCards;

    // Start is called before the first frame update
    void Start()
    {
        Coins = new List<GameObject>();
        Cards = new List<GameObject>();
        playerCards = new List<string>();
        dealerCards = new List<string>();
        ButtonText = ContinueButton.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>();
        // Spawn 3x5 and 5x1. 20 'points'
        for (int n = 0; n < 3; n++)
            createChip(Chip5);
        for (int n = 0; n < 5; n++)
            createChip(Chip1);


        progressPhase();
    }

    private Vector3 getLocationInside(BoxCollider parent)
    {
        Vector3 mins = parent.bounds.min;
        Vector3 maxes = parent.bounds.max;
        return new Vector3(Random.Range(mins.x, maxes.x),
                            Random.Range(mins.y, maxes.y),
                            Random.Range(mins.z, maxes.z));
    }

    private string drawCard(BoxCollider parent, bool FaceDown = false)
    {
        GameObject card = DeckController.DrawFromDeck(false, FaceDown);
        card.transform.position = getLocationInside(parent);
        Cards.Add(card);
        return card.name;
    }

    private void createChip(GameObject coin)
    {
        GameObject NewCoin = Instantiate(coin);
        NewCoin.SetActive(true);
        NewCoin.transform.position = getLocationInside(Chips.GetComponent<BoxCollider>());
        Coins.Add(NewCoin);
    }

    IEnumerator processPhaseShift()
    {
        awaitingPhaseShift = false;
        phase += 1;
        if (phase >= 10) phase = 1;
        switch (phase)
        {
            case 0: // Game Start
                break;
            case 1: // Betting
                ContinueButton.gameObject.SetActive(false);
                ButtonText.text = "Confirm Bet";
                break;
            case 2: // Dealer deals
                ContinueButton.gameObject.SetActive(false);
                // Disable coin dragging
                Bid.DragLock = true;
                Chips.DragLock = true;
                // Change gravity settings
                Bid.OuterPullPower = 0;
                Chips.OuterPullPower = 0;
                Dealer.OuterPullPower = 11;
                Dealer.DragLock = true;
                // Draw 1 FU and 1 FD
                dealerCards.Add(drawCard(Dealer.GetComponent<BoxCollider>()));
                dealerCards.Add(drawCard(Dealer.GetComponent<BoxCollider>(), true));

                // Calculate dealer score (cheat way)
                DealerText.text = "??";

                yield return new WaitForSeconds(1f);
                progressPhase();
                break;
            case 3: // Player Deals
                // Change gravity settings
                Dealer.OuterPullPower = 0;
                Board.OuterPullPower = 11;

                playerCards.Add(drawCard(Board.GetComponent<BoxCollider>()));
                playerCards.Add(drawCard(Board.GetComponent<BoxCollider>()));
                progressPhase();
                break;
            case 4: // Hit/Stand
                DeckController.gameObject.SetActive(true);
                ContinueButton.gameObject.SetActive(true);

                ButtonText.text = "Stand";
                break;
            case 5: // Check bust
                ContinueButton.gameObject.SetActive(false);
                break;
            case 6: // Dealer hits until > 17
                break;
            case 7: // Check dealer bust, compare scores. Bid doubles/disappears.
                break;
            case 8: // Cards disappear, deck shuffles
                break;
            case 9: // Check all money gone.
                break;
        }

    }

    public void progressPhase()
    {
        awaitingPhaseShift = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (awaitingPhaseShift)
            StartCoroutine(processPhaseShift());
        switch (phase)
        {
            case 0: // Game Start
                break;
            case 1: // Betting
                // This is very poorly done.
                // Iterate through the chips and update their positions in the dictionary.
                bool canContinue = true;
                int bid = 0;
                int money = 0;
                foreach (GameObject entry in Coins)
                {
                    int value = 0;
                    switch (entry.tag)
                    {
                        case "Coin1": value = 1; break;
                        case "Coin5": value = 5; break;
                    }
                    if (Bid.isCaught(entry))
                        bid += value;
                    else if (Chips.isCaught(entry))
                        money += value;
                    else
                        canContinue = false;
                }
                MoneyText.text = money.ToString();
                BidText.text = bid.ToString();
                ContinueButton.gameObject.SetActive(canContinue && bid > 0);
                break;
            case 2: // Dealer deals
                break;
            case 3: // Player Deals
                int score = 0;
                bool startingAce = false;
                bool starting10 = false;
                for (int n = 0; n < playerCards.Count; n++)
                {
                    string cardval = playerCards[n];
                    int temp = 0;
                    if (int.TryParse(cardval, out temp))
                        temp = int.Parse(cardval);
                    else if (cardval.Equals("A"))
                    {
                        if (n <= 1)
                            startingAce = true;
                        temp = 1;
                    }
                    else
                        temp = 10;
                    if (n <= 1 && temp == 10) starting10 = true;
                    score += temp;
                }
                if (starting10 && startingAce) score += 10;
                PlayerText.text = score.ToString();
                break;
            case 4: // Hit/Stand
                break;
            case 5: // Check bust
                break;
            case 6: // Dealer hits until > 17
                break;
            case 7: // Check dealer bust, compare scores. Bid doubles/disappears.
                break;
            case 8: // Cards disappear, deck shuffles
                break;
            case 9: // Check all money gone.
                break;
        }
    }
}
