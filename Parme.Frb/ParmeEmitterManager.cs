using System;
using System.Collections.Generic;
using FlatRedBall;
using Parme.CSharp;

namespace Parme.Frb
{
    public class ParmeEmitterManager
    {
        public const string DefaultGroupName = "Default";
        
        private static readonly object Padlock = new object();
        private static ParmeEmitterManager _instance;
        
        private readonly Dictionary<string, ParmeEmitterGroup> _emitterGroups 
            = new Dictionary<string, ParmeEmitterGroup>();
        
        private readonly Dictionary<ParmeFrbEmitter, ParmeEmitterGroup> _emitterToGroupMap
            = new Dictionary<ParmeFrbEmitter, ParmeEmitterGroup>();

        public static ParmeEmitterManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ??= new ParmeEmitterManager();
                }
            } 
        }

        private ParmeEmitterManager()
        {
        }

        public ParmeFrbEmitter CreateEmitter(IEmitterLogic logic, PositionedObject parent, string groupName = null)
        {
            if (logic == null) throw new ArgumentNullException(nameof(logic));
            
            if (string.IsNullOrWhiteSpace(groupName))
            {
                groupName = DefaultGroupName;
            }
            
            var group = GetEmitterGroup(groupName);
            var emitter = group.CreateEmitter(logic, parent);
            _emitterToGroupMap.Add(emitter, group);

            return emitter;
        }

        public void DestroyEmitter(ParmeFrbEmitter emitter, bool waitForAllParticlesToDie = true)
        {
            if (emitter == null) throw new ArgumentNullException(nameof(emitter));

            if (!_emitterToGroupMap.TryGetValue(emitter, out var group))
            {
                const string message = "Attempted to destroy an emitter that's not tracked by the parme manager";
                throw new InvalidOperationException(message);
            }
            
            group.RemoveEmitter(emitter, waitForAllParticlesToDie);
            _emitterToGroupMap.Remove(emitter);
        }

        public ParmeEmitterGroup GetEmitterGroup(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            
            if (!_emitterGroups.TryGetValue(name.Trim(), out var group))
            {
                group = new ParmeEmitterGroup();
                _emitterGroups.Add(name, group);
                SpriteManager.AddDrawableBatch(group);
            }

            return group;
        }
    }
}