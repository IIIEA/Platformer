using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    public bool IsCollected { get; private set; }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        {
            OnPlayerEnter(player);
        }
    }

    private void OnPlayerEnter(PlayerMovement player)
    {
        gameObject.SetActive(false);
    }
}
