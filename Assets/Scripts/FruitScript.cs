using SuikaGame2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    [SerializeField] FruitTier tier;    //�t���[�c�̃e�B�A
    Rigidbody2D rb2;    //���̃I�u�W�F�N�g��rb
    bool combineFg;     //���̃t���O

    public bool onGroundFg = false;    //�n�ʂɂ����t���O

    public FruitTier GetTier() { return tier; }

    private void Start()
    {
        Resize();
    }

    public void MoveFree()  //��������Ƃ��̏���
    {
        gameObject.AddComponent<CircleCollider2D>();
        rb2 = gameObject.AddComponent<Rigidbody2D>();   //���Ƃ��Ƃ��Ă���Ɛe�I�u�W�F�N�g�ɂ��Ă����Ȃ�
        rb2.velocity = Vector3.zero;
        rb2.angularDrag = 0;
        rb2.mass = (float)((int)tier * (int)tier * 0.1) + 3.0f;    //�d����Tier�Ɍ��������d����
    }

    void Resize()   //Scale��Tier�Ɍ����������̂ɂ���
    {
        float size = (float)(0.5 * Mathf.Pow(1.25f, (float)tier));
        transform.localScale = new Vector3(size, size, size);
    }

    private void OnCollisionEnter2D(Collision2D col)    //collisionEnter
    {
        FruitScript fs = col.gameObject.GetComponent<FruitScript>();    //�Ԃ������I�u�W�F�N�g�̃X�N���v�g���擾
        if (fs != null) //�X�N���v�g����������i�t���[�c��������j
        {
            if (fs.GetTier() == tier && !combineFg) //�e�B�A�������Ńt���O�������ĂȂ�������
            {
                combineFg = true;   //�t���O�𗧂Ă�
                fs.combineFg = true;    //����̃t���O�𗧂Ă�i��񂵂����s����Ȃ��Ȃ�j
                GameSystem.CombineFruit((int)tier + 1, col.contacts[0].point);   //�e�B�A�������Đ������Ă��炤
                Destroy(fs.gameObject); //����
                Destroy(gameObject);
            }
        }
    }
}
