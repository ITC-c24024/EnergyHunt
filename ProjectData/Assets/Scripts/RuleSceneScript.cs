using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]

public class RuleSceneScript : MonoBehaviour
{
    bool first;

    [SerializeField] GameObject[] image;

    int select = 0;

    InputAction decideAction, cancelAction, upAction, downAction, stickAction;

    private float timer;

    private bool isTimer = false;

    private AnimationCurve animationCurve;

    private Coroutine currentCoroutine;

    float maxScale = 1.1f;

    float maxTime = 1f;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip selectSE;

    [SerializeField]
    AudioClip decisionSE;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        Cursor.visible = false;

        var input = GetComponent<PlayerInput>();

        var actionMap = input.currentActionMap;

        decideAction = actionMap["PickUp"];
        cancelAction = actionMap["Barrier"];
        //upAction = actionMap["Up"];
        //downAction = actionMap["Down"];
        stickAction = actionMap["Move"];

        animationCurve = new AnimationCurve(
            new Keyframe(0f,0f),
            new Keyframe(0.5f,1f),
            new Keyframe(1f,0f));
        
        image[select].SetActive(true);
        StartAnimationForScene();
    }

    // Update is called once per frame
    void Update()
    {
        var stickAct = stickAction.ReadValue<Vector2>().y;

        if (stickAct > 0.2 && !isTimer)
        {
            isTimer = true;

            SelectChange(true);
        }
        if (stickAct < -0.2 && !isTimer)
        {
            isTimer = true;

            SelectChange(false);
        }

        if (decideAction.triggered)
        {
            Debug.Log("�V�[��");
            switch (select)
            {
                case 0:
                    audioSource.PlayOneShot(decisionSE);
                    StartCoroutine(SelectScene("RuleScene_Main"));

                    break;

                case 1:
                    audioSource.PlayOneShot(decisionSE);
                    StartCoroutine(SelectScene("RuleScene_Action"));

                    break;

                case 2:
                    audioSource.PlayOneShot(decisionSE);
                    StartCoroutine(SelectScene("TitleScene"));

                    break;
            }

            
        }
        /*
        if (cancelAction.triggered)
        {
            SceneManager.LoadScene("TitleScene");
        }
        */
        if (isTimer)
        {
            timer += Time.deltaTime;

            if (timer > 0.2)
            {
                isTimer = false;

                timer = 0;
            }
        }
    }

    private IEnumerator SelectScene(string name)
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(name);
    }


    void SelectChange(bool up)
    {
        image[select].SetActive(false);

        if (up)
        {
            if (select > 0)
            {
                select--;

                audioSource.PlayOneShot(selectSE);

                StartAnimationForScene();
            }
        }
        else
        {
            if (select < image.Length - 1)
            {
                select++;

                audioSource.PlayOneShot(selectSE);

                StartAnimationForScene();
            }
        }
        
        image[select].SetActive(true);
    }

    void StartAnimationForScene()
    {
        // ���݂̃R���[�`�����~
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);

            // ���݂�UI�̃X�P�[�������ɖ߂�
            GameObject currentUI = image[select];
            currentUI.transform.localScale = Vector3.one;  // ���̃X�P�[���Ƀ��Z�b�g
        }

        // �I�����ꂽ UI �̏����X�P�[�����擾
        GameObject targetUI = image[select];
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
                elapsedTime += Time.deltaTime;

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
