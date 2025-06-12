using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpriteNum : MonoBehaviour
{
    [SerializeField] GameObject[] numObj;//桁数分
    [SerializeField] Sprite[] numSprite = new Sprite[10];//0～9のImage
    int[] num;//各位の数字

    private void Start()
    {
        num = new int[numObj.Length];
    }
    public void ChangeSprite(int num)
    {
        var str = num + "";//文字数参照のためにintからstringに変換

        for (int i = 0; i < numObj.Length; i++)
        {
            if (i < str.Length)
            {
                continue;
            }

            //スプライトを非表示
            numObj[i].GetComponent<Image>().enabled = false;
        }

        //各位ごとのスプライトを変更
        if (str.Length <= numObj.Length)
        {
            for (int i = 1; i <= str.Length; i++)
            {
                NumCheck(Int32.Parse(Right(num, str.Length - (str.Length - i), str)), i, str);
            }
        }
    }

    //位とその数字に応じてスプライトを変更
    void NumCheck(int nowNum, int digit, string str)
    {
        //数字が変わる場合のみ変更
        if (num[digit - 1] != nowNum)
        {
            //スプライトを表示
            numObj[digit - 1].GetComponent<Image>().enabled = true;

            var sprite = numObj[digit - 1].GetComponent<Image>();

            sprite.sprite = numSprite[nowNum];

            num[digit - 1] = nowNum;
        }

        if (Int32.Parse(str.Substring(0, 1)) <= 0 && str.Length > 1)
        {
            if (num[digit - 1] <= 0)
            {
                //スプライトを非表示
                numObj[digit - 1].GetComponent<Image>().enabled = false;
            }
        }
    }

    //数値(count)の右から何番目(len)の数を返す
    string Right(int count, int len, string str)
    {
        if (str.Length <= len - 1)
        {
            return str;
        }
        return str.Substring(str.Length - len, 1);
    }
}
