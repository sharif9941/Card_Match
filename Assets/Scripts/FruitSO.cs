using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FruitSO", order = 1)]
public class FruitSO : ScriptableObject
{
    public FruitEnum.Fruit fruitEnum;
    public Sprite fruitSprite;
}
