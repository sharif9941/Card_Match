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
}
