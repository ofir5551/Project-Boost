using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlatMovement : MonoBehaviour
{
    [SerializeField] Vector3 MovementVector;
    [Range(0, 1)] [SerializeField] float MovementFactor;
    [SerializeField] Vector3 StartingPos;

    // Start is called before the first frame update
    void Start()
    {
        StartingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = MovementVector * MovementFactor;
        transform.position = StartingPos + offset;
    }
}
