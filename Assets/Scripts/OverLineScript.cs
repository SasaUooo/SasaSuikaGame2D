using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverLineScript : MonoBehaviour
{
    bool gameExitFg = false;

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!gameExitFg)
        {
            FruitScript fs = col.gameObject.GetComponent<FruitScript>();
            if (fs != null)
            {
                if (fs.onGroundFg)
                {
                    Debug.Log("ゲームオーバー");
                    gameExitFg = true;
                }
            }
        }
    }
}
