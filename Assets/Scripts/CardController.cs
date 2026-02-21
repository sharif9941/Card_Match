using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public static CardController Instance;
    CardsSpawner cardsSpawner;

    int totalMatchCount;
    int totalCardsCount;

    [SerializeField] List<Card> cardsPrefab;

    [Header("Game Play")]
    [Tooltip("How long to show cards at start of the game")]
    [SerializeField] float startRevealTime;
    public float cardFlipAnimTime = 0.5f;
    [SerializeField] float waitTimeForMatchCheck = 1f;

    Card firstSelectedCard;
    Card secondSelectedCard;

    public bool CanSelectCard { get; private set; }

    List<Card> gameCardsList;

    void Awake()
    {
        Instance = this;
        cardsSpawner = GetComponent<CardsSpawner>();
    }

    void Start()
    {
        CanSelectCard = false;
        StartGame(4, 5);
    }

    void StartGame(int row, int column)
    {
        //Cards count should be even i.e all cards hav a match
        if((row * column) % 2 != 0)
        {
            column++;
        }

        totalMatchCount = row * column / 2;
        totalCardsCount = row * column;

        if (totalMatchCount > cardsPrefab.Count)
        {
            Debug.LogError("Don't have enough card prefabs to start game in present config");
        }

        //Cards list with matching cards
        List<Card> cardsList = cardsPrefab.GetRange(0, totalMatchCount);
        cardsList.AddRange(cardsList);

        //Shuffled cards list
        List<Card> shuffledCardsList = cardsList.OrderBy(x => Random.value).ToList();

        //SpawnCards
        cardsSpawner.SpawnCards(row, column, shuffledCardsList);
        gameCardsList = GetComponentsInChildren<Card>().ToList();

        StartCoroutine(RevealCardsAtStart());
    }

    IEnumerator RevealCardsAtStart()
    {
        BroadcastMessage("ShowCardImmediate");

        yield return new WaitForSeconds(startRevealTime);

        BroadcastMessage("HideCard");
        CanSelectCard = true;
    }

    public void CardSelected(Card card)
    {
        if(firstSelectedCard == null)
        {
            firstSelectedCard = card;
        }
        else
        {
            secondSelectedCard = card;
            CanSelectCard = false;
            CheckForMatch();
        }
    }

    void CheckForMatch()
    {
        if(firstSelectedCard.GetFruitType() == secondSelectedCard.GetFruitType())
        {
            //Match
            StartCoroutine(CardsMatched());
        }
        else
        {
            //Not a Match
            StartCoroutine(FlipUnmatchedCardsBack());
        }
    }

    IEnumerator CardsMatched()
    {
        yield return new WaitForSeconds(waitTimeForMatchCheck);

        firstSelectedCard.Disable();
        secondSelectedCard.Disable();

        firstSelectedCard = null;
        secondSelectedCard = null;

        CanSelectCard = true;
    }

    IEnumerator FlipUnmatchedCardsBack()
    {
        yield return new WaitForSeconds(waitTimeForMatchCheck);

        firstSelectedCard.HideCard();
        secondSelectedCard.HideCard();

        firstSelectedCard = null;
        secondSelectedCard = null;

        yield return new WaitForSeconds(cardFlipAnimTime);

        CanSelectCard = true;
    }
    
}
