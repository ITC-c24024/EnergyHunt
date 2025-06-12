using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Blink : MonoBehaviour
{
    MeshRenderer mesh;//オブジェクトのメッシュ
    Coroutine blinkCoroutine;//点滅処理のコルーチン

    void Start()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
    }

    public void BlinkStart(int time, float speed, float lastSpeed)//timeは点滅時間、speedは点滅の速さ、lastSpeedは2秒以下になった際の速さ
    {
        //すでに実行されていた場合に重複しないように処理を止めて新しくスタートさせる
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkCount(time, speed, lastSpeed));
    }

    public IEnumerator BlinkCount(int time, float speed, float lastSpeed)
    {
        var currentTime = 0f;//現在の時間

        while (currentTime < time)
        {
            mesh.enabled = !mesh.enabled;//メッシュの表示切替

            //残り2秒以下になったら点滅速度を変更
            if (time-currentTime <= 2f)
            {
                speed = lastSpeed;
            }

            yield return new WaitForSeconds(speed);//点滅の周期分待つ
            currentTime += speed;//待った分の時間を足す
        }

        mesh.enabled = true;//最終的に表示状態になるようにする
    }
}
