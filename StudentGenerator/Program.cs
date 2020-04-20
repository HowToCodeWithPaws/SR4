using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentLibrary;
using System.IO;
using Newtonsoft.Json;
/// Ср 4
/// Вариант 2
/// Зубарева Наталия 
/// БПИ199
/// 19-20 .04

/// <summary>
/// Консольное приложение для генерации списка студентов и его сериализации.
/// Альтернативное решение: можно было использовать JSON сериализацию из
/// System.Text.Json вместо Newtonsoft.Json, но она требует более слабую
/// инкапсуляцию, поэтому было выбрано то, что выбрано.
/// </summary>
namespace StudentGenerator
{
	class Program
	{
		static Random random = new Random();

		/// <summary>
		/// Метод для создания имени с корректным регистром
		/// той длины, какая требуется по условию для имени студента.
		/// Альтернативно можно было использовать regex.
		/// </summary>
		/// <returns> Возвращает строку с именем. </returns>
		static string GenerateName()
		{
			int length = random.Next(5, 10);
			string name = "" + (char)random.Next('A', 'Z' + 1);

			for (int i = 0; i < length; i++)
			{
				name += (char)random.Next('a', 'z' + 1);
			}

			return name;
		}

		/// <summary>
		/// Метод для сериализации списка студентов с конструкцией
		/// try catch и записью в файл через потоки.
		/// Альтернативно можно было пользоваться File.WriteAllText.
		/// </summary>
		/// <param name="students"> Принимает список, который
		/// нужно сериализовать. </param>
		static void Serialize(List<Student> students)
		{
			// Блок для поимки исключений, связанных с работой с файлами.
			try
			{
				using (var stream =
					new JsonTextWriter(new StreamWriter("../../../students.json")))
				{
					JsonSerializer jsonSerializer = new JsonSerializer();
					jsonSerializer.Serialize(stream, students);
				}
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("Файл не существует. Добаьте файл и попробуйте снова");
			}
			catch (IOException)
			{
				Console.WriteLine("Ошибка ввода/вывода в файл");
			}
			catch (UnauthorizedAccessException)
			{
				Console.WriteLine("Ошибка доступа к файлу. Попробуйте снова");
			}
			catch (System.Security.SecurityException)
			{
				Console.WriteLine("Ошибка безопасности. Попробуйте заново(");
			}
		}

		/// <summary>
		/// Метод мейн, в котором создается список
		/// студентов и вызывается метод сериализации.
		/// </summary>
		static void Main(string[] args)
		{
			// Список для студентов.
			List<Student> students = new List<Student>();

			// Заполняем список 30 студентами, созданными с случайными
			// но корректными параметрами. 
			for (int i = 0; i < 30; i++)
			{
				// На всякий случай оборачиваем в try catch с поимкой 
				// кастомных исключений, хотя в этом нет необходимости, но по канонам лучше.
				try
				{
					students.Add(new Student(GenerateName(),
						(Faculty)random.Next(3), random.NextDouble() + random.Next(4, 10)));
				}
				catch (StudentException e)
				{
					Console.WriteLine(e.Message);
				}
			}

			// Выводим список в консоль.
			foreach (Student student in students)
			{
				Console.WriteLine(student);
			}

			// Вызываем метод для сериализации.
			Serialize(students);
		}
	}
}
