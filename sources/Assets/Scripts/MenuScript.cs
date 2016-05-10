using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuScript : MonoBehaviour {
    
    public Button startButton;
    public Button quitButton;
    public Button helpButton;
    public Button backButton;
    public Canvas helpMenu;
	// Use this for initialization
	void Start () {
        
        startButton = startButton.GetComponent<Button>();
        quitButton = quitButton.GetComponent<Button>();
        helpButton = helpButton.GetComponent<Button>();
        helpMenu = helpMenu.GetComponent<Canvas>();
        helpMenu.enabled = false;
    }
	
	// Update is called once per frame
	public void Play () {
        SceneManager.LoadScene("ludo");
    }

    public void Quit ()
    {
        Application.Quit();
    }

    public void Help()
    {
        helpMenu.enabled = true;
    }
    public void Back()
    {
        helpMenu.enabled = false;
    }
}
