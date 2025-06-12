using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField, Header("GameController�X�N���v�g")]
    GameController gameController;

    private GameObject player;
    float moveMaxTime = 1f;//�J��������������
    float rotateMaxTime = 1f;//�J��������]���鎞��
    [SerializeField] Vector3 originalPosition;
    [SerializeField] Vector3 targetPosition;
    [SerializeField] Quaternion targetRotation;
    [SerializeField] Quaternion originalRotate;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.localPosition;
        targetRotation = Quaternion.identity;
        originalRotate = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(CameraFocus());
            StartCoroutine(CameraRotate());
        }
        */
    }

    public IEnumerator CameraFocus(GameObject player,float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        targetPosition = player.transform.position + new Vector3(0, 0, -1.9f);

        float moveTimer = 0f;
        while(moveTimer < moveMaxTime)
        {
            moveTimer += Time.deltaTime;
            float moveTime = moveTimer / moveMaxTime;
            transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, moveTime);

            yield return null;
        }
        moveTimer = 0;
        transform.localPosition = targetPosition;

        //GameController��winnerNum�����߂�
        var playerNum = player.GetComponent<PlayerController>().controllerNum - 1;

        gameController.SetWinnerNum(playerNum);
    }

    public IEnumerator CameraRotate(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        float rotateTimer = 0f;
        while(rotateTimer < rotateMaxTime)
        {
            rotateTimer += Time.deltaTime;
            float rotateTime = Mathf.Pow(rotateTimer / rotateMaxTime,2);
            transform.localRotation = Quaternion.Lerp(originalRotate, targetRotation, rotateTime);

            yield return null;
        }
        rotateTimer = 0f;
        transform.localRotation = targetRotation;
    }
}
