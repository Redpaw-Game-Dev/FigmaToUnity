using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static LazyRedpaw.FigmaToUnity.Constants;

namespace LazyRedpaw.FigmaToUnity
{
    [CustomEditor(typeof(FigmaToUnityRequester))]
    public class FigmaToUnityRequesterEditor : Editor
    {
        private SerializedProperty _pathProp;
        private SerializedProperty _isRequestProcessingProp;
        private SerializedProperty _requestProgressTextProp;
        private Button _sendReqButton;
        private Label _pathErrorBox;
        private Button _choosePathButton;
        private TextField _pathTextField;
        private ProgressBar _requestProgressBar;
        
        private void OnEnable()
        {
            _pathProp = serializedObject.FindProperty(SavePathProp);
            _isRequestProcessingProp = serializedObject.FindProperty(IsRequestProcessingProp);
            _requestProgressTextProp = serializedObject.FindProperty(RequestProgressTextProp);
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            VisualTreeAsset tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathHelper.FindFilePath(FigmaToUnityRequesterUXML));
            tree.CloneTree(root);

            _sendReqButton = root.Q<Button>(SendRequestButton);
            _sendReqButton.clicked += TrySendRequest;
            
            _pathErrorBox = root.Q<Label>(PathErrorBox);
            
            _choosePathButton = root.Q<Button>(ChoosePathButton);
            _choosePathButton.clicked += OnChoosePathButtonClicked;
            
            _pathTextField = root.Q<TextField>(SavePathField);
            _pathTextField.RegisterValueChangedCallback(OnPathTextChanged);
            
            _requestProgressBar = root.Q<ProgressBar>(RequestProgressBar);
            _requestProgressBar.RegisterValueChangedCallback(OnProgressBarValueChanged);
            _requestProgressBar.style.display = DisplayStyle.None;
            
            UpdateSendRequestButton();
            
            return root;
        }

        private void OnProgressBarValueChanged(ChangeEvent<float> evt)
        {
            UpdateSendRequestButton();
            _requestProgressBar.title = _requestProgressTextProp.stringValue;
        }

        private void TrySendRequest()
        {
            ((FigmaToUnityRequester)target).TrySendRequest();
            UpdateSendRequestButton();
            _requestProgressBar.style.display = DisplayStyle.Flex;
        }

        private void OnPathTextChanged(ChangeEvent<string> evt)
        {
            UpdateSendRequestButton();
        }

        private void OnChoosePathButtonClicked()
        {
            string selectedPath = EditorUtility.OpenFolderPanel("Select Path", DefaultSavePath, "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                _pathProp.stringValue = selectedPath;
                serializedObject.ApplyModifiedProperties();
                UpdateSendRequestButton();
            }
        }

        private bool ValidatePath()
        {
            if (_pathProp.stringValue.StartsWith(Application.dataPath))
            {
                _pathProp.stringValue = DefaultSavePath + _pathProp.stringValue.Substring(Application.dataPath.Length);
            }
            else if(!_pathProp.stringValue.StartsWith(DefaultSavePath))
            {
                _pathErrorBox.text = "Please select an existing folder within the project directory.";
            }
            else if(!Directory.Exists( _pathProp.stringValue))
            {
                _pathErrorBox.text = "The selected directory does not exist.";
            }

            bool isPathValid = (_pathProp.stringValue.StartsWith(Application.dataPath) ||
                                _pathProp.stringValue.StartsWith(DefaultSavePath)) &&
                               Directory.Exists(_pathProp.stringValue);
            if (isPathValid)
            {
                _pathTextField.RemoveFromClassList(FtuBorderError);
                _pathErrorBox.style.display = DisplayStyle.None;
            }
            else
            {
                _pathTextField.AddToClassList(FtuBorderError);
                _pathErrorBox.style.display = DisplayStyle.Flex;
            }
            return isPathValid;
        }

        private void UpdateSendRequestButton()
        {
            _sendReqButton.SetEnabled(ValidatePath() && !_isRequestProcessingProp.boolValue);
        }
    }
}