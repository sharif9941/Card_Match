using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TextMeshProUGUI matchValueText;
    [SerializeField] TextMeshProUGUI turnValueText;

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
}
