using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    const string gameSaveAvailable = "GAME_SAVE_AVAILABLE";

    const string row = "ROW";
    const string column = "COLUMN";

    const string matchCount = "MATCH_COUNT";
    const string turnCount = "TURN_COUNT";

    const string cardCount = "CARD_COUNT";
    const string cardValue = "CARD_VALUE"; 

    const string cardsMatched = "CARDS_MATCHED";

    void Awake()
    {
        Instance = this;
    }

    public void GameSave()
    {
        PlayerPrefs.SetInt(gameSaveAvailable, 1);

        PlayerPrefs.Save();
    }

    public bool CheckGameSave()
    {
        var value = PlayerPrefs.GetInt(gameSaveAvailable, 0);

        return (value == 1);
    }


    public void SaveLayout(int _row, int _column)
    {
        PlayerPrefs.SetInt(row, _row);
        PlayerPrefs.SetInt(column, _column);

        PlayerPrefs.Save();
    }

    public void SaveCardList(List<Card> cardList)
    {
        PlayerPrefs.SetInt(cardCount, cardList.Count);

        for (int i = 0; i < cardList.Count; i++)
        {
            PlayerPrefs.SetInt(cardValue + i, (int)cardList[i].GetFruitType());
        }

        PlayerPrefs.Save();
    }

    public void SaveMatchCount(int value)
    {
        PlayerPrefs.SetInt(matchCount, value);
        PlayerPrefs.Save();
    }

    public void SaveTurnCount(int value)
    {
        PlayerPrefs.SetInt(turnCount, value);
        PlayerPrefs.Save();
    }

    public void SaveCardMatch(FruitEnum.Fruit fruit)
    {
        PlayerPrefs.SetInt(cardsMatched + (int)fruit, 1);
        PlayerPrefs.Save();
    }

    public void ClearSave()
    {
        PlayerPrefs.DeleteAll();
    }

    public SaveData LoadSavedData()
    {
        var saveData = new SaveData();

        saveData.row = PlayerPrefs.GetInt(row, 0);
        saveData.column = PlayerPrefs.GetInt(column, 0);

        saveData.matchCount = PlayerPrefs.GetInt(matchCount, 0);
        saveData.turnCount = PlayerPrefs.GetInt(turnCount, 0);

        saveData.cardCount = PlayerPrefs.GetInt(cardCount, 0);
        saveData.cardList = new List<FruitEnum.Fruit>();
        for (int i = 0; i < saveData.cardCount; i++)
        {
            var item = PlayerPrefs.GetInt(cardValue + i, 0);
            saveData.cardList.Add((FruitEnum.Fruit)item);
        }

        saveData.cardsMatched = new List<FruitEnum.Fruit>();
        for (int i = 0; i < saveData.cardCount / 2; i++)
        {
            var value = PlayerPrefs.GetInt(cardsMatched + i, 0);
            if(value == 1)
            {
                saveData.cardsMatched.Add((FruitEnum.Fruit)i);
            }
        }

        return saveData;
    }
}

public class SaveData
{
    public int row;
    public int column;

    public int matchCount;
    public int turnCount;

    public int cardCount;
    public List<FruitEnum.Fruit> cardList;
    public List<FruitEnum.Fruit> cardsMatched;
}
