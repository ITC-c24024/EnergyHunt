using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class GameController : MonoBehaviour
{
    private int winnerPlayerNum;

    private int imageNum = 1;

    [SerializeField]
    [Header("�X�P�[����max�ɂȂ�܂ł̎���")]
    float maxTime = 1.5f;

    [SerializeField]
    [Header("SelectUI�̃X�P�[���̃}�b�N�X�l")]
    float maxScale = 1.1f;

    private AnimationCurve animationCurve;

    [SerializeField, Header("���U���gUI")]
    GameObject[] resultImage;

    [SerializeField, Header("���҂�Image")]
    GameObject[] winnerImage;

    [SerializeField, Header("�v���C���[�I�u�W�F�N�g")]
    GameObject[] player;

    //UI�I���̓���
    private InputAction stickAction, buttonAction;


    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip selectSE;

    [SerializeField]
    AudioClip decisionSE;

    // ������
    private void Awake()
    {
        // �f�o�C�X�̕ύX���Ď�
        InputSystem.onDeviceChange += OnDeviceChange;

        // �ŏ��ɐڑ�����Ă���f�o�C�X�ꗗ�����O�o��
        PrintAllDevices();
    }

    void Start()
    {
        Application.targetFrameRate = 60;

        Cursor.visible = false;
       
        // �J�[�u���ݒ肳��Ă��Ȃ��ꍇ�A�f�t�H���g�̃J�[�u���쐬
        if (animationCurve == null || animationCurve.length == 0)
        {
            animationCurve = new AnimationCurve(
                new Keyframe(0f, 0f),    // �X�^�[�g
                new Keyframe(0.5f, 1f), // �X�P�[���A�b�v
                new Keyframe(1f, 0f)   // �X�P�[���_�E��
            );
        }

        StartCoroutine(ScaleLoop(resultImage[imageNum], resultImage[imageNum].transform.localScale));
    }

    private void Update()
    {
        if (stickAction != null && buttonAction != null)
        {
            //Image�؂�ւ�
            var stickAct = stickAction.ReadValue<Vector2>().x;

            if (stickAct < -0.2f && imageNum != 0)
            {
                imageNum = 0;

                audioSource.PlayOneShot(selectSE);

                resultImage[1].SetActive(false);

                StartCoroutine(ScaleLoop(resultImage[imageNum], resultImage[imageNum].transform.localScale));
            }

            if (stickAct > 0.2f && imageNum != 1)
            {
                imageNum = 1;

                audioSource.PlayOneShot(selectSE);

                resultImage[0].SetActive(false);

                StartCoroutine(ScaleLoop(resultImage[imageNum], resultImage[imageNum].transform.localScale));
            }

            //�{�^������
            var buttonAct = buttonAction.triggered;

            if (buttonAct)//A�{�^���@or �Z�{�^������������
            {
                switch (imageNum)
                {
                    case 0:
                        audioSource.PlayOneShot(decisionSE);
                        StartCoroutine(SelectScene("TitleScene"));
                        break;
                    case 1:
                        audioSource.PlayOneShot(decisionSE);
                        StartCoroutine(SelectScene("MainGameScene"));
                        break;
                }
            }
        }

    }

    private IEnumerator SelectScene(string name)
    {
        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene(name);
    }

    //�������v���C���[�̔ԍ����擾
    public void SetWinnerNum(int playerNum)
    {
        winnerPlayerNum = playerNum;

        GetPlayerInput();

        //���U���g��UI��\������
        winnerImage[winnerPlayerNum].SetActive(true);
        resultImage[2].SetActive(true);
        resultImage[3].SetActive(true);
    }

    //�������v���C���[��PlayerInput���擾
    private void GetPlayerInput()
    {
        var input = player[winnerPlayerNum].GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;
        stickAction = actionMap["Move"];
        buttonAction = actionMap["PickUp"];
    }

    //�X�P�[���̊g��A�k�������邽�߂̏���
    IEnumerator ScaleLoop(GameObject tagetUI, Vector3 originalScale)
    {
        resultImage[imageNum].SetActive(true);

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
                tagetUI.transform.localScale = Vector3.Lerp(originalScale, targetScale, scaleFactor);

                yield return null;
            }

            elapsedTime = 0f; // �o�ߎ��Ԃ����Z�b�g
        }
    }

    // �v���C���[�������Ɏ󂯎��ʒm
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        print($"�v���C���[#{playerInput.user.index}�������I");
    }

    // �v���C���[�ގ����Ɏ󂯎��ʒm
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        print($"�v���C���[#{playerInput.user.index}���ގ��I");
    }
    
    // �I������
    private void OnDestroy()
    {
        // �f�o�C�X�ύX�̊Ď�����
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    // �S�Ẵf�o�C�X�����O�o��
    private void PrintAllDevices()
    {
        // �f�o�C�X�ꗗ���擾
        var devices = InputSystem.devices;

        // ���݂̃f�o�C�X�ꗗ�����O�o��
        var sb = new StringBuilder();
        sb.AppendLine("���ݐڑ�����Ă���f�o�C�X�ꗗ");

        for (var i = 0; i < devices.Count; i++)
        {
            sb.AppendLine($" - {devices[i]}");
        }

        print(sb);
    }

    // �f�o�C�X�̕ύX�����m�������̏���
    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                print($"�f�o�C�X {device} ���ڑ�����܂����B");
                break;

            case InputDeviceChange.Disconnected:
                print($"�f�o�C�X {device} ���ؒf����܂����B");
                break;

            case InputDeviceChange.Reconnected:
                print($"�f�o�C�X {device} ���Đڑ�����܂����B");
                break;

            default:
                // �ڑ���ؒf�ȊO�̕ύX�͖���
                return;
        }

        // �ڑ���ؒf���������ꍇ�́A�S�Ẵf�o�C�X���Ăу��O�o��
        PrintAllDevices();
    }
}


