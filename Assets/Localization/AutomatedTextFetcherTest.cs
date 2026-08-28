
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;


[ExecuteInEditMode]
public class AutomatedTextFetcherTest : MonoBehaviour
{


    public void FetchStrings()
    {
        TextMeshProUGUI[] components = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();

        //TextMeshProUGUI textLocal = components[0];
        foreach(TextMeshProUGUI textLocal in components)
        {
            bool containsInt = textLocal.text.Any(char.IsDigit);

            if (string.IsNullOrWhiteSpace(textLocal.text)) Debug.Log("Empty");
            else if(!containsInt)
            {
                SetupForLocalization(textLocal);
            }
        }
    }

    public void SetupForLocalization(TextMeshProUGUI target)
    {
        var comp = Undo.AddComponent(target.gameObject, typeof(LocalizeStringEvent)) as LocalizeStringEvent;
        var setStringMethod = target.GetType().GetProperty("text").GetSetMethod();
        var methodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction<string>), target, setStringMethod) as UnityAction<string>;
        UnityEditor.Events.UnityEventTools.AddPersistentListener(comp.OnUpdateString, methodDelegate);

        var table = LocalizationEditorSettings.GetStringTableCollection("Menu_Local");
        var stringTable = table.GetTable("de") as StringTable;

        var newKey = stringTable.SharedData.AddKey("name");

        stringTable.AddEntry(newKey.Id, target.text);

        EditorUtility.SetDirty(stringTable);
        EditorUtility.SetDirty(stringTable.SharedData);
    }
}
