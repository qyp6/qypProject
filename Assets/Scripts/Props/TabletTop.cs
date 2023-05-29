using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletTop : MonoBehaviour
{
    public Animator animator;
    public bool isInteractive=false;

    private void Update()
    {
        if (isInteractive && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("保存成功");
            animator.SetTrigger("Active");
        }
        if (isInteractive && Input.GetKeyDown(KeyCode.Escape))
        {
            animator.SetTrigger("Disactive");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isInteractive && collision.gameObject.layer == LayerMask.NameToLayer("Hero Detector"))
        {
            isInteractive = true;
        }
    }
}
