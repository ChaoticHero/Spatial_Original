using UnityEngine;
using System.Collections;

namespace SpatialPartitionPattern
{

    public class Soldier
    {

        public MeshRenderer soldierMeshRenderer;

        public Transform soldierTrans;

        protected float walkSpeed;

        public Soldier previousSoldier;
        public Soldier nextSoldier;

        public virtual void Move()
        { }
        public virtual void Move(Soldier soldier)
        { }
    }
}