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

namespace Voxell.Rasa
{
  [CreateAssetMenu(fileName = "NewRasaTree", menuName = "Rasa/Tree")]
  public class RasaTree : ScriptableObject
  {
    public RootNode rootNode;
    public RasaState treeState = RasaState.Idle;
    public List<RasaNode> rasaNodes = new List<RasaNode>();
    public RasaNode[] cachedNodes;

    public RasaState Update()
    {
      if (rootNode.state == RasaState.Running)
        treeState = rootNode.Update();

      return treeState;
    }

    public void Cache() => cachedNodes = rasaNodes.ToArray();
    public void ClearCache() => Array.Clear(cachedNodes, 0, cachedNodes.Length);
  }
}