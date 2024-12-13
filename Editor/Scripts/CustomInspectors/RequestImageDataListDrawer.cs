using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static LazyRedpaw.FigmaToUnity.Constants;

namespace LazyRedpaw.FigmaToUnity
{
    [CustomPropertyDrawer(typeof(RequestImageDataList))]
    public class RequestImageDataListDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            // VisualTreeAsset tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathHelper.FindFilePath(RequestImageDataListUXML));
            VisualTreeAsset tree = Resources.Load<VisualTreeAsset>(RequestImageDataListUXML);
            tree.CloneTree(root);
            
            Button addButton = root.Q<Button>(AddButton);
            addButton.clicked += () => OnAddClicked(property, root);
            
            Button removeButton = root.Q<Button>(RemoveButton);
            removeButton.clicked += () => OnRemoveClicked(property, root);
            
            Button includeAllButton = root.Q<Button>(IncludeAllButton);
            includeAllButton.clicked += () => OnIncludeAllClicked(property);
            
            Button excludeAllButton = root.Q<Button>(ExcludeAllButton);
            excludeAllButton.clicked += () => OnExcludeAllClicked(property);
            
            SerializedProperty dataProp = property.FindPropertyRelative(DataProp);
            root.Q<TextField>(SizeField).value = dataProp.arraySize.ToString();
            return root;
        }
        
        private void OnIncludeAllClicked(SerializedProperty property)
        {
            SerializedProperty dataProp = property.FindPropertyRelative(DataProp);
            for (int i = 0; i < dataProp.arraySize; i++)
            {
                SerializedProperty isIncludedProp = dataProp.GetArrayElementAtIndex(i).FindPropertyRelative(IsIncludedProp);
                if (isIncludedProp != null) isIncludedProp.boolValue = true;
            }
            dataProp.serializedObject.ApplyModifiedProperties();
        }
        
        private void OnExcludeAllClicked(SerializedProperty property)
        {
            SerializedProperty dataProp = property.FindPropertyRelative(DataProp);
            for (int i = 0; i < dataProp.arraySize; i++)
            {
                SerializedProperty isIncludedProp = dataProp.GetArrayElementAtIndex(i).FindPropertyRelative(IsIncludedProp);
                if (isIncludedProp != null) isIncludedProp.boolValue = false;
            }
            dataProp.serializedObject.ApplyModifiedProperties();
        }
        
        private void OnAddClicked(SerializedProperty property, VisualElement root)
        {
            SerializedProperty dataProp = property.FindPropertyRelative(DataProp);
            dataProp.InsertArrayElementAtIndex(dataProp.arraySize);
            SerializedProperty lastItem = dataProp.GetArrayElementAtIndex(dataProp.arraySize - 1);
            lastItem.managedReferenceValue = new NewImageData();
            property.serializedObject.ApplyModifiedProperties();
            root.Q<TextField>(SizeField).value = dataProp.arraySize.ToString();
            root.Q<Foldout>(ListFoldout).value = true;
        }
        
        private void OnRemoveClicked(SerializedProperty property, VisualElement root)
        {
            ListView dataList = root.Q<ListView>(DataList);
            SerializedProperty dataProp = property.FindPropertyRelative(DataProp);
            int[] indexes = dataList.selectedIndices.ToArray().OrderByDescending(num => num).ToArray();
            if (indexes.Length == 0 || dataList.selectedIndex == -1)
            {
                RemoveItemFromList(dataProp, dataProp.arraySize - 1);
            }
            else
            {
                for (int i = 0; i < indexes.Length; i++)
                {
                    RemoveItemFromList(dataProp, indexes[i]);
                }
            }
            property.serializedObject.ApplyModifiedProperties();
            dataList.selectedIndex = -1;
            root.Q<TextField>(SizeField).value = dataProp.arraySize.ToString();
            AssetDatabase.Refresh();
        }

        private void RemoveItemFromList(SerializedProperty dataProp, int index)
        {
            SerializedProperty itemProp = dataProp.GetArrayElementAtIndex(index);
            SerializedProperty needDeleteImageProp = itemProp.FindPropertyRelative(NeedDeleteImageProp);
            if (needDeleteImageProp is { boolValue: true })
            {
                string assetPath = itemProp.FindPropertyRelative(AssetPathProp).stringValue;
                if (!string.IsNullOrEmpty(assetPath)) AssetDatabase.DeleteAsset(assetPath);
            }
            dataProp.DeleteArrayElementAtIndex(index);
        }
    }
}