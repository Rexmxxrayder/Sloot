using UnityEngine;

public interface IEntityPhysics
{
    public Vector3 Velocity { get; }
    public Vector3 Direction{ get; }

    #region Get/Set

    public void Add(Force force);
    public void Remove(Force force);

    #endregion

    #region ForcesFunction

    public Vector3 ComputeForces();
    public void Purge();
    #endregion

}
