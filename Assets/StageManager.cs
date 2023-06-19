using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StageManager : MonoBehaviour
{
    public int HP { get => hp; }

    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private ObstacleSpawner spawner;

    [SerializeField] private int hp;
    [SerializeField] private float invincibilityTime;

    private bool invincibility;
    private Vector3 p3_originalCamera;

    public void Hit()
    {
        if (!invincibility)
        {
            hp -= 1;

            Camera.main.transform.DOKill();
            Camera.main.transform.position = p3_originalCamera;
            Camera.main.transform.DOShakePosition(0.3f, 1, 15).SetUpdate(true);

            ZB.TimeScaleBrain.Instance.ChangeTimeScale(0.2f, 0.5f, false);

            if (hp > 0)
            {
                invincibility = true;
                transform.DOMove(Vector3.zero, invincibilityTime).OnComplete(() =>
                {
                    invincibility = false;
                });
            }

            //게임패배판정
            else
            {
                StageEnd();
            }
        }
    }

    void StageEnd()
    {
        playerMove.Disappear();
        spawner.SpawnCycleStop();

        transform.DOMove(Vector3.zero, 3).OnComplete(() =>
        {
            StageStart();
        });
    }
    void StageStart()
    {
        hp = 3;

        playerMove.Appear();
        spawner.SpawnCycleStart();
    }

    private void Start()
    {
        StageStart();
        p3_originalCamera = Camera.main.transform.position;
    }
}
