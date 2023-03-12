using UnityEditor;

public class CreateNewActionScriptFromCustomTemplate
{
    private const string pathToActionScriptTemplate = "Assets/EasyBT/Editor/Settings/ActionScript_Template.txt";

    [MenuItem("Assets/Create/EasyBT/ActionScript", isValidateFunction: false, 0)]
    public static void CreateActionScriptFromTemplate()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(pathToActionScriptTemplate, "NewActionScript.cs");
    }

    private const string pathToConditionScriptTemplate = "Assets/EasyBT/Editor/Settings/ConditionScript_Template.txt";

    [MenuItem("Assets/Create/EasyBT/ConditionScript", isValidateFunction: false, 0)]
    public static void CreateConditionScriptFromTemplate()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(pathToConditionScriptTemplate, "NewConditionScript.cs");
    }

    private const string pathToCompositeNodeTemplate = "Assets/EasyBT/Editor/Settings/CompositeNode_Template.txt";

    [MenuItem("Assets/Create/EasyBT/CompositeNode", isValidateFunction: false, 0)]
    public static void CreateCompositeNodeFromTemplate()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(pathToCompositeNodeTemplate, "NewCompositeNode.cs");
    }

    private const string pathToDecoratorNodeTemplate = "Assets/EasyBT/Editor/Settings/DecoratorNode_Template.txt";

    [MenuItem("Assets/Create/EasyBT/DecoratorNode", isValidateFunction: false, 0)]
    public static void CreateDecoratorNodeFromTemplate()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(pathToDecoratorNodeTemplate, "NewDecoratorNode.cs");
    }

    private const string pathToNodeInspectorDrawerTemplate = "Assets/EasyBT/Editor/Settings/NodeInspectorDrawer_Template.txt";

    [MenuItem("Assets/Create/EasyBT/NodeInspectorDrawer", isValidateFunction: false, 0)]
    public static void CreateNodeInspectorDrawerFromTemplate()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(pathToNodeInspectorDrawerTemplate, "NewNodeInspectorDrawer.cs");
    }
}