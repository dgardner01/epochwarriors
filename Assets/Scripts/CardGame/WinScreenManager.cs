using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinScreenManager : MonoBehaviour
{
    public TextMeshProUGUI header, combo, charge, chain;
    private void Start()
    {
        if (PlayerPrefs.GetInt("playerWon") > 0)
        {
            header.text = "You won! The Time Stone is safe.";
        }
        else
        {
            header.text = "You lost... the Time Stone is in danger.";
        }
        combo.text = PlayerPrefs.GetInt("combo")+"";
        charge.text = "x" + PlayerPrefs.GetInt("charge");
        chain.text = "x" + PlayerPrefs.GetInt("chain") + "";
    }
}
