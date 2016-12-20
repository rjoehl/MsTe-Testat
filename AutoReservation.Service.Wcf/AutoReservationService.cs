using AutoReservation.Common.Interfaces;
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
                List<Auto> autoList = component.GetAllAutos();
                return autoList.ConvertToDtos();
            }
        }

        public List<KundeDto> Kunden
        {
            get
            {
                WriteActualMethod();
                List<Kunde> kundeList = component.GetAllKunden();
                return kundeList.ConvertToDtos();
            }
        }

        public List<ReservationDto> Reservationen
        {
            get
            {
                WriteActualMethod();
                List<Reservation> reservationList = component.GetAllReservations();
                return reservationList.ConvertToDtos();
            }
        }

        private static void WriteActualMethod()
        {
            Console.WriteLine($"Calling: {new StackTrace().GetFrame(1).GetMethod().Name}");
        }

        public void DeleteAuto(AutoDto autoDto)
         {
            component.DeleteAuto(autoDto.ConvertToEntity());
        }

        public void DeleteKunde(KundeDto kundeDto)
        {
            component.DeleteKunde(kundeDto.ConvertToEntity());
        }

        public void DeleteReservation(ReservationDto reservationDto)
        {
            component.DeleteReservation(reservationDto.ConvertToEntity());
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

        public ReservationDto GetReservationByNr(int reservationsNr)
        {
            WriteActualMethod();
            return component.LoadReservation(reservationsNr).ConvertToDto();
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
            Reservation reservation = reservationDto.ConvertToEntity();
            component.InsertReservation(reservation);
            return reservation.ConvertToDto();
        }

        public AutoDto UpdateAuto(AutoDto autoDto)
        {
            WriteActualMethod();
            Auto auto = autoDto.ConvertToEntity();
            component.UpdateAuto(auto);
            return auto.ConvertToDto();
        }

        public KundeDto UpdateKunde(KundeDto kundeDto)
        {
            WriteActualMethod();
            Kunde kunde = kundeDto.ConvertToEntity();
            component.UpdateKunde(kunde);
            return kunde.ConvertToDto();
        }

        public ReservationDto UpdateReservation(ReservationDto reservationDto)
        {
            WriteActualMethod();
            Reservation reservation = reservationDto.ConvertToEntity();
            return component.UpdateReservation(reservation).ConvertToDto();
        }
    }
}
