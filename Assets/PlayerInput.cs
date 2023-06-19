using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] ZB.Screen.ScreenClick2World screenClick;
    [SerializeField] ZB.Screen.ScreenDrag screenDrag;
    [SerializeField] PlayerMove playerMove;
    [SerializeField] StageManager stageManager;

    [Space]
    [SerializeField] float minDragPower;
    [SerializeField] Obstacle currentClickedObstacle;

    void Update()
    {
        //Ŭ������ ��ֹ� ����
        if (Input.GetMouseButtonDown(0) &&
            screenClick.TryClickByMouse(out currentClickedObstacle) &&
            stageManager.HP > 0 &&
            currentClickedObstacle.ClickDetach)
        {
            currentClickedObstacle.Detach();
        }

        //���������� �̵�
        if (screenDrag.Dragging &&
            screenDrag.Magnitude_OneFrame > minDragPower)
        {
            //�����̵�
            if (Vector2.Angle(screenDrag.DragVector_OneFrame, Vector2.left) < 15)
                playerMove.Left();

            //������ �̵�
            else if (Vector2.Angle(screenDrag.DragVector_OneFrame, Vector2.right) < 15)
                playerMove.Right();

            //����
            else if (Vector2.Angle(screenDrag.DragVector_OneFrame, Vector2.up) < 15)
                playerMove.Jump();
        }
    }
}
