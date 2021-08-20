using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace Voxell.Rasa
{
  public class NodeView : Node
  {
    public RasaNode node;
    public Port input;
    public Port output;

    public NodeView(RasaNode node)
    {
      this.node = node;
      this.title = node.name;
      this.viewDataKey = node.guid;

      style.left = node.position.x;
      style.top = node.position.y;

      CreateInputPorts();
      CreateOutputPorts();
    }

    private void CreateInputPorts()
    {
      if (node is ActionNode)
        input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
      else if (node is CompositeNode)
        input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
      else if (node is DecoratorNode)
        input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));

      if (input != null)
      {
        input.portName = "";
        inputContainer.Add(input);
      }
    }

    private void CreateOutputPorts()
    {
      if (node is ActionNode) {}
      else if (node is CompositeNode)
        output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
      else if (node is DecoratorNode)
        output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));

      if (output != null)
      {
        output.portName = "";
        outputContainer.Add(output);
      }
    }

    public override void SetPosition(Rect newPos)
    {
      base.SetPosition(newPos);
      Undo.RecordObject(node, "Rasa Tree (Set Position)");
      node.position.x = newPos.xMin;
      node.position.y = newPos.yMin;
      EditorUtility.SetDirty(node);
    }
  }
}