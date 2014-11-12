using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    /// <summary>
    /// Represents an object implementing <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    [DataContract]
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Notifies about changes in a specified property.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Backing field.
        /// </summary>
        private string _Name;

        /// <summary>
        /// The <see cref="ObservableObject"/>s name.
        /// </summary>
        [DataMember]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                this.SetProperty(ref _Name, value);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="ObservableObject"/>.
        /// </summary>
        protected ObservableObject() { }

        /// <summary>
        /// Initializes a new <see cref="ObservableObject"/> and sets the name.
        /// </summary>
        /// <param name="name">The <see cref="ObservableObject"/>s name.</param>
        protected ObservableObject(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Invokes the specified delegate via late binding. See remarks.
        /// </summary>
        /// <remarks>Late binding is slow, avoid this method wherever possible.</remarks>
        /// <param name="del">The <see cref="Delegate"/> to invoke.</param>
        /// <param name="parameters">Delegate parameters.</param>
        protected void Raise(Delegate del, params object[] parameters)
        {
            Contract.Requires<ArgumentNullException>(parameters != null);

            if (del != null)
            {
                del.DynamicInvoke(parameters);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/>-event for the specified property name.
        /// </summary>
        /// <param name="propertyName">
        /// The property name that changed. Leave this blank, it will be filled out by the compiler.
        /// </param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            RaisePropertyChanged(this, this.PropertyChanged, propertyName);
        }

        /// <summary>
        /// Sets the property with the specified name and raises the <see cref="E:PropertyChanged"/>-event.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the property that changed.</typeparam>
        /// <param name="location">The property's backing field.</param>
        /// <param name="newValue">The property's new value.</param>
        /// <param name="propertyName">
        /// The property name that changed. Leave this blank, it will be filled out by the compiler.
        /// </param>
        protected void SetProperty<T>(ref T location, T newValue, [CallerMemberName] string propertyName = null)
        {
            SetProperty(this, this.PropertyChanged, ref location, newValue, propertyName);
        }

        /// <summary>
        /// Raises the property changed event on the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="sender">The object whose property was changed.</param>
        /// <param name="handler">The <see cref="PropertyChangedEventHandler"/> to raise.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        public static void RaisePropertyChanged(object sender, PropertyChangedEventHandler handler, [CallerMemberName] string propertyName = null)
        {
            if (handler != null)
            {
                handler.Invoke(sender, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Sets the property to the new value and raises the property changed event.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the property to set.</typeparam>
        /// <param name="sender">The object whose property was changed.</param>
        /// <param name="handler">The <see cref="PropertyChangedEventHandler"/> to raise.</param>
        /// <param name="location">The backing field of the property to set.</param>
        /// <param name="newValue">The properties new value.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        public static void SetProperty<T>(object sender, PropertyChangedEventHandler handler, ref T location, T newValue, [CallerMemberName] string propertyName = null)
        {
            location = newValue;
            RaisePropertyChanged(sender, handler, propertyName);
        }
    }
}
