using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BillboardManager : MonoBehaviour
{
    [SerializeField] TextAsset billboardTipsTextAsset;
    [SerializeField] Text[] billboards;

    // Start is called before the first frame update
    void Start()
    {
        Randomize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Randomize()
    {
        // Assuming length of tips >= length of billboards

        string[] billboardTips = billboardTipsTextAsset.text.Split('\n');
        bool[] tipIsUsed = new bool[billboardTips.Length];
        for (int i = 0; i < tipIsUsed.Length; i++)
        {
            tipIsUsed[i] = false;
        }

        foreach (Text billboard in billboards)
        {
            int randomTipIndex = -1;// Random.Range(0, billboardTips.Length);

            do
            {
                randomTipIndex = Random.Range(0, billboardTips.Length);
                if (tipIsUsed[randomTipIndex])
                {
                    randomTipIndex = -1;
                    continue;
                }
            } while (randomTipIndex == -1);

            tipIsUsed[randomTipIndex] = true;
            billboard.text = billboardTips[randomTipIndex];
        }
    }
}
