using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Text;

namespace JJEditor
{
    public class JsonEditor : Editor
    {
		[MenuItem("Window/Json 编辑")]
		public static void OpenTabulationWidow()
		{
			TabulationWindow.AddWindow();
		}
		
	}
	public class TabulationWindow : EditorWindow
	{
		public static void AddWindow()
		{
			TabulationWindow windows = (TabulationWindow)EditorWindow.GetWindow(typeof(TabulationWindow), true, "json 编辑（第一节点为数组式的）");
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
		public static Vector2 scrollerPos, objScrollerPos, idScrollerPos;
		public static string curSelectStr = "";
		private StringBuilder JsonContext;
		private Dictionary<int, string> elements = new Dictionary<int, string>();
		private void Awake()
		{
			for (int i = 0; i < nodeCount * objes.Length; ++i)
			{
				elements[i] = "";
			}
			JsonContext = new StringBuilder();
		}
		private void OnGUI()
		{
			CreateFilePath();
			CreateObjectTypes(objes);
			CreateElements();
			GUILayout.Space(10);
			elements[curD] = GUILayout.TextField(elements[curD], GUILayout.Width(position.width - 5), GUILayout.Height(elementH * 3));
			if (GUILayout.Button("创建竖排", GUILayout.Width(100), GUILayout.Height(30)))
			{
                Debug.Log(GetJson());
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
		private void CreateObjectTypes(string[] objNames)
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

			idScrollerPos = GUILayout.BeginScrollView(idScrollerPos, GUILayout.Width(elementW + 10));
			for (int i = 0; i < nodeCount; ++i)
				GUILayout.Label($"{i + 1}", GUILayout.Width(elementW), GUILayout.Height(elementH));
			GUILayout.EndScrollView();

			scrollerPos = GUILayout.BeginScrollView(scrollerPos);
			for (int i = 0; i < nodeCount; i++)
			{
				GUILayout.BeginHorizontal();
				for (int j = 1; j < objes.Length; j++)
				{
					int d = i * objes.Length + j;
					if (GUILayout.Button(elements[d], GUILayout.Width(elementW), GUILayout.Height(elementH)))
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
		private string GetJsonElement(string objName,Type t,string context)
        {
			StringBuilder str = new StringBuilder();
			str.Clear();
			str.Append(@"""");
			str.Append(objName);
			str.Append(@"""");
			str.Append(":");
			if (t.IsAssignableFrom(typeof(string))||t.IsAssignableFrom(typeof(Array)))
				str.Append(@"""");
			str.Append(context);
			if (t.IsAssignableFrom(typeof(string)) || t.IsAssignableFrom(typeof(Array)))
				str.Append(@"""");
			return str.ToString();
		}
		private string GetJsonElement(string objName,  string context)
        {
			string objNa = objName.Substring(0, objName.IndexOf("(")).Replace("\n","");
			string ty = objName.Substring(objName.IndexOf("(") + 1, objName.IndexOf(")") - objName.IndexOf("(")-1);
			Type tp = getType(ty);
			return GetJsonElement(objNa, tp, context);

		}
		private Type getType(string typeName)
		{
			if (typeName == "INT")
				return typeof(int);
			else if (typeName == "STRING")
				return typeof(string);
			else if (typeName == "LIST")
				return typeof(Array);
			else if (typeName == "FLOAT")
				return typeof(float);
			else return null;
		}
		private string GetJsonNode(int id)
		{
			StringBuilder str = new StringBuilder();
			str.Clear();
			str.Append("{");
			str.Append(GetJsonElement(objes[0], id.ToString()));
			
			for (int i = 1; i <objes.Length; ++i)
            {
				str.Append(",");
				str.Append(GetJsonElement(objes[i], elements[(id-1)*objes.Length +i]));
			}
			str.Append("}");
			return str.ToString();
		}
		private string GetJson()
		{
			StringBuilder str = new StringBuilder();
			str.Append("{");
			str.Append(@"""");
			str.Append(title);
			str.Append(@"""");
			str.Append(":");
			str.Append("[");
			for (int i = 1; i <= nodeCount; i++)
            {
				str.Append(GetJsonNode(i));
				if (i < nodeCount)
					str.Append(",");
			}
			str.Append("]");
			str.Append("}");
			return str.ToString();
		}

	}

}

