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

namespace Voxell.Rasa
{
  public class SequencerNode : CompositeNode
  {
    int current;

    protected override void OnStart() => current = 0;

    protected override void OnStop() {}

    protected override State OnUpdate()
    {
      RasaNode child = children[current];
      switch (child.Update())
      {
        case State.Running:
          return State.Running;

        case State.Failure:
          return State.Failure;

        case State.Success:
          current++;
          break;
      }

      return current == children.Count ? State.Success : State.Running;
    }
  }
}