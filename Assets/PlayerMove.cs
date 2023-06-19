using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Rigidbody rb_body;
    [SerializeField] Transform tf_body;
    [SerializeField] Transform[] posPoints;
    [SerializeField] Transform disappearPoint;
    [SerializeField] float moveDuration;
    [SerializeField] Ease moveEase;
    [SerializeField] float jumpPower;

    [Space]
    [SerializeField] bool footTouching;

    int nowPoint;
    bool moving;

    public void Left()
    {
        if (nowPoint > 0 && !moving)
        {
            moving = true;
            tf_body.DOMoveX(posPoints[nowPoint - 1].position.x, moveDuration).SetEase(moveEase).OnComplete(()=>
            {
                moving = false;
                nowPoint--;
            });
        }
    }

    public void Right()
    {
        if (nowPoint < 2 && !moving)
        {
            moving = true;
            tf_body.DOMoveX(posPoints[nowPoint + 1].position.x, moveDuration).SetEase(moveEase).OnComplete(() =>
            {
                moving = false;
                nowPoint++;
            });
        }
    }

    public void Jump()
    {
        if (footTouching)
        {
            footTouching = false;
            rb_body.AddForce(Vector3.up * jumpPower);
        }
    }

    public void Appear()
    {
        nowPoint = 1;
        tf_body.DOKill();
        moving = true;
        tf_body.position = disappearPoint.position;
        tf_body.DOMoveZ(posPoints[1].position.z, 0.5f).OnComplete(() => { moving = false; });
    }

    public void Disappear()
    {
        tf_body.DOKill();
        tf_body.DOMoveZ(disappearPoint.position.z, 0.5f);
    }

    public void FootTouch(bool active)
    {
        footTouching = active;
    }
}
