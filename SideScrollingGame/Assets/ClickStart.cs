using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickStart : MonoBehaviour
{
    // Start is called before the first frame update
    private Button button; 
    void Start()
    {
       button = this.gameObject.GetComponent<Button>();
       button.onClick.AddListener(click); 
    }


    public void click(){
        SceneManager.LoadScene("MainStage");
    }
}
