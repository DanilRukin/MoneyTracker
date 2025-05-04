namespace SharedKernel.Interfaces
{
    /// <summary>
    /// Локализатор
    /// </summary>
    public interface ILocalizer
    {
        /// <summary>
        /// Получает локализованное сообщение по его ключу
        /// </summary>
        /// <param name="key">Ключ сообщения</param>
        string this[string key] { get; }

        /// <summary>
        /// Получает локализованное сообщение со вставленными параметрами по его ключу
        /// </summary>
        /// <param name="key">Ключ сообщения</param>
        /// <param name="arguments">Параметры для сообщения</param>
        string this[string key, params object[] arguments] { get; }
    }
}
