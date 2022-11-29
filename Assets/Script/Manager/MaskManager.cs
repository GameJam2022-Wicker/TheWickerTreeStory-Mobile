using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskManager : MonoBehaviour
{
    public enum Mask
    {
        None,
        Pig,
        Owl,
    }
    public Mask currentMask;

    public Image fillImage;
    public bool canChangeMask;    // ���� ���� ���� ���� (Map_03 -> false)
    [SerializeField] private Image maskImage;
    [SerializeField] private List<Sprite> maskSpriteList;
    [SerializeField] private List<AudioSource> maskChangeAudioList;
    [SerializeField] private int maskNum;           // ���� ����
    private Animator animator;
    
    public Button maskButton;

    private void Awake()
    {
        maskButton = GameObject.Find("MaskButton").GetComponent<Button>();
        animator=GameObject.Find("Player").GetComponent<Animator>();

        maskButton.onClick.AddListener(OnTouchMaskButton);
    }

    public void OnTouchMaskButton()
    {
        if (canChangeMask & CanChangeMask())
        {
            currentMask++;
            if (currentMask == (Mask)maskNum)
                currentMask = 0;
            ChangeMask();
        }
    }

    // J : ���� �ɷ� ��� ���̸� ���� ���� �Ұ���
    private bool CanChangeMask()
    {
        return (!SkillManager.instance.isPigSkilling && !SkillManager.instance.isOwlSkilling);
    }

    private void ChangeMask()
    {
        maskImage.sprite = maskSpriteList[(int)currentMask];

        animator.SetTrigger(currentMask.ToString());
        animator.SetInteger("maskType", (int)currentMask);

        if (maskChangeAudioList[(int)currentMask] != null)
            maskChangeAudioList[(int)currentMask].Play();

        if (currentMask == Mask.Owl)
            fillImage.gameObject.SetActive(true);
        else
            fillImage.gameObject.SetActive(false);
    }
}
