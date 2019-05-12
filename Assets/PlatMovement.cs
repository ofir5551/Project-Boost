using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlatMovement : MonoBehaviour
{
    [SerializeField] Vector3 MovementVector;
    [SerializeField] float period = 3f;

    float MovementFactor;
    [SerializeField] Vector3 StartingPos;

    // Start is called before the first frame update
    void Start()
    {
        StartingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2f; // ABOUT 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);
        MovementFactor = rawSinWave / 2f + 0.5f;

        Vector3 offset = MovementVector * MovementFactor;
        transform.position = StartingPos + offset;
    }
}
