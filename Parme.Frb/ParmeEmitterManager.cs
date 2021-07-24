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
        
        public IEmitterLogicMapper EmitterLogicMapper { get; set; }

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

        public ParmeFrbEmitter CreateEmitter(string emitterLogicName, PositionedObject parent, string groupName = null)
        {
            if (EmitterLogicMapper == null)
            {
                const string message = "Cannot resolve emitter logic type by name without an emitter logic mapper instance";
                throw new InvalidOperationException(message);
            }

            var type = EmitterLogicMapper.GetEmitterLogicTypeByName(emitterLogicName);
            if (type == null)
            {
                var message = $"No known emitter logic type can be found with the name '{emitterLogicName}'";
                throw new InvalidOperationException(message);
            }

            var instance = (IEmitterLogic) Activator.CreateInstance(type);
            return CreateEmitter(instance, parent, groupName);
        }

        public void DestroyEmitter(ParmeFrbEmitter emitter, bool waitForAllParticlesToDie = true)
        {
            if (emitter == null) throw new ArgumentNullException(nameof(emitter));

            if (!_emitterToGroupMap.TryGetValue(emitter, out var group))
            {
                // Nothing to do since we don't know anything about the emitter
                return;
            }
            
            group.RemoveEmitter(emitter, waitForAllParticlesToDie);
            _emitterToGroupMap.Remove(emitter);
        }

        // ReSharper disable once MemberCanBePrivate.Global
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

        // ReSharper disable once UnusedMember.Global
        public void DestroyActiveEmitterGroups()
        {
            foreach (var emitterGroup in _emitterGroups.Values)
            {
                SpriteManager.RemoveDrawableBatch(emitterGroup);
            }
            
            _emitterGroups.Clear();
            _emitterToGroupMap.Clear();
        }
    }
}