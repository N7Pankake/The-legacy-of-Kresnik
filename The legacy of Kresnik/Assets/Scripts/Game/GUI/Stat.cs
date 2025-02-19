﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour {

    private Image content;

    [SerializeField]
    private TextMeshProUGUI statValue;

    [SerializeField]
    private float lerpSpeed;

    private float overflow;

    public float MyOverflow
    {
        get
        {
            float tmp = overflow;
            overflow = 0;
            return tmp;
        }
    }

    private float currentFill;

    public bool IsFull
    {
        get { return content.fillAmount == 1; }
    }

    public float MyMaxValue { get; set; }

    private float currentValue;

    public float MyCurrentValue
    {
      get
        {
           return currentValue;
        }

      set
        {
            if(value > MyMaxValue)
            {
                overflow = value - MyMaxValue;
                currentValue = MyMaxValue;
            }
            else if (value < 0)
            {
                currentValue = 0;
            }
            else
            {
                currentValue = value;
            }

            currentFill = currentValue / MyMaxValue;

            if (statValue != null)
            {
                statValue.text = currentValue + " / " + MyMaxValue;
            }
            
        }
    }

	// Use this for initialization
	void Start ()
    {
        content = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.MoveTowards(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
	}

    public void Initialize(float currentValue, float maxValue)
    {
        if (content == null)
        {
            content = GetComponent<Image>();
        }
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
        content.fillAmount = MyCurrentValue / MyMaxValue;
    }

    public void Reset()
    {
        content.fillAmount = 0;
    }
}
