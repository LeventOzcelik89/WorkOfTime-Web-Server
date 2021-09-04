using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Infoline.Data
{
    [Serializable]
    public class Entity : INotifyPropertyChanged, INotifyPropertyChanging
    {
        internal List<string> changedProperties;
        internal WeakReference Context { get; set; }


        public virtual Guid id { get; set; }
        public virtual string k_id { get; set; }
    
        //  public virtual DateTime time { get; set; }
        private EntityState _state;

        public EntityState State
        {
            get { return _state; }
            internal set
            {
                if (_state != value)
                {
                    _state = value;

                    if (Context != null && Context.IsAlive && Context.Target is IObjectTracker)
                    {
                        State = value;
                        (Context.Target as IObjectTracker).TrackObject(this);
                    }
                }
            }
        }
        public Entity Orginal { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyChange(string property)
        {

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        protected void NotifyChanging(string property)
        {
            if (State == EntityState.Deleted)
                throw new Exception("Cannot modify deleted object!!");
            if (State == EntityState.UnModified)
            {
                State = EntityState.Modified;
                changedProperties = new List<string>();
                changedProperties.Add(property);
                Orginal = MemberwiseClone() as Entity;
            }
            else if (State == EntityState.Modified)
                if (!changedProperties.Contains(property))
                    changedProperties.Add(property);

            if (PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(property));
        }
        public void Delete()
        {
            State = EntityState.Deleted;
        }

        public event PropertyChangingEventHandler PropertyChanging;

    }
    interface IObjectTracker
    {
        void TrackObject(Entity entity);
    }

    public enum EntityState
    {
        New,
        UnModified,
        Modified,
        Deleted
    }

    //public class NamedEntity : Entity
    //{
    //    public virtual string adi { get; set; }
    //}
    //public interface IEntity
    //{
    //    bool id { get; set; }
    //}

}
