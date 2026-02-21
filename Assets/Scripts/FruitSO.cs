using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FruitSO", order = 1)]
public class FruitSO : ScriptableObject
{
    [SerializeField] FruitEnum.Fruit fruitEnum;
    [SerializeField] Sprite fruitSprite;

    public FruitEnum.Fruit FruitEnum
    {
        get { return fruitEnum; }
    }

    public Sprite FruitSprite
    {
        get { return fruitSprite; }
    }
}
