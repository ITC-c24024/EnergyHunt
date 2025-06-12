using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackScript : MonoBehaviour
{
    [Header("�g���b�N�I�u�W�F�N�g"), SerializeField,]
    GameObject trackObj;

    [Header("�g���b�N�̒�~�ʒu"), SerializeField]
    Vector3 stopPos;

    [Header("�g���b�N�̏o���ʒu"), SerializeField]
    Vector3 startPos;

    [Header("�g���b�N�̖ڕW�ʒu"), SerializeField]
    Vector3 targetPos;
    
    //�g���b�N�̈ړ�����
    public bool isMove = false;

    //�g���b�N�o���܂ł̎���
    [SerializeField] int waitTimeMin,waitTimeMax;

    private Animator animator;

    void Start()
    {
        //�o���ʒu�Ɉړ�
        trackObj.transform.localPosition = startPos;

        StartCoroutine(StartMove());

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    //�g���b�N���~�ʒu�܂ňړ�
    private IEnumerator StartMove()
    {
        //�g���b�N�ړ���
        isMove = true;

        //�ړ��ɂ�����b��
        float moveTime = 2;
        //�o�ߎ���
        float time = 0f;
        
        //�o���ʒu�����~�ʒu�܂ňړ�
        while (time < moveTime)
        {
            time += Time.deltaTime;
            float moveX = Mathf.Sin(Mathf.Lerp(0, 90, time / moveTime) * 3.14f / 180);

            trackObj.transform.localPosition = Vector3.Lerp(startPos, stopPos, moveX);
            

            yield return null;
        }


        //�ړ��I��
        isMove = false;

        animator.SetBool("IsStop", true);

        yield return new WaitForSeconds(Random.Range(waitTimeMin,waitTimeMax));

        StartCoroutine(MoveTrack());
    }

    //�g���b�N���ړ�
    public IEnumerator MoveTrack()
    {
        animator.SetBool("IsStop", false);

        //�g���b�N�ړ���
        isMove = true;

        //�ړ��ɂ�����b��
        float moveTime = 2;
        //�ړ�����
        float time = 0;
        
        //��~�ʒu����ڕW�ʒu�܂ňړ�
        while (time < moveTime)
        {
            time += Time.deltaTime;
            float moveX = Mathf.Sin(Mathf.Lerp(270, 360, time / moveTime) * 3.14f / 180) + 1;

            trackObj.transform.localPosition = Vector3.Lerp(stopPos, targetPos, moveX);


            yield return null;
        }
        time = 0;

        yield return new WaitForSeconds(Random.Range(0, 15));


        //�o���ʒu�����~�ʒu�܂ňړ�
        while (time < moveTime)
        {
            time += Time.deltaTime;

            float moveX = Mathf.Sin(Mathf.Lerp(0, 90, time / moveTime) * 3.14f / 180);

            trackObj.transform.localPosition = Vector3.Lerp(startPos, stopPos, moveX);


            yield return null;
        }

        animator.SetBool("IsStop", true);

        //�ړ��I��
        isMove = false;

        yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));

        StartCoroutine(MoveTrack());
    }
}
