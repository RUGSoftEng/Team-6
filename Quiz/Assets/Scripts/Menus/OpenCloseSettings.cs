using UnityEngine;
using System.Collections;

public class OpenCloseSettings : MonoBehaviour {

    public void Flip() {
        if (this.gameObject.activeInHierarchy) {
            this.gameObject.SetActive(false);
        } else {
            this.gameObject.SetActive(true);
        }
    }
}
