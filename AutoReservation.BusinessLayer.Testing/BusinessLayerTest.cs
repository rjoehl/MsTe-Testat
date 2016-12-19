using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace AutoReservation.BusinessLayer.Testing
{
    [TestClass]
    public class BusinessLayerTest
    {

        private AutoReservationBusinessComponent target;
        private AutoReservationBusinessComponent Target
        {
            get
            {
                if (target == null)
                {
                    target = new AutoReservationBusinessComponent();
                }
                return target;
            }
        }
        
        [TestInitialize]
        public void InitializeTestData()
        {
            TestEnvironmentHelper.InitializeTestData();
        }
        
        [TestMethod]
        public void UpdateAutoTest()
        {
            var expected = Target.Autos.First();
            expected.Tagestarif -= 20;
            Target.UpdateAuto(expected);

            var actual = Target.GetAutoById(expected.Id);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Marke, actual.Marke);
            Assert.AreEqual(expected.Reservationen, actual.Reservationen);
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
            Assert.AreEqual(expected.AutoId, actual.AutoId);
            Assert.AreEqual(expected.Bis, actual.Bis);
            Assert.AreEqual(expected.KundeId, actual.KundeId);
            Assert.AreEqual(expected.Von, actual.Von);
        }

    }

}
