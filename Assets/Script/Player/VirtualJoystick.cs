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
    private RectTransform lever;    // J : 레버 오브젝트의 위치
    private RectTransform rectTransform;    // J : 조이스틱의 위치

    [SerializeField, Range(10f, 150f)]
    private float leverRange;   // J : 레버가 움직이는 범위

    private Vector2 inputVector;
    private bool isInput;   // J : 드래그 여부 플래그 (true인 경우에만 플레이어 이동)

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        playerAction = GameObject.Find("Player").GetComponent<PlayerAction>();
    }

    // 드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin");
        ControlJoystickLever(eventData);
        isInput = true;
        //throw new System.NotImplementedException();
    }

    // J : 오브젝트를 클릭해서 드래그 하는 도중에 들어오는 이벤트 
    // J : 하지만 클릭을 유지한 상태로 마우스를 멈추면 이벤트가 들어오지 않음
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag");
        ControlJoystickLever(eventData);
        //throw new System.NotImplementedException();
    }

    // 드래그 종료
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
        // J : touchPosition : 자기 자신 오브젝트를 기준으로 한 좌표
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 touchPosition))
            return;
        //Debug.Log("touchPosition:" + touchPosition);
        var clampedDir = touchPosition.magnitude < leverRange ? touchPosition : (touchPosition.normalized * leverRange);    // J : 레버가 움직이는 벡터를 레버 범위 안으로
        lever.anchoredPosition = clampedDir;    // J : 레버 위치 설정
        inputVector = clampedDir / leverRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInput)    // J : 드래그 중이면 플레이어 이동
        {
            if (Mathf.Abs(inputVector.x) >= Mathf.Abs(inputVector.y))
            {
                //Debug.Log("horizontal");
                if (inputVector.x > 0)  // J : 오른쪽으로 이동
                {
                    playerAction.horizontalMove = Vector3.right;
                    playerAction.verticalMove = Vector3.zero;
                }
                else if (inputVector.x < 0) // J : 왼쪽으로 이동
                {
                    playerAction.horizontalMove = Vector3.left;
                    playerAction.verticalMove = Vector3.zero;
                }
            }
            else
            {
                //Debug.Log("vertical");
                if (inputVector.y > 0)  // J : 위로 이동
                {
                    playerAction.horizontalMove = Vector3.zero;
                    playerAction.verticalMove = Vector3.up;
                }
                else if (inputVector.y < 0) // J : 아래로 이동
                {
                    playerAction.horizontalMove = Vector3.zero;
                    playerAction.verticalMove = Vector3.down;
                }
            }
        }
        else    // J : 플레이어 이동 X
        {
            //Debug.Log("No");
            playerAction.horizontalMove = playerAction.verticalMove = Vector3.zero;
        }
    }
}
