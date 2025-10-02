using Insania.Geography.Entities;

namespace Insania.Geography.Contracts.Services;

/// <summary>
/// Интерфейс сервиса фонового логгирования в бд
/// </summary>
public interface ILoggingSL
{
    /// <summary>
    /// Метод постановки лога в очередь на обработку
    /// </summary>
    /// <param cref="LogApiGeography" name="log">Лог для записи</param>
    /// <returns cref="ValueTask">Задание</returns>
    ValueTask QueueLogAsync(LogApiGeography log);
}