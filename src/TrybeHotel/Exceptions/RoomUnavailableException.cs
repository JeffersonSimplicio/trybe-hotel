namespace TrybeHotel.Exceptions;

public class RoomUnavailableException : InvalidOperationException {
    public RoomUnavailableException()
        : base("O quarto já está reservado para o período selecionado.") { }

    public RoomUnavailableException(string roomName)
        : base($"O quarto {roomName} já está reservado para o período selecionado.") { }
}
