using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsFitter : MonoBehaviour
{
    [Tooltip("Screen width to be covered [in percentage]")]
    [Range(0.5f, 1f)]
    [SerializeField] float widthOffsetFactor;

    [Tooltip("Screen height to be covered [in percentage]")]
    [Range(0.5f, 1f)]
    [SerializeField] float heightOffsetFactor;

    public void SetScale(int row, int column, float distancBetweenCards)
    {
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * Screen.width / Screen.height; // cameraHeight * aspect ratio
        float width = column * distancBetweenCards;
        float height = row * distancBetweenCards;

        float widthscale = cameraWidth / width;
        float heightscale = cameraHeight / height;

        widthscale *= widthOffsetFactor;
        heightscale *= heightOffsetFactor;

        float scale = Mathf.Min(widthscale, heightscale);

        Debug.Log(widthscale + " " + heightscale + " " + scale);
        
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
