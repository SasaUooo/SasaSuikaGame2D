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
        Vector3 mousePos = Input.mousePosition;     //�}�E�X�̈ʒu���擾
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);    //�}�E�X�̈ʒu��Scene�̍��W�ɕϊ�
        if (Mathf.Abs(worldPos.x) > limit)    //�}�E�X�̍��E�ړ�����ݒ�
        {
            worldPos.x = worldPos.x / Mathf.Abs(worldPos.x) * limit;
        }
        transform.position = new Vector3(worldPos.x, 4.5f, 0);  //Y��Z�͌Œ�l�ŃJ�[�\�����ړ�

        if (Input.GetMouseButtonDown((int)MouseButton.Left) && !geneFg)    //���N���b�N�Ńt���[�c����
        {
            geneFg = true;
            grabFruit.transform.SetParent(null);
            grabFruit.GetComponent<FruitScript>().MoveFree();

            await grabFruit.GetAsyncCollisionEnter2DTrigger().OnCollisionEnter2DAsync(token);   //���������t���[�c�������ɓ�����܂őҋ@
            grabFruit.GetComponent<FruitScript>().onGroundFg = true;

            GenerateFruit();
            geneFg = false;
        }
    }

    void GenerateFruit()
    {
        grabFruit = Instantiate(GameSystem.PickOutNextFruit(), transform.position + offset, transform.rotation);  //���̃t���[�c���J�[�\���̉��ɐ���
        grabFruit.transform.SetParent(transform);   //�e�q�֌W�ɂ��ăJ�[�\���̈ړ��ɂ��Ă���悤�ɂ���
    }
}
