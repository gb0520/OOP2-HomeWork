using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Obstacle : ZB.Object.FlexbieObject
{
    public bool ClickDetach { get => clickDetach; }
    [SerializeField] bool clickDetach;

    protected override void OnAttach()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
    }

    protected override void OnDetach()
    {
        transform.DOKill();
        transform.DOScale(Vector3.zero, 1).SetEase(Ease.InBounce).OnComplete(() => gameObject.SetActive(false));
    }
}
