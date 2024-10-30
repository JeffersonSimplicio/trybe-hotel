namespace TrybeHotel.Exceptions;

public class InvalidBookingDateException : InvalidOperationException {
    public InvalidBookingDateException()
        : base("As datas de check-in e check-out devem estar no futuro, e o check-out deve ser posterior ao check-in.") { }

    public InvalidBookingDateException(DateTime checkIn, DateTime checkOut)
        : base($"Datas inválidas. Check-in: {checkIn}, Check-out: {checkOut}. As datas devem estar no futuro e o check-out ser posterior ao check-in.") { }
}
