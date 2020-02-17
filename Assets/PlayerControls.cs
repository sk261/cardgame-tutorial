using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject Deck;
    public GameObject Card;
    private List<GameObject> ActiveCards;
    private List<string> ValueCards;
    public Material faceUpMaterial;
    public Material faceDownMaterial;
    /* TODO:
     * Make drawable cards dragged
     * */
    // Start is called before the first frame update
    void Start()
    {
        ActiveCards = new List<GameObject>();
        ValueCards = new List<string>();
        for (int a = 0; a < 4; a++)
            for (int b = 1; b <= 13; b++)
            {
                if (b == 1)
                    ValueCards.Add("A");
                else if (b == 11)
                    ValueCards.Add("J");
                else if (b == 12)
                    ValueCards.Add("Q");
                else if (b == 13)
                    ValueCards.Add("K");
                else
                    ValueCards.Add(b.ToString());
            }
        for (int n = 0; n < 300; n++)
        {
            // Shuffling
            int ranA = 0, ranB = 0;
            while (ranA == ranB)
            {
                ranA = (int)(Random.value * (4 * 13));
                ranB = (int)(Random.value * (4 * 13));
            }
            string T = ValueCards[ranA];
            ValueCards[ranA] = ValueCards[ranB];
            ValueCards[ranB] = T;
        }
    }

    public GameObject DrawFromDeck(bool Selected = true, bool FaceDown = false)
    {
        GameObject NewCard = Instantiate(Card);

        string value = ValueCards[0];
        ValueCards.RemoveAt(0);

        NewCard.GetComponent<Transform>().SetParent(Card.GetComponent<Transform>().parent.transform);
        Transform text = NewCard.GetComponent<Transform>().GetChild(0);
        text.GetComponent<TextMesh>().text = value;
        NewCard.name = value;
        flipCard(NewCard, FaceDown);
        NewCard.SetActive(true);
        if (Selected)
            NewCard.GetComponent<ObjectDrag>().SelectCard();
        ActiveCards.Add(NewCard);
        return NewCard;
    }

    private void flipCard(GameObject card, bool FaceDown)
    {
        card.GetComponent<Transform>().GetChild(0).gameObject.SetActive(FaceDown == false);
        Material mat = faceUpMaterial;
        if (FaceDown)
            mat = faceDownMaterial;

        card.GetComponent<MeshRenderer>().material = mat;
    }

    public bool isCardFU(GameObject card)
    {
        return card.GetComponent<Transform>().GetChild(0).gameObject.activeSelf;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            switch (hit.collider.name)
            {
                case "Deck":
                    if (Input.GetMouseButtonDown(0))
                    {
                        DrawFromDeck();
                    }
                    break;
            }
        }
    }
}
