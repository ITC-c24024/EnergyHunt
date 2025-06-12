using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]

public class PoseScript : MonoBehaviour
{
    [SerializeField, Header("�|�[�Y���")]
    GameObject poseImage;

    [SerializeField, Header("�|�[�YUI")]
    GameObject[] poseUI;


    //UI�؂�ւ��ϐ�
    private int uiNum = 0;


    float maxScale = 1.1f;

    float maxTime = 1f;


    //�|�[�Y��ʃA�N�V����
    private InputAction stickAction, selectAction, poseAction;


    private AnimationCurve animationCurve;


    private Coroutine currentCoroutine;


    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip selectSE;

    [SerializeField]
    AudioClip decisionSE;


    void Start()
    {
        //�v���C���[��ActionMap���擾
        var input = GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;

        stickAction = actionMap["Move"];
        selectAction = actionMap["PickUp"];
        poseAction = actionMap["PutIn"];

        animationCurve = new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(0.5f, 1f),
            new Keyframe(1f, 0f));

        poseUI[uiNum].SetActive(true);
        StartAnimationForScene();
    }

    void Update()
    {
        //�|�[�Y��ʂɈړ�
        
        var poseAct = poseAction.triggered;
        
        if (poseAct)
        {
            //�����Ȃ�����
            Time.timeScale = 0;

            poseImage.SetActive(true);
        }
        

        //����
        var selectAct = selectAction.triggered;

        if (selectAct && Time.timeScale == 0)
        {
            switch (uiNum)
            {
                case 0:
                    audioSource.PlayOneShot(decisionSE);

                    Invoke("DeletePanel", 0.3f);

                    //������悤�ɂ���
                    Time.timeScale = 1;
                    break;
                case 1:
                    audioSource.PlayOneShot(decisionSE);
                    //������悤�ɂ���
                    Time.timeScale = 1;
                    Invoke("SelectTitle", 0.3f);
                    break;
            } 
        }


        //UI�؂�ւ�
        var stickAct = stickAction.ReadValue<Vector2>().y;

        if (stickAct > 0 && Time.timeScale == 0 && uiNum != 0)
        {
            poseUI[uiNum].SetActive(false);

            uiNum = 0;

            audioSource.PlayOneShot(selectSE);

            StartAnimationForScene();

            poseUI[uiNum].SetActive(true);
        }

        if (stickAct < 0 && Time.timeScale == 0 && uiNum != 1)
        {
            poseUI[uiNum].SetActive(false);

            uiNum = 1;
            audioSource.PlayOneShot(selectSE);

            StartAnimationForScene();

            poseUI[uiNum].SetActive(true);
        }
    }

    private void DeletePanel()
    {
        poseImage.SetActive(false);
    }

    private void SelectTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    void StartAnimationForScene()
    {
        // ���݂̃R���[�`�����~
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);

            // ���݂�UI�̃X�P�[�������ɖ߂�
            GameObject currentUI = poseUI[uiNum];
            currentUI.transform.localScale = new Vector3(0.3f, 0.3f, 1);  // ���̃X�P�[���Ƀ��Z�b�g
        }

        // �I�����ꂽ UI �̏����X�P�[�����擾
        GameObject targetUI = poseUI[uiNum];
        Vector3 originalScale = targetUI.transform.localScale;

        // �V�����R���[�`�����J�n
        currentCoroutine = StartCoroutine(SelectUIScaleLoop(targetUI, originalScale));
    }

    //Scale��Up��Down�̏���
    IEnumerator SelectUIScaleLoop(GameObject targetUI, Vector3 originalScale)
    {
        float elapsedTime = 0f;
        Vector3 targetScale = originalScale * maxScale;

        //�������[�v
        while (true)
        {
            while (elapsedTime < maxTime)
            {
                elapsedTime += Time.unscaledDeltaTime;

                // �J�[�u�ɏ]�����X�P�[���l���v�Z
                float t = elapsedTime / maxTime;
                float scaleFactor = animationCurve.Evaluate(t);
                targetUI.transform.localScale = Vector3.Lerp(originalScale, targetScale, scaleFactor);

                yield return null;
            }

            elapsedTime = 0f; // �o�ߎ��Ԃ����Z�b�g
        }
    }
}
