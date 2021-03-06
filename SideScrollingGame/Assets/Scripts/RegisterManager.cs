using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class JSONContent {
    public int status;
    public static  JSONContent toJson(string str){
        return JsonUtility.FromJson<JSONContent>(str);
    }
}
public class RegisterManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Text scoreText;
    public Button titleButton;
    public Button registerButton;
    public Button backButton;
    public Button doneButton;
    public GameObject panel;
    public Text confirm;
    public InputField inputField;
    public Text instructions;
    private int score;
    private string userName;

    ///http
    private UnityWebRequest httpClient;
    void Start()
    {
        //スコア代入
        score = Mathf.FloorToInt(PlayerPrefs.GetFloat("Score"));
        Debug.Log(score);
        scoreText.text += score.ToString();
        titleButton.onClick.AddListener(ClickTitle);
        registerButton.onClick.AddListener(ClickRegister);
        //ボタンの有効
        titleButton.GetComponent<Image>().raycastTarget = true;
        registerButton.GetComponent<Image>().raycastTarget = true;
        inputField.GetComponent<Image>().raycastTarget = true;
        //ボタンの無効化
        doneButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        panel.SetActive(false);
        backButton.onClick.AddListener(ClickBack);
        doneButton.onClick.AddListener(ClickDone);
        confirm.text = "";
    }

    
    public void ClickRegister(){
        instructions.text = "";
        if(!CheckUserName(inputField.text, out userName)){
            instructions.text = "<color=red>ユーザ名は1-128文字の半角英数字または記号のみで構成される必要があります</color>";
            return;
        }else{
            StartCoroutine(PostFind("http://5jgames.com:4567/action/",$"find?name={userName}&score={score}"));
        }

    }
    public void ClickTitle(){
        SceneManager.LoadScene("Title");
    }

    public void ClickDone(){
        StartCoroutine(PostRegister("http://5jgames.com:4567/action/",$"register?name={userName}&score={score}"));
    }
    public void ClickBack(){
        //ボタンの有効
        titleButton.gameObject.SetActive(true);
        registerButton.gameObject.SetActive(true);
        inputField.GetComponent<Image>().raycastTarget = true;
        //ボタンの無効化
        doneButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        panel.SetActive(false); 
        confirm.text = "";
    }
    private bool CheckUserName(string name, out string fixedname){
        fixedname = "";
        if(new Regex(@"^[\s]*$").IsMatch(name)){
            return false;
        }else if (!new Regex(@"^[0-9a-zA-Z!-/:-@[-`{-~]+$").IsMatch(name)){
            return false;
        }else if(name.Length > 128){
            return false;
            
        }

        //一応空白文字列削除して終了
        fixedname = Regex.Replace(name, @"[\s]+", "");
        return true;
    }

    public IEnumerator PostFind (string baseURL, string httpparams){
        httpClient = UnityWebRequest.Post(baseURL+httpparams, new WWWForm());
        Debug.Log(baseURL+httpparams);
        yield return httpClient.SendWebRequest();
        if (httpClient.result != UnityWebRequest.Result.Success)
        {
            instructions.text = "エラーが発生しました。インターネットの接続を確認し、しばらくしてからお試しください";
        }
        else
        {
            Debug.Log(httpClient.downloadHandler.text);
            string responce = httpClient.downloadHandler.text;
            int status = JSONContent.toJson(responce).status;
            Debug.Log(status);
            if(status == 1){
                instructions.text = "<color=red>ユーザ名は1-128文字の半角英数字または記号のみで構成される必要があります</color>";
            }else if(status != 1){
                //ボタンの無効
                titleButton.gameObject.SetActive(false);
                registerButton.gameObject.SetActive(false);
                registerButton.GetComponent<Image>().raycastTarget = false;
                inputField.GetComponent<Image>().raycastTarget = false;

                //ボタンの有効禍
                backButton.gameObject.SetActive(true);
                doneButton.gameObject.SetActive(true);
                panel.gameObject.SetActive(true);
                if(status == 2){
                    confirm.text = $"スコア:{score}\nユーザー名:{userName}\nで登録します。\n<color=red>すでに同ユーザー名が登録されています。初回登録の場合はユーザー名を変更してください。</color>";
                }else if(status == 3){
                    confirm.text = $"スコア:{score}\nユーザー名:{userName}\nで登録します。\n<color=red>すでに同ユーザー名が登録されています。初回登録の場合はユーザー名を変更してください。\n登録されているスコアのほうが高いです。</color>";
                }else if(status == 0){
                    confirm.text = $"スコア:{score}\nユーザー名:{userName}\nで登録します。よろしいですか。";
                }
            }
        }
    }

    public IEnumerator PostRegister(string baseURL, string httpparams){
        httpClient = UnityWebRequest.Post(baseURL+httpparams, new WWWForm());
        yield return httpClient.SendWebRequest();
        if (httpClient.result != UnityWebRequest.Result.Success)
        {
            instructions.text = "エラーが発生しました。インターネットの接続を確認し、しばらくしてからお試しください";
        }
        else
        { 
            string responce = httpClient.downloadHandler.text;
            Debug.Log(responce);
            int status = JSONContent.toJson(responce).status;
            confirm.text = "登録が完了しました";
            //ボタンの無効化
            backButton.gameObject.SetActive(false);
            doneButton.gameObject.SetActive(false);
            titleButton.gameObject.SetActive(true); 
        }
    }
}