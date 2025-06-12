using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResultScript : MonoBehaviour
{
    [SerializeField]
    [Header("�X�P�[����max�ɂȂ�܂ł̎���")]
    float maxTime = 1.5f;

    [SerializeField]
    [Header("SelectUI�̃X�P�[���̃}�b�N�X�l")]
    float maxScale = 1.1f;

    [SerializeField]
    [Header("�V�[���̔ԍ�(0 or 1)")]
    int sceneNum = 0;
    
    //�X�P�[����curve
    private AnimationCurve animationCurve;

    [SerializeField]
    [Header("�X�P�[����������UI")]
    GameObject[] ui;

    [SerializeField]
    [Header("�����v���C���[UI")]
    GameObject[] playerUI;

    [SerializeField]
    GameObject[] text;
    
    private Vector3 playerUIMaxScale;

    private PlayerControll inputActions;
    private InputAction stickAction;//�X�e�B�b�N����
    private InputAction buttonAction;

    private Vector3 playerUIOriginalScale;

    private float scaleUpMaxTime = 0.8f;//PlayerUI���g�傷��maxTime

    private Coroutine coroutine;//�R�[���`�������邽�߂̂���


    private void Awake()
    {
        

        inputActions = new PlayerControll();
        inputActions.Enable();

        stickAction = inputActions.Player.Move;
    }


    void Start()
    {
        //text[GameController.winnerPlayerNum].SetActive(true);
        //Debug.Log(GameController.winnerPlayerNum);
        //animationCurve���ݒ肳��ĂȂ��ꍇ�AanimationCurve���Z�b�g����
        if (animationCurve == null || animationCurve.length == 0)
        {
            animationCurve = new AnimationCurve(
                new Keyframe(0f, 0f),
                new Keyframe(0.5f, 1f),
                new Keyframe(1f, 0f)
            );
        }

        playerUIMaxScale = new Vector3(1,1,1);

        StartAnim();//�ŏ��̃R�[���`�����X�^�[�g
    }


    void Update()
    {
        float horizontalStick = stickAction.ReadValue<Vector2>().x;//�������̃X�e�B�b�N���͂��擾

        if (sceneNum < 1)
        {
            if (horizontalStick > 0)
            {
                sceneNum++;
                StartAnim();//�R�[���`����؂�ւ�
            }
        }
        if (sceneNum > 0)
        {
            if (horizontalStick < 0)
            {
                sceneNum--;
                StartAnim();//�R�[���`����؂�ւ�
            }
        }

        if (inputActions.Player.Throw.triggered)//A�{�^���@or �Z�{�^������������
        {
            switch (sceneNum)
            {
                case 0:
                    SceneManager.LoadScene("TitleScene");
                    break;
                case 1:
                    SceneManager.LoadScene("MainGameScene");
                    break;
            }
        }

        //�g��A�k������UI��SetActive����
        for (int i = 0; i < ui.Length; i++)
        {
            if (i == sceneNum)
            {
                ui[i].SetActive(true);
            }
            else
            {
                ui[i].SetActive(false);
            }
        }

        if(scaleUpMaxTime < 2)
        {
            scaleUpMaxTime += Time.deltaTime;
        }

        if(scaleUpMaxTime > 2)
        {
            SetUI();
        }
    }

    private void SetUI()
    {
        //���҂�UI��\��
        for (int i = 0; i < playerUI.Length; i++)
        {
            //if (GameController.winnerPlayerNum == i + 1)
            {
                //Debug.Log(GameController.winnerPlayerNum);
                playerUI[i].SetActive(true);
                playerUIOriginalScale = playerUI[i].transform.localScale;
                StartCoroutine(PlayerUIScaleUp());
            }
        }
    }

    //�R�[���`���̐؂�ւ�����
    void StartAnim()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);//���̃R�[���`�����~�߂�

            GameObject currentUI = ui[sceneNum];
            currentUI.transform.localScale = Vector3.one;//UI�̑傫�������Z�b�g����
        }

        GameObject tagetUI = ui[sceneNum];//ScaleLoop��tagetUI���擾
        Vector3 originalScale = tagetUI.transform.localScale;//ScaleLoop��originalScale���擾

        coroutine = StartCoroutine(ScaleLoop(tagetUI, originalScale));//�R�[���`�����Z�b�g
    }


    //�X�P�[���̊g��A�k�������邽�߂̏���
    IEnumerator ScaleLoop(GameObject tagetUI,Vector3 originalScale)
    {
        float timer = 0f;//maxTime�܂ł̎��Ԃ��v�Z����^�C�}�[
        Vector3 tagetScale = originalScale * maxScale;

        //�������[�v
        while (true)
        {
            while(timer < maxTime)
            {
                timer += Time.deltaTime;
                float t = timer / maxTime;
                float scaleCurve = animationCurve.Evaluate(t);//animationCurve�̎��Ԃ̌v�Z
                tagetUI.transform.localScale = Vector3.Lerp(originalScale, tagetScale, scaleCurve);//�X�P�[���̊g��A�k������

                yield return null;
            }
            timer = 0f;
        }
    }

    //�������v���C���[��UI���X�P�[���A�b�v����
    IEnumerator PlayerUIScaleUp()
    {
        float timer = 0f;
        Vector3 tagetScale = new Vector3(1, 1, 1);

        while(timer < scaleUpMaxTime)
        {
            timer += Time.deltaTime;
            float t = timer / scaleUpMaxTime;
            for(int i = 0;i < playerUI.Length; i++)
            {
                playerUI[i].transform.localScale = Vector3.Lerp(playerUIOriginalScale, tagetScale, t);
            }

            yield return null;
        }
        for (int i = 0; i < playerUI.Length; i++)
        {
            playerUI[i].transform.localScale = tagetScale;
        }
    }
}
