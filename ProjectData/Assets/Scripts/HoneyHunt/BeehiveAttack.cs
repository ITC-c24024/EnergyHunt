using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeehiveAttack : MonoBehaviour
{
    [SerializeField] Beehive beehiveSC;

    [SerializeField] Player[] playerSC;

    [SerializeField] Vector3 tagetScale;

    [SerializeField] int bomberNum;

    public float maxTime = 2f;

    [SerializeField]
    AudioSource audioSource;

    //自爆判定
    public bool isSelf = false;

    void Start()
    {

    }

    void Update()
    {

    }

    //はちみつドロップ
    private IEnumerator DropHoney(GameObject hitPlayer)
    {
        var honeyTank = hitPlayer.GetComponent<HoneyTank>();

        //死亡判定が早いとプレイヤーの死亡関数が呼ばれない為、猶予を設ける
        yield return new WaitForSeconds(0.0f);

        //放電を当てたプレイヤーの死亡判定
        Debug.Log(bomberNum - 1);

        honeyTank.DropHoney();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "P1" || other.gameObject.tag == "P2" || other.gameObject.tag == "P3" || other.gameObject.tag == "P4")
        {
            //放電SE
            //audioSource.Play();
            Debug.Log("atatta");
            StartCoroutine(DropHoney(other.gameObject));
        }

    }

}
