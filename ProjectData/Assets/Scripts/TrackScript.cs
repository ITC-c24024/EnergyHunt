using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackScript : MonoBehaviour
{
    [Header("トラックオブジェクト"), SerializeField,]
    GameObject trackObj;

    [Header("トラックの停止位置"), SerializeField]
    Vector3 stopPos;

    [Header("トラックの出発位置"), SerializeField]
    Vector3 startPos;

    [Header("トラックの目標位置"), SerializeField]
    Vector3 targetPos;
    
    //トラックの移動判定
    public bool isMove = false;

    //トラック出発までの時間
    [SerializeField] int waitTimeMin,waitTimeMax;

    private Animator animator;

    void Start()
    {
        //出発位置に移動
        trackObj.transform.localPosition = startPos;

        StartCoroutine(StartMove());

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    //トラックを停止位置まで移動
    private IEnumerator StartMove()
    {
        //トラック移動中
        isMove = true;

        //移動にかかる秒数
        float moveTime = 2;
        //経過時間
        float time = 0f;
        
        //出発位置から停止位置まで移動
        while (time < moveTime)
        {
            time += Time.deltaTime;
            float moveX = Mathf.Sin(Mathf.Lerp(0, 90, time / moveTime) * 3.14f / 180);

            trackObj.transform.localPosition = Vector3.Lerp(startPos, stopPos, moveX);
            

            yield return null;
        }


        //移動終了
        isMove = false;

        animator.SetBool("IsStop", true);

        yield return new WaitForSeconds(Random.Range(waitTimeMin,waitTimeMax));

        StartCoroutine(MoveTrack());
    }

    //トラックを移動
    public IEnumerator MoveTrack()
    {
        animator.SetBool("IsStop", false);

        //トラック移動中
        isMove = true;

        //移動にかかる秒数
        float moveTime = 2;
        //移動時間
        float time = 0;
        
        //停止位置から目標位置まで移動
        while (time < moveTime)
        {
            time += Time.deltaTime;
            float moveX = Mathf.Sin(Mathf.Lerp(270, 360, time / moveTime) * 3.14f / 180) + 1;

            trackObj.transform.localPosition = Vector3.Lerp(stopPos, targetPos, moveX);


            yield return null;
        }
        time = 0;

        yield return new WaitForSeconds(Random.Range(0, 15));


        //出発位置から停止位置まで移動
        while (time < moveTime)
        {
            time += Time.deltaTime;

            float moveX = Mathf.Sin(Mathf.Lerp(0, 90, time / moveTime) * 3.14f / 180);

            trackObj.transform.localPosition = Vector3.Lerp(startPos, stopPos, moveX);


            yield return null;
        }

        animator.SetBool("IsStop", true);

        //移動終了
        isMove = false;

        yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));

        StartCoroutine(MoveTrack());
    }
}
