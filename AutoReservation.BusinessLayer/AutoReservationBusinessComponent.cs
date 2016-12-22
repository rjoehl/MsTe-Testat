using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace AutoReservation.BusinessLayer
{
    public class AutoReservationBusinessComponent
    {
        public List<Auto> Autos
        {
            get
            {
                using (var context = new AutoReservationContext())
                {
                    return context.Autos.ToList();
                }
            }
        }

        public List<Kunde> Kunden
        {
            get
            {
                using (var context = new AutoReservationContext())
                {
                    return context.Kunden.ToList();
                }
            }
        }

        public List<Reservation> Reservationen
        {
            get
            {
                using (var context = new AutoReservationContext())
                {
                    return context.Reservationen
                        .Include(reservation => reservation.Kunde)
                        .Include(reservation => reservation.Auto)
                        .ToList();
                }
            }
        }

        public Auto GetAutoById(int id)
        {
            using (var context = new AutoReservationContext())
            {
                return context.Autos.FirstOrDefault(auto => auto.Id == id);
            }
        }

        public Kunde GetKundeById(int id)
        {
            using (var context = new AutoReservationContext())
            {
                return context.Kunden.FirstOrDefault(kunde => kunde.Id == id);
            }
        }

        public Reservation GetReservationByNr(int reservationsNr)
        {
            using (var context = new AutoReservationContext())
            {
                return context.Reservationen
                    .Include(reservation => reservation.Kunde)
                    .Include(reservation => reservation.Auto)
                    .FirstOrDefault(reservation => reservation.ReservationsNr == reservationsNr);
            }
        }

        public Auto InsertAuto(Auto auto)
        {
            return updateAuto(auto, EntityState.Added);
        }

        public Kunde InsertKunde(Kunde kunde)
        {
            return updateKunde(kunde, EntityState.Added);
        }

        public Reservation InsertReservation(Reservation reservation)
        {
            return updateReservation(reservation, EntityState.Added);
        }

        public Auto UpdateAuto(Auto auto)
        {
            return updateAuto(auto, EntityState.Modified);
        }

        public Kunde UpdateKunde(Kunde kunde)
        {
            return updateKunde(kunde, EntityState.Modified);
        }

        public Reservation UpdateReservation(Reservation reservation)
        {
            return updateReservation(reservation, EntityState.Modified);
        }

        public void DeleteAuto(Auto auto)
        {
            updateAuto(auto, EntityState.Deleted);
        }

        public void DeleteKunde(Kunde kunde)
        {
            updateKunde(kunde, EntityState.Deleted);
        }

        public void DeleteReservation(Reservation reservation)
        {
            updateReservation(reservation, EntityState.Deleted);
        }

        private static Auto updateAuto(Auto value, EntityState state)
        {
            return updateEntity(value, state);
        }

        private static Kunde updateKunde(Kunde value, EntityState state)
        {
            return updateEntity(value, state);
        }

        private static T updateEntity<T>(T value, EntityState state)
            where T : class
        {
            using (var context = new AutoReservationContext())
            {
                context.Entry(value).State = state;
                context.SaveChanges();
            }

            return value;
        }

        private static Reservation updateReservation(Reservation value, EntityState state)
        {
            using (var context = new AutoReservationContext())
            {
                var entry = context.Entry(value);
                entry.State = state;
                context.SaveChanges();

                entry.Reference(r => r.Auto).Load();
                entry.Reference(r => r.Kunde).Load();
            }

            return value;
        }

        private static LocalOptimisticConcurrencyException<T> CreateLocalOptimisticConcurrencyException<T>(AutoReservationContext context, T entity)
            where T : class
        {
            var dbEntity = (T)context.Entry(entity)
                .GetDatabaseValues()
                .ToObject();

            return new LocalOptimisticConcurrencyException<T>($"Update {typeof(Auto).Name}: Concurrency-Fehler", dbEntity);
        }
    }
}