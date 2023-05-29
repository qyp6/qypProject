using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;//单例
    public TextAsset dialogDataFile;//对话内容
    string[] dialogRows;
    public TMP_Text dialogText;//显示对话内容
    public Image dialogImage;//显示对话的角色头像
    public Button nextBtn;//显示下一条对话信息
    public GameObject talkUI;
    public GameObject diaUI;

    public List<Sprite> images = new List<Sprite>();//保存角色所有头像
    Dictionary<string, Sprite> imagerDic = new Dictionary<string, Sprite>();//保存角色名字和头像的对应关系
    int curDialogIndex = 1;//当前对话ID
    [HideInInspector]
    public string curNPCName;//当前对话名字
    [HideInInspector]
    public string excelNPCName = "Elderbug";//文件中名字
    public GameObject optionButtonPrefab;//选项按钮预制体
    public Transform buttonGroup; //选项按钮父物体
    public float textSpeed = 2f;
    // bool cancelTyping = false;
    // bool textFinish = true;
    public TMP_Text growValueText;//剧情完成度UI
    private Role role = new Role(); //剧情完成度类

    void Awake()
    {
        instance = this;
        ReadText();
        Init();
    }
    //初始化名字和图像对应
    void Init()
    {
        imagerDic["Elderbug"] = images[0];
        imagerDic["Brumm"] = images[1];
        imagerDic["Shaman"] = images[2];
        imagerDic["Knight"] = images[3];
        imagerDic["Lion"] = images[4];
        role._name = "Knight";

    }

    //读取文本
    void ReadText()
    {
        dialogRows = dialogDataFile.text.Split('\n');
    }
    //用文件中的每一行显示对话框中的内容
    public void ShowDialogRow()
    {
        for (int i = 0; i < dialogRows.Length; i++)
        {
            string[] cells = dialogRows[i].Split(',');
            // print(cells[i]);
            if (cells[0] == "#" && int.Parse(cells[1]) == curDialogIndex && curNPCName == excelNPCName)
            {
                nextBtn.gameObject.SetActive(true);
                UpdateText(cells[2], cells[3]);
                curDialogIndex = int.Parse(cells[5]);//更新对话索引
                excelNPCName = cells[4];//更新对话NPC的名字
                if (cells[6] != "")
                {
                    string[] effect = cells[6].Split('@');
                    ShowEffectAttri(effect[0], int.Parse(effect[1])); ;
                }
                break;
            }
            else if (cells[0] == "&" && int.Parse(cells[1]) == curDialogIndex && curNPCName == excelNPCName)
            {
                nextBtn.gameObject.SetActive(false);
                GenerateOptionButton(i);
                break;
            }
            else if (cells[0] == "END" && int.Parse(cells[1]) == curDialogIndex)
            {
                print("结束");
                diaUI.SetActive(true);
                break;
            }
        }
    }
    //更新对话框的文本和头像
    void UpdateText(string name, string text)
    {
            // print(imagerDic[name]);
            dialogImage.sprite = imagerDic[name];//头像赋值
            // dialogText.text = text; //文本内容赋值  
            StartCoroutine(SetTextUI(text));

    }
    IEnumerator SetTextUI(string text)
    {
        
        dialogText.text = "";
        int letter = 0;
        while (letter < text.Length)
        {
            dialogText.text += text[letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }
        dialogText.text = text;

    }
    public void OnNextBtnClick()
    {
        print("当前" + curNPCName);
        print("文件" + excelNPCName);
        if (curNPCName == excelNPCName)
        {
            ShowDialogRow();
        }
        else
        {
            talkUI.SetActive(false);
        }
    }
    //产生分支的按钮
    void GenerateOptionButton(int index)
    {
        //读取Index所在行的内容
        string[] cells = dialogRows[index].Split(',');
        if (cells[0] == "&")
        {
            GameObject btn = Instantiate(optionButtonPrefab, buttonGroup);
            btn.GetComponentInChildren<TMP_Text>().text = cells[3];
            btn.GetComponent<Button>().onClick.AddListener(
                delegate
                {
                    OnOptionClick(int.Parse(cells[5]));
                    if (cells[6] != "")
                    {
                        string[] effect = cells[6].Split('@');
                        ShowEffectAttri(effect[0], int.Parse(effect[1]));
                    }

                }
            );
            GenerateOptionButton(index + 1);
        }

    }
    //选项按钮的点击事件
    void OnOptionClick(int index)
    {
        curDialogIndex = index;//更新当前对话索引
        ShowDialogRow();
        for (int i = 0; i < buttonGroup.childCount; i++)
        { Destroy(buttonGroup.GetChild(i).gameObject); }

    }
    void ShowEffectAttri(string effectName, int param)
    {
        if (effectName == "成长值")
        {
            role.growValue += param;
            growValueText.text = "剧情完成度:" + role.growValue.ToString();
        }
    }
}
