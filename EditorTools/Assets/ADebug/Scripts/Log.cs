using UnityEngine;
using Tools;

public class Log : MonoBehaviour
{
    private void Start()
    {
        Debugger.LogInfo("测试文本");
    }
}