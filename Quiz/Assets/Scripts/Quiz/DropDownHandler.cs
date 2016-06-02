using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DropDownHandler : MonoBehaviour {
    public List<string> strList;
    public Button selectable;
    public Canvas onCanvas;
    public GameObject mainButton;
    private bool open = false, mayBeClosed = false;
    private List<Button> optionList = new List<Button>();
    // Use this for initialization

    void Start()
    {
        if (strList.Count < 1)
        {
            strList = mainButton.GetComponent<DropDownHandler>().GetStrList();
        }
        for (int i = 0; i < strList.Count; i++)
        {
            Button option = Instantiate(selectable) as Button;
            option.transform.SetParent(onCanvas.transform);
            option.transform.localScale = new Vector3(1, 1, 1);
            RectTransform rt = option.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.56f, 1 - 0.15f * (i + 1));
            rt.anchorMax = new Vector2(0.88f, 1 - 0.15f * (i));
            rt.offsetMin = new Vector2(0, 0);
            rt.offsetMax = new Vector2(0, 0);
            rt.GetChild(0).GetComponent<Text>().text = strList[i]; ;
            optionList.Add(option);
            HideOptions();
        }
    }

    public void Pressed () {
        if (!open) {
            open = true;
            mayBeClosed = false;
            ShowOptions();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0)) {
            if (open && mayBeClosed) {
                HideOptions();
                open = false;
            }
        }
        mayBeClosed = open;
    }

    void ShowOptions()
    {
        foreach (Button option in optionList)
        {
            option.interactable = true;
            option.GetComponent<RectTransform>().SetAsLastSibling();
            option.GetComponent<RectTransform>().GetChild(0).GetComponent<Text>().color = Color.black;
        }
    }

    void HideOptions()
    {
        foreach (Button option in optionList)
        {
            option.interactable = false;
            option.GetComponent<RectTransform>().SetAsFirstSibling();
            option.GetComponent<RectTransform>().GetChild(0).GetComponent<Text>().color = Color.clear;
        }
    }

    public List<string> GetStrList()
    {
        return strList;
    }
}
