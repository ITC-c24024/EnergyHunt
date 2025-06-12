using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catch : MonoBehaviour
{
    [SerializeField] Player playerSC;

    [SerializeField] TakeBeehive takeBeehive;

    //�N�[���^�C��
    private float timer;

    //�N�[���^�C������
    private bool isTimer = false;

    void Start()
    {

    }

    void Update()
    {
        //����������A���M�ŃL���b�`�ł��Ȃ��悤�ɂ���
        if (isTimer)
        {
            timer += Time.deltaTime;

            if (timer > 0.2f)
            {
                isTimer = false;

                timer = 0;
            }
        }
    }

    public void TimerStart()
    {
        isTimer = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bomb0" && !playerSC.haveBeehive[0] && !playerSC.haveBeehive[1] && !isTimer)
        {
            if (other.GetComponent<Beehive>().isThrow)
            {
                Debug.Log("�L���b�`");
                takeBeehive.CanTakeChange(0, true);
            }
        }

        if (other.gameObject.tag == "Bomb1" && !playerSC.haveBeehive[0] && !playerSC.haveBeehive[1] && !isTimer)
        {
            if (other.GetComponent<Beehive>().isThrow)
            {
                Debug.Log("�L���b�`");
                takeBeehive.CanTakeChange(1, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Bomb0")
        {
            takeBeehive.CanTakeChange(0, false);
        }

        if (other.gameObject.tag == "Bomb1")
        {
            takeBeehive.CanTakeChange(1, false);
        }
    }
}
