using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Text Components")]
    [SerializeField] TextMeshProUGUI matchValueText;
    [SerializeField] TextMeshProUGUI turnValueText;

    [Header("Game End")]
    [SerializeField] GameObject WonPanel;
    [SerializeField] GameObject LostPanel;

    [Space]
    [SerializeField] GameObject StartPanel;

    private void Awake()
    {
        Instance = this;
    }

    public void SetMatchValueText(string value)
    {
        matchValueText.text = value;
    }

    public void SetTurnValueText(string value)
    {
        turnValueText.text = value;
    }

    public void Won()
    {
        WonPanel.SetActive(true);
    }

    public void Lost()
    {
        LostPanel.SetActive(true);
    }

    public void ShowStartPanel()
    {
        StartPanel.SetActive(true);
    }

    #region StartGame
    public void StartGame2x3()
    {
        StartGame(2,3);
    }

    public void StartGame2x4()
    {
        StartGame(2, 4);
    }

    public void StartGame3x4()
    {
        StartGame(3, 4);
    }

    public void StartGame3x6()
    {
        StartGame(3, 6);
    }

    public void StartGame4x5()
    {
        StartGame(4, 5);
    }

    public void StartGame4x6()
    {
        StartGame(4, 6);
    }

    public void StartGame5x6()
    {
        StartGame(5, 6);
    }

    public void StartGame5x8()
    {
        StartGame(5, 8);
    }

    public void StartGame6x4()
    {
        StartGame(6, 4);
    }

    public void StartGame6x5()
    {
        StartGame(6, 5);
    }

    public void StartGame8x5()
    {
        StartGame(8, 5);
    }

    void StartGame(int row, int column)
    {
        StartPanel.SetActive(false);
        WonPanel.SetActive(false);
        LostPanel.SetActive(false);

        CardController.Instance.StartGame(row, column);
    }
    #endregion
}
