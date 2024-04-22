using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBaseFightOnAwake : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("BaseFight");
    }

}
