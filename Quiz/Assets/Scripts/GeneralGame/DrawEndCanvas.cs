using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DrawEndCanvas : MonoBehaviour {
    public Camera phoneCamera;
    public Camera chromeCastCamera;
    public Canvas phoneCanvas;
    public Canvas chromeCanvas;
    public Canvas endCanvas;

    public void Start()
    {
        endCanvas.enabled = false;
    }

    public void EndScreen()
    {
        Canvas phone = Instantiate(endCanvas);
        Canvas cast = Instantiate(endCanvas);
        Debug.Log("testing:"+phone.GetComponentInChildren<SwitchText>().strList.Count);
        int iStr = Random.Range(0, phone.GetComponentInChildren<SwitchText>().strList.Count);

        phone.enabled = true;
        phoneCanvas.enabled = false;
        phone.worldCamera = phoneCamera;

        cast.enabled = true;
        chromeCanvas.enabled = false;
        cast.worldCamera = chromeCastCamera;
        GraphicRaycaster gR = cast.GetComponent<GraphicRaycaster>();
        gR.enabled = false;

        phone.GetComponentInChildren<SwitchText>().NotRandom(iStr);
        cast.GetComponentInChildren<SwitchText>().NotRandom(iStr);
    }
}
