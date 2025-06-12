using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropHoneyManager : MonoBehaviour
{
    //�h���b�v�I�u�W�F�N�g
    [SerializeField] GameObject[] dropObj = new GameObject[3];

    //�h���b�v�I�u�W�F�N�g�̃X�N���v�g
    DropHoney[] dropHoneySC;

    //�g�p����Ă��Ȃ��h���b�v�I�u�W�F�N�g���i�[���邽�߂̃��X�g
    [SerializeField] List<DropHoney> dropList = new List<DropHoney>();

    void Start()
    {
        dropHoneySC = new DropHoney[dropObj.Length];//�I�u�W�F�N�g�̐��ɉ����Ĕz��͈̔͂��w��

        for (int i = 0; i < dropObj.Length; i++)
        {
            dropHoneySC[i] = dropObj[i].GetComponent<DropHoney>();//�h���b�v�I�u�W�F�N�g�̃X�N���v�g���擾

            dropHoneySC[i].SetNum(i);//�e�I�u�W�F�N�g�ɔԍ���t����(���X�g�ɖ߂��ۂɎ��ʂ��邽��)

            dropList.Add(dropHoneySC[i]);//�h���b�v�I�u�W�F�N�g�����X�g�ɒǉ�
        }
    }

    
    public void Drop(int amount)
    {
        //���X�g����g�p����Ă��Ȃ��I�u�W�F�N�g��I��Ńh���b�v
        if(dropList.Count > 0)
        {
            StartCoroutine(dropList[0].SetHoneyAmount(amount));
            dropList.RemoveAt(0);
        }
    }

    //�E��ꂽ�I�u�W�F�N�g�����X�g�ɉ�����
    public void AddDrop(int num)
    {
        dropList.Add(dropHoneySC[num]);
    }
}
