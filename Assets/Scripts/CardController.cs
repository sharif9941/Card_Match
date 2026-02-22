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
    AudioManager AudioManager;
    SaveSystem SaveSystem;

    int turnsTaken;
    int matchesMade;

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
            if(SaveSystem != null)
            {
                SaveSystem.SaveTurnCount(value);
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
            if (SaveSystem != null)
            {
                SaveSystem.SaveMatchCount(value);
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
        AudioManager = AudioManager.Instance;
        SaveSystem = SaveSystem.Instance;
        gameCardsList = new List<Card>();
    }

    public void StartGame(int row, int column)
    {
        ResetGame();

        CanSelectCard = false;

        //Cards count should be even i.e all cards hav a match
        if ((row * column) % 2 != 0)
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

        //Save
        SaveSystem.GameSave();
        SaveSystem.SaveLayout(row, column);
        SaveSystem.SaveCardList(shuffledCardsList);
    }

    public void LoadGame(SaveData saveData)
    {
        ClearAllCards();

        CanSelectCard = false;
        List<Card> shuffledCardsList = new List<Card>();

        //Load Saved Card Layout
        var cardFruitList = saveData.cardList;        
        for(int i = 0; i <saveData.cardCount; i++)
        {
            var fruitindex = (int)cardFruitList[i];
            shuffledCardsList.Add(cardsPrefab[fruitindex]);
        }

        cardsSpawner.SpawnCards(saveData.row, saveData.column, shuffledCardsList);
        gameCardsList = GetComponentsInChildren<Card>().ToList();

        //Load Stats
        MatchesMade = saveData.matchCount;
        turnsTaken = saveData.turnCount;
        totalMatchCount = saveData.row * saveData.column / 2;
        totalCardsCount = saveData.row * saveData.column;

        //Hide Matched Cards
        var matchedFruits = saveData.cardsMatched;
        foreach(var card in gameCardsList)
        {
            if(matchedFruits.Contains(card.GetFruitType()))
            {
                card.gameObject.SetActive(false);
            }
            card.HideCardImmediate();
        }

        CanSelectCard = true;
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
            SaveSystem.SaveCardMatch(firstSelectedCard.GetFruitType());
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
        AudioManager.PlayMatch();

        yield return new WaitForSeconds(cardFlipAnimTime + (KeepCardsFlippedTime/2));

        _card1.DisableCard();
        _card2.DisableCard();

        CheckForGameEnd();
    }

    IEnumerator FlipUnmatchedCardsBack(Card _card1, Card _card2)
    {
        AudioManager.PlayMismatch();
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
            AudioManager.PlayGameOver();
            SaveSystem.ClearSave();
        }

        if(turnsTaken > 24)
        {
            //Lost
            ClearAllCards();
            UIManager.Lost();
            AudioManager.PlayGameOver();
            SaveSystem.ClearSave();
        }
    }

    void ClearAllCards()
    {
        StopAllCoroutines();

        foreach (var card in gameCardsList)
        {
            if(card != null)
            {
                Destroy(card.gameObject);
            }         
        }
        gameCardsList.Clear();
    }

    void ResetGame()
    {
        StopAllCoroutines();

        totalMatchCount = 0;
        totalCardsCount = 0;

        firstSelectedCard = null;
        secondSelectedCard = null;

        TurnsTaken = 0;
        MatchesMade = 0;

        ClearAllCards();

        SaveSystem.ClearSave();
    }
}
