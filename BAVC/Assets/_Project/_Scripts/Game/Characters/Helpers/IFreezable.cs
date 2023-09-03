using UnityEngine;

namespace Game
{
    public interface IFreezable
    {
        public void Freeze(float freezeTime, Color freezedColor);
    }
}