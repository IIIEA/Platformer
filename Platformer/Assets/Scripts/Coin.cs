using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    [SerializeField] private UnityEvent _collected;
    private bool _isCollected;
    public bool IsCollected => _isCollected;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        {
            if (player != null)
            {
                OnPlayerEnter(player);
            }
        }
    }

    private void OnPlayerEnter(PlayerMovement player)
    {
        if (_isCollected)
            return;

        _isCollected = true;
        _collected?.Invoke();
    }
}
