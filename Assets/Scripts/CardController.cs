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
    public float cardFlipAnimTime;
    [SerializeField] float KeepCardsFlippedTime;

    Card firstSelectedCard;
    Card secondSelectedCard;

    public bool CanSelectCard { get; private set; }
    List<Card> gameCardsList;


    UIManager UIManager;

    int turnsTaken;
    int matchesMade;

    [SerializeField] int row;
    [SerializeField] int column;

    public int TurnsTaken
    {
        get { return turnsTaken; }
        set
        {
            turnsTaken = value;
            if(UIManager != null)
            {
                UIManager.SetTurnValueText(value.ToString());
            }
        }
    }

    public int MatchesMade
    {
        get { return matchesMade; }
        set
        {
            matchesMade = value;
            if (UIManager != null)
            {
                UIManager.SetMatchValueText(value.ToString());
            }
        }
    }


    void Awake()
    {
        Instance = this;
        cardsSpawner = GetComponent<CardsSpawner>();
    }

    void Start()
    {
        UIManager = UIManager.Instance;

        CanSelectCard = false;
        StartGame(row, column);
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
        TurnsTaken++;

        if (firstSelectedCard.GetFruitType() == secondSelectedCard.GetFruitType())
        {
            //Match
            StartCoroutine(CardsMatched(firstSelectedCard, secondSelectedCard));
        }
        else
        {
            //Not a Match
            StartCoroutine(FlipUnmatchedCardsBack(firstSelectedCard, secondSelectedCard));
        }

        firstSelectedCard = null;
        secondSelectedCard = null;

        CanSelectCard = true;
    }

    IEnumerator CardsMatched(Card _card1, Card _card2)
    {
        MatchesMade++;

        yield return new WaitForSeconds(cardFlipAnimTime + (KeepCardsFlippedTime/2));

        _card1.Disable();
        _card2.Disable();

        CheckForGameEnd();
    }

    IEnumerator FlipUnmatchedCardsBack(Card _card1, Card _card2)
    {
        CheckForGameEnd();

        yield return new WaitForSeconds(cardFlipAnimTime + KeepCardsFlippedTime);

        _card1.HideCard();
        _card2.HideCard();
    }
    

    void CheckForGameEnd()
    {
        if(totalMatchCount == matchesMade)
        {
            //Won
            UIManager.Won();
        }

        if(turnsTaken > 25)
        {
            //Lost
            UIManager.Lost();
        }
    }
}
