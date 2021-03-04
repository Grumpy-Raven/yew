using System.Collections.Generic;

namespace YewLib
{
    public interface IState
    {
        
    }
    
    public class State<T> : IState
    {
        private T val;

        public T Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
                Update();
            }
        }

        public void Update()
        {
            foreach (var s in Subscribers) s.Update();
        }

        public HashSet<IUpdatable> Subscribers { get; set; } = new HashSet<IUpdatable>();

        public static implicit operator T(State<T> obj) => obj.Value;
    }
}