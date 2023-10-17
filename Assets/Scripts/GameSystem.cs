using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SuikaGame2D
{
    public enum FruitTier
    {
        cherry = 0,
        strawberry = 1,
        grape = 2,
        orange = 3,
        persimmon = 4,
        apple = 5,
        pear = 6,
        peach = 7,
        pineapple = 8,
        melon = 9,
        watermelon = 10
    }

    public static class GameSystem
    {
        static GameObject[] m_FruitPrefabs = new GameObject[11];   //�t���[�c�̃v���n�u�̃��X�g
        static GameObject[] m_FruitBasket = new GameObject[2];     //Next�\���p
        static GameObject m_nextFruit;
        static Vector3 m_nextPos = new Vector3(7, 4, 0);

        static GameSystem() => Initialize();

        public static void Initialize()    //������
        {
            int i;
            for (i = 0; i < m_FruitPrefabs.Length; i++) //�t���[�c�̃v���n�u��Resources���烍�[�h
            {
                m_FruitPrefabs[i] = (GameObject)Resources.Load("Fruit_" + i);
            }

            for (i = 0; i < m_FruitBasket.Length; i++)   //�o�X�P�b�g�𖄂߂�
            {
                m_FruitBasket[i] = RandomFruit();
            }
        }

        private static GameObject RandomFruit()    //�����_���ȃt���[�c�̃v���n�u��Ԃ�
        {
            return m_FruitPrefabs[Random.Range(0, 4)];   //0,1,2,3�̂��������_���ȃv���n�u��Ԃ�
        }

        public static GameObject PickOutNextFruit()
        {
            GameObject result = m_FruitBasket[0];       //�Ԃ��̂�0�Ԗڂ̃t���[�c
            for (int i = 0; i < m_FruitBasket.Length - 1; i++)  //Next���l�߂�
            {
                m_FruitBasket[i] = m_FruitBasket[i + 1];
            }
            m_FruitBasket[m_FruitBasket.Length - 1] = RandomFruit();    //�Ō�Ƀ����_���Ȃ������

            ShowNextFruit();

            return result;
        }

        public static void CombineFruit(int tier, Vector3 pos)   //�e�B�A�Əꏊ��n���ƃt���[�c�𐶐�
        {
            if (tier <= m_FruitPrefabs.Length) //prefab������Ȃ�
            {
                GameObject obj = Object.Instantiate(m_FruitPrefabs[tier], pos, Quaternion.identity);
                obj.GetComponent<FruitScript>().MoveFree();
            }
        }

        private static void ShowNextFruit() //Next��\������
        {
            if (m_nextFruit != null)    //Next�\���������������
            {
                Object.Destroy(m_nextFruit);
            }
            m_nextFruit = Object.Instantiate(m_FruitBasket[0], m_nextPos, Quaternion.identity); //Next�\��
        }
    }
}