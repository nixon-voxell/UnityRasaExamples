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
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using Voxell.Rasa;

namespace Voxell.UI
{
  public class RasaNodeView : Node
  {
    public Action<RasaNodeView> OnNodeSelected;
    public Action OnNodeUnSelected;
    public RasaNode node;
    public List<PortInfo> inputPorts;
    public List<PortInfo> outputPorts;

    public RasaNodeView(RasaNode node)
    {
      this.node = node;
      this.title = node.name;
      this.viewDataKey = node.guid;

      style.left = node.position.x;
      style.top = node.position.y;

      inputPorts = node.CreateInputPorts();
      outputPorts = node.CreateOutputPorts();

      for (int ip=0; ip < inputPorts.Count; ip++)
      {
        Port port = InstantiatePort(
          Orientation.Horizontal,
          Direction.Input,
          (Port.Capacity)inputPorts[ip].capacityInfo,
          inputPorts[ip].type
        );
        port.portColor = inputPorts[ip].color;
        port.portName = inputPorts[ip].name;
        inputContainer.Add(port);
      }

      for (int op=0; op < outputPorts.Count; op++)
      {
        Port port = InstantiatePort(
          Orientation.Horizontal,
          Direction.Output,
          (Port.Capacity)outputPorts[op].capacityInfo,
          outputPorts[op].type
        );
        port.portColor = outputPorts[op].color;
        port.portName = outputPorts[op].name;
        outputContainer.Add(port);
      }
    }

    public override void SetPosition(Rect newPos)
    {
      base.SetPosition(newPos);
      Undo.RecordObject(node, "Rasa Tree (Set Node Position)");
      node.position.x = newPos.xMin;
      node.position.y = newPos.yMin;
      EditorUtility.SetDirty(node);
    }

    public override void OnSelected()
    {
      base.OnSelected();
      OnNodeSelected?.Invoke(this);
    }

    public override void OnUnselected()
    {
      base.OnUnselected();
      OnNodeUnSelected?.Invoke();
    }
  }
}