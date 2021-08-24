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

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Searcher;
using UnityEditor.Experimental.GraphView;
using Voxell.Rasa;

namespace Voxell.UI
{
  public class RasaGraphView : GraphView
  {
    public new class UxmlFactory : UxmlFactory<RasaGraphView, GraphView.UxmlTraits> {}
    public Action<RasaNodeView> OnNodeSelected;
    public Action OnNodeUnSelected;
    public RasaTree rasaTree;

    private EditorWindow _editorWindow;
    private Vector2 _mousePosition;
    private SearcherItem[] _searcherItems;
    private Type[] _nodeTypes;

    public RasaGraphView()
    {
      Insert(0, new GridBackground());

      this.AddManipulator(new ContentZoomer());
      this.AddManipulator(new ContentDragger());
      this.AddManipulator(new SelectionDragger());
      this.AddManipulator(new SelectionDropper());
      this.AddManipulator(new RectangleSelector());

      StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/RasaEditorWindow.uss");
      styleSheets.Add(styleSheet);

      // add undo redo for adding and removing nodes
      Undo.undoRedoPerformed += PopulateView;
      // when space bar is pressed
      nodeCreationRequest = NodeCreationRequest;
    }

    internal void Initialize(RasaTree rasaTree, EditorWindow editorWindow)
    {
      this.rasaTree = rasaTree;
      _editorWindow = editorWindow;
    }

    internal void PopulateView()
    {
      if (rasaTree != null)
      {
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;
        rasaTree.rasaNodes.ForEach(n => CreateNodeView(n));
      }
    }

    private void NodeCreationRequest(NodeCreationContext c)
    {
      _mousePosition = c.screenMousePosition - _editorWindow.position.position;
      _nodeTypes = TypeCache.GetTypesDerivedFrom<RasaNode>().ToArray();
      _searcherItems = new SearcherItem[_nodeTypes.Length];

      for (int t=0; t < _nodeTypes.Length; t++)
      {
        System.Type type = _nodeTypes[t];
        _searcherItems[t] = new SearcherItem(type.Name.Replace("Node", ""));
      }

      // only display the search window when current graph view is focused
      if (EditorWindow.focusedWindow == _editorWindow)
      {
        SearcherWindow.Show(
          _editorWindow,
          _searcherItems, "Create Rasa Node",
          OnSearcherSelectEntry,
          _mousePosition
        );
      }
    }

    private bool OnSearcherSelectEntry(SearcherItem item)
    {
      if (item != null)
      {
        RasaNode rasaNode = ScriptableObject.CreateInstance(_nodeTypes[item.Id]) as RasaNode;
        CreateNode(rasaNode);
      }

      return true;
    }

    private void CreateNode(RasaNode rasaNode)
    {
      rasaNode.Initialize(
        ObjectNames.NicifyVariableName(rasaNode.GetType().Name),
        GUID.Generate().ToString(),
        contentViewContainer.WorldToLocal(_mousePosition)
      );

      Undo.RecordObject(rasaTree, "Rasa Tree (Add Node)");
      EditorUtility.SetDirty(rasaTree);
      rasaTree.rasaNodes.Add(rasaNode);
      CreateNodeView(rasaNode);
    }

    private void CreateNodeView(RasaNode node)
    {
      RasaNodeView nodeView = new RasaNodeView(node);
      nodeView.OnNodeSelected = OnNodeSelected;
      nodeView.OnNodeUnSelected = OnNodeUnSelected;
      AddElement(nodeView);
      // select the new node so that the inspector shows the content of the node
      ClearSelection();
      AddToSelection(nodeView);
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
      Undo.RecordObject(rasaTree, "Rasa Tree (Remove Node)");
      EditorUtility.SetDirty(rasaTree);
      if (graphViewChange.elementsToRemove != null)
      {
        graphViewChange.elementsToRemove.ForEach(elem =>
        {
          RasaNodeView nodeView = elem as RasaNodeView;
          if (nodeView != null) rasaTree.rasaNodes.Remove(nodeView.rasaNode);
        });
      }
      return graphViewChange;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
      => ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
      Vector2 mousePosition = evt.mousePosition + _editorWindow.position.position;
      evt.menu.InsertAction(0, "Create Node", (a) =>
      {
        NodeCreationRequest(new NodeCreationContext
        {
          screenMousePosition = mousePosition,
          target = this,
          index = -1
        });
      });
    }
  }
}