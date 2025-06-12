using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DropEnergyTank : MonoBehaviour
{
    [SerializeField]
    EnergyTank[] energyTank;

    [SerializeField]
    GameObject[] explotion;

    [SerializeField]
    GameObject[] text;

    [SerializeField, Header("���[�^�[�I�u�W�F�N�g")]
    GameObject meter;

    //�^���N�̃G�l���M�[��
    [SerializeField]
    private int value;

    [SerializeField, Header("�}�b�N�X�e��")]
    int maxValue;

    [SerializeField, Header("�R�A�I�u�W�F�N�g")]
    GameObject[] core;
    [SerializeField, Header("�g���C���I�u�W�F�N�g")]
    GameObject[] trall;
    [SerializeField, Header("�g���C���̈ړ��X�s�[�h")]
    float speed;
    //�g���C���I�u�W�F�N�g�̏����ʒu
    private Vector3 startPos;
    //�C�x���g�̉�
    private int eventNum;
    //�C�x���g���̔���
    private bool isEvent0 = false;
    private bool isEvent1 = false;

    void Start()
    {
        startPos = trall[0].transform.localPosition;
    }

    void Update()
    {
        //�C�x���g������������R�A�Ɍ������ăG�l���M�[���΂�
        if (isEvent0)
        {
            trall[0].transform.position = Vector3.MoveTowards(trall[0].transform.position, core[0].transform.position, speed);

            if(trall[0].transform.position== core[0].transform.position)
            {
                isEvent0 = false;

                trall[0].SetActive(false);
                trall[0].transform.localPosition = startPos;
            }
        }

        if (isEvent1)
        {
            trall[1].transform.position = Vector3.MoveTowards(trall[1].transform.position, core[1].transform.position, speed);

            if (trall[1].transform.position == core[1].transform.position)
            {
                isEvent1 = false;

                trall[1].SetActive(false);
                trall[1].transform.localPosition = startPos;
            }
        }
    }

    public void ScaleChange(int energy)
    {
        if (value == 0)
        {
            meter.SetActive(true);
        }

        //value += energy / (eventNum + 1) * 2;
        value += energy * 2;
        if (value > maxValue)
        {
            value = maxValue;
        }

        meter.transform.localScale = new Vector3(meter.transform.localScale.x, meter.transform.localScale.y, value * 0.009f);
        meter.transform.localPosition = new Vector3(meter.transform.localPosition.x, meter.transform.localPosition.y, 4.03f - (value * 0.0046f));

        if (value == maxValue && eventNum < 5)
        {
            HappenEvent();
        }
    }

    private void HappenEvent()
    {
        StartCoroutine(SetText(0));

        isEvent0 = true;
        isEvent1 = true;

        trall[0].SetActive(true);
        trall[1].SetActive(true);

        for (int i = 0; i < energyTank.Length; i++)
        {
            //�^���N�̃`���[�W�X�s�[�h�𑬂�����
            energyTank[i].chargeSpeed *= 2;
        }

        //�R�A��傫������
        core[0].GetComponent<BombScript>().CoreScaleChange();
        core[1].GetComponent<BombScript>().CoreScaleChange();

        //���d�͈͂��L������
        explotion[0].GetComponent<ExplosionScript>().ChangeRange();
        explotion[1].GetComponent<ExplosionScript>().ChangeRange();
    }

    private IEnumerator SetText(int i)
    {
        //text[i].SetActive(true);

        //�G�l���M�[���Z�b�g
        value = 0;
        ScaleChange(0);

        yield return new WaitForSeconds(2);

        text[i].SetActive(false);

        eventNum++;
    }
}
