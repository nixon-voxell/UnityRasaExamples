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

using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using Voxell.Rasa;

namespace Voxell.UI
{
  public class RasaGraphView : GraphView
  {
    public new class UxmlFactory : UxmlFactory<RasaGraphView, GraphView.UxmlTraits> {}
    [SerializeField] private RasaTree rasaTree;

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

      // DEBUG!
      Undo.undoRedoPerformed += () => PopulateView(rasaTree);
    }

    internal void PopulateView(RasaTree rasaTree)
    {
      this.rasaTree = rasaTree;

      graphViewChanged -= OnGraphViewChanged;
      DeleteElements(graphElements);
      graphViewChanged += OnGraphViewChanged;
      rasaTree.nodes.ForEach(n => CreateNodeView(n));
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
      => ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
      {
        TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<ActionNode>();

        for (int t=0; t < types.Count; t++)
        {
          System.Type type = types[t];
          evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
        }
      }

      {
        TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<CompositeNode>();

        for (int t=0; t < types.Count; t++)
        {
          System.Type type = types[t];
          evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
        }
      }

      {
        TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();

        for (int t=0; t < types.Count; t++)
        {
          System.Type type = types[t];
          evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
        }
      }
    }

    private void CreateNode(System.Type type)
    {
      RasaNode node = ScriptableObject.CreateInstance(type) as RasaNode;
      node.name = type.Name;
      node.guid = GUID.Generate().ToString();
      rasaTree.nodes.Add(node);

      AssetDatabase.AddObjectToAsset(node, rasaTree);
      CreateNodeView(node);
    }

    private void CreateNodeView(RasaNode node)
    {
      NodeView nodeView = new NodeView(node);
      AddElement(nodeView);
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
      if (graphViewChange.elementsToRemove != null)
      {
        graphViewChange.elementsToRemove.ForEach(elem =>
        {
          NodeView nodeView = elem as NodeView;
          if (nodeView != null)
          {
            rasaTree.nodes.Remove(nodeView.node);
            AssetDatabase.RemoveObjectFromAsset(nodeView.node);
          }
        });
      }
      return graphViewChange;
    }
  }
}