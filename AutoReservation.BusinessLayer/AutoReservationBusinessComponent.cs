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
        public List<Auto> Autos { get { return usingContext(context => context.Autos.ToList()); } }

        public List<Kunde> Kunden { get { return usingContext(context => context.Kunden.ToList()); } }

        public List<Reservation> Reservationen { get { return usingContext(context => includeReservationReferences(context.Reservationen).ToList()); } }

        public Auto GetAutoById(int id)
        {
            return usingContext(context => context.Autos.FirstOrDefault(auto => auto.Id == id));
        }

        public Kunde GetKundeById(int id)
        {
            return usingContext(context => context.Kunden.FirstOrDefault(kunde => kunde.Id == id));
        }

        public Reservation GetReservationByNr(int reservationsNr)
        {
            return usingContext(context =>
                includeReservationReferences(context.Reservationen)
                    .FirstOrDefault(reservation => reservation.ReservationsNr == reservationsNr));
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
            return updateEntityWithoutReferences(value, state);
        }

        private static Kunde updateKunde(Kunde value, EntityState state)
        {
            return updateEntityWithoutReferences(value, state);
        }

        private static T usingContext<T>(Func<AutoReservationContext, T> func)
        {
            using (var context = new AutoReservationContext())
            {
                return func(context);
            }
        }

        private static IQueryable<Reservation> includeReservationReferences(IQueryable<Reservation> reservations)
        {
            return reservations
                    .Include(reservation => reservation.Kunde)
                    .Include(reservation => reservation.Auto);
        }

        private static Reservation updateReservation(Reservation reservation, EntityState state)
        {
            return usingContext(context =>
            {
                var entry = context.Entry(reservation);
                entry.State = state;
                saveChanges(context, reservation);

                if (entry.State != EntityState.Detached)
                {
                    entry.Reference(r => r.Auto).Load();
                    entry.Reference(r => r.Kunde).Load();
                }

                return reservation;
            });
        }

        private static T updateEntityWithoutReferences<T>(T entity, EntityState state)
            where T : class
        {
            return usingContext(context =>
            {
                context.Entry(entity).State = state;
                saveChanges(context, entity);

                return entity;
            });
        }

        private static void saveChanges<T>(AutoReservationContext context, T entity)
            where T : class
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw CreateLocalOptimisticConcurrencyException(context, entity);
            }
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