using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    Text loadingText;
    [SerializeField]
    int periodsIndex;
    readonly string[] periodsText = { "", ".", "..", "..." };

    const float DELAY = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        loadingText = GetComponent<Text>();
        periodsIndex = 0;

        StartCoroutine(UpdateText());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator UpdateText()
    {
        while (true)
        {
            loadingText.text = "Loading" + periodsText[periodsIndex++];

            if (periodsIndex == periodsText.Length)
                periodsIndex = 0;

            yield return new WaitForSeconds(DELAY);
        }
    }
}
