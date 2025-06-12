using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyScript : MonoBehaviour
{
    [SerializeField] float power = 450f;//�������(�����Ă�������)
    [SerializeField] Rigidbody rb;//Honey�̃��W�b�h�{�f�B(�����Ă�������)
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
        if (Input.GetKeyDown(KeyCode.Space))//����Space�ł���Ă܂��i�R���g���[���[�Ɍォ��Ή������Ă��������j
        {
            HoneyThrow();
            isThrow = true;
        }

        if(playerScript.stanTimer > 0.3f)//0.3�b����isThrow��true�ɂ���
        {
            isThrow = false;
        }
    }

    //Honey�̔�ԗ́i�����Ă��������j
    void HoneyThrow()
    {
        Vector3 force = new Vector3(-power, 0f, 0f);
        rb.AddForce(force);
    }
}
