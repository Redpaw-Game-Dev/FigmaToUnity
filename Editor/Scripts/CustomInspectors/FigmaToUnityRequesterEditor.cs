using UnityEditor;
using UnityEngine;

namespace LazyRedpaw.FigmaToUnity
{
    [CustomEditor(typeof(FigmaToUnityRequester), true)]
    [CanEditMultipleObjects]
    public class FigmaToUnityRequesterEditor : Editor
    {
        private const float SavePathButtonWidth = 100f;
        private const string DefaultSavePath = "Assets";
        private readonly float _defaultLabelWidth = EditorGUIUtility.labelWidth;

        private SerializedProperty _figmaTokenProp;
        private SerializedProperty _pathProp;
        private SerializedProperty _requestImageDataListProp;
        private SerializedProperty _progressProp;
        private SerializedProperty _progressTextProp;
        private SerializedProperty _isRequestProcessingProp;

        private void OnEnable()
        {
            _figmaTokenProp = serializedObject.FindProperty("_figmaToken");
            _pathProp = serializedObject.FindProperty("_savePath");
            _requestImageDataListProp = serializedObject.FindProperty("_requestImageDataList");
            _progressProp = serializedObject.FindProperty("_requestProgress");
            _progressTextProp = serializedObject.FindProperty("_requestProgressText");
            _isRequestProcessingProp = serializedObject.FindProperty("_isRequestProcessing");
            _progressProp.floatValue = -1f;
            serializedObject.ApplyModifiedProperties();
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUIUtility.labelWidth = SavePathButtonWidth;
            EditorGUILayout.PropertyField(_figmaTokenProp);
            EditorGUIUtility.labelWidth = _defaultLabelWidth;
            
            GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Choose Path", "Choose file save path"), GUILayout.Width(SavePathButtonWidth)))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("Select Path", DefaultSavePath, "");
                if (!string.IsNullOrEmpty(selectedPath)) _pathProp.stringValue = selectedPath;
            }
            _pathProp.stringValue = EditorGUILayout.TextField(_pathProp.stringValue);
            EditorGUILayout.EndHorizontal();
            bool isPathValid = _pathProp.stringValue.StartsWith(Application.dataPath) ||
                               _pathProp.stringValue.StartsWith(DefaultSavePath);
            if (_pathProp.stringValue.StartsWith(Application.dataPath))
            {
                _pathProp.stringValue = DefaultSavePath + _pathProp.stringValue.Substring(Application.dataPath.Length);
            }
            else if(!_pathProp.stringValue.StartsWith(DefaultSavePath))
            {
                string errorMessage = "Please select a folder within the project directory.";
                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
            }

            if (!isPathValid || _isRequestProcessingProp.boolValue) GUI.enabled = false;
            if (GUILayout.Button(new GUIContent("Send Request", "Send web request to get images")))
            {
                ((FigmaToUnityRequester)target).TrySendRequest();
            }
            GUI.enabled = true;
            if (_progressProp.floatValue >= 0f)
            {
                EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(), _progressProp.floatValue,
                    $"{_progressTextProp.stringValue} {Mathf.RoundToInt(_progressProp.floatValue * 100f)}%");
            }
            
            EditorGUILayout.PropertyField(_requestImageDataListProp);
            serializedObject.ApplyModifiedProperties();
        }
    }
}

