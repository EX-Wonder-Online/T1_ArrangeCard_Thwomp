using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*ドッスン風の動き
 * 重力OFF 変数:rb
 * ２秒したら 変数:startTime
 * 重力ON（下に落ちる）
 * 地面に触れたら 変数:grounded
 * ２秒したら 変数:groundedTime
 * 元の位置に戻る 変数:startPos
 * 繰り返す．変数:intervalTime
 */
public class ThwompMove : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    private Vector3 startPos;
    private bool grounded = false;
    [SerializeField]
    private float startTime, intervalTime, groundedTime;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Move() {
        rb.useGravity = false; // 重力OFF
        yield return new WaitForSeconds(startTime); // 初めの待機時間
        while (true) { // ずっと
            rb.useGravity = true; // 重力ON
            yield return new WaitWhile(()=>grounded); // 地面に触れるまで待つ
            yield return new WaitForSeconds(groundedTime); // 地面に触れている時間まで待つ
            rb.useGravity = false; // 重力OFF
            rb.AddForce(0, 100, 0); // 上むきに力を加える
            yield return new WaitWhile(()=>this.transform.position.y <= startPos.y); // 高さが下の位置に戻ったら
            rb.velocity = Vector3.zero; // 速度をゼロに = 止まる
            yield return new WaitForSeconds(intervalTime); // 次落ちるまでの待機時間
        }
        
    }
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Ground") {
            grounded = true;
        }
    }
    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag == "Ground") {
            grounded = false;
        }
    }
}