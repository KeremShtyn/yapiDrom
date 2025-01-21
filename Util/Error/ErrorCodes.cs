using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Util.Error
{
    public sealed class ErrorCodes
    {
        public int Code { get; }
        public string Message { get; }
        public int HttpStatusCode { get; }

        private ErrorCodes(int code, string message, int httpStatusCode)
        {
            Code = code;
            Message = message;
            HttpStatusCode = httpStatusCode;
        }


        public static readonly ErrorCodes SystemFailure = new(-1, "System Failure", StatusCodes.Status500InternalServerError);
        public static readonly ErrorCodes DataNotFound = new(100, "Data Not Found", StatusCodes.Status404NotFound);
        public static readonly ErrorCodes UserNotFound = new(101, "User Not Found", StatusCodes.Status404NotFound);
        public static readonly ErrorCodes ScooterNotFound = new(101, "Scooter Not Found", StatusCodes.Status404NotFound);
        public static readonly ErrorCodes AccessDenied = new(102, "Access Denied", StatusCodes.Status403Forbidden);
        public static readonly ErrorCodes InsufficientBalance = new(103, "Insufficient Balance", StatusCodes.Status402PaymentRequired);
        public static readonly ErrorCodes ScooterNotAvailable = new(201, "Scooter Not Available", StatusCodes.Status409Conflict);
        public static readonly ErrorCodes ReservationAlreadyCompleted = new(202, "Reservation Already Completed", StatusCodes.Status400BadRequest);
        public static readonly ErrorCodes UserHasReservation = new(202, "User Already has a Reservation", StatusCodes.Status400BadRequest);


        private static readonly List<ErrorCodes> AllErrorCodes = new()
        {
            SystemFailure,
            DataNotFound,
            UserNotFound,
            AccessDenied,
            InsufficientBalance,
            ScooterNotAvailable,
            ReservationAlreadyCompleted
        };

        public static ErrorCodes FindByCode(int code)
        {
            return AllErrorCodes.FirstOrDefault(e => e.Code == code) ?? SystemFailure;
        }
    }
}
