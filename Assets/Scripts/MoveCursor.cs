using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using SuikaGame2D;

public class MoveCursor : MonoBehaviour
{
    [SerializeField] Vector3 offset = new Vector3(0, -1, 0);

    GameObject grabFruit;
    bool geneFg = false;
    CancellationToken token;

    // Start is called before the first frame update
    void Start()
    {
        token = this.GetCancellationTokenOnDestroy();
        GenerateFruit();
    }

    // Update is called once per frame
    async void Update()
    {
        float limit = 3.8f - grabFruit.transform.localScale.x / 2;
        Vector3 mousePos = Input.mousePosition;     //マウスの位置を取得
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);    //マウスの位置をSceneの座標に変換
        if (Mathf.Abs(worldPos.x) > limit)    //マウスの左右移動上限設定
        {
            worldPos.x = worldPos.x / Mathf.Abs(worldPos.x) * limit;
        }
        transform.position = new Vector3(worldPos.x, 4.5f, 0);  //YとZは固定値でカーソルを移動

        if (Input.GetMouseButtonDown((int)MouseButton.Left) && !geneFg)    //左クリックでフルーツ投下
        {
            geneFg = true;
            grabFruit.transform.SetParent(null);
            grabFruit.GetComponent<FruitScript>().MoveFree();

            await grabFruit.GetAsyncCollisionEnter2DTrigger().OnCollisionEnter2DAsync(token);   //投下したフルーツが何かに当たるまで待機
            grabFruit.GetComponent<FruitScript>().onGroundFg = true;

            GenerateFruit();
            geneFg = false;
        }
    }

    void GenerateFruit()
    {
        grabFruit = Instantiate(GameSystem.PickOutNextFruit(), transform.position + offset, transform.rotation);  //次のフルーツをカーソルの下に生成
        grabFruit.transform.SetParent(transform);   //親子関係にしてカーソルの移動についてくるようにする
    }
}
