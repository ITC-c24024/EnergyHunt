using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    private MeshRenderer material;

    [SerializeField]
    Material[] blockMat; 

    [SerializeField, Header("��������܂ł̎���")]
    float setTime;

    private int blockHp = 2;

    private float timer;

    private bool isTimer = false;

    //�u���b�N�����Ă��邩�̔���
    public bool isBroken = false;
    

    void Start()
    {
        material= gameObject.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        //�����܂ł̃N�[���^�C��
        if (isTimer)
        {
            timer += Time.deltaTime;

            if (timer > setTime)
            {
                timer = 0;

                blockHp = 2;

                //�u���b�N�̕\���ؑ�
                material.material = blockMat[0];
                GetComponent<MeshRenderer>().enabled = true;
                GetComponent<BoxCollider>().isTrigger = false;

                isBroken = false;

                //�^�C�}�[�؂�ւ�
                isTimer = false;
            }
        }
    }

    
    public void SetBlock(bool blockBool)
    {
        blockHp--;
        if (blockHp == 1)
        {
            material.material = blockMat[1];
        }
       
        if (blockHp == 0)
        {
            //�u���b�N�̕\���ؑ�
            GetComponent<MeshRenderer>().enabled = blockBool;
            GetComponent<BoxCollider>().isTrigger = !blockBool;

            isBroken = !blockBool;

            //�^�C�}�[�؂�ւ�
            isTimer = !blockBool;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="P1"|| other.gameObject.tag == "P2" || other.gameObject.tag == "P3" || other.gameObject.tag == "P4"
            || other.gameObject.tag == "Bomb0" || other.gameObject.tag == "Bomb1")
        {
            //Debug.Log("hoge");
            isTimer = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "P1" || other.tag == "P2" || other.tag == "P3" || other.tag == "P4" 
            || other.gameObject.tag == "Bomb0" || other.gameObject.tag == "Bomb1")
        {
            isTimer = true;
        }
    }
}
