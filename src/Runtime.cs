using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YewLib
{
    public class Runtime : MonoBehaviour
    {
        public static Runtime Instance { get; set; }
        // Start is called before the first frame update
        public float msPerFrame = 16;

        List<Action> rafs = new List<Action>();
        public void RequestAnimationFrame(Action raf)
        {
            rafs.Add(raf);
        }
        
        void Start()
        {
            Instance = this;
        }

        private float timer = 0;
        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            if (timer > (msPerFrame / 1_000))
            {
                // We could use some proper scheduler logic here.
                timer = 0;
                var frameRafs = rafs.ToArray();
                rafs.Clear();
                foreach (var raf in frameRafs)
                {
                    raf();
                }
            }
        }
    }
}