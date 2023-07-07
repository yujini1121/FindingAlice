using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    // 클래스 및 멤버의 접근지정자 public이어야 Parsing 됨
    [System.Serializable]
    public class ScriptData
    {
        public int      num;
        public string   spriteNameL;
        public string   spriteNameR;
        public int      speaker;
        public string   actorName;
        public string   line;
    }

    // 클래스 및 멤버의 접근지정자 public이어야 Parsing 됨
    [System.Serializable]
    public class Script{
        public List<ScriptData> scriptDatas;
    }

    [Header("Dialogue")]
    [SerializeField] private Script script;
    [SerializeField] private int    BeginNum;
    [SerializeField] private int    EndNum;
    private int index = 0;

    [Header("Bounds")]
    [SerializeField] private float  dialogueBoxCenterX;
    [SerializeField] private float  dialogueBoxCenterY;
    [SerializeField] private float  dialogueBoxCenterZ;
    [SerializeField] private float  dialogueBoxScaleX;
    [SerializeField] private float  dialogueBoxScaleY;
    [SerializeField] private float  dialogueBoxScaleZ;

    private GameObject dialogueUI;
    private GameObject dialogueActorName;
    private GameObject dialogueActorScript;

    void Start()
    {
        script = JsonUtility.FromJson<Script>(Resources.Load<TextAsset>("Json/Script").text);
        while (script.scriptDatas[0].num < BeginNum)
        {
            script.scriptDatas.Remove(script.scriptDatas[0]);
        }
        while ((EndNum - BeginNum) < (script.scriptDatas.Count - 1))
        {
            script.scriptDatas.Remove(script.scriptDatas[script.scriptDatas.Count - 1]);
        }

        GetComponent<BoxCollider>().center  = new Vector3(dialogueBoxCenterX, dialogueBoxCenterY, dialogueBoxCenterZ);
        GetComponent<BoxCollider>().size    = new Vector3(dialogueBoxScaleX, dialogueBoxScaleY, dialogueBoxScaleZ);
        dialogueUI          = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        dialogueActorName   = dialogueUI.transform.GetChild(2).GetChild(0).gameObject;
        dialogueActorScript = dialogueUI.transform.GetChild(2).GetChild(1).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((index < EndNum - BeginNum) && other.tag == "Player")
        {
            other.GetComponent<Movement>().StateDialogueBegin();
            other.transform.position = new Vector3(transform.position.x + dialogueBoxCenterX,
                                                   transform.position.y + dialogueBoxCenterY,
                                                   transform.position.z + dialogueBoxCenterZ);
            dialogueUI.SetActive(true);
            ClockManager.instance.clockShootable = false;
            StartCoroutine(PrintDialogue(other.gameObject));
        }
    }

    private IEnumerator PrintDialogue(GameObject player)
    {
        DialogueAction();

        while (index <= (EndNum - BeginNum))
        {
            if (Input.GetMouseButtonDown(0))
            {
                DialogueAction();
            }

            yield return null;
        }

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                player.GetComponent<Movement>().StateDialogueEnd();
                dialogueUI.SetActive(false);
                ClockManager.instance.clockShootable = true;

                yield break;
            }

            yield return null;
        }
    }

    private void DialogueAction()
    {
        GameObject actor = dialogueUI.transform.GetChild(0).gameObject;
        // 왼쪽 Actor가 존재하면 활성화
        if (script.scriptDatas[index].spriteNameL != "")
        {
            actor.SetActive(true);
            actor.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + script.scriptDatas[index].spriteNameL);
        }
        else
        {
            actor.SetActive(false);
        }

        actor = dialogueUI.transform.GetChild(1).gameObject;
        // 오른쪽 Actor가 존재하면 활성화
        if (script.scriptDatas[index].spriteNameR != "")
        {
            actor.SetActive(true);
            actor.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + script.scriptDatas[index].spriteNameR);
        }
        else
        {
            actor.SetActive(false);
        }

        // 말하고 있지 않은 Actor 어둡게 만들기
        if (script.scriptDatas[index].speaker == 0 || script.scriptDatas[index].speaker == 1)
        {
            dialogueUI.transform.GetChild(script.scriptDatas[index].speaker).GetComponent<Image>().color     = Color.white;
            dialogueUI.transform.GetChild(1 - script.scriptDatas[index].speaker).GetComponent<Image>().color = Color.gray;
        }

        dialogueActorName.GetComponent<TextMeshProUGUI>().text   = script.scriptDatas[index].actorName;
        dialogueActorScript.GetComponent<TextMeshProUGUI>().text = script.scriptDatas[index].line;

        index++;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x + dialogueBoxCenterX,
                                        transform.position.y + dialogueBoxCenterY,
                                        transform.position.z + dialogueBoxCenterZ),
                            new Vector3(dialogueBoxScaleX, dialogueBoxScaleY, dialogueBoxScaleZ));
    }
}
