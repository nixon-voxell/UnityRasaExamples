/*
This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software Foundation,
Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.

The Original Code is Copyright (C) 2020 Voxell Technologies.
All rights reserved.
*/

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using Voxell.UI;

namespace Voxell.Rasa
{
  public class RasaEditorWindow : EditorWindow
  {
    public string selectedGuid;
    [SerializeField] private RasaTree rasaTree;
    private RasaGraphView graphView;
    private InspectorView inspectorView;

    public void Initialize(string guid, RasaTree rasaTree)
    {
      this.selectedGuid = guid;
      this.rasaTree = rasaTree;
      graphView.PopulateView(rasaTree);
    }

    public void CreateGUI()
    {
      // Each editor window contains a root VisualElement object
      VisualElement root = rootVisualElement;

      // Import UXML
      VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/RasaEditorWindow.uxml");
      visualTree.CloneTree(root);

      // A stylesheet can be added to a VisualElement.
      // The style will be applied to the VisualElement and all of its children.
      StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/RasaEditorWindow.uss");
      root.styleSheets.Add(styleSheet);

      graphView = root.Q<RasaGraphView>();
      inspectorView = root.Q<InspectorView>();

      if (rasaTree != null) graphView.PopulateView(rasaTree);
    }

    public override void SaveChanges()
    {
      base.SaveChanges();
      AssetDatabase.SaveAssets();
    }
  }
}