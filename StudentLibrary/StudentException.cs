using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLibrary
{
	/// <summary>
	/// Класс для соего собственного ненаглядного исключения для \исключительных\ студентов.
	/// </summary>
	[Serializable]
	public class StudentException : Exception
	{
		public StudentException() { }
		public StudentException(string message) : base(message) { }
		public StudentException(string message, Exception inner) : base(message, inner) { }
		protected StudentException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
