using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    [SerializeField] private Coin[] _coins;

    private void Awake()
    {
        _coins = GetComponentsInChildren<Coin>();
    }

    public void ToCollect()
    {
        for (int i = 0; i < _coins.Length; i++)
        {
            Coin coin = _coins[i];

            if (coin != null)
            {
                if (coin.IsCollected)
                {
                    coin.gameObject.SetActive(false);
                    _coins[i] = null;
                }
            }
        }
    }
}

