using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CoinInstance : MonoBehaviour
{
    private CoinController _controller;

    public bool Collected { get; private set; }

    private void Awake()
    {
        _controller = GetComponentInParent<CoinController>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            var player = playerController;

            if (player != null)
            {
                OnPlayerEnter(player);
            }
        }
    }

    private void OnPlayerEnter(PlayerController player)
    {
        if (Collected) return;

        if (_controller != null)
            Collected = true;
    }
}
