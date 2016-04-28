// Copyright (c) 2010 Bob Berkebile
// Please direct any bugs/comments/suggestions to http://www.pixelplacement.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(iTweenPath))]
public class iTweenPathEditor : Editor
{
	iTweenPath _target;
	GUIStyle style = new GUIStyle();
	public static int count = 0;
	
	void OnEnable(){
		//i like bold handle labels since I'm getting old:
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.white;
		_target = (iTweenPath)target;
		
		//lock in a default path name:
		if(!_target.Initialized){
			_target.Initialized = true;
			_target.PathName = "New Path " + ++count;
			_target.InitialName = _target.PathName;
		}
	}
	
	public override void OnInspectorGUI(){		
		//draw the path?
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Path Visible");
		_target.PathVisible = EditorGUILayout.Toggle(_target.PathVisible);
		EditorGUILayout.EndHorizontal();
		
		//path name:
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Path Name");
		_target.PathName = EditorGUILayout.TextField(_target.PathName);
		EditorGUILayout.EndHorizontal();
		
		if(_target.PathName == ""){
			_target.PathName = _target.InitialName;
		}
		
		//path color:
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Path Color");
		_target.PathColor = EditorGUILayout.ColorField(_target.PathColor);
		EditorGUILayout.EndHorizontal();
		
		//exploration segment count control:
		EditorGUILayout.BeginHorizontal();
		//EditorGUILayout.PrefixLabel("Node Count");
		_target.NodeCount = Mathf.Max(2, EditorGUILayout.IntField("Node Count", _target.NodeCount));
		//_target.nodeCount =  Mathf.Clamp(EditorGUILayout.IntSlider(_target.nodeCount, 0, 10), 2,100);
		EditorGUILayout.EndHorizontal();
		
		//add node?
		if(_target.NodeCount > _target.Nodes.Count){
			for (int i = 0; i < _target.NodeCount - _target.Nodes.Count; i++) {
				_target.Nodes.Add(Vector3.zero);	
			}
		}
				
		//node display:
		EditorGUI.indentLevel = 4;
		for (int i = 0; i < _target.Nodes.Count; i++) {
			_target.Nodes[i] = EditorGUILayout.Vector3Field("Node " + (i+1), _target.Nodes[i]);
		}
		
		//update and redraw:
		if(GUI.changed){
			EditorUtility.SetDirty(_target);			
		}
	}
	
	void OnSceneGUI(){
		if(_target.PathVisible){			
			if(_target.Nodes.Count > 0){
				//allow path adjustment undo:
                Undo.RecordObject(_target, "Adjust iTween Path");
				
				//path begin and end labels:
				Handles.Label(_target.Nodes[0], "'" + _target.PathName + "' Begin", style);
				Handles.Label(_target.Nodes[_target.Nodes.Count-1], "'" + _target.PathName + "' End", style);
				
				//node handle display:
				for (int i = 0; i < _target.Nodes.Count; i++) {
					_target.Nodes[i] = Handles.PositionHandle(_target.Nodes[i], Quaternion.identity);
				}	
			}	
		}
	}
}