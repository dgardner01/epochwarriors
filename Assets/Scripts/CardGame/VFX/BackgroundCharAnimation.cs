using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCharAnimation : MonoBehaviour
{
    BattleSystem battleSystem => FindAnyObjectByType<BattleSystem>();
    public Vector3 startPosition;
    public float magnitude, frequency;
    public float smoothSpeed;
    float _magnitude, _frequency;
    public float excitement;
    public bool win;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        excitement = 200/((float)battleSystem.player.health + battleSystem.enemy.health);
        Vector3 pos;
        pos = startPosition;
        pos.y = startPosition.y + (_magnitude * Mathf.Abs(Mathf.Sin(startPosition.x+(_frequency * Time.time))));
        transform.localPosition = pos;
    }
    private void FixedUpdate()
    {
        if (!win)
        {
            _magnitude = Mathf.Lerp(_magnitude, magnitude, smoothSpeed);
            _frequency = Mathf.Lerp(_frequency, frequency, smoothSpeed);
        }
    }

    public IEnumerator StartBounce(float magnitude, float frequency)
    {
        yield return new WaitForSeconds(0);
        _magnitude = magnitude;
        //_frequency = frequency;
    }
}
