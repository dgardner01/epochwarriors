using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCharAnimation : MonoBehaviour
{
    public Vector3 startPosition;
    public float magnitude, frequency;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos;
        pos = startPosition;
        pos.y = startPosition.y + (magnitude * Mathf.Sin(frequency * Time.time));
        transform.localPosition = pos;
    }
}
