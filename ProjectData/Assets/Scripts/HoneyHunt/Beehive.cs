using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Beehive : MonoBehaviour
{
    [SerializeField] BeehiveAttack beehiveAttackSC;

    public GameObject attackRange;

    [Header("体のオブジェクト"), SerializeField]
    GameObject bodyObj;

    [Header("頭追従用オブジェクト"), SerializeField]
    GameObject headFollow;

    //体が出ているかの判定
    public bool outBody = false;

    [Header("殴りの当たり判定"), SerializeField]
    GameObject PunchCollider;

    [SerializeField, Header("放電エフェクト")]
    GameObject effect;

    [SerializeField] MeshRenderer mesh;
    [SerializeField] SphereCollider sphere;

    private Rigidbody beehiveRb;

    [SerializeField] Vector3 startPos;

    [Header("ノックバックさせる力"), SerializeField]
    float knockbackPower = 100;

    //ハチの巣の衝突位置
    Vector3 collisionPos;

    [SerializeField] int beehiveNum;

    //爆発中かの判定
    public bool isExplosion = false;

    //投げられているかの判定
    public bool isThrow = false;

    public bool isTimer;

    [Header("みつ太郎のAnimator"), SerializeField]
    Animator animator;

    void Start()
    {

        transform.localPosition = startPos;

        beehiveRb = gameObject.GetComponent<Rigidbody>();


    }

    void Update()
    {
        if (!isExplosion)
        {
            attackRange.transform.position = transform.position;
        }

        //みつ太郎が出るとき
        if (outBody)
        {
            //頭を追従用オブジェクトに追従
            transform.position = new Vector3(
                headFollow.transform.position.x,
                headFollow.transform.position.y + 0.5f,
                headFollow.transform.position.z
                );
            transform.rotation = headFollow.transform.rotation;
        }
    }

    public void IsKinematicTrue()
    {
        beehiveRb.isKinematic = true;
    }

    public void SetIsThrow(bool set)
    {
        isThrow = set;
    }

    //爆弾を消して、爆発エフェクトを表示する
    public void Explosion()
    {
        //放電エフェクトを表示
        effect.SetActive(true);
        //放電のコライダーをオン
        attackRange.GetComponent<SphereCollider>().enabled = true;

        isExplosion = true;

        isTimer = false;

        beehiveRb.isKinematic = false;

        sphere.enabled = true;

        StartCoroutine(ExplosionScaleReset(1.0f));
    }

    //爆発エフェクトを非表示
    public IEnumerator ExplosionScaleReset(float waitSecond)
    {
        yield return new WaitForSeconds(waitSecond);


        //放電のコライダーを消す
        attackRange.GetComponent<SphereCollider>().enabled = false;

        //放電エフェクトを非表示
        effect.SetActive(false);

        isExplosion = false;

    }

    //ボムのポジションをリセット
    public IEnumerator PosReset(float waitSecond)
    {
        beehiveRb.isKinematic = true;

        yield return new WaitForSeconds(waitSecond * 2);

        //放電のコライダーを消す
        attackRange.GetComponent<SphereCollider>().enabled = false;

        //放電エフェクトを非表示
        effect.SetActive(false);


        yield return new WaitForSeconds(waitSecond);

        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.localPosition = startPos;

        //視覚化
        mesh.enabled = true;
        sphere.enabled = true;

        beehiveRb.isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //プレイヤーに当たったとき
        if (isThrow && collision.gameObject.tag.StartsWith("P"))
        {
            Knockback(collision.gameObject);
        }

        //他のオブジェクトに当たったら爆発させる
        if (isThrow && (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Ground"))
        {
            SetIsThrow(false);

            //体を出す
            HoneyBody();    

            //Explosion();
        }

        //ステージ外に落ちた場合、スポーンポジションに戻す
        if (collision.gameObject.tag == "Reset" || collision.gameObject.tag == "OutSide")
        {
            SetIsThrow(false);

            StartCoroutine(PosReset(0));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Explosion0" || other.gameObject.tag == "Explosion1")
        {
            SetIsThrow(false);

            Explosion();
        }
    }

    //プレイヤーをノックバックさせる
    void Knockback(GameObject hitObj)
    {
        Debug.Log("ノックバック");
        //プレイヤーとハチの巣の距離を計算し、速度ベクトルを算出
        Vector3 distination = (hitObj.transform.position - transform.position).normalized;

        //力を加える
        Rigidbody rb = hitObj.GetComponent<Rigidbody>();
        rb.AddForce(distination * knockbackPower, ForceMode.VelocityChange);
    }

    //ハチの巣から体を出す
    void HoneyBody()
    {
        outBody = true;

        //ハチの巣の角度
        var headAngle = transform.eulerAngles.y;  

        //ハチの巣を動かなくする
        beehiveRb.isKinematic = true;
        beehiveRb.useGravity = false;

        

        //体を表示
        bodyObj.SetActive(true);

        //体追従
        bodyObj.transform.position = new Vector3(
            transform.position.x,
            bodyObj.transform.position.y,
            transform.position.z
            );
        //体の向きを頭に合わせる
        bodyObj.transform.rotation = Quaternion.Euler(
            bodyObj.transform.rotation.x,
            headAngle,//ハチの巣の向き
            bodyObj.transform.rotation.z
            );


        //殴る
        animator.SetBool("IsHit", true);
        Invoke("StopAnim", 1f);

        //殴りの当たり判定を出す
        Invoke("SetCollider", 1.2f);
        //殴り判定を消す
        Invoke("SetCollider", 1.8f);

        //ハチの巣を取れるようにする
        Invoke("ResetBeeHive", 3.0f);
    }

    //殴りアニメーションを止める
    void StopAnim()
    {
        animator.SetBool("IsHit", false);
    }

    //殴りの当たり判定を設定
    void SetCollider()
    {
        bool isPunch = !PunchCollider.activeSelf;
        PunchCollider.SetActive(isPunch);
    }

    //ハチの巣を取れるようにする
    void ResetBeeHive()
    {
        //体を非表示
        bodyObj.SetActive(false);

        //ハチの巣が動くようにする
        beehiveRb.isKinematic = false;
        beehiveRb.useGravity = true;

        outBody = false;
    }

    public void ColliderEnabled(bool a)
    {
        sphere.enabled = a;
    }

    public void IsTimerTrue()
    {
        //タイマースタート
        isTimer = true;
    }
}
