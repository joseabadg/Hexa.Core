#region Header

// ===================================================================================
// Copyright 2010 HexaSystems Corporation
// ===================================================================================
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// ===================================================================================
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// See the License for the specific language governing permissions and
// ===================================================================================

#endregion Header

namespace Hexa.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public class IoCContainer
    {
        #region Fields

        private readonly Action<Type, object> registerInstanceCallback;
        private readonly Action<Type, Type> registerTypeCallback;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCContainer"/> class.
        /// </summary>
        /// <param name="registerCallback">The register callback.</param>
        public IoCContainer(Action<Type, Type> registerTypeCallback, Action<Type, object> registerInstanceCallback)
        {
            this.registerTypeCallback = registerTypeCallback;
            this.registerInstanceCallback = registerInstanceCallback;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <param name="instance">The instance.</param>
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix"
                         , MessageId = "T"),
        SuppressMessage("Microsoft.Naming",
                         "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "I"),
        SuppressMessage("Microsoft.Design",
                         "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void RegisterInstance<I>(object instance)
        {
            if (this.registerInstanceCallback != null)
            {
                this.registerInstanceCallback(typeof(I), instance);
            }
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instance">The instance.</param>
        public void RegisterInstance(Type @type, object instance)
        {
            if (this.registerInstanceCallback != null)
            {
                this.registerInstanceCallback(@type, instance);
            }
        }

        /// <summary>
        /// Registers a service implementation.
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <typeparam name="T"></typeparam>
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix"
                         , MessageId = "T"),
        SuppressMessage("Microsoft.Naming",
                         "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "I"),
        SuppressMessage("Microsoft.Design",
                         "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void RegisterType<I, T>()
            where T : I
        {
            if (this.registerTypeCallback != null)
            {
                this.registerTypeCallback(typeof(I), typeof(T));
            }
        }

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <param name="interface">The @interface.</param>
        /// <param name="type">The type.</param>
        public void RegisterType(Type @interface, Type @type)
        {
            if (this.registerTypeCallback != null)
            {
                this.registerTypeCallback(@interface, @type);
            }
        }

        #endregion Methods
    }
}