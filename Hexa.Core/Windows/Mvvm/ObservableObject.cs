#if !MONO

// Event Design: http://msdn.microsoft.com/en-us/library/ms229011.aspx
namespace Hexa.Core.Windows.Mvvm
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq.Expressions;

    [Serializable]
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region Events

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Methods

        /// <summary>
        /// Warns the developer if this Object does not have a public property with
        /// the specified name. This method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // verify that the property name matches a real,
            // public, instance property on this Object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                Debug.Fail("Invalid property name: " + propertyName);
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpresssion)
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
            this.RaisePropertyChanged(propertyName);
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #endregion Methods
    }
}

#endif