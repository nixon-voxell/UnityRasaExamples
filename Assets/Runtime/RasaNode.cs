using UnityEngine;

namespace Voxell.Rasa
{
  public abstract class RasaNode : ScriptableObject
  {
    public enum State
    {
      Running,
      Success,
      Failure
    }

    public State state = State.Running;
    public bool started = false;
    public string guid;
    public Vector2 position;

    public State Update()
    {
      if (!started)
      {
        OnStart();
        started = true;
      }

      state = OnUpdate();

      if (state == State.Failure || state == State.Success)
      {
        OnStop();
        started = false;
      }

      return state;
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
  }
}
