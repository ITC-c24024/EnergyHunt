using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BarrierScript : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;

    [SerializeField, Header("�o���A�G�t�F�N�g")]
    GameObject barrierEffect;

    [SerializeField, Header("�o���A���ʎ���")]
    float barrierTime;
    //�o���A�̃^�C�}�[
    private float barrierTimer;
    //�o���A�����̔���
    public bool isBarrier = false;

    [SerializeField, Header("�N�[���^�C��")]
    float coolTime;
    //�N�[���^�C���̃^�C�}�[
    private float coolTimer;
    //�N�[���^�C�������̔���
    private bool isCoolTime = false;


    private bool canBarrier = true;

    [SerializeField] GameObject player;

    private InputAction barrierAction;

    [SerializeField] AudioSource audioSource;


    void Start()
    {
        var input = player.GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;
        barrierAction = actionMap["Barrier"];

        //audioSource.GetComponent<AudioSource>();
    }

    void Update()
    {
        var barrierAct = barrierAction.triggered;

        if (barrierAct && canBarrier && !playerController.haveBomb[0] && !playerController.haveBomb[1] && !playerController.isDead)
        {
            audioSource.Play();

            isBarrier = true;
            canBarrier = false;

            barrierEffect.SetActive(true);
            GetComponent<SphereCollider>().enabled = true;
        }

        //�o���A�̌��ʎ���
        if (isBarrier)
        {
            barrierTimer += Time.deltaTime;

            if (barrierTimer > barrierTime)
            {
                barrierTimer = 0;

                GetComponent<SphereCollider>().enabled = false;
                barrierEffect.SetActive(false);

                isBarrier = false;

                isCoolTime = true;
            }
        }

        //���Ɏg����܂ł̃N�[���^�C��
        if (isCoolTime)
        {
            coolTimer += Time.deltaTime;

            if (coolTimer > coolTime)
            {
                coolTimer = 0;

                isCoolTime = false;
                canBarrier = true;
            }
        }
    }

    public void BarrierCoolTime()
    {
        GetComponent<SphereCollider>().enabled = false;
        barrierEffect.SetActive(false);

        isBarrier = false;
        barrierTimer = 0;
        canBarrier = false;
        
        isCoolTime = true;
    }

    public void CoolTimeReset()
    {
        barrierEffect.SetActive(false);

        isBarrier = false;
        barrierTimer = 0;
        canBarrier = true;

        isCoolTime = false;
    }
}
