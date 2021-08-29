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
using Voxell.Inspector;

namespace Voxell.Rasa
{
  public abstract class RasaNode : ScriptableObject
  {
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
    [InspectOnly] public RasaState state = RasaState.Idle;

    public static string pathName = "Base";
    public Dictionary<string, List<Connection>> inputNodes;

    public virtual void OnEnable()
    {
      if (inputNodes == null)
        inputNodes = new Dictionary<string, List<Connection>>();
    }

    public void Initialize(string name, string guid, Vector2 position)
    {
      this.name = name;
      this.guid = guid;
      this.position = position;
    }

    public abstract List<PortInfo> CreateInputPorts();
    public abstract List<PortInfo> CreateOutputPorts();

    public abstract bool OnAddInputPort(RasaNode rasaNode, Type portType, string portName);
    public abstract bool OnRemoveInputPort(RasaNode rasaNode, Type portType, string portName);
    public abstract bool OnAddOutputPort(RasaNode rasaNode, Type portType, string portName);
    public abstract bool OnRemoveOutputPort(RasaNode rasaNode, Type portType, string portName);
  }
}
