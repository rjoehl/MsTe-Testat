﻿using AutoReservation.Dal;
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
        public IEnumerable<Auto> Autos
        {
            get
            {
                using (var context = new AutoReservationContext())
                {
                    return context.Autos.ToList();
                }
            }
        }

        public IEnumerable<Kunde> Kunden
        {
            get
            {
                using (var context = new AutoReservationContext())
                {
                    return context.Kunden.ToList();
                }
            }
        }

        public IEnumerable<Reservation> Reservationen
        {
            get
            {
                using (var context = new AutoReservationContext())
                {
                    return context.Reservationen.ToList();
                }
            }
        }

        public Auto GetAutoById(int id)
        {
            using (var context = new AutoReservationContext())
            {
                var query = from auto in context.Autos
                            where auto.Id == id
                            select auto;
                return query.First();
            }
        }

        public Kunde GetKundeById(int id)
        {
            using (var context = new AutoReservationContext())
            {
                var query = from kunde in context.Kunden
                            where kunde.Id == id
                            select kunde;
                return query.First();
            }
        }

        public Reservation GetReservationByNr(int reservationsNr)
        {
            using (var context = new AutoReservationContext())
            {
                var query = from reservation in context.Reservationen
                            where reservation.ReservationsNr == reservationsNr
                            select reservation;
                return query.First();
            }
        }

        public Auto InsertAuto(Auto auto)
        {
            return updateEntityAndState(auto, EntityState.Added);
        }

        public Kunde InsertKunde(Kunde kunde)
        {
            return updateEntityAndState(kunde, EntityState.Added);
        }

        public Reservation InsertReservation(Reservation reservation)
        {
            return updateEntityAndState(reservation, EntityState.Added);
        }

        public Auto UpdateAuto(Auto auto)
        {
            return updateEntityAndState(auto, EntityState.Modified);
        }

        public Kunde UpdateKunde(Kunde kunde)
        {
            return updateEntityAndState(kunde, EntityState.Modified);
        }

        public Reservation UpdateReservation(Reservation reservation)
        {
            return updateEntityAndState(reservation, EntityState.Modified);
        }

        public void DeleteAuto(Auto auto)
        {
            updateEntityAndState(auto, EntityState.Deleted);
        }

        public void DeleteKunde(Kunde kunde)
        {
            updateEntityAndState(kunde, EntityState.Deleted);
        }

        public void DeleteReservation(Reservation reservation)
        {
            updateEntityAndState(reservation, EntityState.Deleted);
        }

        private T updateEntityAndState<T>(T value, EntityState state)
            where T : class
        {
            using (var context = new AutoReservationContext())
            {
                context.Entry(value).State = state;
                context.SaveChanges();
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