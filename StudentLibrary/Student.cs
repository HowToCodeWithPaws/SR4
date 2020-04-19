using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLibrary
{
	/// <summary>
	/// Класс для студента: здесь есть Имя, Факультет, Балл 
	/// (все то что есть у нас(мозга нет)), конструктор, 
	/// задающий параметры и проверяющий их корректность, 
	/// метод для вывода информации о студенте и определение
	/// оператора сложения для двух студентов (страшно складывать людей).
	/// Также есть два метода Equals для того, чтобы работали 
	/// тЕсТИКи.
	/// </summary>
	public class Student
	{
		/// <summary>
		/// Свойства для имени, факультета и балла. Стоит заметить, что
		/// в условии требовались свойства только для чтения, но без
		/// сеттеров не сработает сериализация, поэтому они будут, но 
		/// будут такими скрытными, что вы их даже не заметите.
		/// </summary>
		public string Name { get; private set; }
		public Faculty Faculty { get; private set; }
		public double Mark { get; private set; }

		/// <summary>
		/// Конструктор с требуемыми параметрами и проверкой
		/// корректности входных данных.
		/// При неверных данных выбрасываются кастомные исключения.
		/// Альтернативное решение: для проверки имени можно 
		/// было использовать regex.
		/// </summary>
		/// <param name="name"> Параметр для имени. </param>
		/// <param name="faculty"> параметр для факультета. </param>
		/// <param name="mark"> Параметр для балла. </param>
		public Student(string name, Faculty faculty, double mark)
		{
			// Проверка того, что имя корректной длины.
			if (name.Length < 6 || name.Length > 10)
			{
				throw new StudentException("Длина имени должна быть от 6 до 10 символов! " +
					$"Имя {name} некорректно.");
			}

			// Проверка регистров, сделана тоже линками :> .
			if (!(name[0] >= 'A' && name[0] <= 'Z') ||
				!name.Substring(1).All(letter => letter >= 'a' && letter <= 'z'))
			{
				throw new StudentException("Имя должно состоять из первой заглавной" +
					$" латинской буквы и остальных строчных латинских! Имя {name} некорректно.");
			}

			// Оу хай марк. Проверка границ балла.
			if (mark < 4 || mark >= 10)
			{
				throw new StudentException("Балл должен лежать в диапазоне [4;10)!" +
					$"Балл {mark} не корректен. Студент либо слишком хорош, либо отчислен.");
			}

			// Если все хорошо, заполняем свойства студента.
			Name = name;
			Faculty = faculty;
			Mark = mark;
		}

		/// <summary>
		/// Метод для вывода информации о студенте в строку, 
		/// форматирование 3 знака после запятой.
		/// </summary>
		/// <returns> Возвращает строку с информацией о студенте.
		/// </returns>
		public override string ToString()
		{
			return $"{Faculty} Student {Name}: Mark = {Mark:F3}";
		}

		/// <summary>
		/// Определение оператора + для двух студентов.
		/// Здесь было много вариантов трактовки того,
		/// как формировать имя, но стоит действительно сойтись
		/// на том, что имя склеивается из двух частей: 
		/// первая и большая половина самого длинного из имен
		/// (либо имени того, кто идет первым при равенстве длин)
		/// и вторая и меньшая половина другого.
		/// За счет неравного деления в случае нечетных длин
		/// осуществляется то, что а+а=а.
		/// При этом стоит заметить, что операция не коммутативна
		/// в случае разных имен с равными длинами.
		/// </summary>
		/// <param name="a"> Первый студент. </param>
		/// <param name="b"> Второй студент. </param>
		/// <returns> Возвращает нового суперстудента, 
		/// склеенного из двух прежних. </returns>
		public static Student operator +(Student a, Student b)
		{
			// Если факультеты разные, сразу выкидываем исключение 
			// и дальше не идем. Тут только чистокровки.
			if (a.Faculty != b.Faculty)
			{
				throw new ArgumentException("Для сложения учеников их факультеты должны совпадать!");
			}

			// С помощью тернарной операции разделяем случаи, чье имя длиннее,
			// далее склеиваем имя.
			string name = b.Name.Length > a.Name.Length ?
				b.Name.Substring(0, (int)Math.Ceiling((double)b.Name.Length / 2.0))
				+ a.Name.Substring((int)Math.Ceiling((double)a.Name.Length / 2.0)) :
				a.Name.Substring(0, (int)Math.Ceiling((double)a.Name.Length / 2.0))
				+ b.Name.Substring((int)Math.Ceiling((double)b.Name.Length / 2.0));

			// Балл формируется как среднее.
			double mark = (a.Mark + b.Mark) / 2;

			return new Student(name, a.Faculty, mark);
		}

		/// <summary>
		/// Метод для сравнения двух студентов. Интуитивный. /// </summary>
		/// <param name="other"> Другой студент. </param>
		/// <returns> Возвращает тру или фольс. </returns>
		public bool Equals(Student other)
		{
			return Name.Equals(other.Name) 
				&& Faculty.Equals(other.Faculty) 
				&& Mark.Equals(other.Mark);
		}

		/// <summary>
		/// Метод сравнения с объектом, потому что без него 
		/// не работают тестики.
		/// </summary>
		/// <param name="obj"> Принимает какой-то объект. </param>
		/// <returns> Возвращает сравнение с объектом, скастованным
		/// к студенту (а обычно мы людей дегуманизируем((). </returns>
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Student);
		}
	}
}
