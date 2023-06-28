using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    class Script{
        string[] script;
    }

    Script script;

    void Start()
    {
        script = JsonUtility.FromJson<Script>(Resources.Load<TextAsset>("Json/Script").text);
    }

    void Update()
    {
        
    }
}
