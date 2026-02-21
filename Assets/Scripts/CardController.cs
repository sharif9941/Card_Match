using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardController : MonoBehaviour
{
    CardsSpawner cardsSpawner;

    [Header("Cards")]
    [SerializeField] List<Card> cardsPrefab;

    void Awake()
    {
        cardsSpawner = GetComponent<CardsSpawner>();
    }

    void Start()
    {
        StartGame(4, 5);
    }

    void StartGame(int row, int column)
    {
        //Cards count should be even i.e all cards hav a match
        if((row * column) % 2 != 0)
        {
            column++;
        }

        int totalMatchCount = row * column / 2;
        int totalCardsCount = row * column;

        //Cards list with matching cards
        List<Card> cardsList = cardsPrefab.GetRange(0, totalMatchCount);
        cardsList.AddRange(cardsList);

        //Shuffled cards list
        List<Card> shuffledCardsList = cardsList.OrderBy(x => Random.value).ToList();

        cardsSpawner.SpawnCards(row, column, shuffledCardsList);
    }
}
