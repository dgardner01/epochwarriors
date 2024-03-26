using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Coffee.UIExtensions;

public class TextParticle : MonoBehaviour
{
    public Camera textRenderer;
    public TextMeshProUGUI renderText;
    public Material material;
    public Transform particleParent;
    public GameObject numberPopUp, blockPopUp, statusPopUp;

    Material TextMat(string text)
    {
        Material mat = new Material(material.shader);
        renderText.text = text;
        mat.mainTexture = textRenderer.targetTexture;
        return mat;
    }

    public void NumberPopUp(string text, Vector2 position)
    {
        GameObject instance = Instantiate(numberPopUp, particleParent);
        instance.GetComponent<UIParticle>().material = TextMat(text);
        instance.transform.position = position;
    }

    public void TextPopUp(string text, Vector2 position)
    {
        GameObject instance = Instantiate(blockPopUp, particleParent);
        instance.GetComponent<UIParticle>().material = TextMat(text);
        instance.transform.position = position;
    }
    
    public void StatusPopUp(string text, Vector2 position)
    {
        GameObject instance = Instantiate(statusPopUp, particleParent);
        instance.GetComponent<UIParticle>().material = TextMat(text);
        instance.transform.position = position;
    }

}
