using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyScript : MonoBehaviour
{
    [SerializeField] float power = 450f;//投げる力(消してください)
    [SerializeField] Rigidbody rb;//Honeyのリジッドボディ(消してください)
    [SerializeField] PlayerScript playerScript;
    public bool isThrow = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))//仮でSpaceでやってます（コントローラーに後から対応させてください）
        {
            HoneyThrow();
            isThrow = true;
        }

        if(playerScript.stanTimer > 0.3f)//0.3秒だけisThrowをtrueにする
        {
            isThrow = false;
        }
    }

    //Honeyの飛ぶ力（消してください）
    void HoneyThrow()
    {
        Vector3 force = new Vector3(-power, 0f, 0f);
        rb.AddForce(force);
    }
}
