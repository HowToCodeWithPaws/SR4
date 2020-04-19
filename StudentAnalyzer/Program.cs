using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using StudentLibrary;
using Newtonsoft.Json;

/// <summary>
/// Консольное приложение для десериализации списка
/// студентов и разнообразного применения LINQов для 
/// анализа этих данных.
/// </summary>
namespace StudentAnalyzer
{
	class Program
	{
		/// <summary>
		/// Метод для десериализации через потоки с поимкой исключений.
		/// </summary>
		/// <param name="students"> Возвращает список, полученный
		/// в результате десериализации. </param>
		static List<Student> Deserialize()
		{
			List<Student> students;

			// Блок для обработки исключений, возникающих при 
			// работе с файлами и десериализации.
			try
			{
				File.WriteAllText("../../../.gitignore", ",vs\nbin\nobj");
				using (var stream =
					new JsonTextReader(new StreamReader("../../../students.json")))
				{
					JsonSerializer serializer = new JsonSerializer();
					students = serializer.Deserialize(stream, typeof(List<Student>))
						as List<Student>;
					return students;
				}
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("Файл не существует. Добавьте файл и попробуйте снова.");
			}
			catch (IOException)
			{
				Console.WriteLine("Ошибка ввода/вывода");
			}
			catch (UnauthorizedAccessException)
			{
				Console.WriteLine("Ошибка доступа к файлу. Попробуйте снова с другим файлом");
			}
			catch (System.Security.SecurityException)
			{
				Console.WriteLine("Ошибка безопасности. Попробуйте снова с другим файлом");
			}
			catch (JsonException e)
			{
				Console.WriteLine(e.Message);
			}
			catch (StudentException e)
			{
				Console.WriteLine(e.Message);
			}

			return new List<Student>();
		}

		/// <summary>
		/// Метод, в котором происходят все упражнения с линками, 
		/// которые нужно было проделать.
		/// Замечание: половину переменных, которые тут используются,
		/// можно не использовать, делая запрос прямо внутри Console.WRiteLine,
		/// но тогда строчки получаются слишком длинными, это как-то жабно
		/// с точки зрения кодстайла, поэтому вот. Переменные.
		/// </summary>
		/// <param name="students"> Принимает на вход список студентов,
		/// который в дальнейшем подвергается анализу. </param>
		static void Linqs(List<Student> students)
		{
			// Для начала выводим количество студентов из МИЭМ.
			Console.WriteLine("\nСтудентов из МИЭМ: " +
				$"{students.Where(st => st.Faculty == Faculty.MIEM).Count()}");

			// Теперь упорядочиваем по рейтингу и выводим первые 10.
			var rating = students.OrderByDescending(st => st.Mark).Take(10);

			Console.WriteLine("\nПервые 10 студентов с самым высоким баллом: " +
				$"\n{string.Join(Environment.NewLine, rating)}\n");

			// Этот блок здесь для соблюдения идеологической безопасности,
			// потому что метод + у Student может выбрасывать исключения,
			// если студенты с разных факультетов, но тут мы складываем
			// людей внутри групп по факультетам, так что просто 
			// лекарство от тревожности.
			try
			{
				// Группируем по факультетам, выбираем группы и складываем
				// студентов внутри них, формируя мегастудентов
				// (АЛЬТЕРНАТИВНО ВОЛЬТРОН).
				var byfacs = students.GroupBy(st => st.Faculty);
				var megastudents =
					byfacs.Select(gr => gr.Aggregate((x, y) => x + y)).ToList();

				Console.WriteLine("Среднее лицо факультета:\n"
					+ string.Join(Environment.NewLine, megastudents));

				// У мегастудентов тоже есть мегарейтинг, где они упорядочиваются
				// по баллу, а при равенстве баллов по именам.
				var megarating =
					megastudents.OrderByDescending(st => st.Mark).ThenBy(st => st.Name);

				Console.WriteLine("\nОни же, по убыванию баллов и имен: \n" +
					$"{string.Join(Environment.NewLine, megarating)}");
			}
			catch (ArgumentException e)
			{
				Console.WriteLine(e.Message);
			}
		}

		/// <summary>
		/// Метод мейн, в котором вызываются методы десериализации
		/// и анализа списка студентов с помощью линков.
		/// </summary>
		static void Main(string[] args)
		{
			List<Student> students;

			// Десериализовали, приравняли, вывели.
			// Альтернативно можно было вывести в цикле
			// (так сначала и было).
			students = Deserialize();

			Console.WriteLine("\nДесереализованный список:"
				+ $"\n{string.Join(Environment.NewLine, students)}");


			// Вызов метода, который показывает всю мощь LINQ.
			Linqs(students);
		}
	}
}
