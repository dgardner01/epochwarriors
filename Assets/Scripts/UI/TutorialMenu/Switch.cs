using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Switch : MonoBehaviour
{
    public TMP_Text[] text;
    int index;
    void Start()
    {
        index = 0;
    }

    
    void Update()
    {
        if (index >= 3) index = 3;

        if (index < 0) index = 0;


        if (index == 0) text[0].gameObject.SetActive(true);
    }

    public void Next()
    {
        if(index < 3)
        {
            index += 1;

            for (int i = 0; i < text.Length; i++)
            {
                text[i].gameObject.SetActive(false);
                text[index].gameObject.SetActive(true);
            }

            Debug.Log(index);
        }
        
    }

    public void Back()
    {
        if(index > 0)
        {
            index -= 1;

            for (int i = 0; i < text.Length; i++)
            {
                text[i].gameObject.SetActive(false);
                text[index].gameObject.SetActive(true);
            }

            Debug.Log(index);
        }
       
    }
}
