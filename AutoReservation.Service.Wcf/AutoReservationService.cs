﻿using AutoReservation.Common.Interfaces;
using System;
using System.Diagnostics;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.FaultExceptions;
using System.Collections.Generic;
using AutoReservation.BusinessLayer;
using AutoReservation.Dal.Entities;
using System.ServiceModel;

namespace AutoReservation.Service.Wcf
{
    public class AutoReservationService : IAutoReservationService
    {
        private readonly AutoReservationBusinessComponent component = new AutoReservationBusinessComponent();

        public List<AutoDto> Autos
        {
            get
            {
                WriteActualMethod();
                return component.Autos.ConvertToDtos();
            }
        }

        public List<KundeDto> Kunden
        {
            get
            {
                WriteActualMethod();
                return component.Kunden.ConvertToDtos();
            }
        }

        public List<ReservationDto> Reservationen
        {
            get
            {
                WriteActualMethod();
                return component.Reservationen.ConvertToDtos();
            }
        }

        public AutoDto GetAutoById(int id)
        {
            WriteActualMethod();
            return component.GetAutoById(id).ConvertToDto();
        }

        public KundeDto GetKundeById(int id)
        {
            WriteActualMethod();
            return component.GetKundeById(id).ConvertToDto();
        }

        public ReservationDto GetReservationByNr(int id)
        {
            WriteActualMethod();
            return component.GetReservationByNr(id).ConvertToDto();
        }

        public AutoDto InsertAuto(AutoDto autoDto)
        {
            WriteActualMethod();
            return component.InsertAuto(autoDto.ConvertToEntity()).ConvertToDto();
        }

        public KundeDto InsertKunde(KundeDto kundeDto)
        {
            WriteActualMethod();
            return component.InsertKunde(kundeDto.ConvertToEntity()).ConvertToDto();
        }

        public ReservationDto InsertReservation(ReservationDto reservationDto)
        {
            WriteActualMethod();
            return component.InsertReservation(reservationDto.ConvertToEntity()).ConvertToDto();
        }

        public AutoDto UpdateAuto(AutoDto autoDto)
        {
            WriteActualMethod();

            return handlingOptimisticConcurrencyException<Auto, AutoDto>("UpdateAuto",
                () => component.UpdateAuto(autoDto.ConvertToEntity()).ConvertToDto());
        }

        public KundeDto UpdateKunde(KundeDto kundeDto)
        {
            WriteActualMethod();

            return handlingOptimisticConcurrencyException<Kunde, KundeDto>("UpdateKunde",
                () => component.UpdateKunde(kundeDto.ConvertToEntity()).ConvertToDto());
        }

        public ReservationDto UpdateReservation(ReservationDto reservationDto)
        {
            WriteActualMethod();

            return handlingOptimisticConcurrencyException<Reservation, ReservationDto>("UpdateReservation",
                () => component.UpdateReservation(reservationDto.ConvertToEntity()).ConvertToDto());
        }

        public void DeleteAuto(AutoDto autoDto)
        {
            WriteActualMethod();
            component.DeleteAuto(autoDto.ConvertToEntity());
        }

        public void DeleteKunde(KundeDto kundeDto)
        {
            WriteActualMethod();
            component.DeleteKunde(kundeDto.ConvertToEntity());
        }

        public void DeleteReservation(ReservationDto reservationDto)
        {
            WriteActualMethod();
            component.DeleteReservation(reservationDto.ConvertToEntity());
        }

        private static TReturn handlingOptimisticConcurrencyException<TEntity, TReturn>(string operation, Func<TReturn> func)
        {
            try
            {
                return func();
            }
            catch (LocalOptimisticConcurrencyException<TEntity> e)
            {
                var fault = new OptimisticConcurrencyFaultContract()
                {
                    Operation = operation,
                    Message = e.Message
                };
                throw new FaultException<OptimisticConcurrencyFaultContract>(fault);
            }
        }

        private static void WriteActualMethod()
        {
            Console.WriteLine($"Calling: {new StackTrace().GetFrame(1).GetMethod().Name}");
        }
    }
}
