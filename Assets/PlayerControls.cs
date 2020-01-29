using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject Deck;
    public GameObject Card;
    private List<GameObject> Cards;
    /* TODO:
     * Make draggable cards.
     * Make drawable cards.
     * */
    // Start is called before the first frame update
    void Start()
    {
        Cards = new List<GameObject>();
    }

    void DrawFromDeck()
    {
        GameObject NewCard = Instantiate(Card);
        NewCard.SetActive(true);
        Cards.Add(NewCard);
        print("Card drawn");
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
