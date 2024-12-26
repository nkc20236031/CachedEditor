#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace RizeLibrary.Attribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CachedEditorAttribute : PropertyAttribute { }

    [CustomPropertyDrawer(typeof(CachedEditorAttribute))]
    public class CachedEditorDrawer : PropertyDrawer
    {
        private static readonly Color LineColor = new(0.5f, 0.5f, 0.5f, 1);
        private readonly int _indentLevel = EditorGUI.indentLevel * 15;
        private bool _isFoldoutOpen;
        private Editor _editor;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference || property.objectReferenceValue == null)
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property, label, true);

            // オブジェクトのキャッシュされたエディターを描画 / Draw the cached editor for the object
            DrawCachedObjectEditor(property.objectReferenceValue);

            EditorGUI.EndProperty();

            // 開いている場合、下部に境界線を追加 / If open, add a border at the bottom
            AddSeparatorIfNeeded();
        }

        /// <summary>
        /// 指定されたオブジェクトのキャッシュされたエディターを描画する / Draws the cached editor for the given object
        /// </summary>
        /// <param name="targetObject">描画させるオブジェクト / Object to be drawn</param>
        private void DrawCachedObjectEditor(Object targetObject)
        {
            if (targetObject == null) { return; }
            
            // インデントを追加 /　Add an indent
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, new GUIStyle("IN Title"));
            rect.x += _indentLevel; 
            rect.width -= _indentLevel;
            
            // 開閉状態を切り替え / Switches between open and closed states
            _isFoldoutOpen = EditorGUI.InspectorTitlebar(rect, _isFoldoutOpen, targetObject, true);
            if (_isFoldoutOpen)
            {
                EditorGUI.indentLevel++;

                // キャッシュされたエディターを作成 / Create a cached editor
                Editor.CreateCachedEditor(targetObject, null, ref _editor);

                // 最後に追加されたエディターを描画 / Draw the last added editor
                DrawLastCachedEditor();

                EditorGUI.indentLevel--;
            }
        }

        /// <summary>
        /// 最後に追加されたキャッシュされたエディターを描画し、シーンの再描画を処理する / Draws the last added cached editor and handles scene repaint
        /// </summary>
        private void DrawLastCachedEditor()
        {
            // 最後のキャッシュされたエディターのインスペクターGUIを描画 / Draw the inspector GUI for the last cached editor
            _editor.OnInspectorGUI();

            // 変更を確認し、必要に応じてシーンを再描画 / Check for changes and repaint scene if needed
            using var changeCheck = new EditorGUI.ChangeCheckScope();
            if (changeCheck.changed)
            {
                SceneView.RepaintAll();
            }
        }

        /// <summary>
        /// 開いている場合に線を追加する / Adds a line if open
        /// </summary>
        private void AddSeparatorIfNeeded()
        {
            if (_isFoldoutOpen)
            {
                // 線を追加する / Add a line
                GUILayout.Space(1f);
                Rect lineRect = EditorGUILayout.GetControlRect(false, 2f);
                lineRect.x += _indentLevel;
                lineRect.width -= _indentLevel;
                EditorGUI.DrawRect(lineRect, LineColor);
            }
            
            GUILayout.Space(15f);
        }
    }
}
#endif