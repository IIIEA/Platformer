using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualButtonStyle : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;

    private Vector3 _startTransform;
    private Vector3 _targetTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _startTransform = _rectTransform.localScale;
    }

    private void OnMouseEnter()
    {

    }

}
