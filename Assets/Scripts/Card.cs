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
    [SerializeField] FruitSO fruitSO;
    [SerializeField] SpriteRenderer fruitSR;

    CardController CardController;
    AudioManager AudioManager;

    bool showHide = false;  //true -> shown && false -> hidden
    float cardFlipAnimTime;

    public FruitEnum.Fruit GetFruitType()
    {
        return fruitSO.FruitEnum;
    }

    void Start()
    {
        CardController = CardController.Instance;
        AudioManager = AudioManager.Instance;
        cardFlipAnimTime = CardController.cardFlipAnimTime;
        SetFruitSprite();
    }

    void SetFruitSprite()
    {
        fruitSR.sprite = fruitSO.FruitSprite;
    }

    private void OnMouseDown()
    {
        if(!showHide && CardController.CanSelectCard)
        {
            ShowCard();
            CardController.CardSelected(this);
        }
    }

    public void ShowCard()
    {
        showHide = true;
        AudioManager.PlayCardFlip();
        StartCoroutine(Show());
    }

    public void ShowCardImmediate()
    {
        showHide = true;

        cardBack.gameObject.SetActive(false);
        cardFront.gameObject.SetActive(true);
    }

    public void HideCard()
    {
        StartCoroutine(Hide());
    }

    public void HideCardImmediate()
    {
        cardBack.gameObject.SetActive(true);
        cardFront.gameObject.SetActive(false);

        showHide = false;
    }

    IEnumerator Show()
    {
        yield return StartCoroutine(RotateObject(cardView, new Vector3(0, 90, 0), cardFlipAnimTime / 2));

        cardBack.gameObject.SetActive(false);
        cardFront.gameObject.SetActive(true);

        yield return StartCoroutine(RotateObject(cardView, new Vector3(0, 180, 0), cardFlipAnimTime / 2));
    }


    IEnumerator Hide()
    {
        yield return StartCoroutine(RotateObject(cardView, new Vector3(0, 270, 0), cardFlipAnimTime / 2));

        cardBack.gameObject.SetActive(true);
        cardFront.gameObject.SetActive(false);

        yield return StartCoroutine(RotateObject(cardView, new Vector3(0, 360, 0), cardFlipAnimTime / 2, true));

        showHide = false;
    }


    IEnumerator RotateObject(Transform obj, Vector3 byAngles, float inTime, bool resetToZero = false)
    {
        Quaternion fromAngle = obj.rotation;
        Quaternion toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);

        float timer = 0f;
        while (timer < inTime)
        {
            obj.rotation = Quaternion.Lerp(fromAngle, toAngle, timer / inTime);
            timer += Time.deltaTime;
            yield return null;
        }

        obj.rotation = toAngle;
        if(resetToZero)
        {
            obj.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void DisableCard()
    {
        StartCoroutine(Disable());
    }

    IEnumerator Disable()
    {
        float timer = 0f;
        float inTime = 0.15f;
        while (timer < inTime)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / inTime);
            timer += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
