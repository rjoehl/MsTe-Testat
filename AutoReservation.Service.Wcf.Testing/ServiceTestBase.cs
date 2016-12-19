using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.Interfaces;
using AutoReservation.TestEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;

namespace AutoReservation.Service.Wcf.Testing
{
    [TestClass]
    public abstract class ServiceTestBase
    {
        protected abstract IAutoReservationService Target { get; }

        [TestInitialize]
        public void InitializeTestData()
        {
            TestEnvironmentHelper.InitializeTestData();
        }

        #region Read all entities

        [TestMethod]
        public void GetAutosTest()
        {
            Assert.AreEqual(3, Target.Autos.Count());
        }

        [TestMethod]
        public void GetKundenTest()
        {
            Assert.AreEqual(4, Target.Kunden.Count());
        }

        [TestMethod]
        public void GetReservationenTest()
        {
            Assert.AreEqual(3, Target.Reservationen.Count());
        }

        #endregion

        #region Get by existing ID

        [TestMethod]
        public void GetAutoByIdTest()
        {
            foreach (var expected in Target.Autos)
            {
                var actual = Target.GetAutoById(expected.Id);
                Assert.AreEqual(expected.AutoKlasse, actual.AutoKlasse);
                Assert.AreEqual(expected.Basistarif, actual.Basistarif);
                Assert.AreEqual(expected.Id, actual.Id);
                Assert.AreEqual(expected.Marke, actual.Marke);
                Assert.AreEqual(expected.Tagestarif, actual.Tagestarif);
            }
        }

        [TestMethod]
        public void GetKundeByIdTest()
        {
            foreach (var expected in Target.Kunden)
            {
                var actual = Target.GetKundeById(expected.Id);
                Assert.AreEqual(expected.Geburtsdatum, actual.Geburtsdatum);
                Assert.AreEqual(expected.Id, actual.Id);
                Assert.AreEqual(expected.Nachname, actual.Nachname);
                Assert.AreEqual(expected.Vorname, actual.Vorname);
            }
        }

        [TestMethod]
        public void GetReservationByNrTest()
        {
            foreach (var expected in Target.Reservationen)
            {
                var actual = Target.GetReservationByNr(expected.ReservationsNr);
                Assert.AreEqual(expected.Auto, actual.Auto);
                Assert.AreEqual(expected.Bis, actual.Bis);
                Assert.AreEqual(expected.Kunde, actual.Kunde);
                Assert.AreEqual(expected.Von, actual.Von);
            }
        }

        #endregion

        #region Get by not existing ID

        [TestMethod]
        public void GetAutoByIdWithIllegalIdTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void GetKundeByIdWithIllegalIdTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void GetReservationByNrWithIllegalIdTest()
        {
        }

        #endregion

        #region Insert

        [TestMethod]
        public void InsertAutoTest()
        {
            var expected = Target.InsertAuto(new AutoDto()
            {
                AutoKlasse = AutoKlasse.Luxusklasse,
                Basistarif = 42,
                Marke = "The Answer",
                Tagestarif = 9999
            });

            var actual = Target.GetAutoById(expected.Id);
            Assert.AreEqual(expected.AutoKlasse, actual.AutoKlasse);
            Assert.AreEqual(expected.Basistarif, actual.Basistarif);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Marke, actual.Marke);
            Assert.AreEqual(expected.Tagestarif, actual.Tagestarif);
        }

        [TestMethod]
        public void InsertKundeTest()
        {
            var expected = Target.InsertKunde(new KundeDto()
            {
                Geburtsdatum = new DateTime(2000, 1, 1),
                Nachname = "Hansen",
                Vorname = "Jürg"
            });

            var actual = Target.GetKundeById(expected.Id);
            Assert.AreEqual(expected.Geburtsdatum, actual.Geburtsdatum);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Nachname, actual.Nachname);
            Assert.AreEqual(expected.Vorname, actual.Vorname);
        }

        [TestMethod]
        public void InsertReservationTest()
        {
            var expected = Target.InsertReservation(new ReservationDto()
            {
                Auto = Target.Autos.Last(),
                Bis = new DateTime(2038, 5, 1),
                Kunde = Target.Kunden.Last(),
                Von = new DateTime(2016, 12, 19)
            });

            var actual = Target.GetReservationByNr(expected.ReservationsNr);
            Assert.AreEqual(expected.Auto, actual.Auto);
            Assert.AreEqual(expected.Bis, actual.Bis);
            Assert.AreEqual(expected.Kunde, actual.Kunde);
            Assert.AreEqual(expected.Von, actual.Von);
        }

        #endregion

        #region Delete  

        [TestMethod]
        public void DeleteAutoTest()
        {
            var expected = Target.Autos.Count() - 1;
            Target.DeleteAuto(Target.Autos.Last());

            Assert.AreEqual(expected, Target.Autos.Count());
        }

        [TestMethod]
        public void DeleteKundeTest()
        {
            var expected = Target.Kunden.Count() - 1;
            Target.DeleteKunde(Target.Kunden.Last());

            Assert.AreEqual(expected, Target.Kunden.Count());
        }

        [TestMethod]
        public void DeleteReservationTest()
        {
            var expected = Target.Reservationen.Count() - 1;
            Target.DeleteReservation(Target.Reservationen.Last());

            Assert.AreEqual(expected, Target.Reservationen.Count());
        }

        #endregion

        #region Update

        [TestMethod]
        public void UpdateAutoTest()
        {
            var expected = Target.Autos.First();
            expected.Tagestarif -= 20;
            Target.UpdateAuto(expected);

            var actual = Target.GetAutoById(expected.Id);
            Assert.AreEqual(expected.Basistarif, actual.Basistarif);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Marke, actual.Marke);
            Assert.AreEqual(expected.Tagestarif, actual.Tagestarif);
        }

        [TestMethod]
        public void UpdateKundeTest()
        {
            var expected = Target.Kunden.First();
            expected.Vorname += " J.";
            Target.UpdateKunde(expected);

            var actual = Target.GetKundeById(expected.Id);
            Assert.AreEqual(expected.Geburtsdatum, actual.Geburtsdatum);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Nachname, actual.Nachname);
            Assert.AreEqual(expected.Vorname, actual.Vorname);
        }

        [TestMethod]
        public void UpdateReservationTest()
        {
            var expected = Target.Reservationen.First();
            expected.Von = expected.Von.Subtract(new TimeSpan(1, 0, 0));
            Target.UpdateReservation(expected);

            var actual = Target.GetReservationByNr(expected.ReservationsNr);
            Assert.AreEqual(expected.Auto, actual.Auto);
            Assert.AreEqual(expected.Bis, actual.Bis);
            Assert.AreEqual(expected.Kunde, actual.Kunde);
            Assert.AreEqual(expected.Von, actual.Von);
        }

        #endregion

        #region Update with optimistic concurrency violation

        [TestMethod]
        public void UpdateAutoWithOptimisticConcurrencyTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void UpdateKundeWithOptimisticConcurrencyTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void UpdateReservationWithOptimisticConcurrencyTest()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        #endregion
    }
}
