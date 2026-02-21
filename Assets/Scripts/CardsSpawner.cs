using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsSpawner : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] float padding = 1.5f;

    public void SpawnCards(int row, int column, List<Card> cards)
    {
        //Check
        if((row * column) != cards.Count)
        {
            Debug.LogError("Count mismatch");
        }

        float rowStart;
        float columnStart;
        Vector3 pos;

        if (row % 2 != 0)
        {
            rowStart = ((row - 1) / 2) * padding;
        }
        else
        {
            rowStart = ((row / 2) * padding) - (padding / 2);
        }

        if (column % 2 != 0)
        {
            columnStart = ((column - 1) / 2) * padding * -1f;
        }
        else
        {
            columnStart = ((column / 2) * padding * -1f) + (padding / 2);
        }

        int index = 0;
        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < column; j++)
            {
                pos = new Vector3(columnStart + (j * padding), rowStart - (i * padding)) + transform.position;
                Instantiate(cards[index], pos, Quaternion.identity, transform);
                index++;
            }
        }
    }
}
