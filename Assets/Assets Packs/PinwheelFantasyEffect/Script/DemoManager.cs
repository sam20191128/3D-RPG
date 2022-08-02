using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class DemoManager : MonoBehaviour {
    public Text text;
    public GameObject[] effects;

    protected int currentIndex = 0;
    protected List<Vector3> initPos = new List<Vector3>();

    public void Start()
    {
        GetInitPos();
        effects[currentIndex].transform.position = initPos[currentIndex];
        effects[currentIndex].SetActive(true);
        UpdateText();
    }

    public void GetInitPos()
    {
        for (int i=0;i<effects.Length;++i)
        {
            initPos.Add(effects[i].transform.position);
        }
    }

    public void IncreaseIndex()
    {
        effects[currentIndex].SetActive(false);
        ++currentIndex;
        if (currentIndex == effects.Length)
            currentIndex = 0;
        effects[currentIndex].transform.position = initPos[currentIndex];
        effects[currentIndex].SetActive(true);
    }

    public void DecreaseIndex()
    {
        effects[currentIndex].SetActive(false);
        --currentIndex;
        if (currentIndex < 0)
            currentIndex = effects.Length - 1;
        effects[currentIndex].transform.position = initPos[currentIndex];
        effects[currentIndex].SetActive(true);
    }

    public void UpdateText()
    {
        text.text = "Number " + currentIndex + ": " + effects[currentIndex].name;
    }

    public void Replay()
    {
        effects[currentIndex].SetActive(false);
        effects[currentIndex].transform.position = initPos[currentIndex];
        effects[currentIndex].SetActive(true);
    }
}
