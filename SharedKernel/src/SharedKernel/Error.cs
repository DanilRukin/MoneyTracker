using SharedKernel.Interfaces;

namespace SharedKernel
{
    /// <summary>
    /// Ошибка
    /// </summary>
    public sealed class Error : ValueObject
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Параметры для форматирования
        /// </summary>
        public object[] Parameters { get; }

        public Error(string code, params object[] parameters)
        {
            Code = code;
            Parameters = parameters;
        }

        /// <summary>
        /// Получает локализованное сообщение ошибки
        /// </summary>
        /// <param name="localizer">Локализатор</param>
        public string Localize(ILocalizer localizer) =>
            localizer[Code, Parameters];

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}
