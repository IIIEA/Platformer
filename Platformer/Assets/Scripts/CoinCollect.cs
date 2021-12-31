using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    [SerializeField] private CoinInstance[] _coins;

    private void Awake()
    {
        if (_coins.Length == 0)
            FindAllCoinsInScene();
    }

    private void Update()
    {
        for (int i = 0; i < _coins.Length; i++)
        {
            CoinInstance _coin = _coins[i];

            if (_coin != null)
            {
                if (_coin.Collected)
                {
                    _coin.gameObject.SetActive(false);
                    _coins[i] = null;
                }
            }
        }
    }
    private void FindAllCoinsInScene()
    {
        _coins = GetComponentsInChildren<CoinInstance>();
    }
}

