using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickHelp : MonoBehaviour
{
   private bool clickFlag = false; 
   [SerializeField] private Text title;
   [SerializeField] private Text instruction;
   [SerializeField] private Button button;

   void Start()
   {
      button.onClick.AddListener(helpclick);
   }

   public void helpclick(){
       clickFlag = !clickFlag;
       if(clickFlag){
           //instruction
            title.enabled = false;
            instruction.enabled = true;
            button.gameObject.transform.Find("Text").GetComponent<Text>().text = "もどる";
        }else{
            title.enabled = true;
            instruction.enabled = false;
            button.gameObject.transform.Find("Text").GetComponent<Text>().text = "ヘルプ";
        }
   }
}
