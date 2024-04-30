using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MusicManager.Instance.PlayMusic("A");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            MusicManager.Instance.PlayMusicOver("A", "B");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            MusicManager.Instance.PlayMusicOver("A", "C");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MusicManager.Instance.StopMusic("A");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            MusicManager.Instance.StopMusic("B");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            MusicManager.Instance.StopMusic("C");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            MusicManager.Instance.PlayMusic("D");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            MusicManager.Instance.PlayMusicOver("D", "E");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            MusicManager.Instance.StopMusic("D");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            MusicManager.Instance.StopMusic("E");
        }
    }
}
