using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkUI : MonoBehaviour
{
    public GameObject jint;
    public GameObject talkUI;
    public bool active = false;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (DialogManager.instance.excelNPCName == gameObject.name)
        {
            jint.SetActive(true);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(KeyCode.Space) && active == false)
        {
            if (DialogManager.instance.excelNPCName == gameObject.name)
            {
                // print(gameObject.name);
                active = true;
                talkUI.SetActive(true);
                DialogManager.instance.curNPCName = gameObject.name;//获取当前对话NPC名字
                DialogManager.instance.ShowDialogRow();//显示对话内容}
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        jint.SetActive(false);
        talkUI.SetActive(false);
        active = false;
    }
}
