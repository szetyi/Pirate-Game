using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ButtonLoadMapView() {
        SceneManager.LoadScene("Map View");
    }

    public void ButtonLoadWorld() {
        SceneManager.LoadScene("World View");
    }

    public void ButtonLoadMainMenu() {
        SceneManager.LoadScene("Main Menu");
    }
}
