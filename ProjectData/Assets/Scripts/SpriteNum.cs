using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpriteNum : MonoBehaviour
{
    [SerializeField] GameObject[] numObj;//������
    [SerializeField] Sprite[] numSprite = new Sprite[10];//0�`9��Image
    int[] num;//�e�ʂ̐���

    private void Start()
    {
        num = new int[numObj.Length];
    }
    public void ChangeSprite(int num)
    {
        var str = num + "";//�������Q�Ƃ̂��߂�int����string�ɕϊ�

        for (int i = 0; i < numObj.Length; i++)
        {
            if (i < str.Length)
            {
                continue;
            }

            //�X�v���C�g���\��
            numObj[i].GetComponent<Image>().enabled = false;
        }

        //�e�ʂ��Ƃ̃X�v���C�g��ύX
        if (str.Length <= numObj.Length)
        {
            for (int i = 1; i <= str.Length; i++)
            {
                NumCheck(Int32.Parse(Right(num, str.Length - (str.Length - i), str)), i, str);
            }
        }
    }

    //�ʂƂ��̐����ɉ����ăX�v���C�g��ύX
    void NumCheck(int nowNum, int digit, string str)
    {
        //�������ς��ꍇ�̂ݕύX
        if (num[digit - 1] != nowNum)
        {
            //�X�v���C�g��\��
            numObj[digit - 1].GetComponent<Image>().enabled = true;

            var sprite = numObj[digit - 1].GetComponent<Image>();

            sprite.sprite = numSprite[nowNum];

            num[digit - 1] = nowNum;
        }

        if (Int32.Parse(str.Substring(0, 1)) <= 0 && str.Length > 1)
        {
            if (num[digit - 1] <= 0)
            {
                //�X�v���C�g���\��
                numObj[digit - 1].GetComponent<Image>().enabled = false;
            }
        }
    }

    //���l(count)�̉E���牽�Ԗ�(len)�̐���Ԃ�
    string Right(int count, int len, string str)
    {
        if (str.Length <= len - 1)
        {
            return str;
        }
        return str.Substring(str.Length - len, 1);
    }
}
