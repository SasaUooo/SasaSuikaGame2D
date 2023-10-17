using SuikaGame2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    [SerializeField] FruitTier tier;    //フルーツのティア
    Rigidbody2D rb2;    //このオブジェクトのrb
    bool combineFg;     //合体フラグ

    public bool onGroundFg = false;    //地面についたフラグ

    public FruitTier GetTier() { return tier; }

    private void Start()
    {
        Resize();
    }

    public void MoveFree()  //投下するときの処理
    {
        gameObject.AddComponent<CircleCollider2D>();
        rb2 = gameObject.AddComponent<Rigidbody2D>();   //もともとついていると親オブジェクトについていかない
        rb2.velocity = Vector3.zero;
        rb2.angularDrag = 0;
        rb2.mass = (float)((int)tier * (int)tier * 0.1) + 3.0f;    //重さをTierに見合った重さに
    }

    void Resize()   //ScaleをTierに見合ったものにする
    {
        float size = (float)(0.5 * Mathf.Pow(1.25f, (float)tier));
        transform.localScale = new Vector3(size, size, size);
    }

    private void OnCollisionEnter2D(Collision2D col)    //collisionEnter
    {
        FruitScript fs = col.gameObject.GetComponent<FruitScript>();    //ぶつかったオブジェクトのスクリプトを取得
        if (fs != null) //スクリプトがあったら（フルーツだったら）
        {
            if (fs.GetTier() == tier && !combineFg) //ティアが同じでフラグが立ってなかったら
            {
                combineFg = true;   //フラグを立てる
                fs.combineFg = true;    //相手のフラグを立てる（一回しか実行されなくなる）
                GameSystem.CombineFruit((int)tier + 1, col.contacts[0].point);   //ティアをあげて生成してもらう
                Destroy(fs.gameObject); //消滅
                Destroy(gameObject);
            }
        }
    }
}
