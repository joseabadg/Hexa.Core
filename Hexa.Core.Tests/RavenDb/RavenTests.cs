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

#if !MONO

namespace Hexa.Core.Tests.Raven
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Core.Data;
    using Core.Domain;

    using Data;

    using Domain;

    using Logging;

    using NUnit.Framework;

    using Validation;

    [TestFixture]
    public class RavenTests
    {
        #region Methods

        [Test]
        public void Add_Human()
        {
            Human human = this._Add_Human();

            Assert.IsNotNull(human);
            //Assert.IsNotNull(human.Version);
            Assert.IsFalse(human.UniqueId == Guid.Empty);
            Assert.AreEqual("Martin", human.Name);

            //return human.UniqueId;
        }

        [Test]
        public void Delete_Human()
        {
            Human human = this._Add_Human();

            using (IUnitOfWork ctx = UnitOfWorkScope.Start())
            {
                var repo = ServiceLocator.GetInstance<IHumanRepository>();
                IEnumerable<Human> results = repo.GetFilteredElements(u => u.UniqueId == human.UniqueId);
                Assert.IsTrue(results.Count() > 0);

                Human human2Delete = results.First();

                repo.Remove(human2Delete);

                ctx.Commit();
            }

            using (IUnitOfWork ctx = UnitOfWorkScope.Start())
            {
                var repo = ServiceLocator.GetInstance<IHumanRepository>();
                Assert.AreEqual(0, repo.GetFilteredElements(u => u.UniqueId == human.UniqueId).Count());
            }
        }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            ApplicationContext.Start("Data");

            // Validator and TraceManager
            IoCContainer container = ApplicationContext.Container;
            container.RegisterInstance<ILoggerFactory>(new Log4NetLoggerFactory());

            // Context Factory
            var ctxFactory = new RavenUnitOfWorkFactory();

            container.RegisterInstance<IUnitOfWorkFactory>(ctxFactory);
            container.RegisterInstance<IDatabaseManager>(ctxFactory);

            // Repositories
            container.RegisterType<IHumanRepository, HumanRepository>();

            // Services

            if (!ctxFactory.DatabaseExists())
            {
                ctxFactory.CreateDatabase();
            }

            ctxFactory.ValidateDatabaseSchema();

            ctxFactory.RegisterSessionFactory(container);
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            var dbManager = ServiceLocator.GetInstance<IDatabaseManager>();
            dbManager.DeleteDatabase();

            ApplicationContext.Stop();
        }

        [Test]
        public void Query_Human()
        {
            Human human = this._Add_Human();

            using (IUnitOfWork ctx = UnitOfWorkScope.Start())
            {
                var repo = ServiceLocator.GetInstance<IHumanRepository>();
                IEnumerable<Human> results = repo.GetFilteredElements(u => u.UniqueId == human.UniqueId);
                Assert.IsTrue(results.Count() > 0);

                results = repo.GetFilteredElements(u => u.isMale);
                Assert.IsTrue(results.Count() > 0);
            }
        }

        [Test]
        public void Update_Human()
        {
            Human human = this._Add_Human();

            using (IUnitOfWork ctx = UnitOfWorkScope.Start())
            {
                var repo = ServiceLocator.GetInstance<IHumanRepository>();
                IEnumerable<Human> results = repo.GetFilteredElements(u => u.UniqueId == human.UniqueId);
                Assert.IsTrue(results.Count() > 0);

                Human human2Update = results.First();
                human2Update.Name = "Maria";
                repo.Modify(human2Update);

                Thread.Sleep(1000);

                ctx.Commit();
            }

            using (IUnitOfWork ctx = UnitOfWorkScope.Start())
            {
                var repo = ServiceLocator.GetInstance<IHumanRepository>();
                Human human2 = repo.GetFilteredElements(u => u.UniqueId == human.UniqueId).Single();
                Assert.AreEqual("Maria", human2.Name);
                //Assert.Greater(human2.UpdatedAt, human2.CreatedAt);
            }
        }

        private Human _Add_Human()
        {
            var human = new Human();
            human.Name = "Martin";
            human.isMale = true;

            using (IUnitOfWork ctx = UnitOfWorkScope.Start())
            {
                var repo = ServiceLocator.GetInstance<IHumanRepository>();
                repo.Add(human);
                ctx.Commit();
            }

            return human;
        }

        #endregion Methods
    }
}

#endif