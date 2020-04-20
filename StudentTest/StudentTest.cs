using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StudentLibrary;
using System.IO;

namespace StudentTest
{
	[TestClass]
	public class StudentTest
	{
		/// <summary>
		/// Тестирование метода ToString() с проверкой округления
		/// до трех цифр после запятой, соответствия локали и общего формата.
		/// </summary>
		[TestMethod]
		public void TestToString()
		{
			Student a = new Student("Aaaaaaaaaa", Faculty.CS, 8.88888888);
			Student b = new Student("Bbbbbb", Faculty.Design, 8);

			string exp1 = "CS Student Aaaaaaaaaa: Mark = 8,889";
			string exp2 = "Design Student Bbbbbb: Mark = 8,000";

			Assert.AreEqual(exp1, a.ToString());
			Assert.AreEqual(exp2, b.ToString());
		}

		/// <summary>
		/// Это не было обязательно, но здесь я тестирую конструктор 
		/// на предмет выброса всех необходимых исключений по требованиям
		/// условия: имя меньше миниальной и больше максимальной длины,
		/// несоблюдение заглавной буквы и всех остальных строчных,
		/// небуквенные символы (в частности кириллица, так, например,
		/// у Андрея эксепшн), а также значения балла < 4 или >= 10.
		/// </summary>
		[TestMethod]
		public void TestConstructor()
		{
			Assert.ThrowsException<StudentException>(() =>
			{
				Student a = new Student("Aaaaa", Faculty.CS, 8);
			});
			Assert.ThrowsException<StudentException>(() =>
			{
				Student b = new Student("Bbbbbbbbbbb", Faculty.Design, 8);
			});
			Assert.ThrowsException<StudentException>(() =>
			{
				Student c = new Student("cccccc", Faculty.MIEM, 8);
			});
			Assert.ThrowsException<StudentException>(() =>
			{
				Student d = new Student("DDDDDD", Faculty.CS, 8);
			});
			Assert.ThrowsException<StudentException>(() =>
			{
				Student e = new Student("Ee2eeee", Faculty.Design, 8);
			});
			Assert.ThrowsException<StudentException>(() =>
			{
				Student Андрей = new Student("Андрей", Faculty.CS, 8);
			});
			Assert.ThrowsException<StudentException>(() =>
			{
				Student f = new Student("Ffffffff", Faculty.MIEM, 4.0 - double.MinValue);
			});
			Assert.ThrowsException<StudentException>(() =>
			{
				Student g = new Student("Ggggggggg", Faculty.CS, 10);
			});
		}

		/// <summary>
		/// Тестирование для сериализации и десериализации:
		/// объект должен быть равен себе же после сериализации
		/// и десериализации.
		/// </summary>
		[TestMethod]
		public void TestSerialization()
		{
			Student studentExp = new Student("Aaaaaa", Faculty.MIEM, 7.7777777);
			Student studentRes;

			using (var fs =
				new JsonTextWriter(new StreamWriter("testStudent.json")))
			{
				JsonSerializer jsonSerializer = new JsonSerializer();
				jsonSerializer.Serialize(fs, studentExp);
			}

			using (var stream =
				new JsonTextReader(new StreamReader("testStudent.json")))
			{
				JsonSerializer serializer = new JsonSerializer();
				studentRes = serializer.Deserialize(stream, typeof(Student)) as Student;
			}

			Assert.AreEqual(studentExp, studentRes);
		}

		/// <summary>
		/// Тестирование оператора сложения. Проверяем корректность
		/// всех параметров, особенно внимательно смотрим на сложение 
		/// имен - приоритет и некоммутативность при равной длине, 
		/// корректное деление на части при двух именах нечетной длины,
		/// при именах четной и нечетной длины, корректность равенства 
		/// при сложении с собой для имен четной и нечетной длин,
		/// проверка наличия исключения при попытке сложить двух 
		/// студентов с разных факультетов.
		/// </summary>
		[TestMethod]
		public void TestPlusOperator()
		{
			Student a = new Student("Aaaaaa", Faculty.CS, 7.7777777);
			Student b = new Student("Bbbbbb", Faculty.CS, 8.88888888);
			Student c = new Student("Ccccccc", Faculty.Design, 6.666666);
			Student d = new Student("Ddddddddd", Faculty.Design, 4.4444);
			Student e = new Student("Eeeeeeeeee", Faculty.Design, 9.999999999);

			Student exp1 = new Student("Aaabbb", Faculty.CS, (7.7777777+ 8.88888888)/2);
			Student exp2 = new Student("Bbbaaa", Faculty.CS, (7.7777777 + 8.88888888) / 2);
			Student exp3 = new Student("Dddddccc", Faculty.Design, (6.666666 + 4.4444) / 2);
			Student exp4 = new Student("Eeeeedddd", Faculty.Design, (4.4444 + 9.999999999) / 2);

			Assert.AreEqual(exp1, a + b);
			Assert.AreEqual(exp2, b + a);
			Assert.AreEqual(exp3, c + d);
			Assert.AreEqual(exp4, d + e);
			Assert.AreEqual(a, a + a);
			Assert.AreEqual(d, d + d);
			Assert.ThrowsException<ArgumentException>(() => a + d);
		}
	}
}
