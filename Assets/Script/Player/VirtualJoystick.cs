using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// https://wergia.tistory.com/231
public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private PlayerAction playerAction;

    [SerializeField]
    private RectTransform lever;    // J : ���� ������Ʈ�� ��ġ
    private RectTransform rectTransform;    // J : ���̽�ƽ�� ��ġ

    [SerializeField, Range(10f, 150f)]
    private float leverRange;   // J : ������ �����̴� ����

    private Vector2 inputVector;
    private bool isInput;   // J : �巡�� ���� �÷��� (true�� ��쿡�� �÷��̾� �̵�)

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        playerAction = GameObject.Find("Player").GetComponent<PlayerAction>();
    }

    // �巡�� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin");
        ControlJoystickLever(eventData);
        isInput = true;
        //throw new System.NotImplementedException();
    }

    // J : ������Ʈ�� Ŭ���ؼ� �巡�� �ϴ� ���߿� ������ �̺�Ʈ 
    // J : ������ Ŭ���� ������ ���·� ���콺�� ���߸� �̺�Ʈ�� ������ ����
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag");
        ControlJoystickLever(eventData);
        //throw new System.NotImplementedException();
    }

    // �巡�� ����
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End");
        lever.anchoredPosition = Vector2.zero;
        isInput = false;
        //throw new System.NotImplementedException();
    }

    public void ControlJoystickLever(PointerEventData eventData)
    {
        //https://indala.tistory.com/50
        // J : touchPosition : �ڱ� �ڽ� ������Ʈ�� �������� �� ��ǥ
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 touchPosition))
            return;
        //Debug.Log("touchPosition:" + touchPosition);
        var clampedDir = touchPosition.magnitude < leverRange ? touchPosition : (touchPosition.normalized * leverRange);    // J : ������ �����̴� ���͸� ���� ���� ������
        lever.anchoredPosition = clampedDir;    // J : ���� ��ġ ����
        inputVector = clampedDir / leverRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInput)    // J : �巡�� ���̸� �÷��̾� �̵�
        {
            if (Mathf.Abs(inputVector.x) >= Mathf.Abs(inputVector.y))
            {
                //Debug.Log("horizontal");
                if (inputVector.x > 0)  // J : ���������� �̵�
                {
                    playerAction.horizontalMove = Vector3.right;
                    playerAction.verticalMove = Vector3.zero;
                }
                else if (inputVector.x < 0) // J : �������� �̵�
                {
                    playerAction.horizontalMove = Vector3.left;
                    playerAction.verticalMove = Vector3.zero;
                }
            }
            else
            {
                //Debug.Log("vertical");
                if (inputVector.y > 0)  // J : ���� �̵�
                {
                    playerAction.horizontalMove = Vector3.zero;
                    playerAction.verticalMove = Vector3.up;
                }
                else if (inputVector.y < 0) // J : �Ʒ��� �̵�
                {
                    playerAction.horizontalMove = Vector3.zero;
                    playerAction.verticalMove = Vector3.down;
                }
            }
        }
        else    // J : �÷��̾� �̵� X
        {
            //Debug.Log("No");
            playerAction.horizontalMove = playerAction.verticalMove = Vector3.zero;
        }
    }
}
