using Game.Common.Map.PylonMap;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace Game.Common.Map.Editor
{
    [EditorTool("Pylon Connection Tool")]
    public class PylonConnectorTool : EditorTool
    {
        [SerializeField] Texture2D m_ToolIcon;

        GUIContent m_IconContent;
        
        void OnEnable() {
            m_IconContent = new GUIContent() {
                image = m_ToolIcon,
                text = "Platform Tool",
                tooltip = "Platform Tool"
            };
            Selection.selectionChanged += SelectionChange;
        }

           public void SelectionChange() {
               
            if (Tools.current != Tool.Custom) {
                lastSelectedTransform = null;
                return;
            }
            
            
            
            var selected = Selection.activeTransform;
            if (selected != null && selected.GetComponent<Pylon>() != null) {

                if (lastSelectedTransform != null && lastSelectedTransform != selected) {
                    Debug.Log("Selected " + lastSelectedTransform.name + " to " +  selected.name);
                    
                    var pylonConnectorFrom = lastSelectedTransform.GetComponent<Pylon>();
                    var pylonConnector = selected.GetComponent<Pylon>();
                    
                    if (pylonConnector.ConnectedTo != null)
                    {
                        if (pylonConnector.ConnectedTo == pylonConnectorFrom)
                        {
                            pylonConnector.ConnectedTo = null;
                        }
                        else if (pylonConnectorFrom.ConnectedTo == pylonConnector)
                        {
                            pylonConnectorFrom.ConnectedTo = null;
                        }
                        else
                        {
                            if (pylonConnector.ConnectedTo == null)
                            {
                                pylonConnector.ConnectedTo = pylonConnectorFrom;
                            }
                            else if (pylonConnectorFrom.ConnectedTo == null)
                            {
                                pylonConnectorFrom.ConnectedTo = pylonConnector;
                            }
                            else
                            {
                                if (!FixConnections(pylonConnector, pylonConnectorFrom, pylonConnector))
                                {
                                    if (!FixConnections(pylonConnectorFrom, pylonConnector, pylonConnectorFrom))
                                    {
                                        pylonConnectorFrom.ConnectedTo = pylonConnector;
                                    }
                                }
                            }
                        }
                        
                    } else {
                        
                        if (pylonConnector.ConnectedTo == null)
                        {
                            pylonConnector.ConnectedTo = pylonConnectorFrom;
                        }
                        else if (pylonConnectorFrom.ConnectedTo == null)
                        {
                            pylonConnectorFrom.ConnectedTo = pylonConnector;
                        }
                        else
                        {
                            if (!FixConnections(pylonConnector, pylonConnectorFrom, pylonConnector))
                            {
                                if (!FixConnections(pylonConnectorFrom, pylonConnector, pylonConnectorFrom))
                                {
                                    pylonConnectorFrom.ConnectedTo = pylonConnector;
                                }
                            }
                        }
                    }
                        
                    EditorUtility.SetDirty(pylonConnector);
                    EditorUtility.SetDirty(pylonConnectorFrom);
                    Undo.RecordObject(pylonConnector, "Changed Connection");
                    Undo.RecordObject(pylonConnectorFrom, "Changed Connection");

                    Deselect = true;
                    
                    lastSelectedTransform = null;
                } else {
                    lastSelectedTransform = selected;
                    
                }
                    
            } else {

                lastSelectedTransform = null;
            }
        }

           public bool FixConnections(Pylon pylon, Pylon target, Pylon start)
           {
               if (pylon == null)
               {
                   return true;
               }

               if (pylon == start)
               {
                   return false;
               }
               // Go down the line, because this will override something

               if (FixConnections(pylon.ConnectedTo, null, start))
               {
                   if (pylon.ConnectedTo != null)
                   {
                       pylon.ConnectedTo.ConnectedTo = pylon;
                       
                       EditorUtility.SetDirty(pylon);
                       Undo.RecordObject(pylon, "Changed Connection");
                   }

                   if (target)
                   {
                       pylon.ConnectedTo = target;
                       
                       EditorUtility.SetDirty(pylon);
                       Undo.RecordObject(pylon, "Changed Connection");
                   }
                   return true;
               }

               return false;
           }
           
        public override GUIContent toolbarIcon {
            get { return m_IconContent; }
        }

        private bool Deselect = false;

        private Transform lastSelectedTransform;
        
        // This is called for each window that your tool is active in. Put the functionality of your tool here.
        public override void OnToolGUI(EditorWindow window) {
            if (Deselect) {
                Selection.objects = new Object[0];
                Selection.activeTransform = null;
                Selection.activeObject = null;
                Selection.activeGameObject = null;
                Deselect = false;
            }
            if (lastSelectedTransform != null) {
                Handles.color = Color.red;
                Handles.DrawSolidDisc(lastSelectedTransform.position, -Vector3.forward, 0.15f);
            }
            
        }
        

        private void OnDisable() {
            if (Selection.selectionChanged != null) Selection.selectionChanged -= SelectionChange;
        }
    }
}