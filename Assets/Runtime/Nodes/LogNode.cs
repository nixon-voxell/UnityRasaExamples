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

namespace Voxell.Rasa
{
  public class LogNode : RasaNode
  {
    public string message;

    private static List<PortInfo> inputPortInfos = new List<PortInfo>
    {
      new PortInfo(CapacityInfo.Single, typeof(bool), "Flow", Color.red),
      new PortInfo(CapacityInfo.Single, typeof(string), "Message", Color.green)
    };

    private static List<PortInfo> outputPortInfos = new List<PortInfo>
    {
      new PortInfo(CapacityInfo.Multi, typeof(bool), "Flow", Color.red)
    };

    protected override void OnStart() => Debug.Log(message);
    protected override RasaState OnUpdate() => RasaState.Success;
    protected override void OnStop() {}

    public override List<PortInfo> CreateInputPorts() => inputPortInfos;
    public override List<PortInfo> CreateOutputPorts() => outputPortInfos;
  }
}