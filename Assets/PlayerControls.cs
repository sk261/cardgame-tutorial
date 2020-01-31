using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject Deck;
    public GameObject Card;
    private List<GameObject> Cards;
    /* TODO:
     * Make drawable cards dragged
     * */
    // Start is called before the first frame update
    void Start()
    {
        Cards = new List<GameObject>();
    }

    void DrawFromDeck()
    {
        GameObject NewCard = Instantiate(Card);
        NewCard.GetComponent<Transform>().SetParent(Card.GetComponent<Transform>().parent.transform);
        // NewCard.GetComponent<Transform>().localScale = Card.GetComponent<Transform>().localScale;
        NewCard.SetActive(true);
        NewCard.GetComponent<ObjectDrag>().SelectCard(); 
        Cards.Add(NewCard);
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
