using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropHoneyManager : MonoBehaviour
{
    //ドロップオブジェクト
    [SerializeField] GameObject[] dropObj = new GameObject[3];

    //ドロップオブジェクトのスクリプト
    DropHoney[] dropHoneySC;

    //使用されていないドロップオブジェクトを格納するためのリスト
    [SerializeField] List<DropHoney> dropList = new List<DropHoney>();

    void Start()
    {
        dropHoneySC = new DropHoney[dropObj.Length];//オブジェクトの数に応じて配列の範囲を指定

        for (int i = 0; i < dropObj.Length; i++)
        {
            dropHoneySC[i] = dropObj[i].GetComponent<DropHoney>();//ドロップオブジェクトのスクリプトを取得

            dropHoneySC[i].SetNum(i);//各オブジェクトに番号を付ける(リストに戻す際に識別するため)

            dropList.Add(dropHoneySC[i]);//ドロップオブジェクトをリストに追加
        }
    }

    
    public void Drop(int amount)
    {
        //リストから使用されていないオブジェクトを選んでドロップ
        if(dropList.Count > 0)
        {
            StartCoroutine(dropList[0].SetHoneyAmount(amount));
            dropList.RemoveAt(0);
        }
    }

    //拾われたオブジェクトをリストに加える
    public void AddDrop(int num)
    {
        dropList.Add(dropHoneySC[num]);
    }
}
