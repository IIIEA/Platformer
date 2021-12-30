using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class PatrolPath : MonoBehaviour
{
    public Vector2 startPosition, endPosition;

    public Mover CreateMover(float speed = 1) => new Mover(this, speed);

    void Reset()
    {
        startPosition = Vector3.left;
        endPosition = Vector3.right;
    }
}