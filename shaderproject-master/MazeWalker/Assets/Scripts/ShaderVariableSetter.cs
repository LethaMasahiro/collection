using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderVariableSetter : MonoBehaviour
{
    public string variableName;
    public int variableValue;

    void OnPreRender() {
        Shader.SetGlobalInt(variableName, variableValue);
    }
}
