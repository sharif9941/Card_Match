using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Face")]
    [SerializeField] Transform cardView;
    [SerializeField] Transform cardBack;
    [SerializeField] Transform cardFront;

    [Header("Fruit")]
    public FruitSO fruitSO;
    [SerializeField] SpriteRenderer fruitSR;

    bool showHide = false;  //true -> shown && false -> hidden

    void Start()
    {
        SetFruitSprite();
    }

    void SetFruitSprite()
    {
        fruitSR.sprite = fruitSO.fruitSprite;
    }

    private void OnMouseDown()
    {
        if(!showHide)
        {
            Debug.Log("Show");
            showHide = true;
            StartCoroutine(Show());
        }
    }

    public IEnumerator Show()
    {
        yield return StartCoroutine(RotateObject(cardView, new Vector3(0, 90, 0), 1f));

        cardBack.gameObject.SetActive(false);
        cardFront.gameObject.SetActive(true);

        yield return StartCoroutine(RotateObject(cardView, new Vector3(0, 180, 0), 1f));
    }


    public IEnumerator Hide()
    {
        yield return StartCoroutine(RotateObject(cardView, new Vector3(0, 270, 0), 1f));

        cardBack.gameObject.SetActive(true);
        cardFront.gameObject.SetActive(false);

        yield return StartCoroutine(RotateObject(cardView, new Vector3(0, 360, 0), 1f, true));

        showHide = false;
    }


    IEnumerator RotateObject(Transform obj, Vector3 byAngles, float inTime, bool resetToZero = false)
    {
        Quaternion fromAngle = obj.rotation;
        Quaternion toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);

        float timer = 0f;
        while (timer < inTime)
        {
            obj.rotation = Quaternion.Lerp(fromAngle, toAngle, timer);
            timer += Time.deltaTime / inTime;
            yield return null;
        }

        obj.rotation = toAngle;
        if(resetToZero)
        {
            obj.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
