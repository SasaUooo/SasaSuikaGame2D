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
        static GameObject[] m_FruitPrefabs = new GameObject[11];   //フルーツのプレハブのリスト
        static GameObject[] m_FruitBasket = new GameObject[2];     //Next表示用
        static GameObject m_nextFruit;
        static Vector3 m_nextPos = new Vector3(7, 4, 0);

        static GameSystem() => Initialize();

        public static void Initialize()    //初期化
        {
            int i;
            for (i = 0; i < m_FruitPrefabs.Length; i++) //フルーツのプレハブをResourcesからロード
            {
                m_FruitPrefabs[i] = (GameObject)Resources.Load("Fruit_" + i);
            }

            for (i = 0; i < m_FruitBasket.Length; i++)   //バスケットを埋める
            {
                m_FruitBasket[i] = RandomFruit();
            }
        }

        private static GameObject RandomFruit()    //ランダムなフルーツのプレハブを返す
        {
            return m_FruitPrefabs[Random.Range(0, 4)];   //0,1,2,3のうちランダムなプレハブを返す
        }

        public static GameObject PickOutNextFruit()
        {
            GameObject result = m_FruitBasket[0];       //返すのは0番目のフルーツ
            for (int i = 0; i < m_FruitBasket.Length - 1; i++)  //Nextを詰める
            {
                m_FruitBasket[i] = m_FruitBasket[i + 1];
            }
            m_FruitBasket[m_FruitBasket.Length - 1] = RandomFruit();    //最後にランダムなやつを入れる

            ShowNextFruit();

            return result;
        }

        public static void CombineFruit(int tier, Vector3 pos)   //ティアと場所を渡すとフルーツを生成
        {
            if (tier <= m_FruitPrefabs.Length) //prefabがあるなら
            {
                GameObject obj = Object.Instantiate(m_FruitPrefabs[tier], pos, Quaternion.identity);
                obj.GetComponent<FruitScript>().MoveFree();
            }
        }

        private static void ShowNextFruit() //Nextを表示する
        {
            if (m_nextFruit != null)    //Next表示があったら消す
            {
                Object.Destroy(m_nextFruit);
            }
            m_nextFruit = Object.Instantiate(m_FruitBasket[0], m_nextPos, Quaternion.identity); //Next表示
        }
    }
}