using MoneyTracker.SharedConstants.ErrorCodes;
using SharedKernel;

namespace MoneyTracker.CurrencyService.Domain.Base
{
    /// <summary>
    /// Базовая сущность домена CurrencyService
    /// </summary>
    /// <typeparam name="T">Тип первичного ключа</typeparam>
    public abstract class CurrencyServiceBaseEntity<T> : EntityBase<T>
    {
        protected bool isDropped = false;

        /// <summary>
        /// Удаляет сущность из предметной области
        /// </summary>
        public void Drop()
        {
            if (!isDropped)
            {
                isDropped = true;
                Invalidate();
            }
        }

        /// <summary>
        /// Определяет шаги, которые должны быть выполнены для приведения сущности в удаленное состояние
        /// </summary>
        protected abstract void Invalidate();

        /// <summary>
        /// Предотвращает выполнение любой операции, если сущность удалена
        /// </summary>
        protected void ThrowIfDropped()
        {
            if (isDropped)
            {
                throw new InvalidOperationException(Errors.Common.CouldNotApplyOperationForDroppedEntity);
            }
        }
    }
}
