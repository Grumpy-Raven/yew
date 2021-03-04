using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YewLib
{
    public class Runtime : MonoBehaviour
    {
        public static bool HasInstance { get; private set; }
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
            HasInstance = true;
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

            if (nodeUpdates.Any())
            {
                var localNodeUpdates = nodeUpdates.ToArray();
                nodeUpdates.Clear();
                updatedNodes.Clear();
                foreach(var node in localNodeUpdates)
                    node.Update();
            }
        }

        private HashSet<Node> nodeUpdates = new HashSet<Node>();
        private HashSet<Node> updatedNodes = new HashSet<Node>();
        public void QueueForUpdate(Node node)
        {
            nodeUpdates.Add(node);
        }

        public void MarkUpdated(Node node)
        {
            updatedNodes.Add(node);
        }

        public bool HasBeenUpdatedThisFrame(Node node) => updatedNodes.Contains(node);
    }
}