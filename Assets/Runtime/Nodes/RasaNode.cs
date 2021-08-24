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

using System.Collections.Generic;
using UnityEngine;
using Voxell.Inspector;

namespace Voxell.Rasa
{
  public abstract class RasaNode : ScriptableObject
  {
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
    [HideInInspector] public List<string> childGuids;
    [InspectOnly] public RasaState state = RasaState.Idle;

    public void Initialize(string name, string guid, Vector2 position)
    {
      this.name = name;
      this.guid = guid;
      this.position = position;
    }

    public RasaState Update()
    {
      if (state == RasaState.Idle)
      {
        OnStart();
        state = RasaState.Running;
      }

      state = OnUpdate();

      if (state == RasaState.Failure || state == RasaState.Success)
      {
        OnStop();
        state = RasaState.Idle;
      }

      return state;
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract RasaState OnUpdate();

    public abstract List<PortInfo> CreateInputPorts();
    public abstract List<PortInfo> CreateOutputPorts();
  }

  public enum RasaState { Idle = 0, Running = 1, Success = 2, Failure = 3 }
  public enum CapacityInfo { Single = 0, Multi = 1 }

  public struct PortInfo
  {
    public PortInfo(CapacityInfo capacityInfo, System.Type type, string name, Color color)
    {
      this.capacityInfo = capacityInfo;
      this.type = type;
      this.name = name;
      this.color = color;
    }

    public PortInfo(CapacityInfo capacityInfo, System.Type type, string name)
    {
      this.capacityInfo = capacityInfo;
      this.type = type;
      this.name = name;
      this.color = Color.white;
    }

    public CapacityInfo capacityInfo;
    public System.Type type;
    public string name;
    public Color color;
  }
}
