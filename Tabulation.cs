using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Tabulation : Editor
{
    [MenuItem("Window/Json 编辑")]
   public static void OpenTabulationWidow()
    {
		TabulationWindow.AddWindow();
	}
	[MenuItem("Window/test")]
	public static void Test()
    {
		TestWidow.AddWindow();

	}
}
public class TabulationWindow : EditorWindow
{
	public static void AddWindow()
	{
        TabulationWindow windows = (TabulationWindow)EditorWindow.GetWindow(typeof(TabulationWindow),true , "json 编辑（第一节点为数组式的）");
		windows.Show();
	}
	void OnInspectorUpdate()
    {
		this.Repaint();
		objScrollerPos = new Vector2(scrollerPos.x, 0);
		idScrollerPos = new Vector2(0, scrollerPos.y);
	}
	private string path;
	private string title; 
	private int elementW = 100, elementH = 30;
	private string[] objes = { "ID\n(INT)", "NameID\n(INT)", "TypeName\n(INT)", "FunctionaType\n(INT)", "Subclass\n(INT)", "IconId\n(INT)",
		"Appearance\n(STRING)", "HitRadius\n(FLOAT)", "BuffSlot\n(LIST)" , "MagicTag\n(INT)" };
	private int nodeCount = 30;
	public static Vector2 scrollerPos,objScrollerPos,idScrollerPos;
	public static string curSelectStr = "";
	private Dictionary<int ,string> elements =new Dictionary<int, string>();
    private void Awake()
    {
		for(int i= 0; i < 	nodeCount * objes.Length; ++i)
        {
			elements[i] = "";
        }

	}
    private void OnGUI()
    {
		CreateFilePath();
		CreateObjectTypes(objes);
        CreateElements();
		GUILayout.Space(10);
		elements[curD] = GUILayout.TextArea(elements[curD], GUILayout.Width(position.width -5), GUILayout.Height(elementH * 3));
        if (GUILayout.Button("创建竖排", GUILayout.Width(100), GUILayout.Height(30)))
        {
			Debug.Log(curSelectStr);
			elements[curD] = curSelectStr;
        }

	}
	private void CreateFilePath()
    {
		EditorGUILayout.BeginVertical();
		GUILayout.Space(10);
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("文件名：", GUILayout.Width(50), GUILayout.Height(20));
		Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(position.width - 100), GUILayout.Height(20));
		path = EditorGUI.TextField(rect, path);
		if ((Event.current.type == EventType.DragUpdated
			 || Event.current.type == EventType.DragExited)
			 && rect.Contains(Event.current.mousePosition))
		{
			//改变鼠标的外表
			DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
			if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
			{
				path = DragAndDrop.paths[0];
			}
		}
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(10);
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Titile  : ", GUILayout.Width(50), GUILayout.Height(20));
        title = GUILayout.TextField(title, GUILayout.Width(200), GUILayout.Height(20));
        EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
    }
	private void CreateObjectTypes(string[]objNames)
    {
		GUILayout.BeginVertical();
		GUILayout.Space(5);
		GUILayout.BeginHorizontal();
		GUILayout.Label(objNames[0], GUILayout.Width(elementW), GUILayout.Height(elementH));
		objScrollerPos = GUILayout.BeginScrollView(objScrollerPos, GUILayout.Height(elementH + 25));
		GUILayout.BeginHorizontal();
		for (int i = 1; i < objNames.Length; i++)
        {
			GUILayout.Label(objNames[i], GUILayout.Width(elementW), GUILayout.Height(elementH));
        }
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
    }
	public static int curD;
	private void CreateElements()
    {
        GUILayout.BeginVertical();
        GUILayout.Space(5);
		GUILayout.BeginHorizontal();

		idScrollerPos = GUILayout.BeginScrollView(idScrollerPos, GUILayout.Width(elementW +10 ));
		for(int i = 0; i< nodeCount;++i)
			GUILayout.Label($"{i + 1}", GUILayout.Width(elementW), GUILayout.Height(elementH));
		GUILayout.EndScrollView();

		scrollerPos = GUILayout.BeginScrollView(scrollerPos);
        for (int i = 0; i < nodeCount; i++)
        {
			GUILayout.BeginHorizontal();
            for (int j = 0; j < objes.Length; j++)
            {
				int d = i * objes.Length + j;
				if (GUILayout.Button(elements[i * objes.Length + j], GUILayout.Width(elementW), GUILayout.Height(elementH)))
				{
					curD = d;
				}
			}
			GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();

		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}
}
public class TestWidow : EditorWindow
{
	public static void AddWindow()
	{
		TestWidow windows = (TestWidow)EditorWindow.GetWindow(typeof(TestWidow), true, "json 编辑（第一节点为数组式的）");
		windows.Show();
	}
    private void OnInspectorUpdate()
    {
		this.Repaint();
        scrollerPos1 = new Vector2(scrollerPos2.x, 0);

    }
    private Vector2 scrollerPos1, scrollerPos2;
    private void OnGUI()
    {
		scrollerPos1 = GUILayout.BeginScrollView(scrollerPos1,false ,false, GUILayout.Height(50));
		for(int i = 0; i <1; i ++)
        {
			GUILayout.BeginHorizontal();
			for (int j = 0; j < 20; j++)
				GUILayout.Label($"控件{j + 1}", GUILayout.Width(100), GUILayout.Height(30));
			GUILayout.EndHorizontal();
        }
		GUILayout.EndScrollView();

		scrollerPos2 = GUILayout.BeginScrollView(scrollerPos2);
		for (int j = 0; j < 1; j++)
		{
			GUILayout.BeginHorizontal();
			for (int i = 0; i < 20; i++)
			{
				GUILayout.Label($"控件{i + 1}", GUILayout.Width(100), GUILayout.Height(30));
			}
			GUILayout.EndHorizontal();
		}
	
		GUILayout.EndScrollView();
	}
}




