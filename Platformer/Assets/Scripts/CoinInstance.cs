using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CoinInstance : MonoBehaviour
{
    private CoinCollect _coinCollect;

    public bool Collected { get; private set; }

    private void Awake()
    {
        _coinCollect = GetComponentInParent<CoinCollect>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement playerController))
        {
            var player = playerController;

            if (player != null)
            {
                OnPlayerEnter(player);
            }
        }
    }

    private void OnPlayerEnter(PlayerMovement player)
    {
        if (Collected) return;

        if (_coinCollect != null)
            Collected = true;
    }
}
