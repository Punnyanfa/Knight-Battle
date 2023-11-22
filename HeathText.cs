using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HeathText : MonoBehaviour
{   //Pixels peer second 
    public Vector3 moveSpeed = new Vector3(0, 75, 0);
    public float timeToFade = 1f;


    RectTransform textTrasform;
    TextMeshProUGUI textMeshPro;

    private float timeElapsed = 0f;
    private Color startColor;


    private void Awake()
    {
        textTrasform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        startColor = textMeshPro.color;
    }
    private void Update()
    {
        textTrasform.position += moveSpeed * Time.deltaTime;
        timeElapsed += Time.deltaTime;
        if(timeElapsed < timeToFade)
        {
            float fadeAlpha= startColor.a * (1 - (timeElapsed / timeToFade));
            textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
        }else
        {
            Destroy(gameObject);
        }
    }
}
