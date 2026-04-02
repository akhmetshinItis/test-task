namespace Vitacore.Test.Core.Exceptions
{
	/// <summary>
	/// Исключение "незаполнено обязательное поле"
	/// </summary>
	public class RequiredFieldNotSpecifiedException : AppException
	{
		/// <summary>
		/// Исключение "незаполнено обязательное поле"
		/// </summary>
		public RequiredFieldNotSpecifiedException()
			: base("Обязательное поле не заполнено")
		{
		}

		/// <summary>
		/// Исключение "незаполнено обязательное поле"
		/// </summary>
		/// <param name="field">Обязательное для заполнения поле</param>
		public RequiredFieldNotSpecifiedException(string field)
			: base($"Поле '{field}' обязательно для заполнения")
		{
		}
	}
}
